// --------------------------------------------------------------------------
// <copyright file="GetSportsbookPopupResultCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// The criteria GetSportsBookPopupResultCriteria.
    /// </summary>
    public class GetSportsbookPopupResultCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public string RaceNo { get; set; }

        [SpParam(Ignored = true)]
        public int SportType { get; set; }

        [SpParam(Ignored = true)]
        public bool IsViewTennisDetail { get; set; }

        public int BetType { get; set; }

        public long BetId { get; set; }

        public override string GetSettingKey()
        {
            return this.IsViewTennisDetail ? "Age_Result_MatchSelResultDetail" : "Age_BetList_SelMatchResult";
        }

        public override bool IsValid()
        {
            return this.MatchId > 0;
        }
    }
}