using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using System.Linq;
using System.Transactions;
using System.Collections;

namespace ng.Net1.Models
{
    /*
    public class User : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<ClaimsIdentity> CreateAsync(IdentityUser user, string authenticationType)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.NameIdentifier, ClaimTypes.Role);
            claimsIdentity.AddClaim(new Claim("ID",user.Id.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), "http://www.w3.org/2001/XMLSchema#string"));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName, "http://www.w3.org/2001/XMLSchema#string"));
            claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"));
            return claimsIdentity;
        }


        public string firstName { get; set; }

        public virtual List<todoItem> todoItems { get; set; }
    }
    */

    public class todoItem
    {
        [Key]
        public int id { get; set; }
        public string task { get; set; }
        public bool completed { get; set; }
    }

    /*
    public class DBContext : IdentityDbContext<User>
    {
        public DBContext() : base("applicationDB")
        {

        }
        //Override default table names
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static DBContext Create()
        {
            return new DBContext();
        }

        //public DbSet<todoItem> todos { get; set; }
    }
    */

    public class BaseDBContext : System.Data.Entity.DbContext
    {
        public BaseDBContext(string name= "applicationDB")
            :base(name)
        { }
        
        public DbSet<ChatGroup> ChatGroups { get; set; }

        public DbSet<Group> Groups { get; set; }

        //public DbSet<AdminUser> AdminUsers { get; set; }

        //public DbSet<Role> Roles { get; set; }

        //public DbSet<Menu> Menus { get; set; }

        //public void reset()
        //{
        //    /*
        //    { name: "儀表版", icon:"fa fa-dashboard", path:"#/dashboard", isVisible: true },
        //    { name: "院方人員帳號管理", icon: "fa fa-file", path: "#/adminuser-list", isVisible: true },
        //    { name: "APP使用者帳號管理", icon: "fa fa-file", path: "#/user-list", isVisible: true },
        //    { name: "最新消息維護", icon: "fa fa-file", path: "#/news-list", isVisible: true },
        //    { name: "APP傳輸記錄查詢", icon: "fa fa-table", path: "#/frontend_log", isVisible: true },
        //    { name: "院方傳輸記錄查詢", icon: "fa fa-table", path: "#/hospital_log", isVisible: true },
        //    { name: "群組資料維護", icon: "fa fa-sitemap", path: "#/groups", isVisible: true },
        //    { name: "社群群組資料維護", icon: "fa fa-sitemap", path: "#/chatgroups", isVisible: true },
        //    { name: "版本修定記錄", icon: "fa fa-edit", path: "#/release", isVisible: true },
        //    { name: "關於長安醫院", icon: "fa fa-qrcode", path: "#/about", isVisible: true },
        //    */



        //    var manager = new ApplicationUserManager(new UserStore<User>(this));
            
        //    var r1 = new Role() { name = "admin" };
        //    r1.Menus.Add(new Menu() { name = "儀表版", path = "#/dashboard", icon = "fa fa-dashboard" });
        //    r1.Menus.Add(new Menu() { name = "院方人員帳號管理", path = "#/dashboard" });
        //    r1.Menus.Add(new Menu() { name = "APP使用者帳號管理", path = "#/user-list" });
        //    var mNews = new Menu() { name = "最新消息維護", path = "#/news-list" };
        //    r1.Menus.Add(mNews);
        //    r1.Menus.Add(new Menu() { name = "APP傳輸記錄查詢", path = "#/frontend_log", icon = "fa fa-table" });
        //    r1.Menus.Add(new Menu() { name = "院方傳輸記錄查詢", path = "#/hospital_log", icon = "fa fa-table" });
        //    r1.Menus.Add(new Menu() { name = "群組資料維護", path = "#/groups", icon = "fa fa-sitemap" });
        //    r1.Menus.Add(new Menu() { name = "社群群組資料維護", path = "#/chatgroups", icon = "fa fa-sitemap" });
        //    r1.Menus.Add(new Menu() { name = "版本修定記錄", path = "#/release", icon = "fa fa-edit" });
        //    var mAbout = new Menu() { name = "關於長安醫院", path = "#/about", icon = "fa fa-qrcode" };
        //    r1.Menus.Add(mAbout);
        //    r1.AdminUsers.Add(new AdminUser() { name = "系統管理者" , account="admin" , password = manager.PasswordHasher.HashPassword("admin") });
        //    this.Roles.Add(r1);

        //    var r2 = new Role() { name = "anonymous" };
        //    r2.Menus.Add(mAbout);
        //    this.Roles.Add(r2);

        //    var r3 = new Role() { name = "role1" };
        //    r3.Menus.Add(mNews);
        //    r3.Menus.Add(mAbout);
        //    r3.AdminUsers.Add(new AdminUser() { name = "user1", account = "user1" , password = manager.PasswordHasher.HashPassword("user1") });
        //    this.Roles.Add(r3);

        //    this.SaveChanges();
        //}
    }

    public class DBContext_CRU : BaseDBContext
    {
        public DBContext_CRU() : base("falcota_mysqlDB_CRU")
        {
            
        }
        //Override default table names
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}

        //public DbSet<todoItem> todos { get; set; }

        public List<dynamic> Query(string sql)
        {
            using (var cn = new MySql.Data.MySqlClient.MySqlConnection(this.Database.Connection.ConnectionString))
            {
                return cn.Query(sql).ToList();
            }
        }

        public List<dynamic> Query(string sql,object param)
        {
            using (var cn = new MySql.Data.MySqlClient.MySqlConnection(this.Database.Connection.ConnectionString))
            {
                return cn.Query(sql,param).ToList();
            }
        }

        public long QueryTotalCount(string sql)
        {
            using (var cn = new MySql.Data.MySqlClient.MySqlConnection(this.Database.Connection.ConnectionString))
            {
                return cn.Query(sql).First().totalCount;
            }
        }

        public int Execute(string sql, object param)
        {
            using (var cn = new MySql.Data.MySqlClient.MySqlConnection(this.Database.Connection.ConnectionString))
            {
                return cn.Execute(sql, param);
            }
        }

        public int Execute(ArrayList al)
        {
            int i = 0;
            using (var cn = new MySql.Data.MySqlClient.MySqlConnection(this.Database.Connection.ConnectionString))
            {
                using (var transactionScope = new TransactionScope())
                {
                    foreach (SqlObject d in al)
                    {
                        i += cn.Execute(d.sql, d.param);
                    }
                    transactionScope.Complete();
                    
                }
            }
            return i;
        }

        public int InsertMasterAndDetail(SqlObject firstSO,ArrayList secondSOs,string masterIdInDetailColumnName)
        {
            int i = 0;
            using (var cn = new MySql.Data.MySqlClient.MySqlConnection(this.Database.Connection.ConnectionString))
            {
                using (var transactionScope = new TransactionScope())
                {
                    int id =cn.Query<int>(firstSO.sql,firstSO.param).Single();

                    foreach (SqlObject so in secondSOs)
                    {
                        DynamicParameters dp = (DynamicParameters)so.param;
                        dp.Add(masterIdInDetailColumnName, id, System.Data.DbType.Int16);

                        i += cn.Execute(so.sql, dp);
                    }
                    transactionScope.Complete();
                }
            }
            return i;
        }

        public int Insert(ArrayList alSqlObjectl)
        {
            int i = 0;
            using (var cn = new MySql.Data.MySqlClient.MySqlConnection(this.Database.Connection.ConnectionString))
            {
                using (var transactionScope = new TransactionScope())
                {
                    foreach (SqlObject so in alSqlObjectl)
                    {
                        DynamicParameters dp = (DynamicParameters)so.param;
                        i += cn.Execute(so.sql, dp);
                    }
                    transactionScope.Complete();
                }
            }
            return i;
        }


        public object ExecuteScale(string sql, object param)
        {
            using (var cn = new MySql.Data.MySqlClient.MySqlConnection(this.Database.Connection.ConnectionString))
            {
                return cn.ExecuteScalar(sql, param);
            }
        }

        public List<dynamic> Query(string sql, int currentPage, int recordsPerPage)
        {
            using (var cn = new MySql.Data.MySqlClient.MySqlConnection(this.Database.Connection.ConnectionString))
            {

                string limit = " LIMIT {0},{1}";
                limit = string.Format(
                    limit,
                    ((currentPage - 1) < 0 ? 0 : (currentPage - 1) * recordsPerPage),
                    recordsPerPage);
                return cn.Query(sql + limit).ToList();
            }
        }
    }


    public class DBContext_RD : BaseDBContext
    {
        public DBContext_RD() : base("falcota_mysqlDB_RD")
        {

        }
        //Override default table names
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}

        public int Delete(string sql, object param)
        {
            using (var cn = new MySql.Data.MySqlClient.MySqlConnection(this.Database.Connection.ConnectionString))
            {
                return cn.Execute(sql, param);
            }
        }


        public int Execute(ArrayList al)
        {
            int i = 0;
            using (var cn = new MySql.Data.MySqlClient.MySqlConnection(this.Database.Connection.ConnectionString))
            {
                using (var transactionScope = new TransactionScope())
                {
                    foreach (SqlObject d in al)
                    {
                        i += cn.Execute(d.sql, d.param);
                    }
                    transactionScope.Complete();
                }
            }
            return i;
        }
    }
    /*
    public class DBContext_MSSQL : BaseDBContext
    {
        public DBContext_MSSQL() : base("falcota_mssqlDB")
        {

        }

        public List<dynamic> Query(string sql)
        {
            using (var cn = new System.Data.SqlClient.SqlConnection (this.Database.Connection.ConnectionString))
            {
                return cn.Query(sql).ToList();
            }
        }

        public List<dynamic> Query(string sql, object param)
        {
            using (var cn = new System.Data.SqlClient.SqlConnection(this.Database.Connection.ConnectionString))
            {
                return cn.Query(sql, param).ToList();
            }
        }

    }
    */
    //This function will ensure the database is created and seeded with any default data.
    /*
    public class DBInitializer : CreateDatabaseIfNotExists<DBContext>
    {
        protected override void Seed(DBContext context)
        {
            //Create an seed data you wish in the database.
        }
    }
    */
    public class SqlObject
    {
        public SqlObject(){}
        public SqlObject(string sql, object param)
        {
            this.sql = sql;
            this.param = param;
        }
        public SqlObject(string sql, DynamicParameters dp)
        {
            this.sql = sql;
            this.param = param;
        }
        public string sql { get; set; }
        public object param { get; set; }
    }
}

