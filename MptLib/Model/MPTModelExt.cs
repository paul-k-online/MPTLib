using System.Data.Entity;
using System.Linq;

namespace MPT.Model
{
// ReSharper disable once InconsistentNaming
    public static class MPTModelExt
    {
        public static IQueryable<PlcEvent> GetEventsByPlc(this MPTEntities dbContext, PLC plc)
        {
            return dbContext.PlcEvents
                            .Where(x => x.PlcId == plc.Id)
                            .OrderByDescending(x => x.DateTime).ThenByDescending(x => x.Msec)
                            .Include(e => e.PlcMessage)
                            .Include(e => e.PlcEventCode)
                ;
        }
        
// ReSharper disable once InconsistentNaming
        public static IQueryable<PLC> GetPLCs(this MPTEntities db)
        {
            var a = db.PLCs
                .Where(p => p.ProtocolType > 0)
                //.Include(x => x.Factory)
                ;
            return a;
        }

        public static PLC GetPlc(this MPTEntities db, int plcId)
        {
            return GetPLCs(db).Single(p => p.Id == plcId);
        }

        public static IQueryable<Workstation> GetWorkstations(this MPTEntities db)
        {
            return db.Workstations.AsQueryable()
                .Where(w => w.Enable==1)
                .Include(ws => ws.Project)
                .Include(ws => ws.Project.Factory)
                ;
        }

        public static Workstation GetWorkstation(this MPTEntities db, int workstationId = 0)
        {
            return GetWorkstations(db).Single(ws => ws.Id == workstationId);
        }

        public static IQueryable<PcEvent> GetPcEvents(this MPTEntities db, int wsId = 0)
        {
            var words = db.PcEventIgnoreWords.ToList().Select(x => x.Word.ToUpper());
            var events = db.PcEvents.OrderByDescending(e => e.DateTime).AsQueryable();
            
            if (wsId != 0)
                events = events.Where(w => w.WorkstationId == wsId);
            
            /*
            var a = from pcEvent in events
                    where !words.Any(word => pcEvent.Message.ToUpper().Contains(word))
                    select pcEvent;
            */
            return events;
        }




    }
}
