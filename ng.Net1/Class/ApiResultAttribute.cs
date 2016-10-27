using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Net.Http.Formatting;
using System.Web.Http.Results;
using ng.Net1.Controllers;
using System.Dynamic;

namespace ng.Net1
{
    public enum ApiStatusEnum
    {
        Success = 0,
        Failure = -1
    }


    public class ApiResultAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            // 若發生例外則不在這邊處理
            if (actionExecutedContext.Exception != null)
                return;

            base.OnActionExecuted(actionExecutedContext);

            Type t = null;
            if (actionExecutedContext.Response.Content != null)
            {
                t = actionExecutedContext.Response.Content.ReadAsAsync<Object>().Result.GetType();
                // 只有是自定的格式才轉換
                if (!
                    (
                    t.Equals(typeof(System.Dynamic.ExpandoObject))
                    )
                   )
                    return;
            }
            else
                return;
            object data = actionExecutedContext.Response.Content.ReadAsAsync<Object>().Result;

            int i = ((ICollection<KeyValuePair<string, Object>>)data).Count;

            if (i == 1)
                data = ((ICollection<KeyValuePair<string, Object>>)data).First().Value;

            ApiResultEntity result = new ApiResultEntity();

            // 取得由 API 返回的狀態碼
            result.ErrorCode = ApiStatusEnum.Success;

            // 取得由 API 返回的狀態
            //result.Status = ApiStatusEnum.Success.ToString();

            // 取得由 API 返回的資料
            //result.Data = actionExecutedContext.Response.Content.ReadAsAsync<Object>().Result;
            result.Data = data;

            // 重新封裝回傳格式
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(actionExecutedContext.ActionContext.Response.StatusCode, result);
        }
    }

}
