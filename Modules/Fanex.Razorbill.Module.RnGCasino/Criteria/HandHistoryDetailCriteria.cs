// --------------------------------------------------------------------------
// <copyright file="HandHistoryDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.Criteria
{
    /// <summary>
    /// Hand History Detail Criteria.
    /// </summary>
    public class HandHistoryDetailCriteria
    {
        public int UserBetId { get; set; }

        public string GameType { get; set; }
    }
}
