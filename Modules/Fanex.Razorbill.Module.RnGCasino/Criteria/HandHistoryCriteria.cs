// --------------------------------------------------------------------------
// <copyright file="HandHistoryCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.11">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.Criteria
{
    using System;

    /// <summary>
    /// Hand History Criteria.
    /// </summary>
    public class HandHistoryCriteria
    {
        public DateTime FDate { get; set; }

        public DateTime TDate { get; set; }

        public int IBCCustId { get; set; }

        public long IBCRefNo { get; set; }

        public int CustId { get; set; }

        public string UName { get; set; }

        public string BetType { get; set; }

        public bool ShowPic { get; set; }

        public bool IsHistoryReport { get; set; }
    }
}
