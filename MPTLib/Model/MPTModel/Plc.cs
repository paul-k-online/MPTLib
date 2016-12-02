using System.ComponentModel.DataAnnotations;

// ReSharper disable once CheckNamespace
namespace MPT.Model
{

    // ReSharper disable once InconsistentNaming
    public partial class PLC
    {
        public string FullName
        {
            get { return string.Format("{1}, ���� {0}", Factory.Number, Description); }
        }
    }
    

    [MetadataType(typeof(PlcMetadata))]
    // ReSharper disable once InconsistentNaming
    public partial class PLC
    { }
    

    public class PlcMetadata
    {
        [StringLength(255)]
        [Display(Name = "������")]
        public string Name { get; set; }

        [StringLength(255)]
        [Display(Name = "��������")]
        public string Description { get; set; }

        [Display(Name = "���������")]
        public string LastEventDateTime { get; set; }
    }
}