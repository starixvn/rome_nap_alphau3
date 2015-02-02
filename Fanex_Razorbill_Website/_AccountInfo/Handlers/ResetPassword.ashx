<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._AccountInfo.ResetPassword_Handler" %>

namespace Sunplus.Agent.UI._AccountInfo
{
    using System;
    using System.Web;
    using Sunplus.Agent.Utilities;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.BL.AccountInfo;
    using Fanex.Age.UI;
    using CasinoLibrary;
    using CasinoLibrary.SBAgent;

    public class ResetPassword_Handler : Fanex.Age.UI.TplHandler
    {
        private string newPassword = string.Empty;
        private string custId;
        bool isSynCasino = false;
        int roleId = 0;
        string errMsg = string.Empty;
        int errCode = (int)ErrorType.Unknown;
        string ph;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            newPassword = Request["password"] + string.Empty;

            ph = Session["ph"] + string.Empty;
            newPassword = Encryption.RC4(ph, Encryption.HexDecode(newPassword));
            
            custId = Request["custId"];
            bool.TryParse(Request["isSyncCasino"], out isSynCasino);
            int.TryParse(Request["role"], out roleId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.SubPageAccess.UpdateAccount 
                || CurrentUserSession.IsInternal 
                || string.IsNullOrEmpty(newPassword)
                || string.IsNullOrEmpty(custId)
                || (!Validation.CheckValidPassword(newPassword) && roleId != (int)(Sunplus.Agent.Model.BL.Definitions.UserTypes.Member))
                || (!Validation.CheckValidMemberPassword(newPassword) && roleId == (int)(Sunplus.Agent.Model.BL.Definitions.UserTypes.Member))
                || string.IsNullOrEmpty(ph)
                || roleId == 0
                )
            {
                return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }
            else if (string.Compare(newPassword, "59D7!4$h", true) == 0
                || string.Compare(newPassword, "493abcDE", true) == 0
                )
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblPasswordCSample);
            }
            else if (newPassword.ToLower().Contains((Request["username"] + string.Empty).ToLower()) 
                && !string.IsNullOrEmpty(Request["username"]) // Must not be username
                )
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblPasswordDAccountName);
            }
            
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);

			errCode = bo.ResetPassword(custId, newPassword, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), out errMsg);

            if (isSynCasino)
            {
                Customer cust = new Customer();

                try // Casino cheat
                {
                    cust.UpdateCustPwd(int.Parse(custId), Encryption.CfsEnCode(newPassword), Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), UserSession.Current.CustId, UserSession.Current.SubAccId == 0 ? UserSession.Current.UserName : UserSession.Current.SubUserName, 2);
                }
                catch { }
            }

            return new Result(errCode, errMsg);
        }
    }
}