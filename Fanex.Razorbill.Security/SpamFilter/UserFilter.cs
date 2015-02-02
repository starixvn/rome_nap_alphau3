// ************************************************************************************
// <copyright file="UserFilter.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ************************************************************************************
// <history>
//     <change who="kan.nguyen">Create/Run StyleCop.</change>
// </history>
// ************************************************************************************
namespace Fanex.Razorbill.Security.SpamFilter
{
    using System.Web;
    using Fanex.Security.SpamFilter.Filter;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserFilter : IFilter
    {
        public string GetIdentity(HttpContext current)
        {
            var composite = current.Session[current.Session.SessionID + "_" + "USERNAME"];
            composite = composite ?? current.Request.UserHostAddress;
            var uid = string.Join("$", composite);
            return uid;
        }
    }
}