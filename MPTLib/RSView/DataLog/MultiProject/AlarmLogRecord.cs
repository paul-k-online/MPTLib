using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.RSView.DataLog.MultiProject
{
    public class AlarmLogRecord : AlarmLog.AlarmLogRecord, IProjectId
    {
        public int ProjectId { get; set; }

        public static AlarmLogRecord FromBase(AlarmLog.AlarmLogRecord baseClass, int projectId)
        {
            return new AlarmLogRecord()
            {
                AlarmType = baseClass.AlarmType,
                DateTime = baseClass.DateTime,
                DestinationFlag = baseClass.DestinationFlag,
                LoggingStation = baseClass.LoggingStation,
                Severity = baseClass.Severity,
                TagName = baseClass.TagName,
                TagType = baseClass.TagType,
                TagValue = baseClass.TagValue,
                ThreshLabel = baseClass.ThreshLabel,
                ThreshNumber = baseClass.ThreshNumber,
                ThreshValue = baseClass.ThreshValue,
                TransType = baseClass.TransType,
                UserId = baseClass.UserId,
                UserStation = baseClass.UserStation,

                ProjectId = projectId,
            };
        }
    }
}
