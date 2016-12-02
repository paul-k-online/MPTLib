using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.Model
{
// ReSharper disable once InconsistentNaming
    public partial class PLC
    {
        public string FullName
        {
            get { return string.Format("{1} цеха {0}", Factory.Number, Description); }
        }
    }
}
