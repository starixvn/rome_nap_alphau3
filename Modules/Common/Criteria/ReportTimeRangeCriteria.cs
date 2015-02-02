// --------------------------------------------------------------------------
// <copyright file="ReportTimeRangeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.09.26">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Common.Criteria
{
    using System.Collections;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Report time range criteria.
    /// </summary>
    public class ReportTimeRangeCriteria : CriteriaBase
    {
        public byte ReportType { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Rpt_GetTimeRange";
        }

        public override Hashtable GetSpParams()
        {
            return new Hashtable 
            { 
                { "rpttypeid", ReportType }
            };
        }

        public override bool IsValid()
        {
            return ReportType >= 0;
        }
    }
}