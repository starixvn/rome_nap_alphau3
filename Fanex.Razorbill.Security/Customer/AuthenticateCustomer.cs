// -----------------------------------------------------------------------
// <copyright file="AuthenticateCustomer.cs" company="Nexcel">
// Steven.Nguyen
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Razorbill.Security.Customer
{
    using System.Globalization;
    using System.Web;
    using Fanex.CustomerSetting.Security.Authenticate;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AuthenticateCustomer : IAuthenticate
    {
        private IUserSession _currentUserSession;

        public AuthenticateAction.AuthenticationType Policy { get; set; }

        public string Function { get; set; }

        public IUserSession CurrentUserSession
        {
            get
            {
                if (null == HttpContext.Current.Session)
                {
                    return null;
                }

                return _currentUserSession ??
                       (_currentUserSession =
                        UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID));
            }
        }

        /// <summary>
        /// Allows customer the view specific function.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Allow to access or not.</returns>
        public bool AllowRead(string action)
        {
            return IsBelong();
        }

        /// <summary>
        /// Allows customer the update specific function.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Allow to update or not.</returns>
        public bool AllowUpdate(string action)
        {
            bool subAccountPermission = !CurrentUserSession.IsSubAcc || CurrentUserSession.SubPageAccess.UpdateAccount;
            return !CurrentUserSession.IsInternal && subAccountPermission && IsBelong();
        }

        /// <summary>
        /// Allows customer the read specific function.
        /// </summary>
        /// <param name="username">The user-name.</param>
        /// <returns>Allow to read or not.</returns>
        public bool AllowReadDepenOnUsernameBelong(string username)
        {
            return IsBelong(0, username);
        }

        /// <summary>
        /// Allows customer the update specific function.
        /// </summary>
        /// <param name="action">The action name.</param>
        /// <param name="username">The user-name.</param>
        /// <returns>Allow to update or not.</returns>
        public bool AllowUpdateDepenOnUsernameBelong(string action, string username)
        {
            bool subAccountPermission = !CurrentUserSession.IsSubAcc || CurrentUserSession.SubPageAccess.UpdateAccount;
            return !CurrentUserSession.IsInternal && subAccountPermission && IsBelong(0, username);
        }

        /// <summary>
        /// Determines whether the specified group name is belong.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="getUplineInfo">If set to <c>true</c> [get UPLINE information].</param>
        /// <returns>Customer is updating his DOWNLINE.</returns>
        public bool IsBelong(string groupName = null, bool getUplineInfo = true)
        {
            return IsBelong(0, string.Empty, groupName, getUplineInfo);
        }

        /// <summary>
        /// Determines whether the specified customer identifier is belong.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="getUplineInfo">If set to <c>true</c> [get UPLINE information].</param>
        /// <returns>Customer is updating his DOWNLINE.</returns>
        public bool IsBelong(int custId, string username, string groupName = null, bool getUplineInfo = true)
        {
            if (string.IsNullOrEmpty(username))
            {
                const string UserNameParam = "username";
                if (HttpContext.Current.Request.Form[UserNameParam] != null)
                {
                    username = HttpContext.Current.Request.Form[UserNameParam].ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    username = HttpContext.Current.Request.QueryString[UserNameParam];
                }
            }

            custId = CurrentUserSession.CustId;
            var bo = new AuthorizationBO(CurrentUserSession.NetSessionId);
            int customerId = 0, roleId = 0;
            var isSubAccount = false;
            var isValidUser = bo.IsUsernameBelong(username.Trim(), custId, out customerId, out isSubAccount, out roleId);
            return isValidUser;
        }
    }
}