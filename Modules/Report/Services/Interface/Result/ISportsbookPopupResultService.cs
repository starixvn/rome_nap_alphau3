// --------------------------------------------------------------------------
// <copyright file="ISportsbookPopupResultService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Interface.Result
{
    using Fanex.Razorbill.Module.Report.Entities;

    /// <summary>
    /// The interface ISportsBookPopupResultService.
    /// </summary>
    public interface ISportsbookPopupResultService
    {
        string GetResult(ResultRequest resultRequest);
    }
}