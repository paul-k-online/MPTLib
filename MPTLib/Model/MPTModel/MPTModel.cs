using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;

using MPT.PrimitiveType;

// ReSharper disable once CheckNamespace
namespace MPT.Model
{
    public partial class MPTEntities
    {
        //const  string EFConectionString = @" metadata = "res://*/Model.MPTModel.csdl|res://*/Model.MPTModel.ssdl|res://*/Model.MPTModel.msl;
        private const string Provider = "System.Data.SqlClient";

        private const string ProviderConnectionString = @"data source=192.168.100.220\wincc;initial catalog=MPT;persist security info=True;user id=RemoteWorkstation;password=RemoteWorkstation123;MultipleActiveResultSets=True;App=EntityFramework&quot;";

        private const string Metadate = "res://*/Model.MPTModel.csdl|res://*/Model.MPTModel.ssdl|res://*/Model.MPTModel.msl";

        public static EntityConnectionStringBuilder GetBuilder(string providerConnectionString = null, string provider = null)
        {
            var csb = new EntityConnectionStringBuilder();
            csb.Provider = string.IsNullOrWhiteSpace(provider) ? Provider : provider;
            csb.Metadata = Metadate;
            csb.ProviderConnectionString = string.IsNullOrWhiteSpace(providerConnectionString)
                ? ProviderConnectionString
                : providerConnectionString;
            return csb;
        }

        public MPTEntities(EntityConnectionStringBuilder connectionString) : base(connectionString.ToString())
        {}

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

        public Dictionary<DateTime, string> GetHolidays(params int[] years)
        {
            if (years == null)
                years = new[] { DateTime.Now.Year };

            var allHolidays = WorkScheduleHolidays.AsNoTracking().ToList();
            var holidaysDict = new Dictionary<DateTime, string>();
            foreach (var year in years)
            {
                var fromDT = new DateTime(year, 1, 1);
                var toDT = fromDT.AddYears(1);

                var yearHolidays = allHolidays
                    .Where(x => (x.Date >= fromDT && x.Date < toDT) || x.isYearly == true)
                    .ToDictionary(x=> new DateTime(year, x.Date.Month, x.Date.Day),y=>y.Name)
                    ;
                holidaysDict.AddRange(yearHolidays);
                
            }
            return  holidaysDict;
        }

        public Dictionary<DateTime,DateTime> GetOverWorkdays(params int[] years)
        {
            if (years == null)
                years = new[]{ DateTime.Now.Year};

            var allMoves = WorkScheduleMoves.AsNoTracking().ToList();
            var moveDict = new Dictionary<DateTime, DateTime>();
            foreach (var year in years)
            {
                var yearFromMoves = allMoves.Where(x => x.DateFrom.Year == year)
                    .ToDictionary(x => x.DateFrom, y => y.DateTo);
                var yearToMoves = allMoves.Where(x => x.DateTo.Year == year)
                    .ToDictionary(x => x.DateTo, y => y.DateFrom);
                moveDict.AddRange(yearFromMoves);
                moveDict.AddRange(yearToMoves);
            }
            return moveDict;
        }
    }
}
