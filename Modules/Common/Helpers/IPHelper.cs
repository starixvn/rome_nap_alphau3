// --------------------------------------------------------------------------
// <copyright file="IPHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Razorbill.Module.Common.Helpers
{
    using System.Web;
    
    /// <summary>
    /// IP Helper.
    /// </summary>
    public class IPHelper
    {
        public static string CustomerIp()
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return ip.Equals("::1") ? "127.0.0.1" : (string.IsNullOrEmpty(ip) ? "127.0.0.1" : ip.Split(',')[0]);
        }
    }
}