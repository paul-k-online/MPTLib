﻿using System;
using System.Linq;
using System.Data.Entity;

namespace MPT.Model
{
    public partial class MPTEntities
    {
        public IQueryable<PlcEvent> GetPlcEvents(int plcId)
        {
            return PlcEvents
                .AsNoTracking()
                .Where(x => x.PlcId == plcId);
        }
    }
}
