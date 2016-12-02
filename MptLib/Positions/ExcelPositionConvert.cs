using System;
using System.Data;
using MPT.Model;
using MPT.RSView;
using MPT.Strings;

namespace MPT.Positions
{
    public static class ExcelPositionConvert
    {
        public enum AiExcelField
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
            GroupId = 19,
        }

        public static AiPosition ToAiPosition(this DataRow row)
        {
            if (row == null)
                return null;

            /*
            var maxIndex = (int)Enum.GetValues(typeof(AiField)).Cast<AiField>().Max();

            if (row.ItemArray.Length < maxIndex)
                throw new IndexOutOfRangeException("row length");
            */

            try
            {
                var number = row.Field<double?>((int)AiExcelField.Number);
                var name = row.Field<string>((int)AiExcelField.Name);
                if (number == null || string.IsNullOrWhiteSpace(name))
                    return null;

                // ReSharper disable once UseObjectOrCollectionInitializer
                var ai = new AiPosition();
                ai.Number = Convert.ToInt32(number);
                ai.Name = name;

                ai.Description = row.Field<string>((int) AiExcelField.Description);
                ai.Units = row.Field<string>((int)AiExcelField.Dimension);

                ai.Scale = new RangePair
                {
                    Low = row.Field<double?>((int) AiExcelField.ScaleLow),
                    High = row.Field<double?>((int) AiExcelField.ScaleHigh)
                };
                ai.Reglament = new RangePair()
                {
                    Low = row.Field<double?>((int) AiExcelField.ReglamentLow),
                    High = row.Field<double?>((int) AiExcelField.ReglamentHigh)
                };
                ai.Alarming = new RangePair
                {
                    Low = row.Field<double?>((int) AiExcelField.AlarmLow),
                    High = row.Field<double?>((int) AiExcelField.AlarmHigh)
                };
                ai.Blocking = new RangePair
                {
                    Low = row.Field<double?>((int) AiExcelField.BlockLow),
                    High = row.Field<double?>((int) AiExcelField.BlockHigh)
                };

                ai.Note = row.Field<string>((int)AiExcelField.Note);

                ai.GroupId = Convert.ToInt32(row.Field<double?>((int)AiExcelField.GroupId));
                return ai;
            }
            catch (Exception e)
            {
                throw new Exception("ToAiPosition", e);
            }
        }
        

        public enum AoExcelField
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

        public static AoPosition ToAoPosition(this DataRow row)
        {
            if (row == null)
                return null;

            /*
            var maxIndex = (int)Enum.GetValues(typeof(AiField)).Cast<AiField>().Max();

            if (row.ItemArray.Length < maxIndex)
                throw new IndexOutOfRangeException("row length");
            */

            try
            {
                var number = row.Field<double?>((int)AoExcelField.Number);
                var name = row.Field<string>((int)AoExcelField.Name);
                if (number == null || string.IsNullOrWhiteSpace(name))
                    return null;

                // ReSharper disable once UseObjectOrCollectionInitializer
                var aoPosition = new AoPosition();
                aoPosition.Number = Convert.ToInt32(number);
                aoPosition.Name = name;
                aoPosition.Description = row.Field<string>((int)AoExcelField.Description);

                try
                {
                    var aiNumStr = row.Field<double?>((int)AoExcelField.AiNum);
                    aoPosition.AiNum = Convert.ToInt32(aiNumStr);
                }
                catch (Exception)
                {
                    aoPosition.AiNum = null;
                }

                try
                {
                    var aoTypeString = row.Field<string>((int)AoExcelField.AoType);
                    aoPosition.AoType = aoTypeString.OnlyLetterAndDigit().ToEnum<AoPosition.AoTypeEnum>();
                }
                catch
                {
                    aoPosition.AoType = default(AoPosition.AoTypeEnum);
                }
                
                aoPosition.Note = row.Field<string>((int)AoExcelField.Note);

                try
                {
                    aoPosition.GroupId = Convert.ToInt32(row.Field<double?>((int)AoExcelField.GroupId));
                }
                catch (Exception)
                {
                    aoPosition.GroupId = 0;
                }
                
                return aoPosition;
            }
            catch (Exception e)
            {
                throw new Exception("ToAoPosition", e);
            }
        }
        


        private enum DioExcelField
        {
            Number = 3,
            Name = 4,
            Description = 5,
            AlarmText = 6,
            NormValue = 7,
            IsAlarm = 8,
            GroupId = 11,
        }

        public static DioPosition ToDioPosition(this DataRow row)
        {
            if (row == null)
                return null;

            try
            {
                var number = row.Field<double?>((int)DioExcelField.Number);
                var name = row.Field<string>((int)DioExcelField.Name);
                if (number == null || string.IsNullOrWhiteSpace(name))
                    return null;

                // ReSharper disable once UseObjectOrCollectionInitializer
                var dioPosition = new DioPosition();
                dioPosition.Number = Convert.ToInt32(number);
                dioPosition.Name = name;
                dioPosition.Description = row.Field<string>((int)DioExcelField.Description);
                dioPosition.AlarmText = row.Field<string>((int)DioExcelField.AlarmText);
                dioPosition.NormValue = Convert.ToBoolean(row.Field<double?>((int)DioExcelField.NormValue));
                dioPosition.IsAlarm = Convert.ToBoolean(row.Field<double?>((int)DioExcelField.IsAlarm));
                dioPosition.GroupId = Convert.ToInt32(row.Field<double?>((int)DioExcelField.GroupId));
                return dioPosition;
            }
            catch (Exception e)
            {
                throw new Exception("ToDioPosition", e);
            }
        }



        private enum MessageExcelField
        {
            Number = 3,
            Text = 4
        }

        public static PlcMessage ToPlcMessage(this DataRow row)
        {
            if (row == null)
                return null;

            try
            {
                var number = row.Field<double?>((int) MessageExcelField.Number);
                var text = row.Field<string>((int) MessageExcelField.Text);
                if (number == null || string.IsNullOrWhiteSpace(text))
                    return null;

                var plcMessage = new PlcMessage()
                {
                    Number = Convert.ToInt32(number),
                    Text = text,
                };
                return plcMessage;
            }
            catch (Exception e)
            {
                throw new Exception("ToPlcMessage", e);
            }
        }
    }
}