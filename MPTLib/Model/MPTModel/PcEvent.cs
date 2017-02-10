using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.Model
{
    public partial class MPTEntities
    {
        public IQueryable<PcEvent> GetPcEvents(int? wsID = null, bool hideIgnored = true)
        {
            var events = PcEvents
                .AsNoTracking()
                .OrderByDescending(e => e.DateTime).AsQueryable();

            if (wsID != null)
                events = events.Where(w => w.WorkstationId == wsID.Value);
            if (!hideIgnored)
                return events;

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

            var c = from pcEvent in events
                    where !words.Any(x => pcEvent.Message.ToUpper().Contains(x))
                    select pcEvent;
            return c.AsQueryable();
        }
    }

    public partial class PcEvent
    {
    }
}
