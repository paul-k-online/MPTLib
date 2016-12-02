using System.Collections.Generic;
using System.Linq;

namespace MPT.RSView.Csv
{
    public class CsvAlarmTreshold
    {
        public enum ThresholdType
        {
            /// <summary>
            /// Checked ?
            /// </summary>
            C
        }
        
        /*
    Thresh1 type, Threshold, Label                  , MessageSource, File msg, Printer msg, Direction, Severity, 
    Thresh2 type, Threshold, Label                  , MessageSource, File msg, Printer msg, Direction, Severity,    
    Thresh3 type, Threshold, Label,                   MessageSource, File msg, Printer msg, Direction, Severity,   
    Thresh4 type, Threshold, Label,                   MessageSource, File msg, Printer msg, Direction, Severity,  
    Thresh5 type, Threshold, Label,                   MessageSource, File msg, Printer msg, Direction, Severity,   
    Thresh6 type, Threshold, Label,                   MessageSource, File msg, Printer msg, Direction, Severity,   
    Thresh7 type, Threshold, Label,                   MessageSource, File msg, Printer msg, Direction, Severity,   
    Thresh8 type, Threshold, Label,                   MessageSource, File msg, Printer msg, Direction, Severity,
    "C"         , "1.5"    , "FRCSA1011_1<0.0 (LL)" , "S"          , ""      , ""         , "D"      , "1"     , 
    "C"         , "2.5"    , "FRCSA1011_1<1.0 (L)"  , "S"          , "",       "",          "D",       "1",                                
    "C",          "3.5",     "FRCSA1011_1<1.1",       "S",           "",       "",          "D",       "1",                                           
    "C",          "4.5",     "FRCSA1011_1>2.1",       "S",           "",       "",          "I",       "1",                                       
    "C",          "5.5",     "H",                     "S",           "",       "",          "I",       "1",                                                      
    "C",          "6.5",     "HH",                    "S",           "",       "",          "I",       "1",                                                   
    "",           "",        "",                      "S",           "",       "",          "",        "",                       
    "",           "",        "",                      "S",           "",       "",          "",        ""
    
    "C"         , "1.5"    , "П-1: Ключ - Закрыт"   , "S"          , ""      , ""         , "D"      , "1"     , "C"         , "1.7"    , "П-1: Ключ - Автомат"  , "S"          , "","","I","1",                                 "C","2.5","П-1: Ключ - Открыт","S","","","I","1",                                        "","","",                   "S","","","","",                                         "","","",     "S","","","","",                                                        "","","",        "S","","","","",                                                     "","","","S","","","","",                                                             "","","","S","","","",""
    "C"         , "1.1"    , "П-1: Режим - Простой" , "S"          , ""      , ""         , "D"      , "1"     , "C"         , "1.9"    , "П-1: Режим - В Работе", "S"          , "","","I","1",                                 "","","","S","","","","",                                                                "","","",                   "S","","","","",                                         "","","",     "S","","","","",                                                        "","","",        "S","","","","",                                                     "","","","S","","","","",                                                             "","","","S","","","",""
*/
        public bool? Enabled;
        public ThresholdType? Type = null;
        public double? Threshold = null;
        public string Label = "";
        public AlarmMessage AlarmMessage = new AlarmMessage();
        public RSTresholdDirection? Direction = null;
        public ushort? Severity = null;

        public CsvAlarmTreshold(double threshold, string label, RSTresholdDirection direction, ushort severity = 1, ThresholdType type = ThresholdType.C)
        {
            Type = type;
            Threshold = threshold;
            Label = label;
            Direction = direction;
            Severity = severity;
        }

        public CsvAlarmTreshold()
        {
        }

        public override string ToString()
        {
            var fieldList = new List<object>()
                       {
                           Type.ToString(),
                           Threshold.ToString(),
                           Label,
                           AlarmMessage,
                           Direction.ToString(),
                           Severity.ToString(),
                       };
            return string.Join(", ", fieldList.Select(x => x.ToRS()));
        }
    }


    /*
;Analog Alarms
;Analog A, Tagname        , HandshakeTagName, HandshakeAutoReset, Acktagname, AckAutoreset, Deadbandvalue, DeadbandType, Outofalarmlabel, AlarmIdentification, Ack File Msg, Ack Printer Message, Ack Message Source, Out of Alm file msg, Out of Alm printer msg, Out of Alm Message Source, Thresh1 type, Threshold, Label                  , MessageSource, File msg, Printer msg, Direction, Severity, Thresh2 type, Threshold, Label                  , MessageSource, File msg, Printer msg, Direction, Severity,    Thresh3 type, Threshold, Label, MessageSource,File msg,Printer msg,Direction,Severity,   Thresh4 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,  Thresh5 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,   Thresh6 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,   Thresh7 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,   Thresh8 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,
"A"      , "AI\F1011_1\s" , ""              , "N"               , ""        , "N"         , 0            , "A"         , ""             , ""                 , ""          , ""                 , "S"               , ""                 , ""                    , "S"                      , "C"         , "1.5"    , "FRCSA1011_1<0.0 (LL)" , "S"          , ""      , ""         , "D"      , "1"     , "C"         , "2.5"    , "FRCSA1011_1<1.0 (L)"  , "S"          ,"","","D","1",                                  "C","3.5","FRCSA1011_1<1.1","S","","","D","1",                                           "C","4.5","FRCSA1011_1>2.1","S","","","I","1",                                       "C","5.5","H","S","","","I","1",                                                      "C","6.5","HH",  "S","","","I","1",                                                   "","","","S","","","","",                                                             "","","","S","","","",""
"A"      ,"Logic\P1\KEY"  , ""              , "N"               , ""        , "N"         , 0            , "A"         , ""             , ""                 , ""          , ""                 , "S"               , ""                 , ""                    , "S"                      , "C"         , "1.5"    , "П-1: Ключ - Закрыт"   , "S"          , ""      , ""         , "D"      , "1"     , "C"         , "1.7"    , "П-1: Ключ - Автомат"  , "S"          ,"","","I","1",                                  "C","2.5","П-1: Ключ - Открыт","S","","","I","1",                                        "","","",                   "S","","","","",                                         "","","",     "S","","","","",                                                        "","","",        "S","","","","",                                                     "","","","S","","","","",                                                             "","","","S","","","",""
"A"      ,"Logic\P1\MODE" , ""              , "N"               , ""        , "N"         , 0            , "A"         , ""             , ""                 , ""          , ""                 , "S"               , ""                 , ""                    , "S"                      , "C"         , "1.1"    , "П-1: Режим - Простой" , "S"          , ""      , ""         , "D"      , "1"     , "C"         , "1.9"    , "П-1: Режим - В Работе", "S"          ,"","","I","1",                                  "","","","S","","","","",                                                                "","","",                   "S","","","","",                                         "","","",     "S","","","","",                                                        "","","",        "S","","","","",                                                     "","","","S","","","","",                                                             "","","","S","","","",""
     * */
    public class AnalogAlarm
    {
        public RSTagType TagType = RSTagType.A;
        public string TagName;

        public string HandshakeTagName = "";
        public string HandshakeAutoReset = "N";
        public string Acktagname = "";
        public string AckAutoreset = "N";

        public double Deadbandvalue = 0;
        public RSTagType DeadbandType = RSTagType.A;
        public string Outofalarmlabel = "";
        public string AlarmIdentification = "";

        public AnalogAlarmMessage AckMessage = new AnalogAlarmMessage();
        public AnalogAlarmMessage OutofAlmMessage = new AnalogAlarmMessage();

        public Dictionary<int, CsvAlarmTreshold> Tresholds = new Dictionary<int, CsvAlarmTreshold>(8)
                                                           {
                                                               {1, new CsvAlarmTreshold()},
                                                               {2, new CsvAlarmTreshold()},
                                                               {3, new CsvAlarmTreshold()},
                                                               {4, new CsvAlarmTreshold()},
                                                               {5, new CsvAlarmTreshold()},
                                                               {6, new CsvAlarmTreshold()},
                                                               {7, new CsvAlarmTreshold()},
                                                               {8, new CsvAlarmTreshold()},
                                                           };
        
        public override string ToString()
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

            var stringFieldsList = fieldList.Select(x => x.ToRS()).ToList();
            return string.Join(",", stringFieldsList);
        }
    }
}
