using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static MPT.RSView.RSViewAnalogAlarm;

namespace MPT.RSView.ImportExport.Csv
{
    public class CsvAnalogAlarmMessage : CsvAlarmMessage
    {
        public new string ToCsvString()
        {
            var fields = new List<object>()
            {
                FileMessage,
                PrinterMessage,
                Source,
            };
            return string.Join(",", fields.Select(x => x.ToCsvString()));
        }
    }


    /*
    Thresh1 type, Threshold, Label,                   MessageSource, File msg, Printer msg, Direction,  Severity, 
    Thresh2 type, Threshold, Label,                   MessageSource, File msg, Printer msg, Direction,  Severity,    
    Thresh3 type, Threshold, Label,                   MessageSource, File msg, Printer msg, Direction,  Severity,   
    Thresh4 type, Threshold, Label,                   MessageSource, File msg, Printer msg, Direction,  Severity,  
    Thresh5 type, Threshold, Label,                   MessageSource, File msg, Printer msg, Direction,  Severity,   
    Thresh6 type, Threshold, Label,                   MessageSource, File msg, Printer msg, Direction,  Severity,   
    Thresh7 type, Threshold, Label,                   MessageSource, File msg, Printer msg, Direction,  Severity,   
    Thresh8 type, Threshold, Label,                   MessageSource, File msg, Printer msg, Direction,  Severity,

    "C",          "1.5",     "FRCSA1011_1<0.0 (LL)" , "S"          , "",        ""         , "D"      ,  "1"     , 
    "C",          "2.5",     "FRCSA1011_1<1.0 (L)"  , "S"          , "",        "",          "D",        "1",                                
    "C",          "3.5",     "FRCSA1011_1<1.1",       "S",           "",        "",          "D",        "1",                                           
    "C",          "4.5",     "FRCSA1011_1>2.1",       "S",           "",        "",          "I",        "1",                                       
    "C",          "5.5",     "H",                     "S",           "",        "",          "I",        "1",                                                      
    "C",          "6.5",     "HH",                    "S",           "",        "",          "I",        "1",                                                   
    "",           "",        "",                      "S",           "",        "",          "",         "",                       
    "",           "",        "",                      "S",           "",        "",          "",         ""

    "C",          "1.5",     "П-1: Ключ - Закрыт"   , "S"          , "",        ""         , "D"      ,  "1"     , 
    "C",          "1.7",     "П-1: Ключ - Автомат"  , "S"          , "",        "",          "I",        "1",                                 
    "C",          "2.5",     "П-1: Ключ - Открыт"   , "S"          , "",        "",          "I",        "1",    
    "",           "",        "",                      "S",           "",        "",          "",         "",
    "",           "",        "",                      "S",           "",        "",         "",         "",
    "",           "",        "",                      "S",           "",        "",         "",         "",
    "",           "",        "",                      "S",           "",         "",         "",         "",
    "",           "",        "",                      "S",           "",         "",         "",         ""

    "C"         , "1.1"    , "П-1: Режим - Простой" , "S"          , ""      , ""         , "D"      ,  "1"     , 
    "C"         , "1.9"    , "П-1: Режим - В Работе", "S"          , "",        "",         "I",        "1",
    "","","","S","","","","",
    "","","",                   "S","","","","",
    "","","",     "S","","","","",
    "","","",        "S","","","","",
    "","","","S","","","","",
    "","","","S","","","",""
    */
    public class CsvAnalogAlarmTreshold
    {
        public enum CsvThresholdType
        {
            /// <summary>
            /// Checked ?
            /// </summary>
            C
        }

        public bool? Enabled;
        public CsvThresholdType? Type;
        public string Threshold = "";
        public string Label = "";
        public CsvAlarmMessage AlarmMessage = new CsvAlarmMessage();
        public TresholdDirection? Direction;
        public ushort? Severity;

        public CsvAnalogAlarmTreshold(string threshold, string label, TresholdDirection direction, ushort severity = 1, CsvThresholdType type = CsvThresholdType.C)
        {
            Threshold = threshold;
            Label = label;
            Direction = direction;
            Severity = severity;
            Type = type;
        }

        public CsvAnalogAlarmTreshold()
        { }

        public string ToCsvString()
        {
            var fieldList = new List<object>
            {
                Type.ToString(),
                Threshold,
                Label,
                AlarmMessage,
                Direction.ToString(),
                Severity.ToString(),
            };

            var slist = fieldList.Select(x => x.ToCsvString());
            return string.Join(", ", slist);
        }

        public override string ToString()
        {
            return Label;
        }
    }


    /*
    ;Analog Alarms
    ;Analog A, Tagname        , HandshakeTagName, HandshakeAutoReset, Acktagname, AckAutoreset, Deadbandvalue, DeadbandType, Outofalarmlabel, AlarmIdentification, Ack File Msg, Ack Printer Message, Ack Message Source, Out of Alm file msg, Out of Alm printer msg, Out of Alm Message Source, Thresh1 type, Threshold, Label                  , MessageSource, File msg, Printer msg, Direction, Severity, Thresh2 type, Threshold, Label                  , MessageSource, File msg, Printer msg, Direction, Severity,    Thresh3 type, Threshold, Label, MessageSource,File msg,Printer msg,Direction,Severity,   Thresh4 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,  Thresh5 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,   Thresh6 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,   Thresh7 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,   Thresh8 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,
    "A"      , "AI\F1011_1\s" , ""              , "N"               , ""        , "N"         , 0            , "A"         , ""             , ""                 , ""          , ""                 , "S"               , ""                 , ""                    , "S"                      , "C"         , "1.5"    , "FRCSA1011_1<0.0 (LL)" , "S"          , ""      , ""         , "D"      , "1"     , "C"         , "2.5"    , "FRCSA1011_1<1.0 (L)"  , "S"          ,"","","D","1",                                  "C","3.5","FRCSA1011_1<1.1","S","","","D","1",                                           "C","4.5","FRCSA1011_1>2.1","S","","","I","1",                                       "C","5.5","H","S","","","I","1",                                                      "C","6.5","HH",  "S","","","I","1",                                                   "","","","S","","","","",                                                             "","","","S","","","",""
    "A"      ,"Logic\P1\KEY"  , ""              , "N"               , ""        , "N"         , 0            , "A"         , ""             , ""                 , ""          , ""                 , "S"               , ""                 , ""                    , "S"                      , "C"         , "1.5"    , "П-1: Ключ - Закрыт"   , "S"          , ""      , ""         , "D"      , "1"     , "C"         , "1.7"    , "П-1: Ключ - Автомат"  , "S"          ,"","","I","1",                                  "C","2.5","П-1: Ключ - Открыт","S","","","I","1",                                        "","","",                   "S","","","","",                                         "","","",     "S","","","","",                                                        "","","",        "S","","","","",                                                     "","","","S","","","","",                                                             "","","","S","","","",""
    "A"      ,"Logic\P1\MODE" , ""              , "N"               , ""        , "N"         , 0            , "A"         , ""             , ""                 , ""          , ""                 , "S"               , ""                 , ""                    , "S"                      , "C"         , "1.1"    , "П-1: Режим - Простой" , "S"          , ""      , ""         , "D"      , "1"     , "C"         , "1.9"    , "П-1: Режим - В Работе", "S"          ,"","","I","1",                                  "","","","S","","","","",                                                                "","","",                   "S","","","","",                                         "","","",     "S","","","","",                                                        "","","",        "S","","","","",                                                     "","","","S","","","","",                                                             "","","","S","","","",""
    * */
    public class CsvAnalogAlarm
    {
        public RSViewTag.TypeEnum TagType = RSViewTag.TypeEnum.A;
        public string TagName;

        public string HandshakeTagName = "";
        public string HandshakeAutoReset = "N";
        public string Acktagname = "";
        public string AckAutoreset = "N";

        public double Deadbandvalue = 0;
        public RSViewTag.TypeEnum DeadbandType = RSViewTag.TypeEnum.A;
        public string Outofalarmlabel = "";
        public string AlarmIdentification = "";

        public CsvAnalogAlarmMessage AckMessage = new CsvAnalogAlarmMessage();
        public CsvAnalogAlarmMessage OutofAlmMessage = new CsvAnalogAlarmMessage();

        public Dictionary<int, CsvAnalogAlarmTreshold> Tresholds =
            new Dictionary<int, CsvAnalogAlarmTreshold>(8)
            {
                {1, new CsvAnalogAlarmTreshold()},
                {2, new CsvAnalogAlarmTreshold()},
                {3, new CsvAnalogAlarmTreshold()},
                {4, new CsvAnalogAlarmTreshold()},
                {5, new CsvAnalogAlarmTreshold()},
                {6, new CsvAnalogAlarmTreshold()},
                {7, new CsvAnalogAlarmTreshold()},
                {8, new CsvAnalogAlarmTreshold()},
            };

        public CsvAnalogAlarm(string name, Dictionary<int, RSViewAnalogAlarm> alarmList = null)
        {
            TagName = name;

            if (alarmList == null) 
                return;
            foreach (var alarm in alarmList)
            {
                Tresholds[alarm.Key] = alarm.Value.ToCsvAnalogAlarmTreshold();
            }
        }

        public string ToCsvString()
        {
            var fieldList = new List<object>()
                       {
                               TagType,
                               TagName,
                               HandshakeTagName,
                               HandshakeAutoReset,
                               Acktagname,
                               AckAutoreset,
                               Deadbandvalue,
                               DeadbandType,
                               Outofalarmlabel,
                               AlarmIdentification,
                               
                               AckMessage,
                               OutofAlmMessage,
                               
                               Tresholds[1],
                               Tresholds[2],
                               Tresholds[3],
                               Tresholds[4],
                               Tresholds[5],
                               Tresholds[6],
                               Tresholds[7],
                               Tresholds[8],
                       };

            var stringFieldsList = fieldList.Select(x => x.ToCsvString()).ToList();
            return string.Join(",", stringFieldsList);
        }
    }
}
