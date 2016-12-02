using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace MPT.RSView.DataLog
{
    public class DataLogEntities : DbContext
    {
        public DataLogEntities()
            : base("name=DatalogEntities")
        {
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }



        public virtual DbSet<DataLogFloat> DatalogFloats { get; set; }
        public virtual DbSet<DataLogTag>   DatalogTags { get; set; }
    }
}
