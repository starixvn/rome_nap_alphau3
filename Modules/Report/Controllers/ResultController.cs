// --------------------------------------------------------------------------
// <copyright file="ResultController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.12.23">Need to do more refactoring in the future.</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Razorbill.Module.Core.Extensions;
    using Fanex.Razorbill.Module.Core.Web.Controllers;
    using Fanex.Razorbill.Module.Report.Entities;
    using Fanex.Razorbill.Module.Report.Services.Implement.Result;
    using Result.Core.Sportsbook.DTO;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.Reports;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;

    /// <summary>
    /// FOOBET controller.
    /// </summary>
    public class ResultController : BaseController
    {
        private const string _ResourceClass = "Lang";
        private const int _MaxLiveBallCount = 30;
        private const int _MaxTennisSet = 5;
        private const int _MaxGamePerResultLine = 13;
        private DateTime _minReportDate = DateTime.Today;
        private string _outRightResult = string.Empty;
        private List<Outright> _outRights = null;
        private object _reportData = null;
        private Dictionary<int, Action<ResultRequest>> _resultHandlers = new Dictionary<int, Action<ResultRequest>>();
        private Dictionary<int, Func<ResultRequest, ActionResult>> _exportExcelHandlers = new Dictionary<int, Func<ResultRequest, ActionResult>>();

        private string _MinReportDateString
        {
            get
            {
                return _minReportDate.ToString("MM/dd/yyyy");
            }
        }

        private ReportsBO _ReportBO
        {
            get
            {
                return new ReportsBO(UserSession.Current.NetSessionId);
            }
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        /// <returns>The object of ActionResult.</returns>
        public ActionResult GetResult(ResultRequest resultRequest)
        {
            SetDefaultRequestValues(resultRequest);
            GetReportMinMaxDateRange(resultRequest);

            if (resultRequest.IsExportExcel)
            {
                CreateExportExcelHandlers();
                return this.GetResultForExportExcel(resultRequest);
            }
            else
            {
                CreateResultHandlers();
                this._reportData = InvokeData(resultRequest);
                RenderContent(resultRequest);
                return View("~/Areas/Report/Views/Result/GetResult.cshtml", resultRequest);
            }
        }

        private static string[] GetResultTitle(ResultRequest resultRequest)
        {
            return new string[] 
            { 
                "<strong><span class=\"bl_stype\">", 
                CacheRef.GetSportName(resultRequest.SportId), 
                "</span></strong>", " - ", 
                CacheRef.GetLeagueName(resultRequest.LeagueId) 
            };
        }

        /// <summary>
        /// Check show LiveCasino on WebConfig.
        /// </summary>
        /// <param name="reportData">The report data.</param>
        /// <returns>Determines whether [is show live casino] [the specified report data].</returns>
        private static bool IsShowLiveCasino(DataTable reportData)
        {
            return !UserPermission.IsShowLiveCasino && reportData.Columns.Contains("sporttype");
        }

        /// <summary>
        /// Creates the result handlers.
        /// </summary>
        private void CreateResultHandlers()
        {
            _resultHandlers.Add((int)SportType.Soccer, this.RenderSoccer);
            _resultHandlers.Add((int)SportType.BasketBall, this.RenderBasketBall);
            _resultHandlers.Add((int)SportType.Netball, this.RenderBasketBall);
            _resultHandlers.Add((int)SportType.FootBall, this.RenderBasketBall);
            _resultHandlers.Add((int)SportType.Rugby, this.RenderBasketBall);
            _resultHandlers.Add((int)SportType.IceHockey, this.RenderHockey);
            _resultHandlers.Add((int)SportType.FieldHockey, this.RenderHockey);
            _resultHandlers.Add((int)SportType.Tennis, this.RenderTennis);
            _resultHandlers.Add((int)SportType.VolleyBall, this.RenderBadminton);
            _resultHandlers.Add((int)SportType.Badminton, this.RenderBadminton);
            _resultHandlers.Add((int)SportType.Squash, this.RenderBadminton);
            _resultHandlers.Add((int)SportType.BaseBall, this.RenderBaseBall);
            _resultHandlers.Add((int)SportType.Golf, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.Motorsports, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.HRFixedOdds, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.Billard, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.Cycling, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.Fencing, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.Judo, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.ModernPentathlon, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.Rowing, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.Sailing, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.Shooting, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.Taekwondo, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.Triathlon, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.Wrestling, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.Winter, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.ESports, this.RenderBilliard);
            _resultHandlers.Add((int)SportType.HorseRacing, this.RenderHorseRacing);
            _resultHandlers.Add((int)SportType.Greyhound, this.RenderHorseRacing);
            _resultHandlers.Add((int)SportType.Harness, this.RenderHorseRacing);
            _resultHandlers.Add((int)SportType.Lucky3, this.RenderLucky3);
            _resultHandlers.Add((int)SportType.NumberGame, this.RenderNumberGame);
            _resultHandlers.Add((int)SportType.Happy5, this.RenderHappy5);
            _resultHandlers.Add((int)SportType.LiveCasino, this.RenderLiveCasino);
            _resultHandlers.Add((int)SportType.VirtualHorsesRacing, this.RenderVirtualHorsesRacing);
            _resultHandlers.Add((int)SportType.VirtualGreyhounds, this.RenderVirtualHorsesRacing);
            _resultHandlers.Add((int)SportType.VirtualMotors, this.RenderVirtualHorsesRacing);
            _resultHandlers.Add((int)SportType.VirtualSports, this.RenderVirtualSoccer);
            _resultHandlers.Add((int)SportType.VirtualTennis, this.RenderVirtualTennis);
            _resultHandlers.Add((int)SportType.VirtualCycling, this.RenderVirtualCycling);
            _resultHandlers.Add((int)SportType.VirtualSpeedway, this.RenderVirtualSpeedway);
            _resultHandlers.Add((int)SportType.None, this.RenderOtherSport);
        }

        /// <summary>
        /// Creates the result handlers.
        /// </summary>
        private void CreateExportExcelHandlers()
        {
            _exportExcelHandlers.Add((int)SportType.Soccer, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.BasketBall, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Netball, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.FootBall, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Rugby, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.IceHockey, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.FieldHockey, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Tennis, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.VolleyBall, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Badminton, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Squash, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.BaseBall, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Golf, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Motorsports, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.HRFixedOdds, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Billard, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Cycling, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Fencing, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Judo, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.ModernPentathlon, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Rowing, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Sailing, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Shooting, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Taekwondo, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Triathlon, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Wrestling, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.Winter, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.ESports, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.HorseRacing, this.ExportRacing);
            _exportExcelHandlers.Add((int)SportType.Greyhound, this.ExportRacing);
            _exportExcelHandlers.Add((int)SportType.Harness, this.ExportRacing);
            _exportExcelHandlers.Add((int)SportType.Lucky3, this.ExportLucky3);
            _exportExcelHandlers.Add((int)SportType.NumberGame, this.ExportNumberGame);
            _exportExcelHandlers.Add((int)SportType.Happy5, this.ExportHappy5);
            _exportExcelHandlers.Add((int)SportType.VirtualHorsesRacing, this.ExportVirtualRacing);
            _exportExcelHandlers.Add((int)SportType.VirtualGreyhounds, this.ExportVirtualRacing);
            _exportExcelHandlers.Add((int)SportType.VirtualMotors, this.ExportVirtualRacing);
            _exportExcelHandlers.Add((int)SportType.VirtualSports, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.VirtualTennis, this.ExportSportsBook);
            _exportExcelHandlers.Add((int)SportType.VirtualCycling, this.ExportVirtualRacing);
            _exportExcelHandlers.Add((int)SportType.VirtualSpeedway, this.ExportVirtualRacing);
            _exportExcelHandlers.Add((int)SportType.None, this.ExportSportsBook);
        }

        /// <summary>
        /// Gets the result for export excel.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        /// <returns>The object of ActionResult.</returns>
        private ActionResult GetResultForExportExcel(ResultRequest resultRequest)
        {
            if (resultRequest.IsOutright)
            {
                return ExportOutright(resultRequest);
            }
            else
            {
                Func<ResultRequest, ActionResult> exportExcelHandler = this._exportExcelHandlers.ContainsKey(resultRequest.SportId) ?
                        this._exportExcelHandlers[resultRequest.SportId] :
                        this._exportExcelHandlers[(int)SportType.None];

                return exportExcelHandler(resultRequest);
            }
        }

        /// <summary>
        /// Adds the type of the detail information into report data in case of tennis sport.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        /// <param name="reportData">The report data.</param>
        private void AddDetailInformationIntoReportDataInCaseOfTennisSportType(ResultRequest resultRequest, DataTable reportData)
        {
            if (resultRequest.SportId == (int)SportType.Tennis)
            {
                var detailData = (this._reportData as Dictionary<string, DataTable>)["TennisDetail"];

                reportData.Columns.Add("Tennis", typeof(DataTable));
                if (reportData.Rows.Count > 0)
                {
                    reportData.Rows[0]["Tennis"] = detailData;
                }
            }
        }

        /// <summary>
        /// Sets the default request values.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        private void SetDefaultRequestValues(ResultRequest resultRequest)
        {
            if (!resultRequest.FromDate.HasValue)
            {
                resultRequest.FromDate = DateTime.Today;
            }

            if (!resultRequest.ToDate.HasValue)
            {
                resultRequest.ToDate = DateTime.Now;
            }
        }

        private void GetReportMinMaxDateRange(ResultRequest resultRequest)
        {
            _minReportDate = ReportsBO.BalanceUpDate;
            ViewBag.MinReportDate = _minReportDate.ToShortDateString();
        }

        /// <summary>
        /// Renders the content.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        private void RenderContent(ResultRequest resultRequest)
        {
            this.RenderReportHeader(resultRequest);
            ViewBag.ReportTimeDisclaimerContent = string.Format(ResourceExtension.GetResource(_ResourceClass, "ViewDataFromDate"), _MinReportDateString);
            ViewBag.IsVisibleResultTitle = false;
            ViewBag.IsVisibleSportCriteria = false;
            ViewBag.IsVisibleHeaderRegion = false;

            if (!IsViewMatchResultPopup(resultRequest.MatchId))
            {
                ViewBag.IsVisibleResultTitle = true;
                ViewBag.IsVisibleSportCriteria = true;
                ViewBag.IsVisibleHeaderRegion = true;
                this.RenderSportCriteria(resultRequest);
            }

            if (resultRequest.BetType == (int)BetType.Mix_Parlay || resultRequest.BetType == (int)BetType.Combine_Mix_Parlay)
            {
                this.RenderReportContentMixParlay(resultRequest);
            }
            else
            {
                this.RenderReportContent(resultRequest);
            }
        }

        private void RenderReportContentMixParlay(ResultRequest resultRequest)
        {
            string result = new ParlayPopupResultService().GetResult(resultRequest.RefNum);
            ViewBag.Result = result;
        }

        /// <summary>
        /// Renders the content of the report.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        private void RenderReportContent(ResultRequest resultRequest)
        {
            if (resultRequest.IsOutright)
            {
                this.RenderOutright(resultRequest);
            }
            else
            {
                if (IsViewMatchResultPopup(resultRequest.MatchId))
                {
                    ViewBag.ResultTitle = string.Join(null, GetResultTitle(resultRequest));
                }

                Action<ResultRequest> resultHandler = this._resultHandlers.ContainsKey(resultRequest.SportId) ?
                    this._resultHandlers[resultRequest.SportId] :
                    this._resultHandlers[(int)SportType.None];

                resultHandler(resultRequest);
            }
        }

        /// <summary>
        /// Render sport and league dropdown list.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        private void RenderSportCriteria(ResultRequest resultRequest)
        {
            if (resultRequest.SportId == 0)
            {
                ViewBag.IsVisibleSportCriteria = false;
            }
            else
            {
                if (resultRequest.IsOutright)
                {
                    this.RenderOutrightSportCriteria(resultRequest);
                }
                else
                {
                    this.RenderNormalSportCriteria(resultRequest);
                }
            }
        }

        /// <summary>
        /// Render sport and league dropdown list for normal result.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        private void RenderNormalSportCriteria(ResultRequest resultRequest)
        {
            DataTable sportList = (this._reportData as Dictionary<string, DataTable>)["SportList"];
            DataTable leagueList = (this._reportData as Dictionary<string, DataTable>)["LeagueList"];

            if (sportList.Rows.Count == 0)
            {
                ViewBag.IsVisibleSportCriteria = false;
            }
            else
            {
                string sportOptions = string.Empty;
                string leagueOptions = string.Empty;

                foreach (DataRow dr in sportList.Rows)
                {
                    int sportId = Convert.ToInt32(dr["sporttype"]);

                    if (sportId == 180 && !UserPermission.IsShowVirtualSports)
                    {
                        continue;
                    }

                    sportOptions += string.Format(
                        "<option {0} value=\"{1}\">{2}</option>",
                        resultRequest.SportId == sportId ? "selected" : string.Empty,
                        sportId.ToString(),
                        Sunplus.Agent.Model.BL.Cache.CacheRef.GetSportName(sportId));
                }

                ViewBag.SportOption = sportOptions;

                foreach (DataRow dr in leagueList.Rows)
                {
                    int leagueId = Convert.ToInt32(dr["leagueid"]);

                    leagueOptions += string.Format(
                        "<option {0} value=\"{1}\">{2}</option>",
                        resultRequest.LeagueId == leagueId ? "selected" : string.Empty,
                        leagueId.ToString(),
                        Sunplus.Agent.Model.BL.Cache.CacheRef.GetLeagueName(leagueId));
                }

                ViewBag.LeagueOption = leagueOptions;
            }
        }

        /// <summary>
        /// Render sport and league dropdown list for outright result.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        private void RenderOutrightSportCriteria(ResultRequest resultRequest)
        {
            if (this._outRights.Count == 0)
            {
                ViewBag.IsVisibleSportCriteria = false;
            }
            else
            {
                this.RenderOutrightSportCriteriaForSportDropDownList(resultRequest);
                this.RenderOutrightSportCriteriaForLeagueDropDownList(resultRequest);
            }
        }

        /// <summary>
        /// Renders the outright sport criteria for league drop down list.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        private void RenderOutrightSportCriteriaForLeagueDropDownList(ResultRequest resultRequest)
        {
            string leagueOptions = string.Empty;
            List<int> leagueIdList = new List<int>();
            var leagueRows = this._outRights.Where(r => r.SportType == resultRequest.SportId).ToList();

            foreach (Outright league in leagueRows)
            {
                int leagueId = league.LeagueId;

                if (!leagueIdList.Contains(leagueId))
                {
                    leagueOptions += string.Format(
                    "<option {0} value=\"{1}\">{2}</option>",
                    resultRequest.LeagueId == leagueId ? "selected" : string.Empty,
                    leagueId.ToString(),
                    Sunplus.Agent.Model.BL.Cache.CacheRef.GetLeagueName(leagueId));
                    leagueIdList.Add(leagueId);
                }
            }

            ViewBag.LeagueOption = leagueOptions;
        }

        /// <summary>
        /// Renders the outright sport criteria for sport drop down list.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        private void RenderOutrightSportCriteriaForSportDropDownList(ResultRequest resultRequest)
        {
            string sportOptions = string.Empty;
            List<int> sportIdList = new List<int>();

            foreach (Outright sport in this._outRights)
            {
                int sportId = Convert.ToInt32(sport.SportType);

                if (sportId == 180 && !UserPermission.IsShowVirtualSports)
                {
                    continue;
                }

                if (!sportIdList.Contains(sportId))
                {
                    sportOptions += string.Format(
                    "<option {0} value=\"{1}\">{2}</option>",
                    resultRequest.SportId == sportId ? "selected" : string.Empty,
                    sportId.ToString(),
                    Sunplus.Agent.Model.BL.Cache.CacheRef.GetSportName(sportId));
                    sportIdList.Add(sportId);
                }
            }

            ViewBag.SportOption = sportOptions;
        }

        /// <summary>
        /// Invokes the data.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        /// <returns>The report data.</returns>
        private object InvokeData(ResultRequest resultRequest)
        {
            var viewData = new Dictionary<string, DataTable>();

            if (resultRequest.IsOutright)
            {
                this._outRightResult = new OutRightResultService().GetResult(
                    resultRequest.FromDate.Value,
                    resultRequest.ToDate.Value,
                    resultRequest.SportId,
                    resultRequest.LeagueId,
                    out this._outRights);

                if (resultRequest.SportId == 0 && this._outRights.Count > 0)
                {
                    resultRequest.SportId = this._outRights[0].SportType;
                }
            }
            else
            {
                this.GetSportListForNormalResults(viewData, resultRequest);
                this.GetLeagueListForNormalResults(viewData, resultRequest);
            }

            return viewData;
        }

        private void GetLeagueListForNormalResults(Dictionary<string, DataTable> viewData, ResultRequest resultRequest)
        {
            DataTable leagueList = _ReportBO.GetResultLeagueByDate(resultRequest.FromDate.Value, resultRequest.SportId);
            viewData.Add("LeagueList", leagueList);
        }

        /// <summary>
        /// Gets the sport list for normal results.
        /// </summary>
        /// <param name="viewData">The view data.</param>
        /// <param name="resultRequest">The result request.</param>
        private void GetSportListForNormalResults(Dictionary<string, DataTable> viewData, ResultRequest resultRequest)
        {
            DataTable sportList = _ReportBO.GetResultSportByDate(resultRequest.FromDate.Value);

            if (!UserPermission.IsShowLucky3)
            {
                DataRow[] lucky3 = sportList.Select("sporttype=" + ((int)SportType.Lucky3).ToString());
                foreach (DataRow dr in lucky3)
                {
                    sportList.Rows.Remove(dr);
                }
            }

            // Delete number game if IsShowNumberGame=false
            if (!UserPermission.IsShowNumberGame)
            {
                DataRow[] numberGame = sportList.Select("sporttype=" + ((int)SportType.NumberGame).ToString());
                foreach (DataRow dr in numberGame)
                {
                    sportList.Rows.Remove(dr);
                }
            }

            if (!UserPermission.IsShowLiveCasino)
            {
                DataRow[] liveCasino = sportList.Select("sporttype=" + ((int)SportType.LiveCasino).ToString());
                foreach (DataRow dr in liveCasino)
                {
                    sportList.Rows.Remove(dr);
                }
            }

            viewData.Add("SportList", sportList);

            if (ShouldSetDefaultSportId(resultRequest, sportList))
            {
                resultRequest.SportId = Convert.ToInt32(sportList.Rows[0]["sporttype"]);
                resultRequest.LeagueId = 0;
                resultRequest.IsChangeSportType = false;
            }
        }

        private bool ShouldSetDefaultSportId(ResultRequest resultRequest, DataTable sportList)
        {
            return (!IsViewMatchResultPopup(resultRequest.MatchId) && (!resultRequest.IsChangeSportType && sportList.Rows.Count > 0))
                || (resultRequest.SportId == 0 && sportList.Rows.Count > 0);
        }

        /// <summary>
        /// Render Report header.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        private void RenderReportHeader(ResultRequest resultRequest)
        {
            this.RenderReportHeaderForHeaderTitle(resultRequest);
            this.RenderReportHeaderForRightButtons(resultRequest);
            ViewBag.HideKenoTitle = true;

            if (UserPermission.EnableKeno)
            {
                this.RenderReportHeaderInCaseOfKenoIsEnabled(resultRequest);
            }
        }

        /// <summary>
        /// Renders the report header for right buttons.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        private void RenderReportHeaderForRightButtons(ResultRequest resultRequest)
        {
            ViewBag.NormalResultTitle = ResourceExtension.GetResource(_ResourceClass, "lblNormal");
            ViewBag.OutrightResultTitle = ResourceExtension.GetResource(_ResourceClass, "lblOutRight");

            ViewBag.DisabledNormal = resultRequest.IsOutright ? string.Empty : "disabled = 'disabled'";
            ViewBag.DisabledNormalClass = resultRequest.IsOutright ? string.Empty : "btn2";

            ViewBag.DisabledOutright = resultRequest.IsOutright ? "disabled = 'disabled'" : string.Empty;
            ViewBag.DisabledOutrightClass = resultRequest.IsOutright ? "btn2" : string.Empty;
        }

        /// <summary>
        /// Renders the report header for header title.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        private void RenderReportHeaderForHeaderTitle(ResultRequest resultRequest)
        {
            switch (resultRequest.BetType)
            {
                case (int)BetType.Mix_Parlay:
                    ViewBag.ResultTitle = ResourceExtension.GetResource(_ResourceClass, "lblMixParlay");
                    break;
                case (int)BetType.Combine_Mix_Parlay:
                    ViewBag.ResultTitle = ResourceExtension.GetResource(_ResourceClass, "lblSystemParlay");
                    break;
                default:
                    ViewBag.ResultTitle = ResourceExtension.GetResource(_ResourceClass, "result");
                    break;
            }
        }

        /// <summary>
        /// Renders the report header in case of keno is enabled.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        private void RenderReportHeaderInCaseOfKenoIsEnabled(ResultRequest resultRequest)
        {
            ViewBag.HideKenoTitle = false;
            var sessionTmp = "/(S(" + UserSession.Current.NetSessionId + "))/";
            string baseUrl = HttpContext.Request.ApplicationPath.Equals("/") ? sessionTmp : (HttpContext.Request.ApplicationPath + sessionTmp);
            string languageCode = string.Empty;

            switch (UserSession.Current.LangKey)
            {
                case "E": languageCode = "EN";
                    break;
                case "J": languageCode = "JP";
                    break;
                case "CT": languageCode = "CH";
                    break;
                case "CS": languageCode = "CS";
                    break;
                case "TH": languageCode = "TH";
                    break;
                case "KO": languageCode = "KO";
                    break;
                case "VI": languageCode = "VI";
                    break;
                default:
                    languageCode = "EN";
                    break;
            }

            string url = baseUrl + "game/keno/kenoresult/index?date=" + string.Format("{0:yyyyMMdd}", DateTime.Now)
                + "&language=" + languageCode.ToLower() + "&customerId=" + UserSession.Current.CustId;
            ViewBag.KenoURL = url;
            ViewBag.KenoResult = ResourceExtension.GetResource(_ResourceClass, "Keno");
        }

        /// <summary>
        /// Render outright result.
        /// </summary>
        /// <param name="resultRequest">The result request.</param>
        private void RenderOutright(ResultRequest resultRequest)
        {
            ViewBag.Result = this._outRightResult;
        }

        private ActionResult ExportOutright(ResultRequest resultRequest)
        {
            MemoryStream excelResult = new OutRightResultService().ExportExcel(
                resultRequest.FromDate.Value,
                resultRequest.ToDate.Value,
                resultRequest.SportId,
                resultRequest.LeagueId,
                Server.MapPath(@"~/App_Data/Fanex.Result/"));
            return File(excelResult.ToArray(), "application/vnd.ms-excel", "Result.xls");
        }

        /// <summary>
        /// Render badminton result.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderBadminton(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new SportsbookPopupResultService().GetResult(resultRequest) :
                new SportsbookResultService().GetResult(resultRequest.FromDate.Value, resultRequest.SportId, resultRequest.LeagueId);

            ViewBag.Result = result;
        }

        private ActionResult ExportSportsBook(ResultRequest resultRequest)
        {
            MemoryStream excelResult = new SportsbookResultService().ExportExcel(
                resultRequest.FromDate.Value,
                resultRequest.SportId,
                resultRequest.LeagueId,
                Server.MapPath(@"~/App_Data/Fanex.Result/"));
            return File(excelResult.ToArray(), "application/vnd.ms-excel", "Result.xls");
        }

        /// <summary>
        /// Render soccer result.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderSoccer(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new SportsbookPopupResultService().GetResult(resultRequest) :
                new SportsbookResultService().GetResult(resultRequest.FromDate.Value, resultRequest.SportId, resultRequest.LeagueId);

            ViewBag.Result = result;
        }

        /// <summary>
        /// Render BasketBall result.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderBasketBall(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new SportsbookPopupResultService().GetResult(resultRequest) :
                new SportsbookResultService().GetResult(resultRequest.FromDate.Value, resultRequest.SportId, resultRequest.LeagueId);

            ViewBag.Result = result;
        }

        /// <summary>
        /// Render hockey Result.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderHockey(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new SportsbookPopupResultService().GetResult(resultRequest) :
                new SportsbookResultService().GetResult(resultRequest.FromDate.Value, resultRequest.SportId, resultRequest.LeagueId);

            ViewBag.Result = result;
        }

        /// <summary>
        /// Render tennis result.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderTennis(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new SportsbookPopupResultService().GetResult(resultRequest) :
                new SportsbookResultService().GetResult(resultRequest.FromDate.Value, resultRequest.SportId, resultRequest.LeagueId);

            ViewBag.Result = result;
        }

        /// <summary>
        /// Render base ball result.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderBaseBall(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                 new SportsbookPopupResultService().GetResult(resultRequest) :
                 new SportsbookResultService().GetResult(resultRequest.FromDate.Value, resultRequest.SportId, resultRequest.LeagueId);

            ViewBag.Result = result;
        }

        /// <summary>
        /// Render billiard result.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderBilliard(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new SportsbookPopupResultService().GetResult(resultRequest) :
                new SportsbookResultService().GetResult(resultRequest.FromDate.Value, resultRequest.SportId, resultRequest.LeagueId);

            ViewBag.Result = result;
        }

        /// <summary>
        /// Render horse racing result.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderHorseRacing(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new RacingPopupResultService().GetResult(resultRequest.MatchId, this.GetRaceNumber(resultRequest.Race), resultRequest.SportId) :
                new RacingResultService().GetResult(resultRequest.FromDate.Value, resultRequest.SportId, resultRequest.LeagueId);

            ViewBag.Result = result;
        }

        private ActionResult ExportRacing(ResultRequest resultRequest)
        {
            MemoryStream excelResult = new RacingResultService().ExportExcel(
                resultRequest.FromDate.Value,
                resultRequest.SportId,
                resultRequest.LeagueId,
                Server.MapPath(@"~/App_Data/Fanex.Result/"));
            return File(excelResult.ToArray(), "application/vnd.ms-excel", "Result.xls");
        }

        /// <summary>
        /// Render other sport result.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderOtherSport(ResultRequest resultRequest)
        {
            ViewBag.Result = new SportsbookResultService().GetResult(resultRequest.FromDate.Value, resultRequest.SportId, resultRequest.LeagueId);
        }

        /// <summary>
        /// Render LiveCasino.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderLiveCasino(ResultRequest resultRequest)
        {
            string result = new LiveCasinoPopupResultService().GetResult(resultRequest.MatchId, resultRequest.BetType, resultRequest.LeagueId.ToString());
            ViewBag.Result = result;
        }

        private void RenderLucky3(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new Lucky3PopupResultService().GetResult(resultRequest.MatchId) :
                new Lucky3ResultService().GetResult(resultRequest.FromDate.Value, resultRequest.LeagueId, resultRequest.SportId);
            ViewBag.Result = result;
        }

        /// <summary>
        /// Render number result.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderNumberGame(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new NumberGamePopupResultService().GetResult(resultRequest.MatchId) :
                new NumberGameResultService().GetResult(resultRequest.FromDate.Value, resultRequest.LeagueId, resultRequest.SportId);
            ViewBag.Result = result;
        }

        private void RenderHappy5(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new Happy5PopupResultService().GetResult(resultRequest.MatchId) :
                new Happy5ResultService().GetResult(resultRequest.FromDate.Value, resultRequest.LeagueId, resultRequest.SportId);
            ViewBag.Result = result;
        }

        private ActionResult ExportLucky3(ResultRequest resultRequest)
        {
            MemoryStream excelResult = new Lucky3ResultService().ExportExcel(
                resultRequest.FromDate.Value,
                resultRequest.SportId,
                resultRequest.LeagueId,
                Server.MapPath(@"~/App_Data/Fanex.Result/"));
            return File(excelResult.ToArray(), "application/vnd.ms-excel", "Result.xls");
        }

        private ActionResult ExportNumberGame(ResultRequest resultRequest)
        {
            MemoryStream excelResult = new NumberGameResultService().ExportExcel(
                resultRequest.FromDate.Value,
                resultRequest.SportId,
                resultRequest.LeagueId,
                Server.MapPath(@"~/App_Data/Fanex.Result/"));
            return File(excelResult.ToArray(), "application/vnd.ms-excel", "Result.xls");
        }

        private ActionResult ExportHappy5(ResultRequest resultRequest)
        {
            MemoryStream excelResult = new Happy5ResultService().ExportExcel(
                resultRequest.FromDate.Value,
                resultRequest.SportId,
                resultRequest.LeagueId,
                Server.MapPath(@"~/App_Data/Fanex.Result/"));
            return File(excelResult.ToArray(), "application/vnd.ms-excel", "Result.xls");
        }

        /// <summary>
        /// Renders the virtual horses racing.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderVirtualHorsesRacing(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new RacingPopupResultService().GetResultForVirtualRacing(resultRequest.MatchId, this.GetRaceNumber(resultRequest.Race), resultRequest.SportId) :
                new VirtualRacingResultService().GetResult(resultRequest.FromDate.Value, resultRequest.SportId, resultRequest.LeagueId);

            ViewBag.Result = result;
        }

        private ActionResult ExportVirtualRacing(ResultRequest resultRequest)
        {
            MemoryStream excelResult = new VirtualRacingResultService().ExportExcel(
                resultRequest.FromDate.Value,
                resultRequest.SportId,
                resultRequest.LeagueId,
                Server.MapPath(@"~/App_Data/Fanex.Result/"));
            return File(excelResult.ToArray(), "application/vnd.ms-excel", "Result.xls");
        }

        /// <summary>
        /// Renders the virtual tennis.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderVirtualTennis(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new SportsbookPopupResultService().GetResult(resultRequest) :
                new SportsbookResultService().GetResult(resultRequest.FromDate.Value, resultRequest.SportId, resultRequest.LeagueId);

            ViewBag.Result = result;
        }

        /// <summary>
        /// Renders the virtual soccer.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderVirtualSoccer(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new SportsbookPopupResultService().GetResult(resultRequest) :
                new SportsbookResultService().GetResult(resultRequest.FromDate.Value, resultRequest.SportId, resultRequest.LeagueId);

            ViewBag.Result = result;
        }

        /// <summary>
        /// Renders the virtual cycling.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderVirtualCycling(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new RacingPopupResultService().GetResultForVirtualRacing(resultRequest.MatchId, this.GetRaceNumber(resultRequest.Race), resultRequest.SportId) :
                new VirtualRacingResultService().GetResult(resultRequest.FromDate.Value, resultRequest.SportId, resultRequest.LeagueId);

            ViewBag.Result = result;
        }

        /// <summary>
        /// Renders the virtual speedway.
        /// </summary>
        /// <param name="resultRequest">The excel block.</param>
        private void RenderVirtualSpeedway(ResultRequest resultRequest)
        {
            string result = resultRequest.MatchId > 0 ?
                new RacingPopupResultService().GetResultForVirtualRacing(resultRequest.MatchId, this.GetRaceNumber(resultRequest.Race), resultRequest.SportId) :
                new VirtualRacingResultService().GetResult(resultRequest.FromDate.Value, resultRequest.SportId, resultRequest.LeagueId);

            ViewBag.Result = result;
        }

        /// <summary>
        /// Gets the race number.
        /// </summary>
        /// <param name="raceNumber">The race number.</param>
        /// <returns>The race number in the integer format.</returns>
        private int GetRaceNumber(string raceNumber)
        {
            if (string.IsNullOrEmpty(raceNumber) || raceNumber == "0")
            {
                return 0;
            }

            try
            {
                return int.Parse(raceNumber);
            }
            catch (Exception exception)
            {
                if (exception is NotFiniteNumberException || exception is FormatException)
                {
                    return 0;
                }

                throw;
            }
        }

        private bool IsViewMatchResultPopup(int matchId)
        {
            return matchId > 0;
        }
    }
}
