// --------------------------------------------------------------------------
// <copyright file="HandHistoryStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.13">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.Enums
{
    /// <summary>
    /// Hand History Status.
    /// </summary>
    public enum HandHistoryStatus
    {
        /// <summary>
        /// Undetectable status.
        /// </summary>
        None = 0,

        /// <summary>
        /// Is playing.
        /// </summary>
        Playing = 20,

        /// <summary>
        /// Is broken but not finish.
        /// </summary>
        Broken = 25,

        /// <summary>
        /// Is broken and finish.
        /// </summary>
        BrokenFinish = 27
    }
}
