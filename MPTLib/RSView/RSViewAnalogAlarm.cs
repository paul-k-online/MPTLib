using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MPT.RSView
{
    public class RSViewAnalogAlarm
    {
        public enum RSViewTresholdDirection
        {
            /// <summary>
            /// Decreasing
            /// </summary>
            D,
            /// <summary>
            /// Increasing
            /// </summary>
            I
        }

        public string Threshold { get; set; }
        [MaxLength(21)]
        public string Label { get; set; }
        public ushort Severity { get; set; }
        public RSViewTresholdDirection Direction { get; set; }
    }
}
