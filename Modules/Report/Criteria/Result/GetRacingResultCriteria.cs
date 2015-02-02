// --------------------------------------------------------------------------
// <copyright file="GetRacingResultCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Criteria.Result
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria GetRacingResultCriteria.
    /// </summary>
    public class GetRacingResultCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public int LeagueID { get; set; }

        public int SportType { get; set; }

        public bool IsShowAll { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Result_RBRaceSelByMeeting";
        }

        public override bool IsValid()
        {
            return FromDate > DateTime.MinValue && SportType > 0;
        }
    }
}
