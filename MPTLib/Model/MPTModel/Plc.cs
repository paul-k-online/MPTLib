using System.ComponentModel.DataAnnotations;

// ReSharper disable once CheckNamespace
namespace MPT.Model
{

    // ReSharper disable once InconsistentNaming
    public partial class PLC
    {
        public string FullName
        {
            get { return string.Format("{1}, цеха {0}", Factory.Number, Description); }
        }
    }
    

    [MetadataType(typeof(PlcMetadata))]
    // ReSharper disable once InconsistentNaming
    public partial class PLC
    { }
    

    public class PlcMetadata
    {
        [StringLength(255)]
        [Display(Name = "Проект")]
        public string Name { get; set; }

        [StringLength(255)]
        [Display(Name = "Название")]
        public string Description { get; set; }

        [Display(Name = "Последнее")]
        public string LastEventDateTime { get; set; }
    }
}