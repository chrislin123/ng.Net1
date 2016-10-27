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

namespace ng.Net1.Controllers
{
    /// <summary>
    /// 群組 API
    /// </summary>
    [RoutePrefix("api/Groups")]
    public class GroupsController : BaseApiController
    {
        public GroupsController()
        {
            this.TableName = "Groups";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("")]
        public dynamic Get(int type, string name = "", int id = 0)
        {
            var obj = from o in db.Groups
                      where o.type == type
                      select new { o.name, o.id };

            if (name != "")
            {
                if (id == 0)
                    obj = obj.Where(o => o.name == name);
                else
                    obj = obj.Where(o => o.name == name && o.id != id);
            }
            dynamic d = new ExpandoObject();
            d.items = obj;
            return d;


        }

        /// <summary>
        /// 新增社群群組資料
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public dynamic Post(dynamic value)
        {

            string name = value.name;
            int type = value.type;
            var query = db.Groups.Where(g => g.name == name && g.type == type).FirstOrDefault();
            if (query != null)
                throw new Exception("名稱重覆");

            //20160225 Kevin Lin Fix:沒做用了
            //string sql = "select ifnull(max(value),0) from Groups where type = @type order by value";

            //string type = value.type;
            //string maxvalue = db.ExecuteScale(sql, new { type = type }).ToString();
            //if (maxvalue == "0")
            //    maxvalue = "1";
            //else
            //    maxvalue = (Convert.ToInt16(maxvalue) * 2).ToString();
            string sql = "";
            string maxvalue = "0";

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
                    type = new DbString { Value = type.ToString(), IsAnsi = true }
                }) > 0;

            return InsertResult(b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        public dynamic Delete(int id)
        {
            var query = db.Groups.Where(g => g.id == id).FirstOrDefault();
            if (query == null)
                throw new Exception("找不到可刪除的資料");

            var sql = string.Format("select count(1) from `user_group` WHERE `group_id` =  @group_id", id);
            var so = new SqlObject(sql, new { groupd_id = id });
            string count = db.ExecuteScale(sql, new { group_id = id }).ToString();
            if (int.Parse(count) > 0)
                throw new Exception("該群組己被使用,不可刪除");

            sql = string.Format("select count(1) from `AdminUser_group` WHERE `group_id` =  @group_id", id);
            so = new SqlObject(sql, new { groupd_id = id });
            count = db.ExecuteScale(sql, new { group_id = id }).ToString();
            if (int.Parse(count) > 0)
                throw new Exception("該群組己被使用,不可刪除");


            sql = string.Format(@"delete from {0} where id = @id ", TableName);
            bool b = db_rd.Delete(sql, new { id = id }) > 0;
            return ExecuteResult(b);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}")]
        public dynamic Put(int id, dynamic value)
        {
            string name = value.name;
            int type = value.type;
            var query = db.Groups.Where(g => g.name == name && g.type == type && g.id != id).FirstOrDefault();
            if (query != null)
                throw new Exception("名稱重覆");

            var obj = db.Groups.Where(o => o.id == id).FirstOrDefault();
            if (obj == null)
                throw new Exception("找不到可更新的資料");
            else
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
