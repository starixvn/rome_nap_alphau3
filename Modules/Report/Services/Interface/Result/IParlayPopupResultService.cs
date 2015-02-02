// --------------------------------------------------------------------------
// <copyright file="IParlayPopupResultService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Interface.Result
{
    /// <summary>
    /// The interface IParlayPopupResultService.
    /// </summary>
    public interface IParlayPopupResultService
    {
        string GetResult(long refNo);
    }
}