// --------------------------------------------------------------------------
// <copyright file="IFoobetService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2013.12.10">Created</change>
//   <change who="Kan.Nguyen" date="2014.05.22">New requirement #16452</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Foobet.Interface
{
    using System.IO;
    using Fanex.Razorbill.Module.Report.Criteria.Foobet;
    using Fanex.Razorbill.Module.Report.Entities.FooBet;

    /// <summary>
    /// IFooBetService class.
    /// </summary>
    public interface IFoobetService
    {
        FooBetReport GetFoobetReport(FoobetCriteria criteria);

        MemoryStream ExportFoobetReport(FoobetCriteria criteria);
    }
}
