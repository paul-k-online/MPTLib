using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.Model
{
    public partial class ProjectHMI
    {
        public string FullName
        {
            get
            {
                var format = "{1} ({0})";
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description))
                    format = "{1}{0}";
                return string.Format(format, Name, Description);
            }
        }
    }
}
