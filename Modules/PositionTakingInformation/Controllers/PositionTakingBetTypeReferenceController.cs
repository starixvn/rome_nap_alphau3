// --------------------------------------------------------------------------
// <copyright file="PositionTakingBetTypeReferenceController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.09.24">Created</change>
// </history>

namespace Fanex.Razorbill.Module.PositionTaking.Controllers
{
    using System.Web.Mvc;
    using Fanex.PTSetting.BettypeReference;
    using Fanex.Razorbill.Module.Core.Configurations;
    using Fanex.Razorbill.Module.PositionTakingInformation.Utils;

    /// <summary>
    /// Position Taking BetType Reference Controller.
    /// </summary>
    public class PositionTakingBetTypeReferenceController : PositionTakingInforBaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>The action result.</returns>
        public ActionResult Index()
        {
            ViewBag.CustomerId = SessionExtension.CustId;
            ViewBag.RoleId = SessionExtension.RoleId;
            ViewBag.Language = GetLanguageCode(SessionExtension.Language);
            ViewBag.IsShowAll = AppSettings.IsLocalTest;

            return View("~/Areas/PositionTakingInformation/Views/BetTypeReference/Index.cshtml");
        }

        /// <summary>
        /// Gets the language code.
        /// </summary>
        /// <param name="lang">The lang from session.</param>
        /// <returns>The language code.</returns>
        private LanguageCode GetLanguageCode(string lang)
        {
            switch (lang)
            {
                case "en-US": return LanguageCode.English;
                case "zh-TW": return LanguageCode.Taiwan;
                case "zh-CN": return LanguageCode.Chinese;
                case "ja-JP": return LanguageCode.Japanese;
                case "th-TH": return LanguageCode.Thailand;
                case "vi-VN": return LanguageCode.Vietnamese;
                default: return LanguageCode.English;
            }
        }
    }
}
