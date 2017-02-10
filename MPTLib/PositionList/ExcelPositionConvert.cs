using System;
using System.Data;
using MPT.PrimitiveType;
using MPT.Model;
using MPT.RSView;

namespace MPT.Positions
{
    public static class ExcelPositionConvert
    {
        public enum AiPosition_ExcelColumnNumber
        {
            Number = 3,
            Name = 4,
            Description = 5,
            
            ScaleLow = 6,
            ScaleHigh = 7,
            
            Dimension = 8,
            
            ReglamentLow = 9,
            ReglamentHigh = 10,
            
            AlarmLow = 11,
            AlarmHigh = 12,

            BlockLow = 13,
            BlockHigh = 14,
            
            Note = 17,
            GroupId = 20,
        }
        
        public static AiPosition ToAiPosition(this DataRow row, int plcid = 0)
        {
            if (row == null)
                return null;

            try
            {
                var number = row.Field<object>((int)AiPosition_ExcelColumnNumber.Number).ToNullNumeric<int>();
                var name = row.Field<object>((int)AiPosition_ExcelColumnNumber.Name).ToNullString();
                if (number == null || string.IsNullOrWhiteSpace(name))
                    return null;

                var aiPosition = new AiPosition();
                aiPosition.PlcId = plcid;
                aiPosition.Number = number.Value;
                aiPosition.Name = name;
                aiPosition.Description = row.Field<object>((int) AiPosition_ExcelColumnNumber.Description).ToNullString();
                aiPosition.Units = row.Field<object>((int) AiPosition_ExcelColumnNumber.Dimension).ToNullString();
                aiPosition.Note = row.Field<object>((int) AiPosition_ExcelColumnNumber.Note).ToNullString();
                aiPosition.GroupId = row.Field<object>((int) AiPosition_ExcelColumnNumber.GroupId).ToNullNumeric<int>();
                aiPosition.Scale = new RangePair(
                    row.Field<object>((int) AiPosition_ExcelColumnNumber.ScaleLow).ToNullNumeric<double>(),
                    row.Field<object>((int) AiPosition_ExcelColumnNumber.ScaleHigh).ToNullNumeric<double>()
                    );
                aiPosition.Reglament = new RangePair(
                    row.Field<object>((int) AiPosition_ExcelColumnNumber.ReglamentLow).ToNullNumeric<double>(),
                    row.Field<object>((int) AiPosition_ExcelColumnNumber.ReglamentHigh).ToNullNumeric<double>()
                    );
                aiPosition.Alarming = new RangePair(
                    row.Field<object>((int) AiPosition_ExcelColumnNumber.AlarmLow).ToNullNumeric<double>(),
                    row.Field<object>((int) AiPosition_ExcelColumnNumber.AlarmHigh).ToNullNumeric<double>()
                    );
                aiPosition.Blocking = new RangePair(
                    (row.Field<object>((int)AiPosition_ExcelColumnNumber.BlockLow).ToNullNumeric<double>()),
                    (row.Field<object>((int)AiPosition_ExcelColumnNumber.BlockHigh).ToNullNumeric<double>()));

                return aiPosition;
            }
            catch (Exception)
            {                
                return null;
            }
        }
        
        
        
        public enum AoPosition_ExcelFieldNumber
        {
            Number = 3,
            Name = 4,
            Description = 5,

            AiNum = 6,
            AoType = 7,
            Note = 8,
            GroupId = 9,
            IsCascade = 10,
            IsCascadeSlave = 11,
            MasterNum = 12,
        }

        public static AoPosition ToAoPosition(this DataRow row, int plcid =0)
        {
            if (row == null)
                return null;

            try
            {
                var number = row.Field<object>((int)AoPosition_ExcelFieldNumber.Number).ToNullNumeric<int>();
                var name = row.Field<object>((int)AoPosition_ExcelFieldNumber.Name).ToNullString();
                if (number == null || string.IsNullOrWhiteSpace(name))
                    return null;

                var aoPosition = new AoPosition
                {
                    PlcId = plcid,
                    Number = number.Value,
                    Name = name,
                    Description = row.Field<object>((int)AoPosition_ExcelFieldNumber.Description).ToNullString(),
                    AiNum = (row.Field<object>((int) AoPosition_ExcelFieldNumber.AiNum)).ToNullNumeric<int>(),
                    Note = (row.Field<object>((int)AoPosition_ExcelFieldNumber.Note)).ToNullString(),
                    GroupId = row.Field<object>((int) AoPosition_ExcelFieldNumber.GroupId).ToNullNumeric<int>(),
                    AoType = row.Field<object>((int)AoPosition_ExcelFieldNumber.AoType).ToNullString().OnlyLetterAndDigit().ToEnum<AoPosition.AoTypeEnum>(),
                };
                
                return aoPosition;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        

        private enum DioPosition_ExcelFieldNumber
        {
            Number = 3,
            Name = 4,
            Description = 5,
            AlarmText = 6,
            NormValue = 7,
            IsAlarm = 8,
            GroupId = 11,
        }

        public static DioPosition ToDioPosition(this DataRow row, int plcid = 0)
        {
            if (row == null)
                return null;

            try
            {
                var number = row.Field<object>((int)DioPosition_ExcelFieldNumber.Number).ToNullNumeric<int>();
                var name = row.Field<object>((int)DioPosition_ExcelFieldNumber.Name).ToString();
                if (number == null || string.IsNullOrWhiteSpace(name))
                    return null;

                var dioPosition = new DioPosition
                {
                    PlcId = plcid,
                    Number =        number.Value,
                    Name =          name,
                    Description =   row.Field<object>((int)DioPosition_ExcelFieldNumber.Description).ToNullString(),
                    AlarmText =     row.Field<object>((int)DioPosition_ExcelFieldNumber.AlarmText).ToNullString(),
                    NormValue =     row.Field<object>((int)DioPosition_ExcelFieldNumber.NormValue).ToNullNumeric<bool>() ?? false,
                    IsAlarm =       row.Field<object>((int)DioPosition_ExcelFieldNumber.IsAlarm).ToNullNumeric<bool>() ?? false,
                    GroupId =       row.Field<object>((int)DioPosition_ExcelFieldNumber.GroupId).ToNullNumeric<int>(),
                };
                return dioPosition;
            }
            catch (Exception)
            {
                return null;
            }
        }

        

        private enum MessagePosition_ExcelFieldNumber
        {
            Number = 3,
            Text = 4,
            Group = 5,
            Severity = 6,
        }

        public static PlcMessage ToPlcMessage(this DataRow row, int plcid = 0)
        {
            if (row == null) return null;

            try
            {
                var number = row.Field<object>((int) MessagePosition_ExcelFieldNumber.Number).ToNullNumeric<int>();
                var text = row.Field<object>((int)MessagePosition_ExcelFieldNumber.Text).ToNullString();
                if (number == null || string.IsNullOrWhiteSpace(text))
                    return null;

                short? group = null;
                try
                {
                    group = row.Field<object>((int) MessagePosition_ExcelFieldNumber.Group).ToNullNumeric<short>();
                }
                catch {}

                int? severity = null;
                try
                {
                    severity = row.Field<object>((int)MessagePosition_ExcelFieldNumber.Severity).ToNullNumeric<int>();
                }
                catch {}


                var plcMessage = new PlcMessage
                {
                    PlcId = plcid,
                    Number = number.Value,
                    Text = text,
                    Group = group,
                    Severity = severity,
                };
                return plcMessage;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}