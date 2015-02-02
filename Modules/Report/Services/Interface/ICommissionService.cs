// --------------------------------------------------------------------------
// <copyright file="ICommissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.09.26">New requirement #16452</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Interface
{
    using System.IO;
    using Fanex.Razorbill.Module.Report.Criteria.Commission;
    using Fanex.Razorbill.Module.Report.ViewModels.Commission;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICommissionService
    {
        CommissionByBettypeList GetCommissionByBettype(CommissionByBettypeCriteria criteria);

        MemoryStream ExportExcelCommissionByBettype(CommissionByBettypeCriteria criteria, CommissionByBettypeList data, bool isBold);
    }
}