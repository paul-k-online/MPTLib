using System;
using System.Collections.Generic;
using System.Linq;

namespace MPT.RSView.Csv
{
    public enum RSDigitEnum
    {
        // ReSharper disable InconsistentNaming
        OFF,
        ON
        // ReSharper restore InconsistentNaming    
    }

    public enum RSBoolEnum
    {
        /// <summary>
        /// False
        /// </summary>
        F,
        /// <summary>
        /// True
        /// </summary>
        T
    };



// "F"      , "AI"                , ""                                                    , "F"
// "F"      , "AI\FRCA2013_1"     , ""                                                    , "F"

// ;Tag Type, Tag Name            , Tag Description                                       , Read Only, Data Source, Security Code, Alarmed, Data Logged, Native Type, Value Type, Min Analog, Max Analog, Initial Analog, Scale, Offset, DeadBand, Units , Off Label Digital, On Label Digital, Initial Digital, Length String, Initial String, Node Name ,  Address  , Scan Class, System Source Name, System Source Index, RIO Address, Element Size Block, Number Elements Block, Initial Block
// "D"      , "AI\FRCA2013_1\fo"  , "FRCA-2013_1 Расход топливного газа поток 1 dP=16 кПа", "F"      , "D"        , "*"          , "F"    , "F"        ,            ,           ,           ,           ,               ,      ,       ,         ,       , "Off"            , "On"            , "Off"          ,              ,               , "101_pp23", "AI[8].EN", "A"       ,                   ,                    ,            ,                   ,                      , 
// "A"      , "AI\FRCA2013_1\v"   , "FRCA-2013_1 Расход топливного газа поток 1 П-2"      , "F"      , "D"        , "*"          , "F"    , "F"        , "D"        , "L"       , 0         , 1600      , 0             , 1    , 0     , 0       , "м3/ч",                  ,                 ,                ,              ,               , "101_pp23", "AI[8].v" , "A"       ,                   ,                    ,            ,                   ,                      ,
// "A"      , "AI\FRCA2013_2\bmax", "Блокировка по MAX"                                   , "F"      , "M"        , "*"          , "F"    , "F"        , "D"        , "L"       , 0         , 1600      , 0             , 1    , 0     , 0       , "м3/ч",                  ,                 ,                ,              ,               ,           ,           ,           ,                   ,                    ,            ,                   ,                      ,      
// "S",       "AI\FRCSA2011_3\n",   "Расход бензина поток 3 П-2",                           "F",       "M",         "*",           "F",     "F"        ,            ,           ,           ,           ,               ,      ,       ,         ,       ,                  ,                 ,                , 200          , "FRCSA-2011_3",           ,           ,           ,,,,,,

    public class Tag
    {
        public enum RSDataSource
        {
            /// <summary>
            /// Device
            /// </summary>
            D,
            /// <summary>
            /// Memory
            /// </summary>
            M,
        }

        // Generic
        public RSTagType TagType = RSTagType.F;
        public string TagName = "";
        public string TagDescription = "";
        public RSBoolEnum ReadOnly = RSBoolEnum.F;

        // Data
        public RSDataSource DataSource;
        public string SecurityCode = "*";
        public RSBoolEnum Alarmed = RSBoolEnum.F;
        public RSBoolEnum DataLogged = RSBoolEnum.F;

        // Analog
        public string NativeType = null;
        public string ValueType = null;
        public double? MinAnalog = null;
        public double? MaxAnalog = null;
        public double? InitialAnalog = null;
        public double? Scale = null;
        public double? Offset = null;
        public double? DeadBand = null;
        public string Units = null;
        
        // Digital
        public string OffLabelDigital = null;
        public string OnLabelDigital = null;
        public string InitialDigital = null;
        
        // String
        public int? LengthString = null;
        public string InitialString = null;
        
        // Address
        public string NodeName = null;
        public string Address = null;
        public string ScanClass
        {
            get { return DataSource == RSDataSource.D ? "A" : null; }
        }

        // Other
        public string SystemSourceName = null;
        public string SystemSourceIndex = null;
        public string RioAddress = null;
        public string ElementSizeBlock = null;
        public string NumberElementsBlock = null;
        public string InitialBlock = null;

        private Tag()
        {
        }


        public static Tag CreateFolder(string name)
        {
            return new Tag()
                   {
                       TagType = RSTagType.F,
                       TagName = name
                   };
        }
        
        public static Tag CreateAnalog(string name, string description,
                                        double min = 0, double max = 100, string units = "", double initial = 0)
        {
            var tag = new Tag
                        {
                            TagType = RSTagType.A,
                            TagName = name,
                            TagDescription = description,

                            NativeType = "D",
                            ValueType = "L",
                            Scale = 1,
                            Offset = 0,
                            DeadBand = 0,
                            
                            MinAnalog = min,
                            MaxAnalog = max,
                            Units = units,
                            InitialAnalog = initial,
                        };

            return tag;
        }

        public static Tag CreateDigit(string name, string description, bool initial = false)
        {
            var tag = new Tag()
                        {
                            TagType = RSTagType.D,
                            TagName = name,
                            TagDescription = description,
                            
                            OffLabelDigital = "OFF",
                            OnLabelDigital = "ON",
                            InitialDigital = ((RSDigitEnum)(Convert.ToInt16(initial))).ToString(),
                        };
            return tag;
        }

        public static Tag CreateString(string name, string description,
                                        string initial = "", ushort length = 200)
        {
            var tag = new Tag()
                        {
                            TagType = RSTagType.S,
                            TagName = name,
                            TagDescription = description,
                            LengthString = length,
                            InitialString = initial,
                        };
            return tag;

        }
        
        public Tag SetDataSource(string nodeName, string address, RSDataSource dataSource = RSDataSource.D)
        {
            DataSource = dataSource;
            NodeName = nodeName;
            Address = address;
            return this;
        }

        private IEnumerable<object> FieldsTotal
        {
            get
            {
                return new List<object>()
                                 {
                                     TagType,
                                     TagName,
                                     TagDescription,
                                     ReadOnly,

                                     DataSource,
                                     SecurityCode,
                                     Alarmed,
                                     DataLogged,
                                     
                                     NativeType,
                                     ValueType,
                                     MinAnalog,
                                     MaxAnalog,
                                     InitialAnalog,
                                     Scale,
                                     Offset,
                                     DeadBand,
                                     Units,
                                     
                                     OffLabelDigital,
                                     OnLabelDigital,
                                     InitialDigital,
                                     
                                     LengthString,
                                     InitialString,
                                     
                                     NodeName,
                                     Address,
                                     ScanClass,
                                     
                                     SystemSourceName,
                                     SystemSourceIndex,
                                     RioAddress,
                                     ElementSizeBlock,
                                     NumberElementsBlock,
                                     InitialBlock,
                                 };
            }
        }

        private IEnumerable<object> FieldsGeneric
        {
            get
            {
                return new List<object>()
                       {
                           TagType,
                           TagName,
                           TagDescription,
                           ReadOnly
                       };
            }
        }
        
        public override string ToString()
        {
            var fields = (TagType == RSTagType.F) ? FieldsGeneric : FieldsTotal;
            return string.Join(",", fields.Select(x => x.ToRS()));
        }

    }
}
