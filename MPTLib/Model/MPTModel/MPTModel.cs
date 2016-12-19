using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MPT.Model
{
    public partial class MPTEntities
    {
        public MPTEntities(string connectionString) : base(connectionString)
        {
        }

        public IQueryable<Workstation> GetWorkstations()
        {
            return Workstations
                .Include(ws => ws.Project)
                .Include(ws => ws.Project.Factory)
                .OrderBy(ws => ws.Project.Factory.Number)
                .ThenBy(ws => ws.Project.OrderIndex);
        }

        public Workstation GetWorkstation(int workstationId)
        {
            return GetWorkstations().Single(ws => ws.Id == workstationId);
        }

        public IQueryable<PlcEvent> GetEventsByPlc(int plcId)
        {
            return PlcEvents
                            .Where(x => x.PlcId == plcId)
                            //.OrderByDescending(x => x.DateTime).ThenByDescending(x => x.Msec)
                            .Include(e => e.PlcMessage)
                            .Include(e => e.PlcEventCode)
                            .Include(e => e.PlcEventCode.Severity);
                ;
        }

        public IQueryable<PlcOldEvent> GetEventsOldByPlc(PLC plc)
        {
            return PlcEventsOld
                            .Where(x => x.PlcId == plc.Id)
                ;
        }
        
        public IQueryable<PLC> GetPLCs()
        {
            return PLCs.Where(p => p.ProtocolType > 0)
                .Include(x => x.Factory);
        }

        public PLC GetPlc(int plcId)
        {
            return GetPLCs().Single(p => p.Id == plcId);
        }

        public IQueryable<PcEvent> GetPcEvents(int? wsID = null, bool hideIgnored = true)
        {
            var events = PcEvents.OrderByDescending(e => e.DateTime).AsQueryable();
            var events1 = events;
            if (wsID != null)
                events1 = events1.Where(w => w.WorkstationId == wsID.Value);
            if(!hideIgnored)
                return events1;

            var words = PcEventIgnoreWords.ToList().Select(x => x.Word.ToUpper());

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

        public HashSet<DateTime> GetHolidays(int? year = null)
        {
            if (year == null) 
                year = DateTime.Now.Year;

            var fromDT = new DateTime(year.Value, 1, 1);
            var toDT = fromDT.AddYears(1);

            var holidays = WorkScheduleHolidays
                .Where(x => (x.Date >= fromDT && x.Date < toDT) || x.isYearly == true)
                .ToList()
                .Select(x => new DateTime(year.Value, x.Date.Month, x.Date.Day))
                ;
            return new HashSet<DateTime>(holidays.Distinct());
        }

        public HashSet<DateTime> GetHolidays(IEnumerable<int> years)
        {
            HashSet<DateTime> r = new HashSet<DateTime>();
            foreach (var year in years)
            {
                r.UnionWith(GetHolidays(year));
            }
            return r;
        }

        public HashSet<WorkScheduleMove> GetWorkSheduleMove(int? year = null)
        {
            if (year == null)
                year = DateTime.Now.Year;

            var fromDT = new DateTime(year.Value, 1, 1);
            var toDT = fromDT.AddYears(1);

            var workdays = WorkScheduleMoves
                .ToList()
                //.Where(x => (x.DateFrom >= fromDT && x.DateFrom < toDT) || (x.DateTo >= fromDT && x.DateTo < toDT))
                .Where(x=> x.DateFrom.Year == year || x.DateTo.Year == year)
                .ToList()
                //.Select(x => new WorkScheduleMove(year.Value, x.Date.Month, x.Date.Day))
                ;

            return new HashSet<WorkScheduleMove>(workdays.Distinct());
        }

        public HashSet<WorkScheduleMove> GetOverrideWorkdays(IEnumerable<int> years)
        {
            HashSet<WorkScheduleMove> r = new HashSet<WorkScheduleMove>();
            foreach (var year in years)
            {
                r.UnionWith(GetWorkSheduleMove(year));
            }
            return r;
        }
    }
}
