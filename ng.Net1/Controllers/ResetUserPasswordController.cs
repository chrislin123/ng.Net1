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
    [RoutePrefix("api/ResetUserPassword")]
    public class ResetUserPasswordController : BaseApiController
    {
        // Put: api/ResetUserPassword
        [HttpPut]
        [Route("{id:int}")]
        public dynamic Put(int id, dynamic value)
        {

            string targetUrl = value.url;
            string parame = string.Format("user_id={0}&password={1}", id, value.password);
            byte[] postData = Encoding.UTF8.GetBytes(parame);

            HttpWebRequest request = HttpWebRequest.Create(targetUrl) as HttpWebRequest;
            request.Method = "PUT";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 30000;
            request.ContentLength = postData.Length;
            request.Headers.Add("token", value.token.ToString());
            // 寫入 Post Body Message 資料流
            using (Stream st = request.GetRequestStream())
            {
                st.Write(postData, 0, postData.Length);
            }

            string result = "";
            // 取得回應資料
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            }
            dynamic d = new ExpandoObject();
            d.data = result;
            return d;
        }
    }
}
