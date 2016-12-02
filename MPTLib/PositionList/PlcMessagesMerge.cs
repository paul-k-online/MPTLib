using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
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

        public PlcMessagesMerge(IEnumerable<PlcMessage> existMessages, IEnumerable<PlcMessage> newMessages, int plcid = 0)
        {
            if (existMessages == null)
                throw new ArgumentNullException("existMessages");
            
            if (newMessages == null)
                throw new ArgumentNullException("newMessages");



            PlcId = plcid;
            ExistMessages = existMessages;
            NewMessages = newMessages;

            try
            {
                var existDict = ExistMessages.ToDictionary(x => x.Number, y => y);
                var newDict = NewMessages.ToDictionary(x => x.Number, y => y);
                PlcMessagePairDictionary = GetPairDict(existDict, newDict);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*
        public static IGrouping<int, PlcMessage> GetNumbersDuplicates(IEnumerable<PlcMessage> list)
        {
            var dupl = list.GroupBy(s => s.Number);//.SelectMany(grouping => grouping.Skip(1));
            return dupl;
        }

        public bool GetDuplicateExistMessages()
        {
            return GetNumbersDuplicates(ExistMessages) != null;
        }
         * */

        private Dictionary<int, Tuple<PlcMessage, PlcMessage>>  _diffDictionary;
        public Dictionary<int, Tuple<PlcMessage, PlcMessage>> Diff
        {
            get { return _diffDictionary ?? (_diffDictionary = GetDiff()); }
        } 

        private Dictionary<int, Tuple<PlcMessage, PlcMessage>> GetDiff()
        {
            var dict = PlcMessagePairDictionary
                .Where(x => !PlcMessage.ByContentComparer.Comparer.Equals(x.Value.Item1, x.Value.Item2))
                .ToDictionary(x => x.Key, y => y.Value);
            return dict;
        }


        public List<PlcMessage> GetRemoveMessages()
        {
            return Diff
                .Where(x => PlcMessage.IsNullMessage(x.Value.Item2))
                .Select(x => x.Value.Item1)
                .ToList();
        }
        
        public List<PlcMessage> GetAddOrUpdateMessages()
        {
            return Diff
                .Where(x => !PlcMessage.IsNullMessage(x.Value.Item2))
                .Select(x => x.Value.Item2)
                .ToList();
        } 


        public bool SaveToDb(MPTEntities contex)
        {
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                var removeMessages = GetRemoveMessages();
                if (removeMessages != null && removeMessages.Any())
                {
                    var attachedRemoveMessage = removeMessages.Select(x => contex.PlcMessages.Attach(x));
                    contex.PlcMessages.RemoveRange(attachedRemoveMessage);
                    contex.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logger.Error("RemoveRange Messages: {0}", ex.ToString());
            }
            
            try
            {
                var addOrUpdateMessages = GetAddOrUpdateMessages();
                if (addOrUpdateMessages != null && addOrUpdateMessages.Any())
                {
                    addOrUpdateMessages.ForEach(x => x.PlcId = PlcId);
                    contex.PlcMessages.AddOrUpdate(addOrUpdateMessages.ToArray());
                    contex.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logger.Error("AddOrUpdate Messages: {0}", ex.ToString());
            }
            return true;
        }
    }
}
