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
    [RoutePrefix("api/FrontendLog")]
    public class FrontendLogController : BaseApiController
    {
        public FrontendLogController()
        {
            this.TableName = "frontend_log";
        }

        // GET: api/AdminUser
        //public List<dynamic> Get()
        public dynamic Get(int currentPage = 1, int recordsPerPage = 15, int datetimeRange = 1, string keyWord = "")
        {
            string sql       = string.Format("select * from {0}",this.TableName);
            string sql_count = string.Format("select count(*) totalCount from {0}", this.TableName);
            string condition = "";

            string[] columns = { "path", "method", "param", "response", "statuscode" };
            string fuzzySearchCondition = GetFuzzySearchCondition(columns, keyWord);
            if (fuzzySearchCondition.Length > 0)
                condition += " and " + fuzzySearchCondition;

            if (datetimeRange < 1)
                condition += string.Format(" and (date > CURDATE() + INTERVAL {0} DAY) ", datetimeRange);

            if (condition!="")
            {
                sql       += " where 1=1 " + condition;
                sql_count += " where 1=1 " + condition;
            }

            dynamic d = new ExpandoObject();
            d.count = db.QueryTotalCount(sql_count);
            d.items = db.Query(sql, currentPage, recordsPerPage);
            return d;
        }
        
    }
}
