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
using System.Dynamic;
using Microsoft.AspNet.Identity;

namespace ng.Net1.Controllers
{
    [RoutePrefix("api/AdminRoles")]
    [AllowAnonymous]
    public class AdminRolesController : BaseApiController
    {
        public AdminRolesController()
        {
            this.TableName = "AdminRoles";
        }

        // GET: api/AdminUser
        //public List<dynamic> Get()
        public dynamic Get()
        {
            
            string sql = "select name , id from AdminRoles where name <> = 'anonymous' order by 1";

            //dynamic d = new ExpandoObject();
            //d.items = db.Query(sql);
            //return d;

            
        var roleManager = new RoleManager<CustomRole,int>(new CustomRoleStore(new ApplicationDbContext()));
        //var list = roleManager.Roles.ToArray();
        

            var obj = from o in roleManager.Roles
                      where o.Name != "anonymous"
                      select o;

            var list = obj.ToArray();
            //if (name != "")
            //{
            //    if (id == 0)//add
            //        obj = obj.Where(o => o.name == name);
            //    else//modify
            //        obj = obj.Where(o => o.name == name && o.id != id);
            //}

            dynamic d = new ExpandoObject();
            d.items = list;
            return d;
            


        }

        // POST: api/AdminUser
        [HttpPost]
        public dynamic Post(dynamic value)
        {
            string sql = "select ifnull(max(value),0) from Groups where type = @type order by value";

            string type = value.type;
            string maxvalue = db.ExecuteScale(sql, new { type = type }).ToString();
            if (maxvalue == "0")
                maxvalue = "1";
            else
                maxvalue = (Convert.ToInt16(maxvalue) * 2).ToString();


            sql = string.Format(
                @"INSERT INTO `Groups` 
                (`name`,`value`,`type`)
                VALUES 
                (@name,@value,@type);"
                , TableName);

            bool b = db.Execute(
                sql,
                new
                {
                    name = new DbString { Value = value.name, IsAnsi = true },
                    value = new DbString { Value = maxvalue.ToString(), IsAnsi = true },
                    type = new DbString { Value = type, IsAnsi = true }
                }) > 0;

            return InsertResult(b);
        }

        // DELETE: api/AdminUser/5
        [HttpDelete]
        [Route("{id:int}")]
        public dynamic Delete(int id)
        {
            string sql = string.Format(@"delete from {0} where id = @id ", TableName);
            bool b = db_rd.Delete(sql, new { id = id }) > 0;
            return ExecuteResult(b);

        }

        // PUT: api/ChatGroups/5
        [HttpPut]
        [Route("{id:int}")]
        public dynamic Put(int id, dynamic value)
        {
            var obj = db.Groups.Where(o => o.id == id).First();

            if (obj != null)
            {
                obj.name = value.name;
                obj.edit_user = User.Identity.Name;
                obj.edit_datetime = DateTime.Now;
            }

            bool b = db.SaveChanges() > 0;
            return UpdateResult(b);
        }

    }
}
