<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._SubAcc.AddSubAcc_Handler" %>

using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;

using Fanex.Age.UI;
using Sunplus.Agent.Model.BL.SubAcc;
using Sunplus.Agent.Model.Entities;

/*
 * Created 20091224@Lee - Adding a sub-acccount
 * Revision:
 * 20111228@Andy - Add more permissions
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._SubAcc
{
    using Sunplus.Agent.Utilities;
    
    public class AddSubAcc_Handler : Fanex.Age.UI.TplHandler
    {
        private string number1 = null;
        private string number2 = null;
        PageAccess pa = null;
        string password = null;

        public override void ProcessRequest(ref Hashtable p)
        {
            // 'number1', 'number2', 'txtPwd', 'txtFirstName', 'txtLastName',
            // 'chkTotalBets', 'chkReports', 'chkCreateAcc', 'chkUpdateAcc', 'chkTransfer'

            var perms = Permissions.GetPermissions();
            var permsArr = new List<string>()
            {
                "number1",
                "number2",
                "txtPwd",
                "txtFirstName",
                "txtLastName"
            };

            foreach (var item in perms)
            {
                permsArr.Add(item.Key);
            }

            p.AddHttpParameters(permsArr.ToArray(), Request);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            // Sub-account does not have permission to access SubAcc function. Some special Admin account does not have update permission.
            if (CurrentUserSession.SubAccId > 0 || CurrentUserSession.IsLimitedHostPermisionOnSubAccountFunction)
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            // Check parameters in case user try to pass client checking
            number1 = p["number1"] as string;
            number2 = p["number2"] as string;

            if (number1.Length > 1 || number2.Length > 1)
            {
                return new Result(ErrorType.Unknown, Resources.Lang.strNotAccessPage);
            }

            // Check page access in case user try to put an invalid value
            pa = new PageAccess();
            try
            {
                pa.CreateAccount = bool.Parse(p[Permissions.CreateAccount] as string);
                pa.UpdateAccount = bool.Parse(p[Permissions.UpdateAccount] as string);
                pa.ViewReports = bool.Parse(p[Permissions.Reports] as string);
                pa.ViewTotalBets = bool.Parse(p[Permissions.TotalBets] as string);
                pa.Transfer = bool.Parse(p[Permissions.Transfer] as string);
                pa.ViewLog = bool.Parse(p[Permissions.ViewLog] as string);
                pa.ViewMemberInfo = bool.Parse(p[Permissions.ViewMemberInfo] as string);
            }
            catch
            {
                return new Result(ErrorType.Unknown, Resources.Lang.strNotAccessPage);
            }

            password = p["txtPwd"] as string;
            string ph = Session["ph"] + string.Empty;
            password = Encryption.RC4(ph, Encryption.HexDecode(password));

            if (!Fanex.Age.UI.Validation.CheckValidPassword(password))
            {
                return new Result(ErrorType.Unknown, Resources.Lang.lblPasswordWrongRule);
            }
            
            string username = CurrentUserSession.UserName + Sunplus.Agent.Model.BL.Definitions.AppSettings.SUB_ACC_SUFFIX + number1 + number2;
            if (string.Compare(password, "59D7!4$h", true) == 0
                || string.Compare(password, "493abcDE", true) == 0
                )
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblPasswordCSample);
            }
            else if (password.ToLower().Contains(username.ToLower())
                && !string.IsNullOrEmpty(username) // Must not be username
                )
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblPasswordDAccountName);
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            SubAccount subAcc = new SubAccount();

            subAcc.UserName = CurrentUserSession.UserName + Sunplus.Agent.Model.BL.Definitions.AppSettings.SUB_ACC_SUFFIX + number1 + number2;
            subAcc.PassWord = password;
            subAcc.FirstName = p["txtFirstName"] as string;
            subAcc.LastName = p["txtLastName"] as string;

            subAcc.PageAccess = pa;

            SubAccBO bo = new SubAccBO(CurrentUserSession.NetSessionId);

            int errCode;
            string errMsg;

            errCode = bo.InsertSubAccount(ref subAcc,
                Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                out errMsg
                );

            return new Result(errCode, errMsg);
        }

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }
    }
}