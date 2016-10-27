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


namespace ng.Net1.Controllers
{
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : BaseApiController
    { 
        // GET: api/AdminUser
        //public List<dynamic> Get()
        [Route("{id:int}")]
        public dynamic Get(int id = 1)
        {
            if (id == 1)
            {
                string sql = @"
                select 'today_frontend_log_count' as name,count(1) cnt from frontend_log where DATE_FORMAT(`date`,'%Y-%m-%d') = DATE_FORMAT(CURDATE() ,'%Y-%m-%d') 
                union
                select 'frontend_log_totalcount',count(1) cnt from frontend_log 
                union
                select 'user_totalcount',count(1) cnt from user
                union
                select 'adminuser_totalcount',count(1) cnt from AdminUsers";
                List<dynamic> l = db.Query(sql);
                dynamic d = new ExpandoObject();
                for (int i = 0; i < l.Count; i++)
                {
                    string name = l[i].name;
                    switch (name)
                    {
                        case "today_frontend_log_count":
                            d.today_frontend_log_count = l[i].cnt;
                            break;
                        case "frontend_log_totalcount":
                            d.frontend_log_totalcount = l[i].cnt;
                            break;
                        case "user_totalcount":
                            d.user_totalcount = l[i].cnt;
                            break;
                        case "adminuser_totalcount":
                            d.adminuser_totalcount = l[i].cnt;
                            break;
                        default:
                            break;
                    }
                }
                return d;
            }

            return null;
        }


    }


}
