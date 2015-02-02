<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.RaceBook.Language_Handler" %>

namespace Sunplus.Agent.UI._MemberInfo.RaceBook
{
    using System;
    using System.Web;
    using System.Web.SessionState;
    using System.Collections.Generic;
    using System.Resources;
    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Threading;
    using Sunplus.Agent.Model.Entities;
    using System.Text;

    public class Language_Handler : IHttpHandler, IReadOnlySessionState
    {
        private bool isCrossLevel = true;
        
        public void ProcessRequest(HttpContext context)
        {
            int targetCustId = 0;
            int targetRoleId = 0;
            int.TryParse(context.Request["targetCustId"], out targetCustId);
            int.TryParse(context.Request["targetRoleId"], out targetRoleId);
            IUserSession currentUserSession = UserSession.Current;
            if (currentUserSession.RoleId - targetRoleId == 1)
            {
                isCrossLevel = false;
            }
            
            // use english date time format
            CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(UserSession.Current.Language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(UserSession.Current.Language);
            System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;

            context.Response.ContentType = "text/plain";

            string language = this.GetLangString();
            context.Response.Write("(function (){\n");
            context.Response.Write(language);
            context.Response.Write("})();");
        }

        /// <summary>
        /// Set Language from Position Taking
        /// </summary>
        /// <returns></returns>
        private string GetLangString()
        {
            bool isThai = Thread.CurrentThread.CurrentCulture.Name == "th-TH";
            bool isKo = Thread.CurrentThread.CurrentCulture.Name == "ko-KR";
            bool isVN = Thread.CurrentThread.CurrentCulture.Name == "vi-VN";
            
            StringBuilder language = new StringBuilder();
            language.AppendLine(this.SetValue("SUPER", Resources.Lang.super));
            language.AppendLine(this.SetValue("MASTER", Resources.Lang.master));
            language.AppendLine(this.SetValue("AGENT", Resources.Lang.agent));
            language.AppendLine(this.SetValue("MEMBER", Resources.Lang.member));
            language.AppendLine(this.SetValue("AGENT_POSITION", Resources.Lang.agent));
            language.AppendLine(this.SetValue("AUTO_MASTER", Resources.Lang.lblMasterAuto));
            language.AppendLine(this.SetValue("AUTO_SUPER", Resources.Lang.lblSuperAuto));
            language.AppendLine(this.SetValue("MIN_BET", Resources.Lang.lblMinBet));
            language.AppendLine(this.SetValue("MAX_BET", Resources.Lang.lblMaxBet));

            if ((!isThai && !isVN) || (isThai && !isCrossLevel))
            {
                language.AppendLine(this.SetValue("MAX_PER_MATCH", Resources.Lang.lblMaxPerMatch));
            }

            language.AppendLine(this.SetValue("STATUS", Resources.Lang.status));
            language.AppendLine(this.SetValue("BETTING_LIMIT", Resources.Lang.lblBetSetting));
            language.AppendLine(this.SetValue("NOINFO", Resources.Lang.noInfo));
            language.AppendLine(this.SetValue("BETTING_LIMIT", Resources.Lang.lblBetSetting));
            language.AppendLine(this.SetValue("INCORRECT_MIN_BET", Resources.Lang.alertIncorrectMinBet));
            language.AppendLine(this.SetValue("INCORRECT_MAX_BET", Resources.Lang.alertIncorrectMaxBet));
            language.AppendLine(this.SetValue("INCORRECT_MAX_PER_MATCH", Resources.Lang.alertIncorrectMaxPerMatch));
            language.AppendLine(this.SetValue("MIN_BET_GREATER_THAN_MIN_MIN_BET", string.Format("{0} {1}", Resources.Lang.lblalertMinbetCompare, "{0}")));
            language.AppendLine(this.SetValue("MAX_BET_LOWER_THAN_MAX_MAX_BET", string.Format("{0} {1}", Resources.Lang.lblalerMaxbetCompare, "{0}")));
            language.AppendLine(this.SetValue("MAX_PER_MATCH_LOWER_THAN_MAX_PER_MATCH_MAX_BET", Resources.Lang.lblalertMaxPerMatchCompare.Replace("{1}", "{0}")));
            language.AppendLine(this.SetValue("MIN_BET_LOWER_THAN_MIN_MAX_BET", Resources.Lang.minBetShouldBeLessThanOrEqualToMaxBet));
            language.AppendLine(this.SetValue("MIN_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET", Resources.Lang.minBetShouldBeLessThanOrEqualToMaxPerMatch));
            language.AppendLine(this.SetValue("MAX_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET", Resources.Lang.maxBetShouldBeLessThanOrEqualToMaxPerMatch));
            language.AppendLine(this.SetValue("CONFIRM_UPDATE_POSITION_TAKING", Resources.Lang.titleConfirmEditPT));
            language.AppendLine(this.SetValue("WANT_TO_UPDATE_ALL_MEMBER_POSITION_TAKING", Resources.Lang.msgUpdateAllMember));
            language.AppendLine(this.SetValue("CONFIRM_UPDATE_MEMBERS_PT", Resources.Lang.confirmUpdateMembers));
            language.AppendLine(this.SetValue("COPY_FROM", Resources.Lang.copyFrom));
            language.AppendLine(this.SetValue("OTHER_BET_TYPE_OF", Resources.Lang.otherBetTypesOf));
            language.AppendLine(this.SetValue("COPY_ALL_HINT", Resources.Lang.copyAllHint));
            language.AppendLine(this.SetValue("COPY_ALL_OTHER_POSITION_TAKING_FROM", Resources.Lang.copyAllPositionTakingFrom));
            language.AppendLine(this.SetValue("COPY_ALL_POSITION_TAKING_FROM", Resources.Lang.copyPositionTakingFrom));
            language.AppendLine(this.SetValue("COPY_ALL_BETTING_LIMIT_FROM", Resources.Lang.copyAllBetComStatusSettings));
            language.AppendLine(this.SetValue("EDIT", Resources.Lang.edit));

            if (!isVN)
            {
                language.AppendLine(this.SetValue("DOWNLINES", Resources.Lang.downlines));
            }
            
            if (isCrossLevel || (!isThai && !isVN))
            {
                language.AppendLine(this.SetValue("COMMISSION", Resources.Lang.com));
            }

            if (!isVN)
            {
                language.AppendLine(this.SetValue("APPLY_SUPER_PRESET_TO_ALL_MEMBERS", Resources.Lang.copysuperpresetformember));
            }

            if (!isVN)
            {
                language.AppendLine(this.SetValue("APPLY_MASTER_PRESET_TO_ALL_MEMBERS", Resources.Lang.copymasterpresetformember));
            }

            if (!isThai && !isVN)
            {
                language.AppendLine(this.SetValue("SUPER_PRESET", Resources.Lang.lblSuperPresetPosition));
            }

            if (!isThai && !isKo && !isVN)
            {
                language.AppendLine(this.SetValue("AUTO_SUPER_PRESET", Resources.Lang.lblAutoSuperPresetPosition));
            }

            if (!isThai && !isVN)
            {
                language.AppendLine(this.SetValue("MASTER_PRESET", Resources.Lang.lblMasterPresetPosition));
            }
            
            language.AppendLine(this.SetValue("AUTO_MASTER_PRESET", Resources.Lang.lblAutoMasterResestPosition));
            
            return language.ToString();
        }

        private string SetValue(string key, string value)
        {
            return string.Format("Language.{0} = \"{1}\";", key, value);
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