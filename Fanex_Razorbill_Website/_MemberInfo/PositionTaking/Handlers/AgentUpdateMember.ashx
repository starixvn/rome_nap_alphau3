<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.PositionTaking.AgentUpdateMember_Handler" %>

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
    using System.Collections;
    using Sunplus.Agent.Model.Security;

    public class AgentUpdateMember_Handler : Fanex.Age.UI.TplHandler
    {
        private string customerObject = string.Empty;
        private string parameter = string.Empty;
        private Hashtable updateParameters = new Hashtable();

        private void UpdateStatusCasino(Customer customer, bool isCallAPI, bool isMultipleUpdate)
        {
            if (isCallAPI)
            {
                if (UserPermission.IsShowCasino)
                {

                    string strModifyBy = CurrentUserSession.UserName;

                    if (CurrentUserSession.SubAccId > 0)
                    {
                        strModifyBy = CurrentUserSession.SubUserName;
                    }

                    // Sync data to Casino
                    CasinoLibrary.SBAgent.Customer casino = new CasinoLibrary.SBAgent.Customer();                    
                    var customerInfo = IBCAgent.UserInfo.GetCustomerByInfo(customer.CustId);
                    if (customerInfo != null && customerInfo.ResultSet != null &&
                        customerInfo.ResultSet.Tables != null &&
                        customerInfo.ResultSet.Tables.Count > 0 &&
                        customerInfo.ResultSet.Tables[0].Rows != null &&
                        customerInfo.ResultSet.Tables[0].Rows.Count > 0)
                    {
                        if (Fanex.Common.Converter.ToBool(customerInfo.ResultSet.Tables[0].Rows[0]["synccasino"]))
                        {
                            try
                            {
                                // Store data for casino section
                                if (isMultipleUpdate)
                                {
                                    UpdateRnGStatus(customer, strModifyBy);
                                }
                                else
                                {
                                    UpdateRnGInfo(customer, strModifyBy);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (!ex.Message.ToLower().Contains("not casino user"))
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdateRnGInfo(Customer customerInfo, string modifyBy)
        {
            new CasinoLibrary.SBAgent.Customer().UpdateCustInfo(
                customerInfo.CustId, 
                customerInfo.FirstName, 
                customerInfo.LastName, 
                customerInfo.Phone,
                customerInfo.MobilePhone, 
                customerInfo.Fax, 
                customerInfo.StatusSetting.Closed,
                customerInfo.StatusSetting.Suspended, 
                this.Request.UserHostAddress,
                CurrentUserSession.CustId, 
                modifyBy, 
                Sunplus.Agent.Model.BL.Casino.CasinoBO.AGENT_SYSTEM);
        }

        private void UpdateRnGStatus(Customer customerInfo, string modifyBy)
        {
            new CasinoLibrary.SBAgent.Customer().UpdateCustClosed(
                customerInfo.CustId, 
                customerInfo.StatusSetting.Closed, 
                this.Request.UserHostAddress, 
                CurrentUserSession.CustId, 
                modifyBy, 
                Sunplus.Agent.Model.BL.Casino.CasinoBO.AGENT_SYSTEM);
        }

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

            if (this.Request["updateParameters"] != null)
            {
                var tmp = this.Request["updateParameters"];
                for (var i = 0; i < tmp.Length; i++)
                {
                    updateParameters.Add(i, tmp[i].ToString());
                }
            }
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (UserSession.Current.IsInternal ||
                !UserSession.Current.SubPageAccess.UpdateAccount ||
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
                    RoleId = (int)UserSessionBase.UserTypes.Agent,
                    TargetRoleId = (int)UserSessionBase.UserTypes.Member,
                    FormId = FormType.EditPT,
                    SubAccId = CurrentUserSession.SubAccId,
                    AdminId = CurrentUserSession.UserId,
                    IP = Request.UserHostAddress.ToString(),
                    Language = CurrentUserSession.Language
                };

                ICustomerService customerService = new CustomerService();
                Fanex.PTEngine.BusinessObjects.Result result = new Fanex.PTEngine.BusinessObjects.Result();
                var isMultipleUpdate = updateParameters.Count > 0;
                if (isMultipleUpdate)
                {
                    result = customerService.AgentUpdateMultipleMembers(customer, requestContext, updateParameters);
                }
                else
                {                   
                    result = customerService.AgentUpdateMember(customer, requestContext);
                }
                
                string message = string.Empty;
                bool isCallAPI = false;
                int error = CommonGenerateHTML.GenerateErrorMsg(result.ErrorCode, result.ErrorMessage, ref message, string.Empty, ref isCallAPI);

                if (!isMultipleUpdate || (isMultipleUpdate && updateParameters[0].ToString().Equals("1")))
                {
                    this.UpdateStatusCasino(customer, isCallAPI, isMultipleUpdate);
                }                
                
                if (error != 0)
                {
                    return new Fanex.Age.UI.Result(ErrorType.Unknown, message);
                }
                else
                {
                    return new Fanex.Age.UI.Result(ErrorType.Success, Resources.Lang.updatesuccessfull);
                }
            }
            
            return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
        }
    }
}