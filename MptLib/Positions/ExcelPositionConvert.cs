using System;
using System.Data;
using MPT.BaseTypeHelpers;
using MPT.Model;
using MPT.RSView;

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
            GroupId = 20,
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
                var number = row.Field<object>((int)AiExcelField.Number);
                var name = row.Field<object>((int)AiExcelField.Name);
                if (number == null || name == null)
                    return null;

                // ReSharper disable once UseObjectOrCollectionInitializer
                var ai = new AiPosition();
                ai.Number = Convert.ToInt32(number);
                ai.Name = Convert.ToString(name);
                ai.Description = Convert.ToString(row.Field<object>((int)AiExcelField.Description));
                ai.Units = Convert.ToString(row.Field<object>((int)AiExcelField.Dimension));

                ai.Scale = new RangePair
                {
                    Low = Convert.ToDouble(row.Field<object>((int)AiExcelField.ScaleLow)),
                    High = Convert.ToDouble(row.Field<object>((int)AiExcelField.ScaleHigh))
                };
                ai.Reglament = new RangePair()
                {
                    Low = Convert.ToDouble(row.Field<object>((int)AiExcelField.ReglamentLow)),
                    High = Convert.ToDouble(row.Field<object>((int)AiExcelField.ReglamentHigh))
                };
                ai.Alarming = new RangePair
                {
                    Low = Convert.ToDouble(row.Field<object>((int)AiExcelField.AlarmLow)),
                    High = Convert.ToDouble(row.Field<object>((int)AiExcelField.AlarmHigh))
                };
                ai.Blocking = new RangePair
                {
                    Low = Convert.ToDouble(row.Field<object>((int)AiExcelField.BlockLow)),
                    High = Convert.ToDouble(row.Field<object>((int)AiExcelField.BlockHigh))
                };

                ai.Note = Convert.ToString(row.Field<object>((int)AiExcelField.Note));
                ai.GroupId = Convert.ToInt32(row.Field<object>((int)AiExcelField.GroupId));
                return ai;
            }
            catch (Exception e)
            {                
                return null;
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

            try
            {
                var number = row.Field<object>((int)AoExcelField.Number);
                var name = row.Field<object>((int)AoExcelField.Name);
                if (number == null || name == null)
                    return null;

                // ReSharper disable once UseObjectOrCollectionInitializer
                var aoPosition = new AoPosition();
                aoPosition.Number = Convert.ToInt32(number);
                aoPosition.Name = Convert.ToString(name);
                aoPosition.Description = Convert.ToString(row.Field<object>((int)AoExcelField.Description));

                try
                {
                    aoPosition.AiNum = Convert.ToInt32(row.Field<object>((int)AoExcelField.AiNum));
                }
                catch (Exception)
                {
                    aoPosition.AiNum = null;
                }

                try
                {
                    var aoTypeString = Convert.ToString(row.Field<object>((int)AoExcelField.AoType));
                    aoPosition.AoType = aoTypeString.OnlyLetterAndDigit().ToEnum<AoPosition.AoTypeEnum>();
                }
                catch
                {
                    aoPosition.AoType = default(AoPosition.AoTypeEnum);
                }

                aoPosition.Note = Convert.ToString(row.Field<object>((int)AoExcelField.Note));
                aoPosition.GroupId = Convert.ToInt32(row.Field<object>((int)AoExcelField.GroupId));
                return aoPosition;
            }
            catch (Exception e)
            {
                //throw new Exception("ToAoPosition", e);
                return null;
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
                var number = row.Field<object>((int)DioExcelField.Number);
                var name = row.Field<object>((int)DioExcelField.Name);
                if (number == null || name == null)
                    return null;

                var dioPosition = new DioPosition
                {
                    Number =        Convert.ToInt32(number),
                    Name =          Convert.ToString(name),
                    Description =   Convert.ToString(row.Field<object>((int) DioExcelField.Description)),
                    AlarmText =     Convert.ToString(row.Field<object>((int) DioExcelField.AlarmText)),
                    NormValue =     Convert.ToBoolean(row.Field<object>((int) DioExcelField.NormValue)),
                    IsAlarm =       Convert.ToBoolean(row.Field<object>((int) DioExcelField.IsAlarm)),
                    GroupId =       Convert.ToInt32(row.Field<object>((int) DioExcelField.GroupId))
                };
                return dioPosition;
            }
            catch (Exception e)
            {
                //throw new Exception("ToDioPosition", e);
                return null;
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
                var number = row.Field<object>((int) MessageExcelField.Number);
                var text = row.Field<object>((int) MessageExcelField.Text);
                if (number == null || text == null)
                    return null;

                var plcMessage = new PlcMessage()
                {
                    Number = Convert.ToInt32(number),
                    Text = Convert.ToString(text),
                };
                return plcMessage;
            }
            catch (Exception e)
            {
                //throw new Exception("ToPlcMessage", e);
                return null;
            }
        }
    }
}