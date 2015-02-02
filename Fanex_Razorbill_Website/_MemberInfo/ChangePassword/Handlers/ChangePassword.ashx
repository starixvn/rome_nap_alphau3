<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.ChangePassword.ChangePassword_Handler" %>

using System;
using System.Web;

using Fanex.Age.UI;
using Sunplus.Agent.Model.Entities;

using Sunplus.Agent.Model.BL.Authorization;

/*
* Created 20100226@Terry, Lee - Handler for change password
* Revision ?@? - ...
*
*/

namespace Sunplus.Agent.UI._MemberInfo.ChangePassword
{
    using Sunplus.Agent.Utilities;
    
    public class ChangePassword_Handler : Fanex.Age.UI.TplHandler
    {
        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

        string oldPwd, newPwd;
        string ph;
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {		
            newPwd = Request["txtNewPwd"] + string.Empty;
            oldPwd = Request["txtOldPwd"] + string.Empty;
            ph = Session["ph"] + string.Empty;                        
            
            newPwd = Encryption.RC4(ph, Encryption.HexDecode(newPwd));
            oldPwd = Encryption.RC4(ph, Encryption.HexDecode(oldPwd));
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!Validation.CheckValidPassword(newPwd) 
                || string.IsNullOrEmpty(oldPwd) 
                || CurrentUserSession.IsInternal
                || string.IsNullOrEmpty(ph)
                )
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            if (string.Compare(newPwd, oldPwd, true) == 0) // Must be different
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblPasswordDOldOne);
            }
            else if (string.Compare(newPwd, "59D7!4$h", true) == 0
                || string.Compare(newPwd, "493abcDE", true) == 0
                )
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblPasswordCSample);
            }
            else if (newPwd.ToLower().Contains(CurrentUserSession.UserName.ToLower()) // Must not be username
                || (newPwd.ToLower().Contains(CurrentUserSession.NickName.ToLower()) && !string.IsNullOrEmpty(CurrentUserSession.NickName))  // Must not be nick name
                || (newPwd.ToLower().Contains(CurrentUserSession.SubUserName.ToLower()) && !string.IsNullOrEmpty(CurrentUserSession.SubUserName)) // Must not be sub-account name
                )
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblPasswordDAccountName);
            }
              
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AuthorizationBO bo = new AuthorizationBO(CurrentUserSession.NetSessionId);

            int errCode = 0;
            string errMsg = string.Empty;

            errCode = bo.ChangePassword(newPwd, oldPwd, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), out errMsg);
            return new Result(errCode, errMsg);
        }
    }
}