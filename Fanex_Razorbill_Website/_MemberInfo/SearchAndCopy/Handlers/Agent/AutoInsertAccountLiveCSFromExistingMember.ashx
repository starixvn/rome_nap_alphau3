
<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent.AutoInsertAccountLiveCSFromExistingMember_Handler" %>

/*
 * Created 20120409@Jeff - auto insert account from exist
 * Revision:
 * ?@? - ...
 *
 */

using System;
using System.Web;
using System.Collections;

using DAL;
using BLL;

using Fanex.Age.UI;
using Fanex.Age.UI.LiveCasino;
using Sunplus.Agent.Model.Entities.Permission;
using Sunplus.Agent.Model.BL.Authorization;
using Sunplus.Agent.Model.BL.AccountInfo;
using Fanex.LiveCasino.CoreSetting.Services;
using Fanex.LiveCasino.CoreSetting.Models;
using Sunplus.Agent.Model.Common;
using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
using Fanex.LiveCasino.CoreSetting.Enumerations;
using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent
{
    public class AutoInsertAccountLiveCSFromExistingMember_Handler : TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        int custId;
        int custIdOld;
        int FromSystem = 2; // Agent
        string errMsg = string.Empty;
        int errCode = (int)ErrorType.Unknown;
        Sunplus.Agent.Model.Entities.CustomerInfo custInfo;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            SettingService liveCasinoSettingService = new SettingService();
            
            try
            {
                var accBO = new AccountInfoBO(CurrentUserSession.NetSessionId);
                custInfo = accBO.GetCustInfo(custId);

                ICommandResults result = liveCasinoSettingService.AutoInsertAccountFromExisting(custId, custIdOld, custInfo.UserName, CurrentUserSession.CustId, CurrentUserSession.UserName, CurrentUserSession.SubAccId, CurrentUserSession.SubUserName, AgentIPFormat.GetClientIP(), CurrentUserSession.LangKey);

                errCode = result.IsSuccessful ? 0 : 101;
                errMsg = result.IsSuccessful ? "Updated successfully" : "Updated fail";
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