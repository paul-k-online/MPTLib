using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MPT.DataBase;
using MPT.Model;

using DioPosition = MPT.Model.DioPosition;

namespace MPT.Positions
{
    public class ExcelPositionList : IPositionList   
    {
        public ExcelDataBase ExcelDataBase { get; private set; }

        IEnumerable<AiPosition> _aiList;
        IEnumerable<AoPosition> _aoList;
        IEnumerable<DioPosition> _dioList;
        IEnumerable<PlcMessage> _plcMessageList;

        private int _plcId;
        private int PlcId
        {
            get { return _plcId; }
            set 
            {
                _plcId = value;
                UpdatePlcId(_aiList, PlcId);
                UpdatePlcId(_aoList, PlcId);
                UpdatePlcId(_dioList, PlcId);
                UpdatePlcId(_plcMessageList, PlcId);
            }
        }

        public ExcelPositionList(ExcelDataBase excelDataBase, int plcId = 0, bool loadData = false)
        {          
            ExcelDataBase = excelDataBase;
            PlcId = plcId;


            if (loadData) 
                LoadAllData();
        }

        public ExcelPositionList(string excelFile, int plcId = 0, bool loadData = false)
            : this(new ExcelDataBase(excelFile), plcId, loadData) 
        {}


        public bool LoadAllData()
        {
            var readAi = LoadAiSheet();
            var readAo = LoadAoSheet();
            var readDio = LoadDioSheet();
            
            var updateScale = false;
            if(readAi && readAo)
                updateScale = UpdateAoScale();

            var readMessage = LoadMessagesSheet();
            
            return readAi && readAo && readDio;
        }
        public bool LoadAiSheet(string sheetName = "AI")
        {
            try
            {
                _aiList = LoadSheetDataList(sheetName, row => row.ToAiPosition(PlcId));
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool LoadAoSheet(string sheetName = "AO")
        {
            try
            {
                _aoList = LoadSheetDataList(sheetName, row => row.ToAoPosition(PlcId));
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool LoadDioSheet(string sheetName = "DIO")
        {
            try
            {
                _dioList = LoadSheetDataList(sheetName, row => row.ToDioPosition(PlcId));
            }
            catch
            {
                return false;
            }
            
            return true;
        }
        public bool LoadMessagesSheet(string sheetName = "Message")
        {
            try
            {
                _plcMessageList = LoadSheetDataList(sheetName, row => row.ToPlcMessage(PlcId));
            }
            catch
            {
                return false;
            }
            return true;
        }
        
        public bool UpdateAoScale()
        {
            if (_aiList == null || _aoList == null || AiPositions == null)
                return false;

            foreach (var aoPos in _aoList)
            {
                var aiNum = aoPos.AiNum;
                if (aiNum == null ||  !AiPositions.ContainsKey(aiNum.Value))
                {
                    aoPos.Scale = new RangePair { Low = null, High = null };
                    continue;
                }
                aoPos.Scale = AiPositions[aiNum.Value].Scale;
            }
            return true;
        }


        /*
        protected static void UpdatePlcId<T>(IDictionary<int, T> @dick, int plcid) where T : IPlcIdPosition
        {
            if (@dick == null)
                return;
            UpdatePlcId(@dick.Values, plcid);
        }
         * */

        public IDictionary<int, AiPosition> AiPositions
        {
            get { return GetDict(_aiList); }
        }

        public IDictionary<int, AoPosition> AoPositions
        {
            get { return GetDict(_aoList); }
        }

        public IDictionary<int, DioPosition> DioPositions
        {
            get { return GetDict(_dioList); }
        }

        public IDictionary<int, PlcMessage> PlcMessages
        {
            get { return GetDict(_plcMessageList); }
        }

        public IList<Position> AllPositions
        {
            get
            {
                if (_aiList == null && _dioList == null && _aoList == null)
                    return null;

                var list = new List<Position>();
                if (_aiList != null)
                    list.AddRange(_aiList);
                if (_dioList != null)
                    list.AddRange(_dioList);
                if (_aoList != null)
                    list.AddRange(_aoList);
                return list;
            }
        }



        private IEnumerable<T> LoadSheetDataList<T>(string sheetName, Func<DataRow, T> func) where T : IPlcIdPosition
        {
            try
            {
                var list = ExcelDataBase.GetDataList(sheetName, func)
                                        .OrderBy(x => x.PlcId).ThenBy(x => x.Number);
                UpdatePlcId(list, PlcId);
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private static Dictionary<int, T> GetDict<T>(IEnumerable<T> list) where T : IPlcIdPosition
        {
            if (list == null)
                return null;
            if (GetDuplicates(list).Any())
                return null;
            return list.ToDictionary(x => x.Number, y => y);
        }


        private static void UpdatePlcId<T>(IEnumerable<T> @list, int plcid) where T : IPlcIdPosition
        {
            if (list == null)
                return;
            foreach (var x in list)
                x.PlcId = plcid;
        }


        private static IEnumerable<IGrouping<int, T>> GetDuplicates<T>(IEnumerable<T> list) where T : IPlcIdPosition
        {
            return list.GroupBy(x => x.Number).Where(x => x.Count() > 1);
        }

        private static bool HasDuplicates<T>(IEnumerable<T> list) where T : IPlcIdPosition
        {
            if (list == null)
                return false;
            return GetDuplicates(list).Any();
        }
    }
}
