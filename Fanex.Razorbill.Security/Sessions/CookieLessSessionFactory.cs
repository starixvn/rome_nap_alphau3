// ************************************************************************************
// <copyright file="CookieLessSessionFactory.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ************************************************************************************
// <history>
//     <change who="kan.nguyen">Create/Run StyleCop.</change>
// </history>
// ************************************************************************************
namespace Fanex.Razorbill.Security.Sessions
{
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class CookieLessSessionFactory
    {
        public static ISecurityCodeSession SecurityCodeSession 
        {
            get
            {
                return new SecurityCodeSession(HttpContext.Current.Session.SessionID);
            }
        }
    }
}
