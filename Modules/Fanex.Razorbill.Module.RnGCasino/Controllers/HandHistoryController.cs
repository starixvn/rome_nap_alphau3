// --------------------------------------------------------------------------
// <copyright file="HandHistoryController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.11">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.Controllers
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Fanex.Razorbill.Module.Core.Web.Controllers;
    using Fanex.Razorbill.Module.RnGCasino.Configurations;
    using Fanex.Razorbill.Module.RnGCasino.Criteria;
    using Fanex.Razorbill.Module.RnGCasino.Services;
    using Sunplus.Agent.Model.Entities;

    /// <summary>
    /// Hand History Controller.
    /// </summary>
    public class HandHistoryController : BaseController
    {
        private readonly IUserSession _userSession;
        private readonly IHandHistoryService _handHistoryService;

        public HandHistoryController()
            : this(UserSession.Current, null)
        { 
        }

        public HandHistoryController(IUserSession userSession, IHandHistoryService handHistoryService)
        {
            _userSession = userSession;
            _handHistoryService = handHistoryService ?? new HandHistoryService();
        }

        /// <summary>
        /// Indexes the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The index view.</returns>
        public ActionResult Index(HandHistoryCriteria criteria)
        {
            // IMPORTANT: Thailand use Buddhist Era which equal Christian Era plus 543 years,
            // so if user wants to select the year 2012 in Buddhist Era user have to select it 2555
            // If FDate and TDate are DateTime => model binder of MVC will parse wrong.
            CultureInfo def = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;

            ViewBag.RoleId = _userSession.RoleId;
            ViewBag.Username = criteria.UName;
            ViewBag.ShowPic = criteria.ShowPic;
            ViewBag.RefNo = criteria.IBCRefNo;

            var report = _handHistoryService.GetHandHistory(criteria);
            return View("~/Areas/RnGCasino/Views/HandHistory/Index.cshtml", report);
        }

        /// <summary>
        /// Views the detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The ViewDetail view.</returns>
        public ActionResult ViewDetail(HandHistoryDetailCriteria criteria)
        {
            var handHistoryDetail = _handHistoryService.GetHandHistoryDetail(criteria);
            if (handHistoryDetail == null)
            {
                return View("~/Areas/RnGCasino/Views/HandHistoryDetail/NoDataFound.cshtml");        
            }

            var element = GameTypeConfig.GetByGameType(criteria.GameType);
            string fullView = "~/Areas/RnGCasino/Views/HandHistoryDetail/" + element.Url + ".cshtml";
            return View(fullView, handHistoryDetail);
        }
    }
}
