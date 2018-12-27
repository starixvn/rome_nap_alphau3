
namespace Fanex.Razorbill.Module.Report.Services.Implement.Result
{
    using Fanex.Razorbill.Module.Report.Criteria.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Impl.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Interface.Result;
    using Fanex.Razorbill.Module.Report.Services.Interface.Result;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Sunplus.Agent.Model.Entities;
    using System.Linq;
    using Fanex.Result.Core.Happy5.DTO;

    public class Happy5PopupResultService : IHappy5PopupResultService
    {
        private INumberGamePopupResultDataAccess _numberGamePopupResultDataAccess;

        public Happy5PopupResultService(INumberGamePopupResultDataAccess numberGamePopupResultDataAccess)
        {
            this._numberGamePopupResultDataAccess = numberGamePopupResultDataAccess ?? new NumberGamePopupResultDataAccess();
        }

        public Happy5PopupResultService()
            : this(null)
        {
        }

        public string GetResult(int matchId)
        {
            var criteria = new GetNumberGamePopupResultCriteria()
            {
                MatchID = matchId
            };

            var results = _numberGamePopupResultDataAccess.GetNumberGamePopupResult(criteria);
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

            return ResultEngine.HtmlParser(happy5Results, this.GetFanexResultSetting());
        }
    
        private ResultSetting GetFanexResultSetting()
        {
            return new ResultSetting
            {
                SportType = SportType.Happy5,
                IsOutright = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                VisiblePopUpTitle = false,
                LanguageCode = UserSession.Current.Language,
                VisibleMatchCodeIDColumn = false,
                VisibleFreeHeader = true,
                VisibleDetailedLink = true,
            };
        }
    }
}