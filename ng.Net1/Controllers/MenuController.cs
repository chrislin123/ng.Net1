using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using ng.Net1.Models;
using Dapper;
using System.Net.Http.Formatting;
using System.Collections;
using System.Dynamic;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;

namespace ng.Net1.Controllers
{
    
    [RoutePrefix("api/Menu")]
    [AllowAnonymous]
    public class MenuController : BaseApiController
    { 
        // GET: api/AdminUser
        //public List<dynamic> Get()
        //[Route("{id:int}")]
        public dynamic Get(string loggedIn="false")
        {
            //if (type=="1")
            //   db.reset();


            var identity = (ClaimsIdentity)User.Identity;
            string sql;
            dynamic d = new ExpandoObject();
            //if (!identity.IsAuthenticated) {
            if (!Convert.ToBoolean(loggedIn)) { 
                sql = @"
                    select R2.* from AdminRoles M
                    join AdminRoleMenus R1 on M.id = R1.RoleId
                    join AdminMenus R2 on R1.MenuId = R2.Id
                    where R1.RoleId in (
                      select id from AdminRoles where name = 'anonymous' )
                    order by 1";
                d.result = db.Query(sql);
            }
            else
            {
                var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindByNameAsync(identity.Name);

                var userid = identity.FindFirst(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                sql = @"
                    select R2.* from AdminRoles M
                    join AdminRoleMenus R1 on M.id = R1.RoleId
                    join AdminMenus R2 on R1.MenuId = R2.Id
                    where R1.RoleId in (
                      select roleId from AdminUserRoles where userId = @userid)
                    order by 1";

                d.result = db.Query(sql, new { userid = userid });
            }

            return d;

        }


    }


}
