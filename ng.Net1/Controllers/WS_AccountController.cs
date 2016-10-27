using ng.Net1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data;
using ng.Net1.Models;

namespace ng.Net1.Controllers
{
    public class WS_AccountController : ApiController
    {
        //private DBContext db = new DBContext();
        private ApplicationDbContext db = new ApplicationDbContext();
        //HttpContext httpContext = new HttpContext(new Http

        public RoleManager<IdentityRole> RoleManager { get; private set; }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //Based on the OWIN authentication, finds the current authenticated username.        
        [System.Web.Http.ActionName("GetCurrentUserName")]
        public string GetCurrentUserName()
        {
            return Request.GetOwinContext().Authentication.User.Identity.Name;
        }

        //Based on the OWIN authentication, finds the current authenticated username.        
        [System.Web.Http.ActionName("GetCurrentUserId")]
        public string GetCurrentUserId()
        {
            return Request.GetOwinContext().Authentication.User.Identity.GetUserId();
        }

        //Based on the OWIN authentication, tells if the user is authenticated.
        //UNTESTED
        [System.Web.Http.ActionName("GetIsLoggedIn")]
        public bool GetIsLoggedIn()
        {
            return Request.GetOwinContext().Authentication.User.Identity.IsAuthenticated;
        }

        //Get the userID from the Request context, and pass the ID into the usermanager to get the current user roles.
        //UNTESTED
        async public Task<IEnumerable<string>> GetCurrentUserRoles()
        {
            return await UserManager.GetRolesAsync(Request.GetOwinContext().Authentication.User.Identity.GetUserId<int>());
        }

    }
}
