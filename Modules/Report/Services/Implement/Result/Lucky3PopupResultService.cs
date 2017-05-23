namespace Fanex.Razorbill.Module.Report.Services.Implement.Result
{
    using Criteria.Result;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Interface.Result;
    using Repositories.Impl.Result;
    using Repositories.Interface.Result;
    using Sunplus.Agent.Model.Entities;

    public class Lucky3PopupResultService : ILucky3PopupResultService
    {
        private readonly ILucky3PopupResultDataAccess _lucky3PopupResultDataAccess;

        public Lucky3PopupResultService(ILucky3PopupResultDataAccess lucky3PopupResultDataAccess = null)
        {
            _lucky3PopupResultDataAccess = lucky3PopupResultDataAccess ?? new Lucky3PopupResultDataAccess();
        }

        public string GetResult(int matchId)
        {
            var criteria = new GetNumberGamePopupResultCriteria()
            {
                MatchID = matchId
            };

            return ResultEngine.HtmlParser(_lucky3PopupResultDataAccess.GetLucky3PopupResult(criteria), GetFanexResultSetting());
        }

        private ResultSetting GetFanexResultSetting()
        {
            return new ResultSetting
            {
                SportType = SportType.Lucky3,
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