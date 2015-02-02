// --------------------------------------------------------------------------
// <copyright file="GetRacingPopupResultCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Criteria.Result
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria GetRacingPopupResultCriteria.
    /// </summary>
    public class GetRacingPopupResultCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "MatchID")]
        public int MatchId { get; set; }

        public int? RaceNo { get; set; }

        public override string GetSettingKey()
        {
            return "Age_BetList_SelMatchResult2";
        }
    }
}
