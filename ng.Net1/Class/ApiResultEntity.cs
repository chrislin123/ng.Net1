using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace ng.Net1
{
    public class ApiResultEntity
    {
        public HttpStatusCode Status { get; set; }

        public ApiStatusEnum ErrorCode { get; set; }

        public object Data { get; set; }

        public string ErrorMessage { get; set; }
    }
}