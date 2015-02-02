<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.PositionTaking.SaveNewMember_Handler" %>
namespace Sunplus.Agent.UI._MemberInfo.PositionTaking
{
    using System;
    using System.Web;
    using Fanex.Age.UI;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.BL.Authorization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Fanex.PTEngine.BusinessObjects;
    using Fanex.PTEngine.DataAccess;
    using System.Collections;
    using Sunplus.Agent.Model.Entities;
    using Fanex.PTEngine.Services;
    using Sunplus.Agent.Model.Entities.Permission;
    using System.Data;
    using Sunplus.Agent.Model.Security;
    using Sunplus.Agent.Utilities;
    using Fanex.CustomerSetting.Module.KenoStatus.Configurations;
    
    public class SaveNewMember_Handler : Fanex.Age.UI.TplHandlerSpamFilter
    {
        private string customerObject = string.Empty;
        private string captchaCode = string.Empty;
        private string parameter = string.Empty;
        private HttpFrequenceRule rule;
        string ph;


        
        
        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Fanex.Age.UI.Result r = View as Fanex.Age.UI.Result;
            data.AsResult(r, true);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            if (this.Request["txtCustomerObject"] != null)
            {
                customerObject = this.Request["txtCustomerObject"];
            }
            if (this.Request["captchaCode"] != null)
            {
                captchaCode = this.Request["captchaCode"];
            }
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            // Check Captcha
            /*rule = new HttpFrequenceRule("SaveNewMember.ashx", 19, TimeSpan.FromDays(2));
            if (rule.IsViolated)
            {
                if (captchaCode != CurrentUserSession.CaptchaCode)
                {
                    return new Fanex.Age.UI.Result(ErrorType.RequireCaptcha, Resources.Lang.errInvalidCaptcha);
                }
                else
                {
                    rule.Reset();
                }
            }*/
            // End check Captcha
            if (UserSession.Current.IsInternal ||
                !UserSession.Current.SubPageAccess.CreateAccount ||
                UserSession.Current.RoleId != (int)Sunplus.Agent.Model.Entities.UserSessionBase.UserTypes.Agent)
            {
                return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            if (!string.IsNullOrEmpty(customerObject))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting = Formatting.Indented;

                Customer customer = JsonConvert.DeserializeObject<Customer>(customerObject, settings);
                IRequestContext requestContext = new RequestContext()
                {
                    CustId = CurrentUserSession.CustId,
                    TargetCustId = 0,
                    RoleId = (int)UserSessionBase.UserTypes.Agent,
                    TargetRoleId = (int)UserSessionBase.UserTypes.Member,
                    FormId = FormType.Add,
                    SubAccId = CurrentUserSession.SubAccId,
                    AdminId = CurrentUserSession.UserId,
                    IP = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    Language = CurrentUserSession.Language
                };

                int memberId = -1;
                ICustomerService customerService = new CustomerService();

                ph = Session["ph"] + string.Empty;
                customer.Password = Encryption.RC4(ph, Encryption.HexDecode(customer.Password));

                if (string.IsNullOrEmpty(ph) || !Validation.CheckValidMemberPassword(customer.Password))
                {
                    return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblPasswordWrongRule);
                }
                else if (string.Compare(customer.Password, "59D7!4$h", true) == 0
                    || string.Compare(customer.Password, "493abcDE", true) == 0
                    )
                {
                    return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblPasswordCSample);
                }
                else if (customer.Password.ToLower().Contains(customer.UserName.ToLower())
                    && !string.IsNullOrEmpty(customer.UserName) // Must not be username
                    )
                {
                    return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblPasswordDAccountName);
                }
                
                Fanex.PTEngine.BusinessObjects.Result result = customerService.AddNewMember(customer, requestContext, out memberId);

                string s = char.ConvertFromUtf32(7);

                string[] errCodes = result.ErrorCode.Split(new string[] { s }, StringSplitOptions.RemoveEmptyEntries);
                string[] errMsgs = result.ErrorMessage.Split(new string[] { s }, StringSplitOptions.RemoveEmptyEntries);

                string message = string.Empty;
                for (int i = 0; i < errMsgs.Length; i++)
                {
                    if (i < errMsgs.Length - 1)
                    {
                        message += errMsgs[i] + "<br/>";
                    }
                    else
                    {
                        message += errMsgs[i];
                    }
                }

                if (!string.IsNullOrEmpty(message))
                {
                    return new Fanex.Age.UI.Result(ErrorType.Unknown, message);
                }
                else
                {
                    string disabledFI = "1";
                    string disabledHR = UserPermission.IsShowRacing ? "0" : "1";
                    string disabledBG = UserPermission.IsShowBingo ? "0" : "1";
                    string disabledLiveCS = UserPermission.IsShowLiveCasino ? "0" : "1";
                    string disabledCasino = "0";
                    string disabledkeno = KenoSection.Instance.ShowSetting && !CurrentUserSession.IsHideKeno ? "0" : "1";
                    
                    Sunplus.Agent.Model.BL.Authorization.AuthorizationBO bo = new Model.BL.Authorization.AuthorizationBO(CurrentUserSession.NetSessionId);
                    Hashtable outputs = new Hashtable();
                    bo.GetCustomerStatus(CurrentUserSession.CustId, CurrentUserSession.AdminId, ref outputs);

                    bool shownCS = false;
                    if (UserPermission.IsShowCasino)
                    {
                        shownCS = true;
                    }

                    if (!(bool)(outputs["disablecasino"]) && !shownCS)
                    {
                        disabledCasino = "1";
                    }

                    // Count to check Captcha
                    
                    parameter = customer.UserName + "," + memberId.ToString() + ","
                                + disabledFI + "," + disabledCasino + "," + disabledHR + ","
                                + disabledBG + "," + disabledLiveCS + "," + disabledkeno + "," + UserSession.Current.UserId.ToString()
                                + "," + UserSession.Current.SubAccId.ToString(); ;
                    
                    return new Fanex.Age.UI.Result(ErrorType.Success, parameter);
                }
            }
            return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
        }
    }
}