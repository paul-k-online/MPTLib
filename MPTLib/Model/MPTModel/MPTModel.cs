using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace MPT.Model
{
    // ReSharper disable once InconsistentNaming
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



    public partial class GetPlcEventsCount_Result
    {
        public override string ToString()
        {
            return string.Format("{0}: {1} of {2}", PlcId, AlarmCount, TotalCount);
        }
    }

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

// ReSharper disable once InconsistentNaming
    public static class MPTEntitiesExt
    {
        public static IQueryable<PlcEvent> GetEventsByPlc(this MPTEntities db, PLC plc)
        {
            return db.PlcEvents
                            .Where(x => x.PlcId == plc.Id)
                            //.OrderByDescending(x => x.DateTime).ThenByDescending(x => x.Msec)
                            .Include(e => e.PlcMessage)
                            .Include(e => e.PlcEventCode)
                            .Include(e => e.PlcEventCode.Severity)
                ;
        }

        public static IQueryable<PlcOldEvent> GetEventsOldByPlc(this MPTEntities db, PLC plc)
        {
            return db.PlcEventsOld
                            .Where(x => x.PlcId == plc.Id)
                ;
        }
        
// ReSharper disable once InconsistentNaming
        public static IQueryable<PLC> GetPLCs(this MPTEntities db)
        {
            var a = db.PLCs
                .Where(p => p.ProtocolType > 0)
                .Include(x => x.Factory)
                ;
            return a;
        }


        public static PLC GetPlc(this MPTEntities db, int plcId)
        {
            return GetPLCs(db).Single(p => p.Id == plcId);
        }




        public static IQueryable<PcEvent> GetPcEvents(this MPTEntities db, int? wsID = null, bool hideIgnored = true)
        {
            var events = db.PcEvents.OrderByDescending(e => e.DateTime).AsQueryable();
            var events1 = events;
            if (wsID != null)
                events1 = events1.Where(w => w.WorkstationId == wsID.Value);
            if(!hideIgnored)
                return events1;


            var words = db.PcEventIgnoreWords.ToList().Select(x => x.Word.ToUpper());

            /*
            foreach (var word in words)
            {
                events = events.Where(x => !x.Message.ToUpper().Contains(word));
            }
            return events;
            */

            /*
            var a = from pcEvent in events
                where !words.Any(word => pcEvent.Message.ToUpper().Contains(word))
                select pcEvent;
            return a;
                */

            /*
            var b = from pcEvent in events
                where !words.Contains(pcEvent.Message.ToUpper())
                select pcEvent;
            return b;
            */

            var c = from pcEvent in events1
                    where !words.Any(x => pcEvent.Message.ToUpper().Contains(x))
                    select pcEvent;
            return c.AsQueryable();
           
        }


        public static HashSet<DateTime> GetHolidays(this MPTEntities db, int? year = null)
        {
            if (year == null) 
                year = DateTime.Now.Year;

            var fromDT = new DateTime(year.Value, 1, 1);
            var toDT = fromDT.AddYears(1);

            var holidays = db.Holidays
                .Where(x => x.isWork != true)
                .Where(x => (x.Date >= fromDT && x.Date < toDT) || x.isFixed == true)
                .ToList()
                .Select(x => new DateTime(year.Value, x.Date.Month, x.Date.Day))
                ;
            return new HashSet<DateTime>(holidays.Distinct());
        }
        public static HashSet<DateTime> GetHolidays(this MPTEntities db, IEnumerable<int> years)
        {
            HashSet<DateTime> r = new HashSet<DateTime>();
            foreach (var year in years)
            {
                r.UnionWith(db.GetHolidays(year));
            }
            return r;
        }
        public static HashSet<DateTime> GetOverrideWorkdays(this MPTEntities db, int? year = null)
        {
            if (year == null)
                year = DateTime.Now.Year;

            var fromDT = new DateTime(year.Value, 1, 1);
            var toDT = fromDT.AddYears(1);

            var workdays = db.Holidays
                .Where(x => x.isWork == true)
                .Where(x => (x.Date >= fromDT && x.Date < toDT) || x.isFixed == true)
                .ToList()
                .Select(x => new DateTime(year.Value, x.Date.Month, x.Date.Day))
                ;

            return new HashSet<DateTime>(workdays.Distinct());
        }
        public static HashSet<DateTime> GetOverrideWorkdays(this MPTEntities db, IEnumerable<int> years)
        {
            HashSet<DateTime> r = new HashSet<DateTime>();
            foreach (var year in years)
            {
                r.UnionWith(db.GetOverrideWorkdays(year));
            }
            return r;
        }
    }
}
