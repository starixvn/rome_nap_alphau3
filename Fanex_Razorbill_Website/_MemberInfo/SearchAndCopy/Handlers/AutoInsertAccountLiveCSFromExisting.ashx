<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.AutoInsertAccountLiveCSFromExisting_Handler" %>

/*
 * Created 20120409@Jeff - auto insert account from exist
 * Revision:
 * ?@? - ...
 *
 */

using System;
using System.Web;

using DAL;
using BLL;
using Fanex.Age.UI;
using Sunplus.Agent.Model.Entities.Permission;
using Sunplus.Agent.Model.BL.Authorization;
using Sunplus.Agent.Model.BL.AccountInfo;
using Fanex.LiveCasino.CoreSetting.Services;
using Fanex.LiveCasino.CoreSetting.Models;
using  Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.Common;

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers
{
    public class AutoInsertAccountLiveCSFromExisting_Handler : TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        int custId;
        int custIdOld;
        int FromSystem = 2; // Agent
        string errMsg = string.Empty;
        int errCode = (int)ErrorType.Unknown;

        SettingService liveCasinoSettingService = new SettingService();
        
        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            try
            {
                var accBO = new AccountInfoBO(CurrentUserSession.NetSessionId);
                var custInfo = accBO.GetCustInfo(custId);
                var results = liveCasinoSettingService.AutoInsertAccountFromExisting(custId, custIdOld, custInfo.UserName, CurrentUserSession.CustId, CurrentUserSession.UserName, CurrentUserSession.SubAccId, CurrentUserSession.SubUserName, AgentIPFormat.GetClientIP(), CurrentUserSession.LangKey);

                errCode = results.IsSuccessful ? 0 : 101;
                errMsg = results.IsSuccessful ? "Updated successfully" : "Updated fail";
            }
            catch (Exception ex)
            {
                errCode = 102;
                errMsg = ex.Message;
            }

            return new Result(errCode, errMsg);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request["custid"], out custId);
            int.TryParse(Request["custidOld"], out custIdOld);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (custId <= 0 || custIdOld <= 0 ||
                !CurrentUserSession.IsLiveCasinoSync)
            {
                return new Result(errCode, errMsg);
            }

            return null;
        }
    }
}