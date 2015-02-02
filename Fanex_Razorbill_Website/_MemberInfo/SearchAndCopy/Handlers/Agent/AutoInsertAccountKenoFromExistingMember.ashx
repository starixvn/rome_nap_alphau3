<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent.AutoInsertAccountKenoFromExistingMember_Handler" %>
/*
 * Created 20140619@David - auto insert account Keno from exist
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent
{
    using System;
    using Fanex.Age.UI;
    using Sunplus.Agent.Utilities;
    using System.Collections.Generic;
    using System.Web.SessionState;
    using System.Collections;
    using Fanex.CustomerSetting.Module.Keno.KenoSetting.ViewModels;
    using Fanex.CustomerSetting.Module.Keno.KenoSetting.Services;
    using Fanex.CustomerSetting.Module.KenoStatus.Configurations;

    public class AutoInsertAccountKenoFromExistingMember_Handler : TplHandler, IReadOnlySessionState
    {
        int custId;
        int custIdOld;
        string errMsg = string.Empty;
        int errCode = (int)ErrorType.Success;

        KenoSettingService kenoService = new KenoSettingService();
       
        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

        public override object InvokeData(Hashtable p)
        {
            if (!KenoSection.Instance.ShowSetting)
            {
                return new Result(ErrorType.Unknown, string.Empty);
            }

            try
            {
                CustomerInformationViewModel childCriteria = new CustomerInformationViewModel
                {
                    CustId = custIdOld,
                    UserId = CurrentUserSession.UserId,
                    SubAccId = CurrentUserSession.SubAccId,
                    IsMultipleEdit = 0
                };

                AgentEditMemberSettingViewModel childSetting = kenoService.GetSettingInCaseOfAgentEditMember(childCriteria);

                childSetting.CustomerInformation.CustId = custId;

                foreach (var betSetting in childSetting.BetSettings)
                {
                    betSetting.IsStatusUpdated = true;
                    betSetting.IsBetSettingUpdated = true;
                }
                
                foreach (var positionTaking in childSetting.PositionTakings)
                {
                    positionTaking.A1 = positionTaking.A1 < positionTaking.A2 ? positionTaking.A1 : positionTaking.A2;
                    positionTaking.IsUpdateA1 = true;
                }

                 var results = kenoService.UpdateMemberCustomerSettingInCaseOfAgentEditMember(childSetting);
            }
            catch (Exception ex)
            {
                Logs.Write(ex.Message);
            }

            return new Result(errCode, errMsg);
        }

        public override void ProcessRequest(ref Hashtable p)
        {
            int.TryParse(Request["custid"], out custId);
            int.TryParse(Request["custidOld"], out custIdOld);
        }

        public override object ValidateRequest(Hashtable p)
        {
            if (custId <= 0 || custIdOld <= 0)
            {
                return new Result(ErrorType.Unknown, string.Empty);
            }

            return null;
        }
    }
}