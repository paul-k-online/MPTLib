using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MPT.Model
{
    [MetadataType(typeof(PlcMetadata))]
    // ReSharper disable once InconsistentNaming
    public partial class PLC
    {}

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

    [MetadataType(typeof(WorkstationMetadata))]
    public partial class Workstation
    {}

    public class WorkstationMetadata
    {
        [StringLength(255)]
        [Display(Name = "Сетевое имя")]
        public string NetworkName { get; set; }

        [StringLength(128)]
        [Display(Name = "Пользователь администратор")]
        public string Username { get; set; }

        [StringLength(128)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [StringLength(15)]
        [Display(Name = "IP")]
        public string IP { get; set; }
    }

    [MetadataType(typeof(PlcMessageMetadata))]
    public partial class PlcMessage
    {}

    public class PlcMessageMetadata
    {
        [Display(Name = "Номер")]
        public int Number { get; set; }

        [StringLength(255)]
        [Display(Name = "Сообщение")]
        public string Text { get; set; }

        [Display(Name = "Группа")]
        public int Group { get; set; }

    }
}
