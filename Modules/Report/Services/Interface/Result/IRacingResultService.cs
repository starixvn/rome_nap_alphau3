// --------------------------------------------------------------------------
// <copyright file="IRacingResultService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Interface.Result
{
    using System;
    using System.IO;

    /// <summary>
    /// The interface IRacingResultService.
    /// </summary>
    public interface IRacingResultService
    {
        string GetResult(DateTime fromDate, int sportType, int leagueId);

        MemoryStream ExportExcel(DateTime fromDate, int sportType, int leagueId, string excelTemplatePath);
    }
}