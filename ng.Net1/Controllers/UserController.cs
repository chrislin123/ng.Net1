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
using System.Collections.Specialized;
using System.Dynamic;

namespace ng.Net1.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : BaseApiController
    {
        const string GroupTableName = "user_group";
        const string ChatGroupTableName = "user_chatgroup";

        public UserController()
        {
            this.TableName = "user";
        }
        // GET: api/AdminUser
        //public List<dynamic> Get()
        public dynamic Get(int currentPage = 1, int recordsPerPage = 15, string keyWord = "")
        {
            //20160422 Kevin Lin Fix : groups
            //string sql = "select id,social_id,password,salt,name,user_key,nickname,phone,groups from user";
            string sql = "select id,social_id,password,salt,name,user_key,nickname,phone from user";
            string sql_count = "select count(*) totalCount from user";
            string condition = "";

            string[] columns = { "social_id", "name", "nickname", "phone" };
            string fuzzySearchCondition = GetFuzzySearchCondition(columns, keyWord);
            if (fuzzySearchCondition.Length > 0)
                condition += " and " + fuzzySearchCondition;

            if (condition != "")
            {
                sql       += " where 1=1" + condition;
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
            //string sql = "select id,social_id,password,salt,name,user_key,nickname,phone,groups from user where id = @id";
            string sql = "select id,social_id,password,salt,name,user_key,nickname,phone from user where id = @id";
            //sql = string.Format(sql, id);
            List<dynamic> l =
                db.Query(sql, new { id = id });
            /*
            if (l.Count > 0)
                return l[0];
            else
                return null;
                */
            sql = string.Format("select group_id from {0} where user_id = @id", GroupTableName);
            List<dynamic> lg =
                db.Query(sql, new { id = id });
            ArrayList al = new ArrayList();
            foreach (dynamic _lg in lg)
                al.Add(_lg.group_id);

            var _l = l[0];
            _l.groups = (int[])al.ToArray(typeof(int));

            sql = string.Format("select group_id from {0} where user_id = @id", ChatGroupTableName);
            List<dynamic> lChatGroup =
                db.Query(sql, new { id = id });
            al = new ArrayList();
            foreach (dynamic group in lChatGroup)
                al.Add(group.group_id);
            _l.chatgroups = (int[])al.ToArray(typeof(int));


            //string group = Convert.ToString(_l.groups);
            //int iGroup = 0;
            //int.TryParse(group, out iGroup);
            //var aGroup = Convert.ToString(iGroup, 2).ToArray();
            //ArrayList al = new ArrayList();
            //int iStart = 1;
            //int iLoop = 1;
            //for (int i = aGroup.Length - 1; i >= 0; i--)
            //{
            //    if (iLoop > 1)
            //        iStart = iStart * 2;
            //    else
            //        iStart = 1;
            //    iLoop++;
            //    if (aGroup[i] == '1')
            //        al.Add(iStart);
            //}

            //_l.groups = al.ToArray();
            dynamic d = new ExpandoObject();
            d.result = _l;
            return d;

        }

        // POST: api/AdminUser
        [HttpPost]
        public void Post(object value)
        {
            //var a = 1;
        }

        // PUT: api/AdminUser/5
        [HttpPut]
        [Route("{id:int}")]
        public dynamic Put(int id, dynamic value)
        {
            /*
            string sql = @"update user set 
                        name = @name,
                        nickname = @nickname,
                        phone = @phone,
                        groups = @groups,
                        date = current_timestamp
                        where id = @id";
            int g = 0;
            if (value.groups != null)
            {
                foreach (var v in value.groups)
                    g += (int)v;
            }
            bool b = db.Execute(
                sql,
                new{
                    name = new DbString { Value = value.name,IsAnsi = true} ,
                    nickname = new DbString { Value = value.nickname, IsAnsi = true },
                    phone = value.phone,
                    groups = g,
                    id = value.id
                }) > 0;

            return UpdateResult(b);
            */
            string sql = @"update user set 
                        name = @name,
                        nickname = @nickname,
                        phone = @phone,
                        date = current_timestamp
                        where id = @id";

            SqlObject so = new SqlObject(sql, new
            {
                name = new DbString { Value = value.name, IsAnsi = true },
                nickname = new DbString { Value = value.nickname, IsAnsi = true },
                phone = value.phone,
                id = id
            });
            ArrayList al = new ArrayList();
            al.Add(so);
            
            string sql_group_delete = string.Format("DELETE FROM `{0}` WHERE `user_id` =  @user_id", GroupTableName);
            string sql_group_insert = string.Format("INSERT INTO `{0}` (`user_id`,`group_id`) VALUES (@user_id,@group_id)", GroupTableName);
            string sql_chatgroup_delete = string.Format("DELETE FROM `{0}` WHERE `user_id` =  @user_id", ChatGroupTableName);
            string sql_chatgroup_insert = string.Format("INSERT INTO `{0}` (`user_id`,`group_id`) VALUES (@user_id,@group_id)", ChatGroupTableName);

            so = new SqlObject(sql_group_delete, new { user_id = id });
            al.Add(so);
            so = new SqlObject(sql_chatgroup_delete, new { user_id = id });
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

            bool b = db_rd.Execute(al)> 0;

            return UpdateResult(b);
            


        }

        // DELETE: api/AdminUser/5
        [HttpDelete]
        [Route("{id:int}")]
        public dynamic Delete(int id)
        {
            ArrayList al = new ArrayList();
            SqlObject so;
            string sql_group_delete = "DELETE FROM `user_group` WHERE `user_id` =  @user_id";
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
    



}
