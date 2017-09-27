// --------------------------------------------------------------------------
// <copyright file="ParlayPopupResultService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Implement.Result
{
    using System.Linq;
    using Fanex.Razorbill.Module.Report.Criteria.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Impl.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Interface.Result;
    using Fanex.Razorbill.Module.Report.Services.Interface.Result;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Sunplus.Agent.Model.Entities;

    /// <summary>
    /// The class ParlayPopupResultService.
    /// </summary>
    public class ParlayPopupResultService : IParlayPopupResultService
    {
        private IParlayPopupResultDataAccess _parlayResultDataAccess;

        public ParlayPopupResultService(IParlayPopupResultDataAccess parlayResultDataAccess)
        {
            this._parlayResultDataAccess = parlayResultDataAccess ?? new ParlayPopupResultDataAccess();
        }

        public ParlayPopupResultService()
            : this(null)
        {
        }

        public string GetResult(long refNo)
        {
            var parlayData = this._parlayResultDataAccess.GetParlayResult(new GetParlayPopupResultCriteria
            {
                RefNo = refNo
            });

            return ResultEngine.HtmlParser(parlayData.ToList(), this.GetFanexResultSetting());
        }

        /// <summary>
        /// Gets the FANEX result setting.
        /// </summary>
        /// <returns>The object of ResultSetting.</returns>
        private ResultSetting GetFanexResultSetting()
        {
            return new ResultSetting
            {
                IsOutright = false,
                IsParlayPopUp = true,
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