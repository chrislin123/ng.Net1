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
    [RoutePrefix("api/HospitalLog")]
    public class HospitalLogController : FrontendLogController
    {
        public HospitalLogController()
        {
            this.TableName = "hospital_log";
        }
    }
}
