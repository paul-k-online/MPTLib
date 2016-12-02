using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace MPT.RSView.TagDB
{
    public class AllTagItem
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }
        public RSViewTagType? Type { get; set; }
        public RSViewDataSourceType? DataSrcType { get; set; }
        public char? Security { get; set; }
        public int? ExternalRefs { get; set; }
        public short? ReadOnly { get; set; }
        public int? ParentId { get; set; }
        public short? ParentType { get; set; }

        public short? AnaNativeType { get; set; }
        public short? AnaValueType { get; set; }
        public double? AnaMinValue { get; set; }
        public double? AnaMaxValue { get; set; }
        public double? AnaInitValue { get; set; }
        public double? AnaScale { get; set; }
        public double? AnaOffset { get; set; }
        [StringLength(20)]
        public string AnaUnits { get; set; }

        [StringLength(20)]
        public string DigOffLabel { get; set; }
        [StringLength(20)]
        public string DigOnLabel { get; set; }
        public short? DigInitValue { get; set; }

        public short? StrLength { get; set; }
        [StringLength(255)]
        public string StrInitValue { get; set; }

        /*
        public short? BlkElementSize { get; set; }
        public short? BlkNumElements { get; set; }
        public int? BlkInitValue { get; set; }
        */

        [StringLength(40)]
        public string DevNodeName { get; set; }
        [StringLength(259)]
        public string DevAddress { get; set; }
        [StringLength(20)]
        public string DevScanClsName { get; set; }

        /*
        [StringLength(32)]
        public string DdeServiceName { get; set; }
        [StringLength(32)]
        public string DdeTopicName { get; set; }
        [StringLength(32)]
        public string DdeItemName { get; set; }
        */
        
        /*
        [StringLength(48)]
        public string SysItemName { get; set; }
        public int? SysItemIndex { get; set; }
        */

        /*
        [StringLength(259)]
        public string RioAddress { get; set; }
        public short RioCommType { get; set; }
        public short RioRackFileNum { get; set; }
        public short RioStartModule { get; set; }
        */
    }
}
