namespace Информационная_система_медицинской_клиники
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Medical_ClinicEntities : DbContext
    {
        private static Medical_ClinicEntities _context;
        public Medical_ClinicEntities()
            : base("name=Medical_ClinicEntities")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public static Medical_ClinicEntities GetContext()
        {
            if (_context == null || _context.Database.Connection.State == System.Data.ConnectionState.Closed)
            {
                _context = new Medical_ClinicEntities();
            }
            return _context;
        }

        public static void ResetContext()
        {
            _context?.Dispose();
            _context = null;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Appointments> Appointments { get; set; }
        public virtual DbSet<Doctors> Doctors { get; set; }
        public virtual DbSet<Invoices> Invoices { get; set; }
        public virtual DbSet<MedicalRecords> MedicalRecords { get; set; }
        public virtual DbSet<Medications> Medications { get; set; }
        public virtual DbSet<Patients> Patients { get; set; }
        public virtual DbSet<Prescriptions> Prescriptions { get; set; }
        public virtual DbSet<Rooms> Rooms { get; set; }
        public virtual DbSet<Schedules> Schedules { get; set; }
        public virtual DbSet<Servicess> Servicess { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
