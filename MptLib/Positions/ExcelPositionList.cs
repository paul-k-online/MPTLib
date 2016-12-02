using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using MPT.Excel;
using MPT.Model;
using MPT.RSView.ImportExport;
using NLog;

namespace MPT.Positions
{
    public class ExcelPositionList : PositionList   
    {
        public ExcelDataBase Excel { get; set; }

        private int? _plcId;
        public int? PlcId
        {
            get { return _plcId; }
            set 
            {
                _plcId = value;
                if (PlcId == null) return;
                UpdateAiListPlcId(PlcId.Value);
                UpdateAoListPlcId(PlcId.Value);
                UpdateDioListPlcId(PlcId.Value);
                UpdateMessageListPlcId(PlcId.Value);
            }
        }

        public ExcelPositionList(ExcelDataBase excel, int? plcId = null, bool loadData = false)
        {
            Excel = excel;
            PlcId = plcId;
            AiPositions = new Dictionary<int, AiPosition>();
            AoPositions = new Dictionary<int, AoPosition>();
            DioPositions = new Dictionary<int, DioPosition>();
            PlcMessages = new Dictionary<int, PlcMessage>();

            if (loadData)
            {
                LoadAllData();
            }
        }

        
        public ExcelPositionList(string excelFile, int? plcId = null, bool loadData = false)
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
                AiPositions = Excel
                    .GetDataList(sheetName, ExcelPositionConvert.ToAiPosition)
                    .ToDictionary(x => x.Number, y => y);
            }
            catch (Exception)
            {
                return false;
            }
        
            if (PlcId != null)
                UpdateAiListPlcId(PlcId.Value);
            return true;
        }

        public bool LoadAoSheet(string sheetName = "AO")
        {
            try
            {
                AoPositions = Excel.GetDataList(sheetName, ExcelPositionConvert.ToAoPosition)
                    .ToDictionary(x => x.Number, y => y);
            }
            catch
            {
                return false;
            }

            if (PlcId != null)
                UpdateAoListPlcId(PlcId.Value);
            return true;
        }

        public bool LoadDioSheet(string sheetName = "DIO")
        {
            try
            {
                DioPositions = Excel.GetDataList(sheetName, ExcelPositionConvert.ToDioPosition)
                    .ToDictionary(x => x.Number, y => y);
            }
            catch
            {
                return false;
            }
            
            if (PlcId != null) 
                UpdateDioListPlcId(PlcId.Value);
            return true;
        }

        public bool LoadMessagesSheet(string sheetName = "Message")
        {
            try
            {
                PlcMessages = Excel.GetDataList(sheetName, ExcelPositionConvert.ToPlcMessage)
                    .ToDictionary(x => x.Number, y => y);
            }
            catch
            {
                return false;
            }

            if (PlcId != null) 
                UpdateMessageListPlcId(PlcId.Value);
            return true;
        }

        public bool UpdateAoScale()
        {
            if (AiPositions == null) return false;

            foreach (var aoPosition in AoPositions)
            {
                var aiNum = aoPosition.Value.AiNum;
                if (aiNum == null || !AiPositions.ContainsKey(aiNum.Value))
                {
                    aoPosition.Value.Scale = new RangePair { Low = null, High = null };
                    continue;
                }

                var scale = AiPositions[aiNum.Value].Scale;
                aoPosition.Value.Scale = scale;
            }
            return true;
        }

        protected void UpdateAiListPlcId(int plcId)
        {
            if (AiPositions == null) return;

            foreach (var position in AiPositions)
            {
                position.Value.PlcId = plcId;
            }
        }

        protected void UpdateAoListPlcId(int plcId)
        {
            if(AoPositions == null) return;

            foreach (var position in AoPositions)
            {
                position.Value.PlcId = plcId;
            }
        }

        protected void UpdateDioListPlcId(int plcId)
        {
            if(DioPositions == null) return;

            foreach (var position in DioPositions)
            {
                position.Value.PlcId = plcId;
            }
        }

        protected void UpdateMessageListPlcId(int plcId)
        {
            if (PlcMessages == null) return;
            
            foreach (var position in PlcMessages)
            {
                position.Value.PlcId = plcId;
            }
        }
    }
}
