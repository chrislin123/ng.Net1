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
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ng.Net1.Controllers
{
    [RoutePrefix("api/AdminUser")]
    public class AdminUserController : BaseApiController
    {
        const string GroupTableName = "AdminUser_group";
        const string ChatGroupTableName = "AdminUser_chatgroup";
        const string AdminUserRolesTableName = "AdminUserRoles";

        public AdminUserController()
        {
            System.Diagnostics.Debug.WriteLine("Enter AdminUserController...");
            this.TableName = "AdminUsers";
        }

        // GET: api/AdminUser
        //public List<dynamic> Get()
        public dynamic Get(int currentPage = 1, int recordsPerPage = 15,string keyWord = "")
        {
            string sql = string.Format("select * from {0}", TableName);
            string sql_count = string.Format("select count(*) totalCount from {0}", TableName);
            string condition = "";
            string [] columns = { "UserName", "CName", "email", "groups" };
            string fuzzySearchCondition = GetFuzzySearchCondition(columns, keyWord);
            if (fuzzySearchCondition.Length > 0)
                condition += " and " + fuzzySearchCondition;

            if (condition != "")
            {
                sql += " where 1=1" + condition;
                sql_count += " where 1=1" + condition;
            }

            sql += " order by id";

            dynamic d = new ExpandoObject();
            d.count = db.QueryTotalCount(sql_count);
            d.items = db.Query(sql, currentPage, recordsPerPage);
            return d;
        }

        // GET: api/AdminUser/5
        [HttpGet]
        [Route("{id:int}")]
        public dynamic Get(int id)
        {            
            string sql = string.Format("select * from {0} where id = @id", TableName);
            List<dynamic> l =
                db.Query(sql, new { id = id });

            ArrayList al = new ArrayList();
            sql = string.Format("select group_id from {0} where user_id = @id", GroupTableName);
            List<dynamic> lGroup =
                db.Query(sql, new { id = id });
            foreach (dynamic group in lGroup)
                al.Add(group.group_id);

            var _l = l[0];
            _l.groups = (int[])al.ToArray(typeof(int));

            sql = string.Format("select group_id from {0} where user_id = @id", ChatGroupTableName);
            List<dynamic> lChatGroup =
                db.Query(sql, new { id = id });
            al = new ArrayList();
            foreach (dynamic group in lChatGroup)
                al.Add(group.group_id);
            _l.chatgroups = (int[])al.ToArray(typeof(int));

            sql = string.Format(@"select Name from AdminUserRoles M
                                  join AdminRoles R1 on M.RoleId = R1.id
                                  where UserId = @UserID", new {UserID =id} );

            object o = db.ExecuteScale(sql, new { UserId = id});
            string role = (o == null) ? "admin" : o.ToString();
            _l.role = role;

            dynamic d = new ExpandoObject();
            d.result = _l;
            return d;
        }

        // POST: api/AdminUser
        [HttpPost]
        public async Task<ExpandoObject> Post(dynamic value)
        {


            /*
            string sql = string.Format(
                @"INSERT INTO `AdminUsers` 
                (`UserName`,`password`,`CName`,`email`) 
                VALUES 
                (@UserName,@password,@CName,@email);
                SELECT CAST(LAST_INSERT_ID() as int)"
                , TableName);

            SqlObject firstSO = new SqlObject(sql, new
            {
                UserName = new DbString { Value = value.UserName, IsAnsi = true },
                password = new DbString { Value = value.password, IsAnsi = true },
                CName = new DbString { Value = value.CName, IsAnsi = true },
                email = new DbString { Value = value.email, IsAnsi = true }
            });

            ArrayList al = new ArrayList();
            string sql_group_insert =
                string.Format(
                    "INSERT INTO `{0}` (`user_id`,`group_id`) VALUES (@user_id,@group_id)"
                    , GroupTableName);

            SqlObject so;
            if (value.groups != null)
            {
                foreach (var v in value.groups)
                {
                    so = new SqlObject();
                    so.sql = sql_group_insert;
                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("group_id",v, System.Data.DbType.Int16);
                    so.param = dp;
                    al.Add(so);
                }
            }

            bool b = db.InsertMasterAndDetail(firstSO, al,"user_id") > 0;
            return InsertResult(b);
            */

            //var o = JObject.Parse(value);
            //var userName = (string)o["userName"];
            //var email = (string)o["userName"];
            var user = new User {
                UserName = ((JObject)value).GetValue("userName").ToString(),
                Email = ((JObject)value).GetValue("email").ToString(),
                CName = ((JObject)value).GetValue("cName").ToString(),
            };
            //var result = UserManager.CreateAsync(user, value.password);

            var result = await UserManager.CreateAsync(user, ((JObject)value).GetValue("password").ToString());
            //var result = ApplicationUserManager.Create(user, value.password);
            if (result.Succeeded)
            {
                var current = UserManager.FindByNameAsync(user.UserName);
                var roleResult = UserManager.AddToRoleAsync(current.Result.Id, ((JObject)value).GetValue("role").ToString());

                ArrayList al = new ArrayList();
                string sql_group_insert =
                    string.Format(
                        "INSERT INTO `{0}` (`user_id`,`group_id`) VALUES (@user_id,@group_id)"
                        , GroupTableName);  

                SqlObject so;
                if (value.groups != null)
                {
                    foreach (var v in value.groups)
                    {
                        so = new SqlObject();
                        so.sql = sql_group_insert;
                        DynamicParameters dp = new DynamicParameters();
                        dp.Add("user_id", current.Result.Id, System.Data.DbType.Int16);
                        dp.Add("group_id", v, System.Data.DbType.Int16);
                        so.param = dp;
                        al.Add(so);
                    }
                }


                string sql_chatgroup_insert =
                    string.Format(
                        "INSERT INTO `{0}` (`user_id`,`group_id`) VALUES (@user_id,@group_id)"
                        , ChatGroupTableName);
                
                if (value.chatgroups != null)
                {
                    foreach (var v in value.chatgroups)
                    {
                        so = new SqlObject();
                        so.sql = sql_chatgroup_insert;
                        DynamicParameters dp = new DynamicParameters();
                        dp.Add("user_id", current.Result.Id, System.Data.DbType.Int16);
                        dp.Add("group_id", v, System.Data.DbType.Int16);
                        so.param = dp;
                        al.Add(so);
                    }
                }
                bool b = db.Insert(al) > 0;
                

                //wait SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                //return RedirectToAction("Index", "Home");
            }
            //else
            //AddErrors(result);

            //return InsertResult(result.Succeeded);
            //return Request.CreateResponse(HttpStatusCode.OK);
            return InsertResult(result);
        }

        // PUT: api/AdminUser/5
        [HttpPut]
        [Route("{id:int}")]
        public dynamic Put(int id, dynamic value)
        {
           
            string sql = string.Format(
                @"update {0} set 
                        CName = @CName,
                        email = @email
                        where id = @id", TableName);
            SqlObject so = new SqlObject(sql, new
            {
                CName = new DbString { Value = value.cName, IsAnsi = true },
                email = new DbString { Value = value.email, IsAnsi = true },
                id = id
            });
            ArrayList al = new ArrayList();
            al.Add(so);

            string sql_adminuserrole_delete = string.Format("DELETE FROM `{0}` WHERE `UserId` =  @UserId", AdminUserRolesTableName);
            string sql_adminuserrole_insert = string.Format("INSERT INTO `{0}` (`UserId`,`RoleId`) VALUES (@UserId,@RoleId)", AdminUserRolesTableName);
            string sql_group_delete = string.Format("DELETE FROM `{0}` WHERE `user_id` =  @user_id", GroupTableName);
            string sql_group_insert = string.Format("INSERT INTO `{0}` (`user_id`,`group_id`) VALUES (@user_id,@group_id)", GroupTableName);
            string sql_chatgroup_delete = string.Format("DELETE FROM `{0}` WHERE `user_id` =  @user_id", ChatGroupTableName);
            string sql_chatgroup_insert = string.Format("INSERT INTO `{0}` (`user_id`,`group_id`) VALUES (@user_id,@group_id)", ChatGroupTableName);

            so = new SqlObject(sql_adminuserrole_delete, new { UserId = id });
            al.Add(so);
            so = new SqlObject(sql_group_delete, new { user_id = id });
            al.Add(so);
            so = new SqlObject(sql_chatgroup_delete, new { user_id = id });
            al.Add(so);

            int roleid = int.Parse(db.ExecuteScale("select id from AdminRoles where name = @name", new { name = value.role }).ToString());
            so = new SqlObject(sql_adminuserrole_insert, new
            {
                UserId = id,
                RoleId = roleid
            });
            al.Add(so);

            if (value.groups != null)
            {
                foreach (var v in value.groups)
                {
                    so = new SqlObject(sql_group_insert, new
                    {
                        user_id = id,
                        group_id = v
                    });
                    al.Add(so);
                }
            }

            if (value.chatgroups != null)
            {
                foreach (var v in value.chatgroups)
                {
                    so = new SqlObject(sql_chatgroup_insert, new
                    {
                        user_id = id,
                        group_id = v
                    });
                    al.Add(so);
                }
            }

            bool b = db_rd.Execute(al) > 0;

            return UpdateResult(b);
        }

        // DELETE: api/AdminUser/5
        [HttpDelete]
        [Route("{id:int}")]
        public dynamic Delete(int id)
        {
            ArrayList al = new ArrayList();
            SqlObject so;
            string sql_group_delete = string.Format("DELETE FROM `AdminUser_group` WHERE `user_id` =  @user_id", GroupTableName);
            so = new SqlObject(sql_group_delete, new
            {
                user_id = id
            });
            al.Add(so);

            string sql = string.Format(@"delete from {0} where id = @id", this.TableName);
            so = new SqlObject(sql, new
            {
                id = id
            });
            al.Add(so);
            bool b = db_rd.Execute(al) > 0;
            return ExecuteResult(b);
        }
    }

    public class MyCollection : System.Collections.Specialized.ListDictionary
    {
        public MyCollection() { }

        public MyCollection(object o)
        {
            this.Add("o", o);
        }
    }
}
