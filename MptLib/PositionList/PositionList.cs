using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using MPT.Excel;
using MPT.Model;
using NLog;

namespace MPT.PositionList
{
    public class ExcelPositionList
    {
        private static bool FindDuplicatesByKey<T, TKey>(IEnumerable<T> list, Func<T, TKey> keySelector)
        {
            return list.GroupBy(keySelector).Any(g => g.Count() > 1);
        }
        

        public List<AiPosition> AiList { get; private set; }

        public bool AiHasDuplicate
        {
            get { return FindDuplicatesByKey(AiList, x => x.Number); }
        }

        public List<AoPosition> AoList { get; private set; }

        public bool AoHasDuplicate
        {
            get { return FindDuplicatesByKey(AoList, x => x.Number); }
        }

        public List<DioPosition> DioList { get; private set; }

        public bool DioHasDuplicate
        {
            get { return FindDuplicatesByKey(DioList, x => x.Number); }
        }

        public List<PlcMessage> MessageList { get; private set; }

        public bool MessagesHasDuplicate
        {
            get { return FindDuplicatesByKey(MessageList, x => x.Number); } 
        }

        private int? _plcId;
        public int? PlcId {
            get
            {
                return _plcId;
            }
            set 
            {
                _plcId = value;
                if (PlcId == null) 
                    return;
                UpdateAiListPlcId(PlcId.Value);
                UpdateDioListPlcId(PlcId.Value);
                UpdateMessageListPlcId(PlcId.Value);
            }
        }

        ExcelDataBase Excel { get; set; }
        
        public ExcelPositionList(ExcelDataBase excel, int? plcId = null)
        {
            Excel = excel;
            PlcId = plcId;
            
            LoadAiData();
            LoadDioData();
            LoadMessageData();
        }

        public ExcelPositionList(string excelFile, int? plcId = null)
            : this(new ExcelDataBase(excelFile), plcId)
        {
        }
        
        public void LoadAiData(string sheetName = "AI")
        {
            AiList = Excel.GetDataList(sheetName, ExcelPositionConvert.ToAiPosition).ToList();
            if (PlcId != null)
                UpdateAiListPlcId(PlcId.Value);
        }
        
        public void LoadDioData(string sheetName = "DIO")
        {
            DioList = Excel.GetDataList(sheetName, ExcelPositionConvert.ToDioPosition).ToList();
            if (PlcId != null)
                UpdateDioListPlcId(PlcId.Value);
        }

        private void LoadMessageData(string sheetName = "Message")
        {
            MessageList = Excel.GetDataList(sheetName, ExcelPositionConvert.ToPlcMessage).ToList();
            if (PlcId != null) 
                UpdateMessageListPlcId(PlcId.Value);
        }

        
        private void UpdateAiListPlcId(int plcId)
        {
            if (AiList != null)
                AiList.ForEach(x => x.PlcId = plcId);
        }

        private void UpdateDioListPlcId(int plcId)
        {
            if (DioList != null)
                DioList.ForEach(x => x.PlcId = plcId);
        }

        private void UpdateMessageListPlcId(int plcId)
        {
            if (MessageList != null)
            {
                MessageList.ForEach(x => x.PlcId = plcId);
            }
        }

        public IDictionary<int, Tuple<PlcMessage, PlcMessage>> GetPairPlcMessgesDictionary(IEnumerable<PlcMessage> sqlMessages)
        {
            var sqlMessageDict = sqlMessages.ToDictionary(x => x.Number, y => new Tuple<PlcMessage, PlcMessage>(y, null));
            var dict = new SortedDictionary<int, Tuple<PlcMessage, PlcMessage>>(sqlMessageDict);


            if (MessagesHasDuplicate)
                return null;

            var excelMessageDict = MessageList.ToDictionary(x=>x.Number, y=>y);
            foreach (var excelMessage in excelMessageDict)
            {
                var key = excelMessage.Key;
                if (dict.ContainsKey(key))
                {
                    var sqlMessage = dict[key].Item1;
                    dict[key] = new Tuple<PlcMessage, PlcMessage>(sqlMessage, excelMessage.Value);
                }
                else
                {
                    dict.Add(excelMessage.Value.Number, new Tuple<PlcMessage, PlcMessage>(null, excelMessage.Value));
                }
            }
            return dict;
        }
        
        public bool UpdateDbPlcEvent(bool deleteNotExistIdMessages = false, bool updateExistIdMessages = false, bool addNewMessages = false)
        {
            var logger = LogManager.GetCurrentClassLogger();

            using (var contex = new MPTEntities())
            {
                PLC plc;
                try
                {
                    plc = contex.PLCs.SingleOrDefault(x => x.Id == PlcId);
                    if (plc == null)
                        throw new NoNullAllowedException("plc");
                }
                catch (Exception)
                {
                    logger.Error("Plc id=\"{0}\" not found", PlcId);
                    return false;
                }

                var excelMessageList = MessageList;

                if (excelMessageList == null)
                {
                    logger.Error("Excel message list is empty \"{0}\"", Excel.Name);
                    return false;
                }

                var sqlMessageList = plc.Messages;


                //var a = GetPairPlcMessgesDictionary(sqlMessageList, excelMessageList);

                // Null Or WhiteSpace -- DELETE
                var nullOrWhiteSpaceMessages = sqlMessageList.Where(sqlMessage => string.IsNullOrWhiteSpace(sqlMessage.Text)).ToList();
                try
                {
                    contex.PlcMessages.RemoveRange(nullOrWhiteSpaceMessages);
                    contex.SaveChanges();
                }
                catch (Exception exception)
                {
                    logger.Error("Delete null or whiteSpace messages:");
                    logger.Error(exception);
                }


                // not existing ID -- DELETE
                var notExistIdMessages = sqlMessageList.Except(excelMessageList, new PlcMessage.PlcMessageByIdComparer()).ToList();
                if (deleteNotExistIdMessages)
                {
                    try
                    {
                        contex.PlcMessages.RemoveRange(notExistIdMessages);
                        contex.SaveChanges();
                    }
                    catch (Exception exception)
                    {
                        logger.Error("Delete not exist id messages:");
                        logger.Error(exception);
                    }
                }

                
                // existing ID, different Text -- UPDATE
                var intersectMessages = excelMessageList
                    .Intersect(sqlMessageList, new PlcMessage.PlcMessageByIdComparer())
                    .Except(sqlMessageList, new PlcMessage.PlcMessageByTextComparer())
                    .ToList();
                
                if (updateExistIdMessages)
                {
                    try
                    {
                        contex.PlcMessages.AddOrUpdate(intersectMessages.ToArray());
                        contex.SaveChanges();
                    }
                    catch (Exception exception)
                    {
                        logger.Error("Update exist id messages:");
                        logger.Error(exception);
                    }
                }


                // Add new
                var newMessages = excelMessageList.Except(sqlMessageList, new PlcMessage.PlcMessageByIdComparer()).ToList();
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
            }
            return true;
        }
    }
}
