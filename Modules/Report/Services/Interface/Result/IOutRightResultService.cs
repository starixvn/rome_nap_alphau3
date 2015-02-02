// --------------------------------------------------------------------------
// <copyright file="IOutRightResultService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Interface.Result
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.Result.Core.DTO;

    /// <summary>
    /// The interface IOutRightResultService.
    /// </summary>
    public interface IOutRightResultService
    {
        string GetResult(DateTime fromDate, DateTime toDate, int sportId, int leagueId, out List<OutRight> outRights);

        MemoryStream ExportExcel(DateTime fromDate, DateTime toDate, int sportId, int leagueId, string excelTemplatePath);
    }
}