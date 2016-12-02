using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Runtime.InteropServices;
using MPT.RSView.ImportExport.Csv.Tag;

namespace MPT.RSView.ImportExport.Csv
{
    public static class CsvConvertExtensions
    {
        public static string ToRsViewFormat(this object value)
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
            var b = a.ToRsViewFormat();
            return b;
        }


        public static CsvTag ToCsvTag(this RsViewAnalogTag tag)
        {
            var csvTag = CsvTag.CreateAnalog(tag.FullName, tag.Description, tag.Min, tag.Max, tag.Units, tag.InitialValue);
            if (!tag.IsMemoryDataSource)
            {
                csvTag.SetDataSource(tag.NodeName, tag.Address);
            }
            return csvTag;
        }


        public static CsvTag ToCsvTag(this RsViewDigitalTag tag)
        {
            var csvTag = CsvTag.CreateDigit(tag.FullName, tag.Description, tag.InitialValue);
            if (!tag.IsMemoryDataSource)
            {
                csvTag.SetDataSource(tag.NodeName, tag.Address);
            }
            return csvTag;
        }


        public static CsvTag ToCsvTag(this RsViewStringTag tag)
        {
            var csvTag = CsvTag.CreateString(tag.FullName, tag.Description, tag.InitialValue);
            if (!tag.IsMemoryDataSource)
            {
                csvTag.SetDataSource(tag.NodeName, tag.Address);
            }
            return csvTag;
        }


        public static CsvTag ToCsvTag(this RsViewTag tag)
        {
            var analogTag = tag as RsViewAnalogTag;
            if (analogTag != null)
                return analogTag.ToCsvTag();

            var digitalTag = tag as RsViewDigitalTag;
            if (digitalTag != null)
                return digitalTag.ToCsvTag();

            var stringTag = tag as RsViewStringTag;
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (stringTag != null)
                return stringTag.ToCsvTag();

            return CsvTag.CreateFolder(tag.FullName);
        }


        public static CsvAnalogAlarmTreshold ToCsvAnalogAlarmTreshold(this RsViewAnalogTag.RsViewAnalogAlarm analogAlarm)
        {
            if (analogAlarm == null)
                return new CsvAnalogAlarmTreshold();

            return new CsvAnalogAlarmTreshold(analogAlarm.Threshold, analogAlarm.Label, analogAlarm.Direction, analogAlarm.Severity);
        }


        public static CsvAnalogAlarm ToCsvAnalogAlarm(this RsViewAnalogTag tag)
        {
            if (!tag.IsAlarm)
                return null;
            
            var csvAnalogAlarm = new CsvAnalogAlarm(tag.FullName);
            for (var i = 1; i <= 8; i++)
            {
                if (tag.Alarm[i] != null)
                    csvAnalogAlarm.Tresholds[i] = tag.Alarm[i].ToCsvAnalogAlarmTreshold();
            }
            return csvAnalogAlarm;
        }


        public static CsvDigitalAlarm ToCsvDigitalAlarm(this RsViewDigitalTag tag)
        {
            if (!tag.IsAlarm)
                return null;

            var csvDigitalAlarm = new CsvDigitalAlarm(tag.FullName)
            {
                Label = tag.Alarm.Label,
                SeverityValue = tag.Alarm.Severity,
                Type = tag.Alarm.Type
            };

            return csvDigitalAlarm;
        }

    }
}