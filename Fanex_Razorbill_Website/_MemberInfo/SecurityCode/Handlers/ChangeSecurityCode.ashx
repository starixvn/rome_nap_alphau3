<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SecurityCode.ChangeSecurityCode_Handler" %>
/*
* Created 20130415@kan: Handler for change security code
* Revision ?@? - ...
*
*/
namespace Sunplus.Agent.UI._MemberInfo.SecurityCode
{
	using System;
	using System.Web;
	using Fanex.Age.UI;
	using Sunplus.Agent.Utilities;
	using Sunplus.Agent.Model.BL.Authorization;

	public class ChangeSecurityCode_Handler : Fanex.Age.UI.TplHandler
	{
		private string secCode, confirmSecCode;
		private string ph;

		public override void RenderResult(ref Fanex.Age.UI.JSONData data)
		{
			Result r = View as Result;
			data.AsResult(r);
		}

		public override object InvokeData(System.Collections.Hashtable p)
		{
			AuthorizationBO bo = new AuthorizationBO(CurrentUserSession.NetSessionId);

			int errCode = 0;
			string errMsg = string.Empty;

			errCode = bo.ChangeSecurityCode(secCode, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), out errMsg);
			if (errCode == -1) // old and new SC are the same
			{
				errMsg = Resources.Lang.alertSameSecCode;
			}
			return new Result(errCode, errMsg);
		}

		public override void ProcessRequest(ref System.Collections.Hashtable p)
		{
			secCode = Request["txtSecCode"] + string.Empty;
			confirmSecCode = Request["txtConfirmSecCode"] + string.Empty;
			ph = Session["ph"] + string.Empty;

			secCode = Encryption.RC4(ph, Encryption.HexDecode(secCode));
			confirmSecCode = Encryption.RC4(ph, Encryption.HexDecode(confirmSecCode));
		}

		public override object ValidateRequest(System.Collections.Hashtable p)
		{
            bool isAllowed = false;

            if (CurrentUserSession.IsInternal)
            {
                if (!string.IsNullOrEmpty(Request["subaccid"]) && string.IsNullOrEmpty(Request["custid"]))
                {
                    isAllowed = true;
                }
            }
            else if (CurrentUserSession.IsSubAcc)
            {
                if (string.IsNullOrEmpty(Request["custid"]))
                {
                    isAllowed = true;
                }
                else
                {
                    isAllowed = CurrentUserSession.SubPageAccess.UpdateAccount;
                }
            }
            else {
                isAllowed = true;
            }

            if (!isAllowed || string.IsNullOrEmpty(ph))
            {
				return new Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
			}
			
			// sec code is empty
			if (secCode.Length == 0)
			{
				return new Result(ErrorType.AccessDenied, Resources.Lang.alertSecCodeEmpty);
			}

			// security code is not valid
			if (!Validation.CheckValidSecurityCode(secCode) || ph.Length == 0)
			{
				return new Result(ErrorType.AccessDenied, Resources.Lang.lblSecCodeWrongRule);
			}
			
			// security code and its confirm are not the same
			if (!string.Equals(secCode, confirmSecCode))
			{
				return new Result(ErrorType.AccessDenied, Resources.Lang.alertErrorSecCodeConfirm);
			}

			return null;
		}
	}
}