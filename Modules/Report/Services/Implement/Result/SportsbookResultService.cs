// --------------------------------------------------------------------------
// <copyright file="SportsbookResultService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Implement.Result
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Fanex.Razorbill.Module.Core.Extensions;
    using Fanex.Razorbill.Module.Report.Criteria.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Impl.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Interface.Result;
    using Fanex.Razorbill.Module.Report.Services.Interface.Result;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.Model.Entities;

    /// <summary>
    /// The class SportsBookResultService.
    /// </summary>
    public class SportsbookResultService : ISportsbookResultService
    {
        private readonly ISportsbookResultDataAccess _scoreInformationDataAccess;

        public SportsbookResultService(ISportsbookResultDataAccess scoreInformationDataAccess)
        {
            this._scoreInformationDataAccess = scoreInformationDataAccess ?? new SportsbookResultDataAccess();
        }

        public SportsbookResultService()
            : this(null)
        {
        }

        public string GetResult(DateTime fromDate, int sportType, int leagueId)
        {
            var criteria = GetCriteria(fromDate, sportType, leagueId);
            IEnumerable<Sportsbook> sportsbooks =
                UserSession.Current.IsShowSportbook || sportType != 0 ?
                this._scoreInformationDataAccess.GetSportbookResult(criteria) :
                new List<Sportsbook>();
            ResultSetting resultSetting = GetFanexResultSetting(criteria.SportType > 0 ? criteria.SportType : (int)SportType.Others);

            return ResultEngine.HtmlParser<Sportsbook>(sportsbooks.ToList(), resultSetting).Replace("original-title", " title ");
        }

        public MemoryStream ExportExcel(DateTime fromDate, int sportType, int leagueId, string excelTemplatePath)
        {
            return ResultEngine.ExcelParser(
                this._scoreInformationDataAccess.GetSportbookResult(GetCriteria(fromDate, sportType, leagueId)).ToList(),
                GetExportExcelSetting(sportType, excelTemplatePath, GetExcelTitle(sportType, fromDate, leagueId)));
        }

        private static GetSportsbookResultCriteria GetCriteria(DateTime fromDate, int sportType, int leagueId)
        {
            return new GetSportsbookResultCriteria()
            {
                FromDate = fromDate,
                SportType = sportType,
                IsShowAll = true,
                LeagueID = leagueId
            };
        }

        /// <summary>
        /// Gets the FANEX result setting.
        /// </summary>
        /// <param name="sportType">Type of the sport.</param>
        /// <returns>Result Setting Model.</returns>
        private static ResultSetting GetFanexResultSetting(int sportType)
        {
            SportType type = (SportType)sportType;
            return new ResultSetting
            {
                SportType = (SportType)sportType,
                LanguageCode = UserSession.Current.Language,
                VisibleDetailedLink = type == SportType.Soccer || type == SportType.Tennis,
                VisibleFreeHeader = true,
                VisibleMatchCodeIDColumn = false
            };
        }

        private static ResultSetting GetExportExcelSetting(int sportType, string excelTemplatePath, string excelTitle)
        {
            return new ResultSetting
            {
                SportType = (SportType)sportType,
                LanguageCode = UserSession.Current.Language,
                ExcelTitle = excelTitle,
                ExcelTemplatePath = excelTemplatePath,
                VisibleMatchCodeIDColumn = false,
            };
        }

        private static string GetExcelTitle(int sportType, DateTime fromDate, int leagueId)
        {
            string leagueName = leagueId == 0 ? "Show All Leagues" : CacheRef.GetLeagueName(leagueId);
            return ResourceExtension.GetResource("Lang", "result") + " - " + fromDate.ToString("MM/dd/yyyy")
                + " | " + CacheRef.GetSportName(sportType) + " - " + leagueName;
        }
    }
}