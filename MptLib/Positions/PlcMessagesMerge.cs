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
using NLog.Filters;

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

        public IDictionary<int, Tuple<PlcMessage, PlcMessage>> GetDiffPlcMessagePairDictionary()
        {
            return PlcMessagePairDictionary
                .Where(x => !PlcMessage.PlcMessageComparer.Comparer.Equals(x.Value.Item1, x.Value.Item2))
                .ToDictionary(x => x.Key, y => y.Value)
                ;
        }
        
        public static bool IsNullMessage(PlcMessage m)
        {
            return m == null || string.IsNullOrWhiteSpace(m.Text);
        }
        

        public bool MergeWithDb(MPTEntities contex)
        {
            var logger = LogManager.GetCurrentClassLogger();

            var diff = GetDiffPlcMessagePairDictionary();

            var removeMessages = diff
                    .Where(x => IsNullMessage(x.Value.Item2))
                    .Select(x => x.Value.Item1)
                    .ToList();
            if (removeMessages.Any())
            {
                try
                {
                    var attachedRemoveMessage = removeMessages.Select(x => contex.PlcMessages.Attach(x));
                    contex.PlcMessages.RemoveRange(attachedRemoveMessage);
                    contex.SaveChanges();
                }
                catch (Exception exception)
                {
                    logger.Error("RemoveRange Messages:");
                    logger.Error(exception);
                }
            }


            var addOrUpdateMessages = diff
                .Where(x => !IsNullMessage(x.Value.Item2))
                .Select(x => x.Value.Item2)
                .ToList();
            
            if (addOrUpdateMessages.Any())
            {
                try
                {
                    contex.PlcMessages.AddOrUpdate(addOrUpdateMessages.ToArray());
                    contex.SaveChanges();
                }
                catch (Exception exception)
                {
                    logger.Error("AddOrUpdate Messages:");
                    logger.Error(exception);
                }
            }

            return true;
        }
    }
}
