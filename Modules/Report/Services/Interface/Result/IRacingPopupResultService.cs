// --------------------------------------------------------------------------
// <copyright file="IRacingPopupResultService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Interface.Result
{
    /// <summary>
    /// The interface IRacingPopupResultService.
    /// </summary>
    public interface IRacingPopupResultService
    {
        string GetResult(int matchId, int raceNumber, int sportType);

        string GetResultForVirtualRacing(int matchId, int raceNumber, int sportType);
    }
}