// --------------------------------------------------------------------------
// <copyright file="SportsbookPopupResultService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Implement.Result
{
    using System.Collections.Generic;
    using Fanex.Razorbill.Module.Report.Criteria.Result;
    using Fanex.Razorbill.Module.Report.Entities;
    using Fanex.Razorbill.Module.Report.Repositories.Impl.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Interface.Result;
    using Fanex.Razorbill.Module.Report.Services.Interface.Result;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Sunplus.Agent.Model.Entities;

    /// <summary>
    /// The class SportsBookPopupResultService.
    /// </summary>
    public class SportsbookPopupResultService : ISportsbookPopupResultService
    {
        private ISportsbookPopupResultDataAccess _scoreInformationDataAccess;

        public SportsbookPopupResultService(ISportsbookPopupResultDataAccess scoreInformationDataAccess)
        {
            this._scoreInformationDataAccess = scoreInformationDataAccess ?? new SportsbookPopupResultDataAccess();
        }

        public SportsbookPopupResultService()
            : this(null)
        {
        }

        public string GetResult(ResultRequest resultRequest)
        {
            var criteria = GetCriteria(resultRequest);
            List<Sportsbook> sportsbooks = this._scoreInformationDataAccess.GetScoreInformation(ref criteria);
            ResultSetting resultSetting = GetFanexResultSetting(criteria);

            return resultRequest.ViewDetail ? 
                ResultEngine.HtmlGameDetailParser(sportsbooks[0], resultSetting).Replace("original-title", " title ") :
                ResultEngine.HtmlParser(sportsbooks, resultSetting).Replace("original-title", " title ");
        }

        private static GetSportsbookPopupResultCriteria GetCriteria(ResultRequest resultRequest)
        {
            return new GetSportsbookPopupResultCriteria()
            {
                MatchId = resultRequest.MatchId,
                BetType = resultRequest.BetType,
                SportType = resultRequest.SportId,
                BetId = resultRequest.BetId,
                IsViewDetail = resultRequest.ViewDetail
            };
        }

        /// <summary>
        /// Gets the FANEX result setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The object of ResultSetting.</returns>
        private static ResultSetting GetFanexResultSetting(GetSportsbookPopupResultCriteria criteria)
        {
            return new ResultSetting
            {
                SportType = (SportType)criteria.SportType,
                IsOutright = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                LanguageCode = UserSession.Current.Language,
                VisibleMatchCodeIDColumn = false
            };
        }
    }
}