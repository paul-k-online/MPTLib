using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.Model
{
    public partial class GetPlcEventsCount_Result
    {
        public override string ToString()
        {
            return string.Format("{0}: {1} of {2}", PlcId, AlarmCount, TotalCount);
        }
    }
}
