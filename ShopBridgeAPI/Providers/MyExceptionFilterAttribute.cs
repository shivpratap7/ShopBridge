using CrudAPI.Custom;
using CrudAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace CrudAPI.Providers
{
    public class MyExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                string requestBody = "";
                string SessionCode = "0";

                using (var stream = new MemoryStream())
                {
                    var context = (HttpContextBase)actionExecutedContext.Request.Properties["MS_HttpContext"];
                    context.Request.InputStream.Seek(0, SeekOrigin.Begin);
                    context.Request.InputStream.CopyTo(stream);
                    requestBody = Encoding.UTF8.GetString(stream.ToArray());
                }

                if (!(actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() || actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()))
                {
                    Token t = actionExecutedContext.Request.Properties[SiteConfig.LoginKeyName] as Token;
                    SessionCode = t.SessionCode.ToString();
                }
                StringBuilder sbHeader = new StringBuilder();
                sbHeader.AppendLine(actionExecutedContext.Request.Headers.ToString());
                sbHeader.AppendLine(actionExecutedContext.Request.Content.Headers.ToString());

                StaticGeneral.LogException(SessionCode, actionExecutedContext.Exception.Message, actionExecutedContext.Request.RequestUri.ToString(), requestBody, sbHeader.ToString());

                //log to database
                //actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                //{
                //    Content = new StringContent(JsonConvert.SerializeObject(new
                //    {
                //        message = "Encountered Internal Exception : " + actionExecutedContext.Exception.Message,
                //        exception = actionExecutedContext.Exception.GetType().Name
                //    }))
                //};
            }
            catch (Exception)
            {

            }
           
        }
    }
}