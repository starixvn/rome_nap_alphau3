// --------------------------------------------------------------------------
// <copyright file="HandHistoryDetail.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.19">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.DTOs
{
    using System.Collections.Generic;

    /// <summary>
    /// Hand History Detail.
    /// </summary>
    public class HandHistoryDetail
    {
        public HandHistoryDetail()
        {
            GameResults = new List<GameResult>();
            BetTypeMoneys = new List<BetTypeMoney>();
        }

        public IList<GameResult> GameResults { get; set; }

        public IList<BetTypeMoney> BetTypeMoneys { get; set; }
    }
}
