using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using System.ComponentModel.DataAnnotations;

// ReSharper disable once CheckNamespace
namespace MPT.Model
{
    public static class MPTEntitiesWorkstation
    {
        public static IQueryable<Workstation> GetWorkstations(this MPTEntities db)
        {
            return db.Workstations.AsQueryable()
                .Where(w => w.Enable == 1)
                .Include(ws => ws.Project)
                .Include(ws => ws.Project.Factory)
                ;
        }

        public static Workstation GetWorkstation(this MPTEntities db, int workstationId = 0)
        {
            return GetWorkstations(db).Single(ws => ws.Id == workstationId);
        }
    }


    [MetadataType(typeof(WorkstationMetadata))]
    public partial class Workstation {}

    public class WorkstationMetadata
    {
        [StringLength(255)]
        [Display(Name = "Сетевое имя")]
        public string NetworkName { get; set; }

        [StringLength(128)]
        [Display(Name = "Пользователь")]
        public string Username { get; set; }

        [StringLength(128)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [StringLength(15)]
        [Display(Name = "IP")]
        // ReSharper disable once InconsistentNaming
        public string IP { get; set; }
    }
}