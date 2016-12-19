using System;
using System.Collections.Generic;
using System.Linq;

namespace MPT.RSView.ImportExport.Csv.Tag
{
    public enum RsViewDigitEnum
    {
        // ReSharper disable InconsistentNaming
        OFF,
        ON
        // ReSharper restore InconsistentNaming    
    }

    public enum RsViewBoolEnum
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


    public class CsvTag
    {
        // Generic
        public RSViewTagType TagType = RSViewTagType.F;
        public string TagName = "";
        public string TagDescription = "";
        public RsViewBoolEnum ReadOnly = RsViewBoolEnum.F;

        // Data
        public RSViewTagDataSourceType DataSource = RSViewTagDataSourceType.M;
        public string SecurityCode = "*";
        public RsViewBoolEnum Alarmed = RsViewBoolEnum.F;
        public RsViewBoolEnum DataLogged = RsViewBoolEnum.F;

        // Analog
        public string NativeType;
        public string ValueType;
        public double? MinAnalog;
        public double? MaxAnalog;
        public double? InitialAnalog;
        public double? Scale;
        public double? Offset;
        public double? DeadBand;
        public string Units;
        
        // Digital
        public string OffLabelDigital;
        public string OnLabelDigital;
        public string InitialDigital;
        
        // String
        public int? LengthString;
        public string InitialString;
        
        // Address
        public string NodeName;
        public string Address;
        public string ScanClass
        {
            get { return DataSource == RSViewTagDataSourceType.D ? "A" : null; }
        }

        // Other
        public string SystemSourceName = null;
        public string SystemSourceIndex = null;
        public string RioAddress = null;
        public string ElementSizeBlock = null;
        public string NumberElementsBlock = null;
        public string InitialBlock = null;

        public static CsvTag CreateFolder(string name)
        {
            return new CsvTag()
                   {
                       TagType = RSViewTagType.F,
                       TagName = name,
                   };
        }
        
        public static CsvTag CreateAnalog(string name, string description,
                                        double min = 0, double max = 100, string units = "", double initial = 0)
        {
            var tag = new CsvTag
                        {
                            TagType = RSViewTagType.A,
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

        public static CsvTag CreateDigit(string name, string description, bool initialValue = false)
        {
            var tag = new CsvTag()
                        {
                            TagType = RSViewTagType.D,
                            TagName = name,
                            TagDescription = description,
                            
                            OffLabelDigital = "OFF",
                            OnLabelDigital = "ON",
                            InitialDigital = ((RsViewDigitEnum)(Convert.ToInt16(initialValue))).ToString(),
                        };
            return tag;
        }

        public static CsvTag CreateString(string name, string description,
                                        string initial = "", ushort length = 200)
        {
            var tag = new CsvTag()
            {
                TagType = RSViewTagType.S,
                TagName = name,
                TagDescription = description,
                LengthString = length,
                InitialString = initial,
            };
            return tag;

        }
        
        public CsvTag SetDataSource(string nodeName, string address, RSViewTagDataSourceType dataSourceType = RSViewTagDataSourceType.D)
        {
            if (dataSourceType == RSViewTagDataSourceType.D)
            {
                DataSource = dataSourceType;
                NodeName = nodeName;
                Address = address;
            }
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
            var fields = (TagType == RSViewTagType.F) ? FieldsGeneric : FieldsTotal;
            return string.Join(",", fields.Select(x => x.ToRsViewFormat()));
        }

    }
}
