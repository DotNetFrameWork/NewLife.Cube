﻿using System.Web;
using NewLife.Web;

namespace NewLife.Cube
{
    /// <summary>魔方处理模块</summary>
    public class CubeModule : IHttpModule
    {
        #region IHttpModule Members
        void IHttpModule.Dispose() { }

        /// <summary>初始化模块，准备拦截请求。</summary>
        /// <param name="context"></param>
        void IHttpModule.Init(HttpApplication context)
        {
            context.BeginRequest += (s, e) => OnRequest();
        }
        #endregion

        /// <summary>初始化模块，准备拦截请求。</summary>
        void OnRequest()
        {
            var set = Setting.Current;
            if (set.ForceSSL)
            {
                var ctx = HttpContext.Current;
                var req = ctx?.Request;
                if (req != null && !req.IsSecureConnection)
                {
                    // 有可能前端访问的是https，经反向代理后变成http
                    var uri = req.GetRawUrl();
                    if (!uri.Scheme.StartsWith("https"))
                    {
                        var url = $"https://{uri.Host}{uri.PathAndQuery}";

                        ctx.Response.Redirect(url);
                        //ctx.Response.RedirectPermanent(url);
                    }
                }
            }
        }
    }
}