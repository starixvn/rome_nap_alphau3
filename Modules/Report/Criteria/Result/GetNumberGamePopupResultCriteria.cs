// --------------------------------------------------------------------------
// <copyright file="GetNumberGamePopupResultCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// The criteria GetNumberGamePopupResultCriteria.
    /// </summary>
    public class GetNumberGamePopupResultCriteria : CriteriaBase
    {
        public int MatchID { get; set; }

        public override string GetSettingKey()
        {
            return "Age_BetList_SelMatchResultNumberGame";
        }

        public override bool IsValid()
        {
            return MatchID > 0;
        }
    }
}
