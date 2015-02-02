// --------------------------------------------------------------------------
// <copyright file="NumberGamePopupResultService.cs" company="Nexcel Solutions Vietnam">
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
    using Sunplus.Agent.Model.Entities;

    /// <summary>
    /// The class NumberGamePopupResultService.
    /// </summary>
    public class NumberGamePopupResultService : INumberGamePopupResultService
    {
        private INumberGamePopupResultDataAccess _numberGamePopupResultDataAccess;

        public NumberGamePopupResultService(INumberGamePopupResultDataAccess numberGamePopupResultDataAccess)
        {
            this._numberGamePopupResultDataAccess = numberGamePopupResultDataAccess ?? new NumberGamePopupResultDataAccess();
        }

        public NumberGamePopupResultService()
            : this(null)
        {
        }

        public string GetResult(int matchId)
        {
            var criteria = new GetNumberGamePopupResultCriteria()
            {
                MatchID = matchId
            };

            return ResultEngine.HtmlParser(this._numberGamePopupResultDataAccess.GetNumberGamePopupResult(criteria), this.GetFanexResultSetting());
        }

        /// <summary>
        /// Gets the FANEX result setting.
        /// </summary>
        /// <returns>The object of ResultSetting.</returns>
        private ResultSetting GetFanexResultSetting()
        {
            return new ResultSetting
            {
                SportType = SportType.NumberGame,
                IsOutRight = false,
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