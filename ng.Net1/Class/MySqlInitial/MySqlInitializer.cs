using ng.Net1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace WebApplication6
{
    public class MySqlInitializer : IDatabaseInitializer<ApplicationDbContext>
    {
        public void InitializeDatabase(ApplicationDbContext context)
        {
            //if (!context.Database.Exists())
            //{
            //    // if database did not exist before - create it
            //    context.Database.Create();
            //}
            //else
            //{
            //    // query to check if MigrationHistory table is present in the database 
            //    var migrationHistoryTableExists = ((IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<int>(
            //    string.Format(
            //      "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{0}' AND table_name = '__MigrationHistory'",
            //      "identityMySQLDB3"));

            //    // if MigrationHistory table is not there (which is the case first time we run) - create it
            //    if (migrationHistoryTableExists.FirstOrDefault() == 0)
            //    {
            //        context.Database.Delete();
            //        context.Database.Create();
            //    }
            //}

            //Seed(context);
        }
        /*
        protected void Seed(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser,int>(new ApplicationUserStore(new ApplicationDbContext()));
            var roleManager = new RoleManager<ApplicationRole, int>(new RoleStore<ApplicationRole, int, ApplicationUserRole>(new ApplicationDbContext()));
            const string name = "admin@example.com";
            const string password = "Password";
            const string roleName = "Admin";

            var role1 = new ApplicationRole(roleName);
            var roleresult1 = roleManager.Create(role1);

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }
            

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }
        */
    }
}