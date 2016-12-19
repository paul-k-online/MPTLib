using System;
using System.Globalization;

namespace MPT.RSView.ImportExport.Csv
{
    public static class RSViewToCsvTag
    {
        public static string ToCsvString(this object value)
        {           
            if (value == null)
                return "";
            
            if (value is string)
                return string.Format("\"{0}\"", value);

            if (value is double)
                return Convert.ToString(value,CultureInfo.InvariantCulture.NumberFormat);

            if (value is int)
                return value.ToString();


            if (value is CsvAlarmMessage)
                return value.ToString();

            if (value is CsvAnalogAlarmTreshold)
                return value.ToString();
            
            var a = value.ToString();
            var b = a.ToCsvString();
            return b;
        }

        public static CsvTag ToCsvTag(this RSViewAnalogTag tag)
        {
            var csvTag = CsvTag.CreateAnalog(tag.Name, tag.Description, tag.Min, tag.Max, tag.Units, tag.InitialValue);
            if (!tag.IsMemoryDataSourceType)
            {
                csvTag.SetDataSource(tag.NodeName, tag.Address);
            }
            return csvTag;
        }

        public static CsvTag ToCsvTag(this RSViewDigitalTag tag)
        {
            var csvTag = CsvTag.CreateDigit(tag.Name, tag.Description, tag.InitialValue);
            if (!tag.IsMemoryDataSourceType)
            {
                csvTag.SetDataSource(tag.NodeName, tag.Address);
            }
            return csvTag;
        }

        public static CsvTag ToCsvTag(this RSViewStringTag tag)
        {
            var csvTag = CsvTag.CreateString(tag.Name, tag.Description, tag.InitialValue);
            if (!tag.IsMemoryDataSourceType)
            {
                csvTag.SetDataSource(tag.NodeName, tag.Address);
            }
            return csvTag;
        }

        public static CsvTag ToCsvTag(this RSViewTag tag)
        {
            var analogTag = tag as RSViewAnalogTag;
            if (analogTag != null)
                return analogTag.ToCsvTag();

            var digitalTag = tag as RSViewDigitalTag;
            if (digitalTag != null)
                return digitalTag.ToCsvTag();

            var stringTag = tag as RSViewStringTag;
            if (stringTag != null)
                return stringTag.ToCsvTag();

            return CsvTag.CreateFolder(tag.Name);
        }

        public static CsvAnalogAlarmTreshold ToCsvAnalogAlarmTreshold(this RSViewAnalogTag.RsViewAnalogAlarm analogAlarm)
        {
            return analogAlarm == null ? 
                new CsvAnalogAlarmTreshold() : 
                new CsvAnalogAlarmTreshold(analogAlarm.Threshold, analogAlarm.Label, analogAlarm.Direction, analogAlarm.Severity);
        }

        public static CsvAnalogAlarm ToCsvAnalogAlarm(this RSViewAnalogTag tag)
        {
            if (!tag.IsAlarm)
                return null;

            var csvAnalogAlarm = new CsvAnalogAlarm(tag.Name, tag.Alarm);
            /*
            for (var i = 1; i <= 8; i++)
            {
                if (tag.Alarm[i] == null) 
                    continue;
                csvAnalogAlarm.Tresholds[i] = tag.Alarm[i].ToCsvAnalogAlarmTreshold();
            }
            */

            return csvAnalogAlarm;
        }

        public static CsvDigitalAlarm ToCsvDigitalAlarm(this RSViewDigitalTag tag)
        {
            if (!tag.IsAlarm)
                return null;

            var csvDigitalAlarm = new CsvDigitalAlarm(tag.Name)
            {
                Label = tag.Alarm.Label,
                SeverityValue = tag.Alarm.Severity,
                Type = tag.Alarm.Type
            };

            return csvDigitalAlarm;
        }
    }
}