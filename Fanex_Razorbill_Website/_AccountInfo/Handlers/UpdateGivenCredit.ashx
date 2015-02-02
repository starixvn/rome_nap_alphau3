<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._AccountInfo.UpdateGivenCredit_Handler" %>

namespace Sunplus.Agent.UI._AccountInfo
{
    using System;
    using System.Web;
    using Sunplus.Agent.Model.BL.AccountInfo;    
    
    public class UpdateGivenCredit_Handler : AccountInfoBase_Handler
    {
        #region Variable
        private int _custId = 0;
        private int _roleId = 0;
        private double _amount = 0;
        #endregion
        public UpdateGivenCredit_Handler()
        {
            IsValidateRequest = false;
        }

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            base.RenderResult(ref data);
            data.Add("amount", Sunplus.Agent.Utilities.Formater.FormatNumber20(_amount, CurrentUserSession.LangKey));
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            _custId = GetIntRequest("custid", 0);
            _roleId = GetIntRequest("custRoleId", 0);
            _amount = GetDoubleRequest("amount", 0);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (Request["amount"] != null)
            {
                if (!double.TryParse(Request["amount"], out _amount))
                {
                    _errCode = -1;
                    _errMsg = Resources.Lang.errorinputnumber;
                    return new Fanex.Age.UI.Result(_errCode, _errMsg);

                }
            }

            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            return base.ValidateRequest(p);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AccountInfoBO accountInfoBO = new AccountInfoBO(CurrentUserSession.NetSessionId);
            _errCode = accountInfoBO.UpdateCreditBalance(_roleId, _custId, _amount, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), ref _errMsg);
            return null;
        }
    }
}