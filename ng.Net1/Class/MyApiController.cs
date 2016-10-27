using ng.Net1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace ng.Net1.Controllers
{
    public class BaseApiController : ApiController
    {
        protected DBContext_CRU db = new DBContext_CRU();
        protected DBContext_RD  db_rd = new DBContext_RD();
        //protected DBContext_CRU db_mssql = new DBContext_CRU();
        protected string TableName { get; set; }

        protected dynamic InsertResult(bool b,string message="")
        {
            dynamic d = new ExpandoObject();
            d.result = ((b) ? "新增成功" : "新增失敗") + message;
            return d;
        }
        protected dynamic InsertResult(IdentityResult result)
        {
            if (!result.Succeeded)
                throw new Exception(result.Errors.ToArray<string>()[0]);
            else
            {
                dynamic d = new ExpandoObject();
                d.result = "新增成功";
                return d;
            }
        }
        protected dynamic UpdateResult(bool b)
        {
            dynamic d = new ExpandoObject();
            d.result = (b) ? "更新成功" : "更新失敗";
            return d;
        }
        protected dynamic ExecuteResult(bool b)
        {
            dynamic d = new ExpandoObject();
            d.result = (b) ? "執行成功" : "執行失敗";
            return d;
        }
        protected string GetFuzzySearchCondition(string [] columns , string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return "";

            string[] a = keyword.Trim().Split(' ');
            StringBuilder sb = new StringBuilder();
            List<string> l = new List<string>();

            foreach (string k in a)
            {
                if (k.Trim() == "") continue;
                foreach (string col in columns)
                {
                    if (col.Trim() == "") continue;
                    sb.AppendFormat("{0} like '%{1}%' or ", col.Trim(), k.Trim());
                }

                if (sb.Length > 0)
                    l.Add(string.Format("({0})", sb.ToString(0, sb.Length - 3)));

                sb.Clear();
            }
            
            if (l.Count > 0)
                return string.Join(" and ", l.ToArray());
            else
                return "";
        }
        public static void writeLog(string s)
        {
            StreamWriter sw = new StreamWriter("c:/temp/log.txt", true, Encoding.GetEncoding("Big5"));
            sw.WriteLineAsync(s);
            sw.Close();
        }

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

        //public  void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }
        //}

    }


}
