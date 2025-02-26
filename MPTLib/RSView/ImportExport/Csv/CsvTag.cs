﻿using MPT.PrimitiveType;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPT.RSView.ImportExport.Csv
{
    public class CsvTag
    {
        // Generic
        RSViewTag.TypeEnum TagType = RSViewTag.TypeEnum.F;
        string TagName = "";
        string TagDescription = "";
        RSViewBoolEnum ReadOnly = RSViewBoolEnum.F;

        // Data
        RSViewTag.DataSourceTypeEnum DataSource = RSViewTag.DataSourceTypeEnum.M;
        string SecurityCode = "*";
        RSViewBoolEnum Alarmed = RSViewBoolEnum.F;
        RSViewBoolEnum DataLogged = RSViewBoolEnum.F;

        // Analog
        string NativeType;
        string ValueType;
        double? MinAnalog;
        double? MaxAnalog;
        double? InitialAnalog;
        double? Scale;
        double? Offset;
        double? DeadBand;
        string Units;
        
        // Digital
        string OffLabelDigital;
        string OnLabelDigital;
        string InitialDigital;
        
        // String
        int? LengthString;
        string InitialString;
        
        // Address
        string NodeName;
        string Address;
        string ScanClass
        {
            get { return DataSource == RSViewTag.DataSourceTypeEnum.D ? "A" : null; }
        }

        // Other
        string SystemSourceName = null;
        string SystemSourceIndex = null;
        string RioAddress = null;
        string ElementSizeBlock = null;
        string NumberElementsBlock = null;
        string InitialBlock = null;

        public static CsvTag CreateFolder(string name)
        {
            return new CsvTag()
                   {
                       TagType = RSViewTag.TypeEnum.F,
                       TagName = name,
                   };
        }
        
        public static CsvTag CreateAnalog(string name, string description,
                                        double min = 0, double max = 100, string units = "", double initial = 0)
        {
            var tag = new CsvTag
                        {
                            TagType = RSViewTag.TypeEnum.A,
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
                            TagType = RSViewTag.TypeEnum.D,
                            TagName = name,
                            TagDescription = description,
                            
                            OffLabelDigital = "OFF",
                            OnLabelDigital = "ON",
                            InitialDigital = initialValue.ToEnum<RSViewDigitEnum>().ToString(),
                        };
            return tag;
        }

        public static CsvTag CreateString(string name, string description, string initial = "", ushort length = 200)
        {
            var tag = new CsvTag()
            {
                TagType = RSViewTag.TypeEnum.S,
                TagName = name,
                TagDescription = description,
                LengthString = length,
                InitialString = initial,
            };
            return tag;
        }
        
        public CsvTag SetDataSource(string nodeName, string address, RSViewTag.DataSourceTypeEnum dataSourceType = RSViewTag.DataSourceTypeEnum.D)
        {
            if (dataSourceType == RSViewTag.DataSourceTypeEnum.D)
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
        
        public string ToCsvString()
        {
            var fields = (TagType == RSViewTag.TypeEnum.F) ? FieldsGeneric : FieldsTotal;
            return string.Join(",", fields.Select(x => x.ToCsvString()));
        }
    }
}
