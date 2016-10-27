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
    [RoutePrefix("api/CheckPrivilege")]
    public class CheckPrivilegeController : BaseApiController
    {
        public dynamic Get(string functionName = "")
        {
            if (functionName == "/news-list")
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            else
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public dynamic Post(dynamic value)
        {
            if (value.functionName == "/news-list")
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            else
                return new HttpResponseMessage(HttpStatusCode.OK);
        }

        
    }
}
