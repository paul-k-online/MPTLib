using System;
using System.Globalization;

namespace MPT.RSView.ImportExport.Csv
{
    public static class CsvTagExtension
    {
        public static string ToCsvString(this object value)
        {           
            if (value == null)
                return "";
            
            if (value is string)
                return string.Format("\"{0}\"", value);

            if (value is double)
                return Convert.ToString(value, CultureInfo.InvariantCulture.NumberFormat);

            if (value is int)
                return ((int)value).ToString();

            if (value is CsvAnalogAlarmMessage)
                return ((CsvAnalogAlarmMessage)value).ToCsvString();


            if (value is CsvAlarmMessage)
                return ((CsvAlarmMessage)value).ToCsvString();

            if (value is CsvAnalogAlarmTreshold)
                return ((CsvAnalogAlarmTreshold)value).ToCsvString();

            return value.ToString().ToCsvString();
        }

        public static CsvTag ToCsvTag(this RSViewAnalogTag tag)
        {
            var csvTag = CsvTag.CreateAnalog(tag.TagPath, tag.Description, tag.Min, tag.Max, tag.Units, tag.InitialValue);
            if (tag.IsDeviceDataSourceType)
                csvTag.SetDataSource(tag.NodeName, tag.Address);
            return csvTag;
        }

        public static CsvTag ToCsvTag(this RSViewDigitalTag tag)
        {
            var csvTag = CsvTag.CreateDigit(tag.TagPath, tag.Description, tag.InitialValue);
            if (tag.IsDeviceDataSourceType)
                csvTag.SetDataSource(tag.NodeName, tag.Address);
            return csvTag;
        }

        public static CsvTag ToCsvTag(this RSViewStringTag tag)
        {
            var csvTag = CsvTag.CreateString(tag.TagPath, tag.Description, tag.InitialValue);
            if (tag.IsDeviceDataSourceType)
                csvTag.SetDataSource(tag.NodeName, tag.Address);
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

            return CsvTag.CreateFolder(tag.TagPath);
        }

        public static CsvAnalogAlarmTreshold ToCsvAnalogAlarmTreshold(this RSViewAnalogAlarm analogAlarm)
        {
            return analogAlarm == null ? 
                new CsvAnalogAlarmTreshold() : 
                new CsvAnalogAlarmTreshold(analogAlarm.Threshold, analogAlarm.Label, analogAlarm.Direction, analogAlarm.Severity);
        }

        public static CsvAnalogAlarm ToCsvAnalogAlarm(this RSViewAnalogTag tag)
        {
            if (!tag.HasAlarm)
                return null;
            var csvAnalogAlarm = new CsvAnalogAlarm(tag.TagPath, tag.Alarms);
            return csvAnalogAlarm;
        }

        public static CsvDigitalAlarm GetCsvDigitalAlarm(this RSViewDigitalTag tag)
        {
            if (!tag.HasAlarm)
                return null;
            return new CsvDigitalAlarm(tag.TagPath, tag.Alarm.Label, tag.Alarm.Severity, tag.Alarm.Type);
        }
    }
}