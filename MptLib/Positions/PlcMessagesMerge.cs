using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Configuration;
using System.Security.Cryptography.Xml;
using System.Text;
using MPT.Model;
using NLog;

namespace MPT.Positions
{
    public class PlcMessagesMerge
    {
        public static IDictionary<int, Tuple<T, T>> GetPairDict<T>(IDictionary<int, T> dict1, IDictionary<int, T> dict2)
        {
            var pairDict = dict1.ToDictionary(x => x.Key, y => new Tuple<T, T>(y.Value, default(T)));
            foreach (var item2 in dict2)
            {
                var key = item2.Key;
                pairDict[key] = pairDict.ContainsKey(key)
                    ? new Tuple<T, T>(pairDict[key].Item1, item2.Value)
                    : new Tuple<T, T>(default(T), item2.Value);
            }
            return new SortedDictionary<int, Tuple<T, T>>(pairDict);
        }
        
        public int PlcId { get; set; }
        public IEnumerable<PlcMessage> ExistMessages { get; private set; }
        public IEnumerable<PlcMessage> NewMessages { get; private set; }
        public IDictionary<int, Tuple<PlcMessage, PlcMessage>> PlcMessagePairDictionary { get; private set; }

        public IDictionary<int, Tuple<string, string>>  MessagePairDictionary
        {
            get
            {
                return PlcMessagePairDictionary
                    .ToDictionary(
                        x => x.Key,
                        y => new Tuple<string, string>(y.Value.Item1.Text, y.Value.Item2.Text));
            }
        }

        public PlcMessagesMerge(IEnumerable<PlcMessage> existMessages, IEnumerable<PlcMessage> newMessages, int plcId)
        {
            PlcId = plcId;

            ExistMessages = existMessages;
            NewMessages = newMessages.Where(x => !string.IsNullOrWhiteSpace(x.Text));
            
            var existDict = ExistMessages.ToDictionary(x => x.Number, y => y);
            var newDict = NewMessages.ToDictionary(x => x.Number, y => y);
            
            foreach (var msg in newDict.Values)
            {
                msg.PlcId = PlcId;
            }
            
            PlcMessagePairDictionary = GetPairDict(existDict, newDict);
        }



        private static bool CheckToRemove(PlcMessage m1, PlcMessage m2)
        {
            return (m2 == null) 
                || (m1 != null && string.IsNullOrWhiteSpace(m1.Text) && string.IsNullOrWhiteSpace(m2.Text));
        }

        public IEnumerable<PlcMessage> GetMessagesToRemove()
        {
            //var messagesToRemove = dbMessages.Where(x => string.IsNullOrWhiteSpace(x.Text)).ToList();
            // var notExistDbMessages = sqlMessageList.Except(excelMessageList.Values, new PlcMessage.PlcMessageByIdComparer()).ToList();

            var messagesToRemove = PlcMessagePairDictionary
                .Where(x => CheckToRemove(x.Value.Item1, x.Value.Item2) )
                .Select(x=>x.Value.Item1)
                .ToList()
                ;
            return messagesToRemove;
        }


        private static bool CheckToDifferent(PlcMessage m1, PlcMessage m2)
        {
            return m1 != null && m2 != null
                // && !string.IsNullOrWhiteSpace(m1.Text)
                // && !string.IsNullOrWhiteSpace(m2.Text)
                && !string.Equals(m1.Text, m2.Text, StringComparison.InvariantCultureIgnoreCase);
        }

        public IEnumerable<PlcMessage> GetDifferentMessages()
        {
            // var intersectMessages = excelMessageList.Values.Intersect(sqlMessageList, new PlcMessage.PlcMessageByIdComparer()).Except(sqlMessageList, new PlcMessage.PlcMessageByTextComparer()).ToList();
            var intersectMessages = PlcMessagePairDictionary
                .Where(x => CheckToDifferent(x.Value.Item1, x.Value.Item2))
                .Select(x => x.Value.Item2);
            return intersectMessages;
        }

        
        private static bool CheckNewMessage(PlcMessage m1, PlcMessage m2)
        {
            return m1 == null && m2 != null && !string.IsNullOrWhiteSpace(m2.Text);
        }

        public IEnumerable<PlcMessage> GetNewMesages()
        {
            //var newMessages = excelMessageList.Values.Except(sqlMessageList, new PlcMessage.PlcMessageByIdComparer()).ToList();
            var newMessages = PlcMessagePairDictionary
                .Where(x => CheckNewMessage(x.Value.Item1, x.Value.Item2))
                .Select(x => x.Value.Item2);
            return newMessages;
        }



        public bool MergeWithDb(MPTEntities contex)
        {
            var logger = LogManager.GetCurrentClassLogger();

            
            // Clear of empty, Delete not existed
            var nullOrWhiteSpaceMessages = GetMessagesToRemove();
            try
            {
                contex.PlcMessages.RemoveRange(nullOrWhiteSpaceMessages);
                contex.SaveChanges();
            }
            catch (Exception exception)
            {
                logger.Error("Remove Messages:");
                logger.Error(exception);
            }


            // UPDATE: different message by Text
            var differentMessages = GetDifferentMessages();
            try
            {
                contex.PlcMessages.AddOrUpdate(differentMessages.ToArray());
                contex.SaveChanges();
            }
            catch (Exception exception)
            {
                logger.Error("Different Messages:");
                logger.Error(exception);
            }


            // Add new
            var newMessages = GetNewMesages();
            try
            {
                contex.PlcMessages.AddRange(newMessages.ToArray());
                contex.SaveChanges();
            }
            catch (Exception exception)
            {
                logger.Error("New Mesages:");
                logger.Error(exception);
            }

            return true;
        }
        
        
        private static bool UpdateDbPlcMessages(MPTEntities contex, IDictionary<int,PlcMessage> extMessages, int plcId, 
            bool deleteNotExistIdMessages = false, 
            bool updateExistIdMessages = false, 
            bool addNewMessages = false)
        {
            var logger = LogManager.GetCurrentClassLogger();

            List<PlcMessage> dbMessages;
            try
            {
                var plc = contex.PLCs.SingleOrDefault(x => x.Id == plcId);
                if (plc == null)
                    throw new Exception("plc not found");

                dbMessages = contex.PlcMessages.Where(x=>x.PlcId==plc.Id).AsNoTracking().ToList();
            }
            catch
            {
                logger.Error("Plc id=\"{0}\" not found", plcId);
                return false;
            }

            
            //var pairDict = GetPairDict(dbMessageDict, extMessages);
            var messagesMerge = new PlcMessagesMerge(dbMessages, extMessages.Values.ToList(), plcId );

                
            // Clear db
            var nullOrWhiteSpaceMessages = messagesMerge.GetMessagesToRemove();
            try
            {
                contex.PlcMessages.RemoveRange(nullOrWhiteSpaceMessages);
                contex.SaveChanges();
            }
            catch (Exception exception)
            {
                logger.Error("Delete null, whiteSpace or not exist messages:");
                logger.Error(exception);
            }
                
            // UPDATE: different message by Text
            if (updateExistIdMessages)
            {
                var differentMessages = messagesMerge.GetDifferentMessages();
                try
                {
                    contex.PlcMessages.AddOrUpdate(differentMessages.ToArray());
                    contex.SaveChanges();
                }
                catch (Exception exception)
                {
                    logger.Error("Update exist id messages:");
                    logger.Error(exception);
                }
            }


            // Add new
            var newMessages = messagesMerge.GetNewMesages();


            if (addNewMessages)
            {
                try
                {
                    contex.PlcMessages.AddRange(newMessages.ToArray());
                    contex.SaveChanges();
                }
                catch (Exception exception)
                {
                    logger.Error("Add new messages:");
                    logger.Error(exception);
                }
            }

            return true;

        }


    }
}
