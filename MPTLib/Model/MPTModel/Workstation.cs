using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace MPT.Model
{
    [MetadataType(typeof(WorkstationMetadata))]
    public partial class Workstation
    {
        public override string ToString()
        {
            return FullName;
        }

        public string FullName
        {
            get { return string.Format("{0} ({1})", NetworkName, Address); }
        }
    }

    public class WorkstationMetadata
    {
        [StringLength(255)]
        [Display(Name = "������� ���")]
        public string NetworkName { get; set; }

        [StringLength(128)]
        [Display(Name = "������������")]
        public string Username { get; set; }

        [StringLength(128)]
        [Display(Name = "������")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "�����")]
        public string Address { get; set; }
    }
}