// --------------------------------------------------------------------------
// <copyright file="GetOutRightResultCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// The criteria GetOutRightResultCriteria.
    /// </summary>
    public class GetOutRightResultCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsShowAll { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Result_Outright";
        }

        public override bool IsValid()
        {
            return FromDate > DateTime.MinValue && FromDate <= ToDate && ToDate > DateTime.MinValue;
        }
    }
}
