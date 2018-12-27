namespace Fanex.Razorbill.Module.Report.Services.Implement.Result
{
    using System;
    using System.IO;
    using System.Linq;
    using Fanex.Razorbill.Module.Core.Extensions;
    using Fanex.Razorbill.Module.Report.Criteria.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Impl.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Interface.Result;
    using Fanex.Razorbill.Module.Report.Services.Interface.Result;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Happy5.DTO;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.Model.Entities;

    public class Happy5ResultService : IHappy5ResultService
    {
        private INumberGameResultDataAccess _numberGameResultDataAccess;

        public Happy5ResultService(INumberGameResultDataAccess numberGameResultDataAccess)
        {
            this._numberGameResultDataAccess = numberGameResultDataAccess ?? new NumberGameResultDataAccess();
        }

        public Happy5ResultService()
            : this(null)
        {
        }

        public string GetResult(DateTime fromDate, int leagueId, int sportType)
        {
            var results = _numberGameResultDataAccess.GetNumberGameResult(GetCriteria(fromDate, leagueId));
            var happy5Results = results.Select(
                i => new Happy5
                {
                    Ball = i.Ball,
                    EventDate = i.EventDate,
                    EventStatus = i.EventStatus,
                    IsPending = i.IsPending,
                    KickOffTime = i.KickOffTime,
                    LeagueId = i.LeagueId,
                    MatchCode = i.MatchCode,
                    MatchId = i.MatchId,
                    Seq = i.Seg
                }).ToList();

            return ResultEngine.HtmlParser(happy5Results, GetFanexResultSetting(sportType));
        }

        public MemoryStream ExportExcel(DateTime fromDate, int sportType, int leagueId, string excelTemplatePath)
        {
            var results = _numberGameResultDataAccess.GetNumberGameResult(GetCriteria(fromDate, leagueId));
            var happy5Results = results.Select(
                i => new Happy5
                {
                    Ball = i.Ball,
                    EventDate = i.EventDate,
                    EventStatus = i.EventStatus,
                    IsPending = i.IsPending,
                    KickOffTime = i.KickOffTime,
                    LeagueId = i.LeagueId,
                    MatchCode = i.MatchCode,
                    MatchId = i.MatchId,
                    Seq = i.Seg
                }).ToList();

            return ResultEngine.ExcelParser(happy5Results,
                GetExportExcelSetting(sportType, excelTemplatePath, GetExcelTitle(sportType, fromDate, leagueId)));
        }

        private static GetNumberGameResultCriteria GetCriteria(DateTime fromDate, int leagueId)
        {
            return new GetNumberGameResultCriteria()
            {
                FromDate = fromDate,
                LeagueID = leagueId,
                Language = UserSession.Current.Language,
                SportType = (int)SportType.Happy5
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