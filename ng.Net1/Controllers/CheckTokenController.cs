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
using System.Text;
using System.IO;

namespace ng.Net1.Controllers
{
    [RoutePrefix("api/CheckToken")]
    public class CheckTokenController : BaseApiController
    {
        // GET: api/AdminUser
        //public List<dynamic> Get()
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // POST: api/AdminUser
        [HttpPost]
        public HttpResponseMessage Post(dynamic value)
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // POST: api/AdminUser
        [HttpPut]
        public HttpResponseMessage Put(dynamic value)
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}
