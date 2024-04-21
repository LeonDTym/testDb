using Microsoft.EntityFrameworkCore;
using SCRepository.Entity.Models;

namespace SCRepository.Entity
{
    public class DBContext : DbContext
    {
        public DbSet<Students> Students { get; set; }
        public DbQuery<StateModel> StateModel { get; set; }
        public DbSet<PhotoModel> StudentPhoto { get; set; }
        public DbSet<StatusZayavkaModel> StatusZayavka { get; set; }
        public DbSet<SchoolModel> School { get; set; }
        public DbSet<Logs> Logs { get; set; }
        //public DbSet<Logs> Logs_Out { get; set; }
        public DbSet<Risk> Risks { get; set; }
        public DbSet<StateModel> State { get; set; }
        public DbQuery<Transfer> Transfer { get; set; }
        public DbQuery<TransferBlank> TransferBlank { get; set; }
        public DbSet<ExportUID> ExportUID { get; set; }
        public DbQuery<Report> Reports { get; set; }

        public DBContext()
        {
            Database.SetCommandTimeout(180);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {/*;Integrated Security=true;Encrypt=yes*/
            //test DB
            //optionsBuilder.UseMySQL("server=sca-mysql-test;database=student_cards;user=lnaedit;password=lnaedit;Convert Zero Datetime=True");
            //optionsBuilder.UseSqlServer("data source=sca-orporas-t;initial catalog=StudentCard;persist security info=True;user id=userwebclient;password=795525;MultipleActiveResultSets=True;App=EntityFramework&quot; ");

            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;initial catalog=testDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;");

            //real DB
            //optionsBuilder.UseSqlServer("data source=cl-mssql-1-ag,1433;initial catalog=StudentCard;persist security info=True;user id=Checks;password=checks;MultipleActiveResultSets=True;App=EntityFramework&quot; ");
        }
    }
}
