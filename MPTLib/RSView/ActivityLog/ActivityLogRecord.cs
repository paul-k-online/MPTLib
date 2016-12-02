using System;
using System.ComponentModel.DataAnnotations;

namespace MPT.RSView.Activity
{
    public class ActivityLogRecord
    {
        //n 1
        public byte Type { get; set; }
        
        //n 10
        public int Id { get; set; }
        
        //Date 8
        public DateTime Date { get; set; }
        
        //c 8
        [StringLength(8)]
        public string Time { get; set; }
        
        //n 5
        public int MiliTime { get; set; }

        //c 1
        [StringLength(1)]
        public char DstFlag { get; set; }

        //c 10
        [StringLength(10)]
        public string Category { get; set; }

        //c 30
        [StringLength(30)]
        public string Source { get; set; }

        //c 20
        [StringLength(20)]
        public string User { get; set; }

        //c 255
        [StringLength(255)]
        public string Dscrptn { get; set; }

        //c 15
        [StringLength(15)]
        public string UserStn { get; set; }

        //c 15
        [StringLength(15)]
        public string LoggingStn { get; set; }
    }
}
