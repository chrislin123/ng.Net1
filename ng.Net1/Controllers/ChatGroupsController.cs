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
    /// 社群群組 API
    /// </summary>
    [RoutePrefix("api/ChatGroups")]
    public class ChatGroupsController : BaseApiController
    {
        public ChatGroupsController()
        {
            this.TableName = "ChatGroups";
        }

        /// <summary>
        /// 取得群組資料
        /// </summary>
        /// <param name="name">唯一名稱</param>
        /// <param name="id">唯一id</param>
        /// <remarks>經由唯一名稱或唯一id取得單一群組資料</remarks>
        /// <response code="404">Not found</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>1231456456789798</returns>
        [Route("")]
        public dynamic Get(string name = "",int id = 0)
        {
            var obj = from o in db.ChatGroups
                      select new { o.name, o.id };

            if (name != "")
            {
                if (id == 0)//add
                    obj = obj.Where(o => o.name == name);
                else//modify
                    obj = obj.Where(o => o.name == name && o.id != id);
            }
            dynamic d = new ExpandoObject();
            d.items = obj;
            return d;
        }

        /// <summary>
        /// 新增群組資料
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public dynamic Post(dynamic value)
        {
            string name = value.name;
            var query = db.ChatGroups.Where(g => g.name == name).FirstOrDefault();
            if (query != null)
                throw new Exception("名稱重覆");

            ChatGroup chatGroup = new ChatGroup();
            chatGroup.name = value.name;
            chatGroup.name = value.name;
            chatGroup.create_user = User.Identity.Name;
            
            db.ChatGroups.Add(chatGroup);
            bool b = db.SaveChanges() > 0;
            return InsertResult(b);
        }

        /// <summary>
        /// 刪除與 id 相符合的群組資料
        /// </summary>
        /// <param name="id">id為資料主鍵值</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        public dynamic Delete(int id)
        {
            var query = db.ChatGroups.Where(g => g.id == id).FirstOrDefault();
            if (query == null)
                throw new Exception("找不到可刪除的資料");

            var sql = string.Format("select count(1) from `user_chatgroup` WHERE `group_id` =  @group_id", id);
            var so = new SqlObject(sql, new { groupd_id = id });
            string count = db.ExecuteScale(sql, new { group_id = id }).ToString();
            if (int.Parse(count) > 0)
                throw new Exception("該群組己被使用,不可刪除");

            sql = string.Format("select count(1) from `AdminUser_chatgroup` WHERE `group_id` =  @group_id", id);
            so = new SqlObject(sql, new { groupd_id = id });
            count = db.ExecuteScale(sql, new { group_id = id }).ToString();
            if (int.Parse(count) > 0)
                throw new Exception("該群組己被使用,不可刪除");

            var obj = db_rd.ChatGroups.Where(o => o.id == id).First();
            db_rd.ChatGroups.Remove(obj);

            bool b = db_rd.SaveChanges() > 0;
            return ExecuteResult(b);
        }

        /// <summary>
        /// 更新與 id 相符合的群組資料
        /// </summary>
        /// <param name="id">id為資料主鍵值</param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}")]
        public dynamic Put(int id, dynamic value)
        {
            string name = value.name;
            var query = db.ChatGroups.Where(g => g.name == name).FirstOrDefault();
            if (query != null)
                throw new Exception("名稱重覆");

            var obj = db.ChatGroups.Where(o => o.id == id).FirstOrDefault();
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
