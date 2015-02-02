// --------------------------------------------------------------------------
// <copyright file="GroupHandHistory.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.11">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.DTOs
{
    using System.Collections.Generic;

    /// <summary>
    /// Session Hand History.
    /// </summary>
    public class GroupHandHistory
    {
        public GroupHandHistory(string sessionId)
        {
            SessionId = sessionId;
            Total = new HandHistory();
            HandHistories = new List<HandHistory>();
        }

        public string SessionId { get; set; }

        public IList<HandHistory> HandHistories { get; set; }

        public HandHistory Total { get; set; }
    }
}
