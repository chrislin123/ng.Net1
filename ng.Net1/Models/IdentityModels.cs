using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ng.Net1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser<int, CustomUserLogin, CustomUserRole,
    CustomUserClaim>
    {
        public string CName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User,int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<User, CustomRole,
    int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
            : base("applicationDB")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            modelBuilder.Entity<User>().ToTable("AdminUsers");
            modelBuilder.Entity<CustomRole>().ToTable("AdminRoles");
            modelBuilder.Entity<CustomUserRole>().ToTable("AdminUserRoles");
            modelBuilder.Entity<CustomUserClaim>().ToTable("AdminUserClaims");
            modelBuilder.Entity<CustomUserLogin>().ToTable("AdminUserLogins");
            modelBuilder.Entity<CustomRole>()
                .HasMany<AdminMenu>(r => r.AdminMenus)
                .WithMany(m => m.AdminRoles)
                .Map(rm =>
                {
                    rm.MapLeftKey("RoleId");
                    rm.MapRightKey("MenuId");
                    rm.ToTable("AdminRoleMenus");
                });
            
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole()
        {
            this.AdminMenus = new HashSet<AdminMenu>();
        }

        //public CustomRole() { }
        public CustomRole(string name) {
            this.Name = name;
            this.AdminMenus = new HashSet<AdminMenu>();
        }

        public ICollection<AdminMenu> AdminMenus { get; set; }
    }

    public class CustomUserStore : UserStore<User, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class AdminMenu
    {
        public AdminMenu()
        {
            AdminRoles = new HashSet<CustomRole>();
            isVisible = true;
            icon = "fa fa-file";
        }

        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public string path { get; set; }
        public string icon { get; set; }
        public bool isVisible { get; set; }
        public int? ParentId { get; set; }
        public int? orderSerial { get; set; }

        public ICollection<CustomRole> AdminRoles { get; set; }
    }
}