// --------------------------------------------------------------------------
// <copyright file="LiveCasinoPopupResultService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Implement.Result
{
    using Fanex.Razorbill.Module.Report.Criteria.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Impl.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Interface.Result;
    using Fanex.Razorbill.Module.Report.Services.Interface.Result;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Sunplus.Agent.Model.Entities;

    /// <summary>
    /// The class LiveCasinoPopupResultService.
    /// </summary>
    public class LiveCasinoPopupResultService : ILiveCasinoPopupResultService
    {
        private ILiveCasinoPopupResultDataAccess _liveCasinoPopupResultDataAccess;

        public LiveCasinoPopupResultService(ILiveCasinoPopupResultDataAccess liveCasinoPopupResultDataAccess)
        {
            this._liveCasinoPopupResultDataAccess = liveCasinoPopupResultDataAccess ?? new LiveCasinoPopupResultDataAccess();
        }

        public LiveCasinoPopupResultService()
            : this(null)
        {
        }

        public string GetResult(int matchId, int betType, string league)
        {
            var criteria = new GetLiveCasinoPopupResultCriteria()
            {
                MatchId = matchId,
                BetType = betType,
                League = league
            };

            return ResultEngine.HtmlParser(this._liveCasinoPopupResultDataAccess.GetLiveCasinoPopupResult(criteria), GetFanexResultSetting());
        }

        /// <summary>
        /// Gets the FANEX result setting.
        /// </summary>
        /// <returns>The object of ResultSetting.</returns>
        private static ResultSetting GetFanexResultSetting()
        {
            return new ResultSetting
            {
                SportType = SportType.LiveCasino,
                IsOutright = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                VisiblePopUpTitle = false,
                LanguageCode = UserSession.Current.Language,
                VisibleMatchCodeIDColumn = false,
                VisibleFreeHeader = true,
                VisibleDetailedLink = true
            };
        }
    }
}