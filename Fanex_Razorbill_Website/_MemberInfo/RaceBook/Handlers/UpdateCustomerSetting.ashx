<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.RaceBook.UpdateCustomerSetting_Handler" %>

namespace Sunplus.Agent.UI._MemberInfo.RaceBook
{
    using System;
    using System.Web;
    using Sunplus.Agent.Model.Entities;
    using System.Globalization;
    using System.Threading;
    using System.Web.SessionState;
    using System.Collections;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.Collections.Generic;
    using Fanex.PTRacing.BusinessObjects;
    using Fanex.PTRacing.Services;
    using System.Resources;

    public class UpdateCustomerSetting_Handler : Fanex.Age.UI.TplHandler, IReadOnlySessionState
    {
        private int targetCustId = 0;
        private int targetRoleId = 0;
        private bool? updateDownline = null;
        private ICustomer oldCustomer;
        private ICustomer newCustomer;
        private Hashtable otherSetting;
        private bool isMulti = false;
        private int id = 0;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            Response.Write(JsonConvert.SerializeObject(View, settings));
            Response.End();
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request["targetCustId"], out targetCustId);
            int.TryParse(Request["targetRoleId"], out targetRoleId);
            bool.TryParse(Request["isMulti"], out isMulti);
            int.TryParse(Request["id"], out id);

            oldCustomer = JsonConvert.DeserializeObject<Customer>(Request.Form[0]);
            newCustomer = JsonConvert.DeserializeObject<Customer>(Request.Form[1]);
            otherSetting = JsonConvert.DeserializeObject<Hashtable>(Request.Form[2]);

            if (isMulti)
            {
                if (id != 0)
                {
                    // update oldCustomer
                    var customerService = new CustomerService();
                    IRequestContext currentContext = CreateCurrentContext();
                    oldCustomer = customerService.GetCustomer(currentContext);
                }

                bool isupdatebetsetting = Convert.ToBoolean(otherSetting["isupdatebetsetting"]);
                bool isupdatecomm = Convert.ToBoolean(otherSetting["isupdatecomm"]);
                bool isupdatestatus = Convert.ToBoolean(otherSetting["isupdatestatus"]);
                bool isupdatept = Convert.ToBoolean(otherSetting["isupdatept"]);
                for (int i = 0; i < newCustomer.Settings.SportTypes.Count; i++)
                {
                    var sportType = newCustomer.Settings.SportTypes[i];
                    for (int j = 0; j < sportType.BetTypeGroups.Count; j++)
                    {
                        var tote = sportType.BetTypeGroups[j];
                        var oldTote = oldCustomer.Settings.SportTypes[i].BetTypeGroups[j];

                        // bet settings
                        if (tote.BettingCommSetting != null)
                        {
                            bool isupdatebettote = false;
                            if (!tote.IsHide)
                            {
                                isupdatebettote = Convert.ToBoolean(otherSetting["isupdatebettote_" + sportType.Id + "_" + tote.Id]);
                            }

                            bool copybetsetting = isupdatebettote && isupdatebetsetting;
                            if (!copybetsetting)
                            {
                                tote.BettingCommSetting.MinBet = oldTote.BettingCommSetting.MinBet;
                                tote.BettingCommSetting.MaxBet = oldTote.BettingCommSetting.MaxBet;
                                tote.BettingCommSetting.MaxPerMatch = oldTote.BettingCommSetting.MaxPerMatch;
                            }

                            bool copycomm = isupdatebettote && isupdatecomm;
                            if (!copycomm)
                            {
                                tote.BettingCommSetting.Discount = oldTote.BettingCommSetting.Discount;
                            }

                            bool copystatus = isupdatebettote && isupdatestatus;
                            if (!copystatus)
                            {
                                tote.BettingCommSetting.Disable = oldTote.BettingCommSetting.Disable;
                                tote.BettingCommSetting.DisableDownlines = oldTote.BettingCommSetting.DisableDownlines;
                            }
                        }

                        // pt
                        bool isupdatepttote = false;
                        if (!tote.IsHide)
                        {
                            isupdatepttote = Convert.ToBoolean(otherSetting["isupdatepttote_" + sportType.Id + "_" + tote.Id]);
                        }
                        bool copypt = isupdatepttote && isupdatept;
                        if (!copypt)
                        {
                            if (tote.BettingCommSetting != null)
                            {
                                tote.BettingCommSetting.IsApplyPresetPt = oldTote.BettingCommSetting.IsApplyPresetPt;
                            }
                            
                            tote.BetTypes = oldTote.BetTypes;
                        }
                    }
                }
            }
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            return null;
        }

        private IRequestContext CreateCurrentContext()
        {
            IRequestContext currentContext = new RequestContext
            {
                CustId = CurrentUserSession.CustId,
                TargetCustId = targetCustId,
                TargetRoleId = targetRoleId,
                FormId = FormType.EditPT,
                SubAccId = CurrentUserSession.SubAccId,
                AdminId = CurrentUserSession.UserId,
                Language = CurrentUserSession.LangKey,
                LangKey = CurrentUserSession.LangKey,
                IP = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP()
            };

            IUserSession userSession = UserSession.Current;

            if (userSession.IsSuper)
            {
                currentContext.RoleId = (int)UserSessionBase.UserTypes.Super;
                if (targetRoleId == 2)
                {
                    currentContext.FormId = FormType.ViewPT;
                }
            }
            else if (userSession.IsMaster)
            {
                currentContext.RoleId = (int)UserSessionBase.UserTypes.Master;
            }
            else if (userSession.IsAgent)
            {
                currentContext.RoleId = (int)UserSessionBase.UserTypes.Agent;
            }

            return currentContext;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            if (newCustomer != null)
            {
                IRequestContext currentContext = CreateCurrentContext();
                if (currentContext.FormId == FormType.ViewPT)
                {
                    return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
                }

                CustomerService customerService = new CustomerService();
                var result = customerService.SaveCustomer(oldCustomer, newCustomer, currentContext, otherSetting);


                List<Fanex.Age.UI.Result> uiResult = new List<Fanex.Age.UI.Result>();

                if (string.IsNullOrEmpty(result.ErrorCode))
                {
                    uiResult.Add(new Fanex.Age.UI.Result(50, Resources.Lang.noChangeToUpdate));
                }
                else
                {
                    var errorCodes = result.ErrorCode.Split('\a');
                    var errorMsgs = result.ErrorMessage.Split('\a');
                    for (int i = 0; i < errorCodes.Length - 1; i++)
                    {
                        var errorCode = Convert.ToInt32(errorCodes[i]);
                        var errorMsg = errorMsgs[i];
                        if (string.IsNullOrEmpty(errorMsg))
                        {
                            errorMsg = this.ReadResourceValue(errorCode);
                        }

                        uiResult.Add(new Fanex.Age.UI.Result(errorCode, errorMsg));
                    }
                }


                return uiResult;
            }

            return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
        }

        public string ReadResourceValue(int key)
        {
            string resourceValue = string.Empty;
            string file = string.Empty;
            switch (Sunplus.Agent.Model.Entities.UserSession.Current.Language)
            {
                case "en-US": file = "LangMsg.resx"; break;
                case "ja-JP": file = "LangMsg.ja-JP.resx"; break;
                case "zh-TW": file = "LangMsg.zh-TW.resx"; break;
                case "zh-CN": file = "LangMsg.zh-CN.resx"; break;
                case "th-TH": file = "LangMsg.th-TH.resx"; break;
                case "ko-KR": file = "LangMsg.ko-KR.resx"; break;
                case "vi-VN": file = "LangMsg.vi-VN.resx"; break;
                default: file = "LangMsg.resx"; break;
            }
            try
            {
                string myfile = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\App_GlobalResources\\" + file;
                if (System.IO.File.Exists(myfile))
                {
                    ResXResourceReader resReader = new ResXResourceReader(myfile);
                    foreach (DictionaryEntry die in resReader)
                    {
                        if (die.Key.ToString().ToLower() == "succ_" + (key * -1).ToString())
                        {
                            resourceValue = die.Value.ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                resourceValue = "";
            }
            return resourceValue;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}