using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MPT.RSView.DataLog.MultiProject
{
    public class MultiProjectsDataLogEntities : DbContext
    {
        public MultiProjectsDataLogEntities()
            : base("name=MultiProjectsDataLogEntities")
        {
        }

        public MultiProjectsDataLogEntities(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<AlarmLog> AlarmLogs { get; set; }
        public virtual DbSet<DataLogFloat> DatalogFloats { get; set; }
        public virtual DbSet<DataLogTag>   DatalogTags { get; set; }
    }
}
