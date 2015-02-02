<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.CheckUser_Handler" %>

using System;
using System.Web;
using System.Data;

using Fanex.Age.UI;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.BL.Authorization;

/*
 * Created 20120216@Jeff - Check existing user
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers
{
    public class CheckUser_Handler : TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        string userName = string.Empty;
        string errMsg = string.Empty;
        int errCode = 0;
        int roleId = 0;
        bool isSubAccount = false;
        int customerId = -1;

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            if (Request.Form["username"] != null)
                userName = Request.Form["username"].ToString();
            else
                userName = Request.QueryString["username"];
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
           /* if (this.CurrentUserSession.IsInternal)
            {
                errCode = 601;
                errMsg = Resources.Lang.lblNotAuthorized;
                return new Result(errCode, errMsg);
            }*/

            AuthorizationBO bo = new AuthorizationBO(this.CurrentUserSession.NetSessionId);
            bool isValidUser = bo.IsUsernameBelong(userName.Trim(), this.CurrentUserSession.CustId, out customerId, out isSubAccount, out roleId);

            if (isValidUser && this.CurrentUserSession.UserName.Trim().ToLower() != userName.Trim().ToLower() && (this.CurrentUserSession.RoleId - 1) == roleId)
            {
                // #17366: Prevent using disabled customers in Search & Copy function.
                bool isEnabled = bo.IsCustomerEnable(customerId, out errMsg);
                if (isEnabled)
                {
                    errCode = 999;  
                    errMsg = customerId.ToString();
                }
                else
                {
                    errCode = 101;
                    errMsg = Resources.Lang.accdisabled;    
                }
            }
            else
            {
                errCode = 101;
                errMsg = Resources.Lang.notExist;
            }
                
            return new Result(errCode, errMsg);
        }

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }
    }
}