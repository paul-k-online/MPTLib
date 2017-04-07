using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MPT.RSView.ImportExport.XML
{
    #region Other types
    [XmlRoot("PositionList")]
    public class PositionList
    {
        [XmlElement("AiPosition")]
        public SchemaAiPosition AiPosition { get; set; }
        [XmlElement("DioPosition")]
        public SchemaDioPosition DioPosition { get; set; }
        [XmlElement("AoPosition")]
        public SchemaAoPosition AoPosition { get; set; }
    }

    public class SchemaPosition
    {
        [XmlElement("TAG")]
        public List<SchemaTag> Tags { get; set; }
    }

    [XmlRoot("AiPosition")]
    public class SchemaAiPosition : SchemaPosition
    { }

    [XmlRoot("DioPosition")]
    public class SchemaDioPosition : SchemaPosition
    { }

    [XmlRoot("AoPosition")]
    public class SchemaAoPosition : SchemaPosition
    { }
    #endregion

    [XmlRoot("TAG")]
    public class SchemaTag
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("folder")]
        public string Folder { get; set; }
        [XmlAttribute("description")]
        public string Description { get; set; }
        [XmlAttribute("dataSource")]
        public string DataSource { get; set; }
        [XmlAttribute("address")]
        public string Address { get; set; }
        [XmlAttribute("length")]
        public string Length { get; set; }
        [XmlAttribute("initialValue")]
        public string InitialValue { get; set; }
        [XmlAttribute("min")]
        public string Min { get; set; }
        [XmlAttribute("max")]
        public string Max { get; set; }
        [XmlAttribute("units")]
        public string Units { get; set; }
        [XmlElement("DATALOG")]
        public List<SchemaDatalog> Datalogs { get; set; }
        [XmlElement("ALARM")]
        public List<SchemaAlarm> Alarms { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }


    [XmlRoot("DATALOG")]
    public class SchemaDatalog
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }


    [XmlRoot("ALARM")]
    public class SchemaAlarm
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("number")]
        public string Number { get; set; }
        [XmlAttribute("threshold")]
        public string Threshold { get; set; }
        [XmlAttribute("label")]
        public string Label { get; set; }
        [XmlAttribute("direction")]
        public string Direction { get; set; }
        [XmlAttribute("severity")]
        public string Severity { get; set; }
        public override string ToString()
        {
            return string.Format("[{0}]: {1}", Number, Label);
        }
    }
}
