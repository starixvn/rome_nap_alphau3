// --------------------------------------------------------------------------
// <copyright file="GetNumberGameResultCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// The criteria GetNumberGameResultCriteria.
    /// </summary>
    public class GetNumberGameResultCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public int LeagueID { get; set; }

        public string Language { get; set; }

        public int SportType { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Result_MatchSelNumberGameByLeague";
        }

        public override bool IsValid()
        {
            return FromDate > DateTime.MinValue && LeagueID > 0;
        }
    }
}
