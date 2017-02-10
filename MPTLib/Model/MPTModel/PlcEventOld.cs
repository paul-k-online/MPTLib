using System;
using System.Collections.Generic;
using System.Linq;

namespace MPT.Model
{
    public partial class MPTEntities
    { 
        public IQueryable<PlcEventOld> GetPlcEventsOld(int plcId)
        {
            return PlcEventsOld
                .AsNoTracking()
                .Where(x => x.PlcId == plcId);
        }
    }

}
