using System;
using System.Web;

namespace TplNetCore.Web
{
    class HttpHeaderModule : IHttpModule
    {

        #region IHttpModule Members

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += new EventHandler(context_PreSendRequestHeaders);
        }

        void context_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            if (context.Request.Url.AbsoluteUri.Contains(".css?") || context.Request.Url.AbsoluteUri.Contains(".js?"))
            {
                context.Response.CacheControl = "public";
                context.Response.Cache.SetMaxAge(TimeSpan.FromDays(10));
            }
        }

        #endregion
    }
}