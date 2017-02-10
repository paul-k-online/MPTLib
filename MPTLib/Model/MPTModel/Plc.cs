using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;


namespace MPT.Model
{
    public partial class MPTEntities
    {
        public IQueryable<PLC> GetPLCs(bool protocolOnly = false)
        {
            var plcs = PLCs.AsNoTracking().Include(x => x.Factory);
            if (protocolOnly)
                plcs = plcs.Where(p => p.ProtocolType > 0);
            return plcs;
        }

        public PLC GetPLC(string name)
        {
            var plcs = PLCs.AsNoTracking()
                .Include(x => x.Factory).ToList()
                .Where(plc =>  plc.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            return plcs.SingleOrDefault();
        }

        public PLC GetPLC(int id)
        {
            var plcs = PLCs.AsNoTracking()
                .Include(x => x.Factory)
                .Where(p => p.Id == id).ToList();
            return plcs.SingleOrDefault();
        }
    }


    [MetadataType(typeof(PLCMetadata))]
    public partial class PLC
    {
        public string FullName
        {
            get { return string.Format("{1}, цеха {0}", Factory.Number, Description); }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class PLCMetadata
    {
        [StringLength(255)]
        [Display(Name = "Проект")]
        public string Name { get; set; }

        [StringLength(255)]
        [Display(Name = "Название")]
        public string Description { get; set; }

        [Display(Name = "Последнее")]
        public string LastEventDateTime { get; set; }
    }
}