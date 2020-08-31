using CrudAPI.Custom;
using CrudAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CrudAPI.Providers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ApiAuthenticationFilter : AuthorizationFilterAttribute
    {
        private readonly bool _isActive = true;

        /// <summary>
        /// parameter isActive explicitly enables/disables this filter.
        /// </summary>
        /// <param name="isActive"></param>
        public ApiAuthenticationFilter(bool isActive)
        {
            _isActive = isActive;
        }

        /// <summary>
        /// Checks basic authentication request
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (!_isActive) return;

            try
            {
                string requestBody = "";
                using (var stream = new MemoryStream())
                {
                    var context = (HttpContextBase)filterContext.Request.Properties["MS_HttpContext"];
                    context.Request.InputStream.Seek(0, SeekOrigin.Begin);
                    context.Request.InputStream.CopyTo(stream);
                    requestBody = Encoding.UTF8.GetString(stream.ToArray());
                }
                StaticGeneral.LogRequests("1", requestBody, filterContext.Request.Headers.ToString(), 
                    filterContext.Request.Method.ToString(), filterContext.Request.RequestUri.ToString());
            }
            catch (Exception)
            {
            }
            
            //if (SkipAuthorization(filterContext)) return;
            
            //var identity = FetchAuthHeader(filterContext);
            //if (identity == null)
            //{
            //    ChallengeAuthRequest(filterContext);
            //    return;
            //}

            //if (!OnAuthorizeUser(identity))
            //{
            //    ChallengeAuthRequest(filterContext, identity.GetError());
            //    return;
            //}

            //HttpContext.Current.Items.Add("Login", identity);
            //filterContext.ActionArguments.Add("Login", identity);
      //      filterContext.Request.Properties.Add(SiteConfig.LoginKeyName, identity);
            base.OnAuthorization(filterContext);

        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        protected bool OnAuthorizeUser(Token t)
        {
            return (t.IsValid());
        }

        /// <summary>
        /// Checks for autrhorization header in the request and parses it, creates user credentials and returns as BasicAuthenticationIdentity
        /// </summary>
        /// <param name="filterContext"></param>
        //protected virtual Token FetchAuthHeader(HttpActionContext filterContext)
        //{
        //    if(!filterContext.Request.Headers.Contains(SiteConfig.AuthServiceHeaderName))
        //        return null;

        //    var token = filterContext.Request.Headers.GetValues(SiteConfig.AuthServiceHeaderName);
        //    if (token == null || token.Count() == 0)
        //    {
        //        return null;
        //    }

        //    string strDec = token.First();
        //    if (string.IsNullOrEmpty(strDec)) return null;
        //    if (strDec.Trim().Length == 0) return null;
        // //   Token r = new Token(strDec);
        //    //Token r = new Token(strDec);
            
        // //   return r;
        //}

        /// <summary>
        /// Send the Authentication Challenge request
        /// </summary>
        /// <param name="filterContext"></param>
        private static void ChallengeAuthRequest(HttpActionContext filterContext, string Message = "Access Token Not Found")
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.Content = new StringContent(Message);
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            filterContext.Response = response;
        }
    }
}