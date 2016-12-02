using System;
using System.Data;
using MPT.Model;

namespace MPT.PositionList
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
            GroupId = 19
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

                var ai = new AiPosition();
                ai.Number = Convert.ToInt32(number);
                ai.Name = name;

                ai.Description = row.Field<string>((int)AiExcelField.Description);

                ai.Scale = new AiPosition.AlarmPair(
                    row.Field<double?>((int)AiExcelField.ScaleLow),
                    row.Field<double?>((int)AiExcelField.ScaleHigh));

                ai.Units = row.Field<string>((int)AiExcelField.Dimension);

                ai.Reglament = new AiPosition.AlarmPair(
                    row.Field<double?>((int)AiExcelField.ReglamentLow),
                    row.Field<double?>((int)AiExcelField.ReglamentHigh));

                ai.Alarming = new AiPosition.AlarmPair(
                    row.Field<double?>((int)AiExcelField.AlarmLow),
                    row.Field<double?>((int)AiExcelField.AlarmHigh));

                ai.Blocking = new AiPosition.AlarmPair(
                    row.Field<double?>((int)AiExcelField.BlockLow),
                    row.Field<double?>((int)AiExcelField.BlockHigh));

                ai.Note = row.Field<string>((int)AiExcelField.Note);

                ai.GroupId = Convert.ToInt32(row.Field<double?>((int)AiExcelField.GroupId));
                return ai;
            }
            catch
            {
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
                var number = row.Field<double?>((int)DioExcelField.Number);
                var name = row.Field<string>((int)DioExcelField.Name);
                if (number == null || string.IsNullOrWhiteSpace(name))
                    return null;

                var dioPosition = new DioPosition()
                                  {
                                      Number = Convert.ToInt32(number),
                                      Name = name,
                                      Description = row.Field<string>((int)DioExcelField.Description),
                                      AlarmText = row.Field<string>((int)DioExcelField.AlarmText),
                                      NormValue = Convert.ToBoolean(row.Field<string>((int)DioExcelField.NormValue)),
                                      IsAlarm = Convert.ToBoolean(row.Field<string>((int)DioExcelField.IsAlarm)),
                                      GroupId = Convert.ToInt32(row.Field<double?>((int)DioExcelField.GroupId)),
                                  };
                return dioPosition;
            }
            catch
            {
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
                var number = row.Field<double?>((int)MessageExcelField.Number);
                var text = row.Field<string>((int)MessageExcelField.Text);
                if (number == null || string.IsNullOrWhiteSpace(text))
                    return null;
                
                var plcMessage = new PlcMessage()
                                 {
                                     Number = Convert.ToInt32(number),
                                     Text = text,
                                 };
                return plcMessage;
            }
            catch
            {
                return null;
            }
        }
    }
}
