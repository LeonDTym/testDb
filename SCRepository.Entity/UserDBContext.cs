using Microsoft.EntityFrameworkCore;
using SCRepository.Entity.Models.UserModels;

namespace SCRepository.Entity
{
    public class UserDBContext : DbContext
    {
        public DbQuery<UserInfoModel> UserInfoModel { get; set; }
        public DbQuery<UserEtcData> UserEtcDatas { get; set; }

        public UserDBContext()
        {
            Database.SetCommandTimeout(180);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //test DB
            optionsBuilder.UseMySQL("server=sca-mysql-test;database=users;user=users_user;password=123123;Convert Zero Datetime=True");
            //optionsBuilder.UseSqlServer("data source=sca-orporas-t;initial catalog=StudentCard;persist security info=True;user id=userwebclient;password=795525;MultipleActiveResultSets=True;App=EntityFramework&quot; ");
            //real DB
            //optionsBuilder.UseMySQL("server=cl-sql;database=users;user=users_user;password=754ba376072ee7f0f0474f67005ac25b;Convert Zero Datetime=True");
        }
    }
}
