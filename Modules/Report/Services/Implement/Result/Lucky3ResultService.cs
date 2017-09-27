namespace Fanex.Razorbill.Module.Report.Services.Implement.Result
{
    using System;
    using System.IO;
    using System.Linq;
    using Core.Extensions;
    using Criteria.Result;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Interface.Result;
    using Repositories.Impl.Result;
    using Repositories.Interface.Result;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.Model.Entities;

    public class Lucky3ResultService : ILucky3ResultService
    {
        private readonly ILucky3ResultDataAccess _lucky3ResultDataAccess;

        public Lucky3ResultService(ILucky3ResultDataAccess lucky3ResultDataAccess = null)
        {
            _lucky3ResultDataAccess = lucky3ResultDataAccess ?? new Lucky3ResultDataAccess();
        }

        public string GetResult(DateTime fromDate, int leagueId, int sportType)
        {
            return ResultEngine.HtmlParser(
                _lucky3ResultDataAccess.GetLucky3Result(GetCriteria(fromDate, leagueId)).ToList(),
                GetFanexResultSetting(sportType));
        }

        public MemoryStream ExportExcel(DateTime fromDate, int sportType, int leagueId, string excelTemplatePath)
        {
            return ResultEngine.ExcelParser(
                _lucky3ResultDataAccess.GetLucky3Result(GetCriteria(fromDate, leagueId)).ToList(),
                GetExportExcelSetting(sportType, excelTemplatePath, GetExcelTitle(sportType, fromDate, leagueId)));
        }

        private static GetNumberGameResultCriteria GetCriteria(DateTime fromDate, int leagueId)
        {
            return new GetNumberGameResultCriteria()
            {
                FromDate = fromDate,
                LeagueID = leagueId,
                Language = UserSession.Current.Language
            };
        }

        private static ResultSetting GetFanexResultSetting(int sportType)
        {
            return new ResultSetting
            {
                SportType = (SportType)sportType,
                LanguageCode = UserSession.Current.Language,
                VisibleDetailedLink = true,
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