<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._SubAcc.ResetSubAccSC_Handler" %>
namespace Sunplus.Agent.UI._SubAcc
{
	using System;
	using System.Web;
	using Fanex.Age.UI;
	using Sunplus.Agent.Utilities;
	using Sunplus.Agent.Model.BL.MemberInfo.SecurityCode;

	public class ResetSubAccSC_Handler : Fanex.Age.UI.TplHandler
	{
		private int subAccId = 0;
		private string username = string.Empty;
		private string secCode, confirmSecCode;
		private string ph;
		
		public override void RenderResult(ref Fanex.Age.UI.JSONData data)
		{
			Result r = View as Result;
			data.AsResult(r);
		}

		public override object InvokeData(System.Collections.Hashtable p)
		{
			int errCode = 0;
			string errMsg = string.Empty;

			SecurityCodeBO bo = new SecurityCodeBO(CurrentUserSession.NetSessionId);
			errCode = bo.ChangeSubAccountSecurityCode(subAccId, secCode, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), out errMsg);

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
			if (!int.TryParse(Request["subaccid"], out subAccId) 
                || !CurrentUserSession.SubPageAccess.UpdateAccount
                || CurrentUserSession.IsLimitedHostPermisionOnSubAccountFunction
                )
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