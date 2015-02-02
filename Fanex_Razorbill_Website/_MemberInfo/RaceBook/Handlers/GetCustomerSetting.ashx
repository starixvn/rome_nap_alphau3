<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.RaceBook.GetCustomerSetting_Handler" %>

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
    using Fanex.PTRacing.BusinessObjects;
    using Fanex.PTRacing.Services;
    using Sunplus.Agent.Model.BL.Definitions;
    using System.Resources;

    /// <summary>
    /// Summary description for GetCustomerSetting
    /// </summary>
    public class GetCustomerSetting_Handler : IHttpHandler, IReadOnlySessionState
    {
        private bool isCrossLevel = true;
        
        public void ProcessRequest(HttpContext context)
        {
            // use english date time format
            CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(UserSession.Current.Language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(UserSession.Current.Language);
            System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;

            context.Response.ContentType = "text/javascript";
            context.Response.Cache.SetNoStore();

            IUserSession CurrentUserSession = UserSession.Current;
            string currentUserName = CurrentUserSession.UserName;
            int custId = CurrentUserSession.CustId;
            int targetCustId = 0;
            int targetRoleId = 0;
            int.TryParse(context.Request["targetCustId"], out targetCustId);
            int.TryParse(context.Request["targetRoleId"], out targetRoleId);
            if (CurrentUserSession.RoleId - targetRoleId == 1)
            {
                isCrossLevel = false;
            }

            IRequestContext currentContext = new RequestContext
            {
                CustId = CurrentUserSession.CustId,
                FormId = FormType.EditPT,
                TargetCustId = targetCustId,
                TargetRoleId = targetRoleId,
                SubAccId = CurrentUserSession.SubAccId,
                AdminId = CurrentUserSession.AdminId,
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

            CustomerService customerService = new CustomerService();
            var customer = customerService.GetCustomer(currentContext);
            foreach (var sportType in customer.Settings.SportTypes)
            {
                foreach (var betTypeGroup in sportType.BetTypeGroups)
                {
                    if (sportType.Id == 151 && betTypeGroup.Id == 31)
                    {
                        if (AppSettings.ISPOSuperId == CurrentUserSession.SRecommendId || AppSettings.SpecialSuperId == CurrentUserSession.SRecommendId)
                        {
                            betTypeGroup.IsHide = true;
                        }
                    }
                }
            }
            
            // apply languages
            foreach (var sportType in customer.Settings.SportTypes)
            {
                sportType.Title = GetLangResource(sportType.ResourceKey);
                if (IsTranslateSport(sportType.Id, customer.Language))
                {
                    sportType.Name = sportType.Title;
                }
                
                foreach (var betTypeGroup in sportType.BetTypeGroups)
                {
                    betTypeGroup.Title = GetRacingResource(betTypeGroup.ResourseKey);
                    if (IsTranslateTote(betTypeGroup.Id, customer.Language))
                    {
                        betTypeGroup.Name = betTypeGroup.Title;
                    }
                    
                    foreach (var bettype in betTypeGroup.BetTypes)
                    {
                        bettype.Title = GetLangResource(bettype.ResourceKey);
                        if (IsTranslateBetType(bettype.Id, customer.Language))
                        {
                            bettype.Name = bettype.Title;
                        }
                    }
                }
            }

            Hashtable ht = new Hashtable();
            ht.Add("customer", customer);
            ht.Add("context", currentContext);
            context.Response.Write(SerializeToJs(ht));
        }
        
        private bool IsTranslateSport(int sportId, string language)
        {
            switch (sportId)
            {
                case 152:
                    if (language == "TH")
                    {
                        return false;
                    }
                    break;
            }
            
            return true;
        }

        private bool IsTranslateTote(int toteId, string language)
        {
            switch(toteId)
            {
                case 41:
                    if (!isCrossLevel && (language == "CT" || language == "CS"))
                    {
                        return false;
                    }
                    break;
            }
            
            return true;
        }

        private bool IsTranslateBetType(int bettypeId, string language)
        {
            return true;
        }

        /// <summary>
        /// Get Resource from key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetLangResource(string key)
        {
            //var ResourceManager = new ResourceManager(typeof(Resources.Lang));
            //return ResourceManager.GetString(key, Thread.CurrentThread.CurrentCulture) ?? string.Empty;
            return _Internal.ResourceManager.GetString(key) ?? string.Empty;
        }

        public static string GetRacingResource(string key)
        {
            //var ResourceManager = new ResourceManager(typeof(Resources.Racing));
            //return ResourceManager.GetString(key, Thread.CurrentThread.CurrentCulture) ?? string.Empty;
            return _Internal.RacingResourceManager.GetString(key) ?? string.Empty;
        }

        public static string SerializeToJs(Hashtable tb)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return SerializeToJs(tb, settings);
        }

        public static string SerializeToJs(Hashtable tb, JsonSerializerSettings settings)
        {
            string result = string.Empty;

            foreach (var key in tb.Keys)
            {
                var value = tb[key];
                result += "var " + key + " = " + JsonConvert.SerializeObject(value, settings) + ";\n";
            }

            return result;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    static class _Internal
    {
        public static ResourceManager ResourceManager = null;

        public static ResourceManager RacingResourceManager = null;

        static _Internal()
        {
            ResourceManager = new ResourceManager(typeof(Resources.Lang));
            ResourceManager.IgnoreCase = true;

            RacingResourceManager = new ResourceManager(typeof(Resources.Racing));
            RacingResourceManager.IgnoreCase = true;
        }
    }
}