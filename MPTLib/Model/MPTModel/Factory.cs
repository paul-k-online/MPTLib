using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.Model
{
    public partial class Factory
    {
        public string FullName
        {
            get
            {
                var format = "{0} - {1}";
                if (Number == null)
                    format = "{1}";
                return string.Format(format, Number, Description);
            }
        }
    }

}
