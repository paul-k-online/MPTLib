using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;

using MPT.PrimitiveType;



namespace MPT.Model
{
    public partial class MPTEntities
    {
        public Dictionary<DateTime, string> GetHolidays(params int[] years)
        {
            if (years == null)
                years = new[] { DateTime.Now.Year };

            var allHolidays = WorkScheduleHolidays
                .AsNoTracking()
                .ToList();

            var holidaysDict = new Dictionary<DateTime, string>();
            foreach (var year in years)
            {
                var fromDT = new DateTime(year, 1, 1);
                var toDT = fromDT.AddYears(1);

                var yearHolidays = allHolidays
                    .Where(x => (x.Date >= fromDT && x.Date < toDT) || x.isYearly == true)
                    .ToDictionary(x => new DateTime(year, x.Date.Month, x.Date.Day), y => y.Name)
                    ;
                holidaysDict.AddRangeWithUpdate(yearHolidays);

            }
            return holidaysDict;
        }

        public Dictionary<DateTime, DateTime> GetOverWorkdays(params int[] years)
        {
            if (years == null)
                years = new[] { DateTime.Now.Year };

            var allMoves = WorkScheduleMoves
                .AsNoTracking()
                .ToList();
            var moveDict = new Dictionary<DateTime, DateTime>();
            foreach (var year in years)
            {
                var yearFromMoves = allMoves.Where(x => x.DateFrom.Year == year)
                    .ToDictionary(x => x.DateFrom, y => y.DateTo);
                var yearToMoves = allMoves.Where(x => x.DateTo.Year == year)
                    .ToDictionary(x => x.DateTo, y => y.DateFrom);
                moveDict.AddRangeWithUpdate(yearFromMoves);
                moveDict.AddRangeWithUpdate(yearToMoves);
            }
            return moveDict;
        }
    }

    public partial class WorkScheduleHoliday
    {
        public override string ToString()
        {
            return string.Format("{0}, {1}", Date.ToString("yyyy-MM-dd"), Name);
        }
    }
}