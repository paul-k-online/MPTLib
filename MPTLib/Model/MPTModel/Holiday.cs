using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable once CheckNamespace
namespace MPT.Model
{
    public partial class Holiday
    {
        public override string ToString()
        {
            return string.Format("{0}, {1}", Date.ToString("yyyy-MM-dd"), Name);
        }
    }
}
