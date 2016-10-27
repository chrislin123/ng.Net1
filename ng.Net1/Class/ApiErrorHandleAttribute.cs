using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ng.Net1
{
    public class ApiErrorHandleAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        public override void OnException(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
                Elmah.ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception);

            base.OnException(actionExecutedContext);

            // 取得發生例外時的錯誤訊息
            var errorMessage = actionExecutedContext.Exception.Message;

            var result = new ApiResultEntity()
            {
                Status = HttpStatusCode.BadRequest,
                ErrorCode = ApiStatusEnum.Failure,
                ErrorMessage = errorMessage
            };

            // 重新打包回傳的訊息
            actionExecutedContext.Response = actionExecutedContext.Request
                .CreateResponse(result.Status, result);
        }
    }


    /*public class ApiErrorHandleAttribute : HandleErrorAttribute, IExceptionFilter
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //If message is null or empty, then fill with generic message
            var errorMessage = filterContext.Exception.Message;

            //Set the response status code to 500
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //Needed for IIS7.0
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            var result = new ApiResultEntity()
            {
                //Status = ApiStatusEnum.Failure.ToString(),
                ErrorMessage = errorMessage
            };

            filterContext.Result = new JsonResult
            {
                Data = result,
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            //Let the system know that the exception has been handled
            filterContext.ExceptionHandled = true;
        }
    }
    */
}