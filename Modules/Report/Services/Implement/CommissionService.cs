// --------------------------------------------------------------------------
// <copyright file="CommissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.09.26">New requirement #23163</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Implement
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Fanex.Razorbill.Module.Core.Services.Impl;
    using Fanex.Razorbill.Module.Report.Criteria.Commission;
    using Fanex.Razorbill.Module.Report.Entities.CommissionByBettype;
    using Fanex.Razorbill.Module.Report.ExcelBuilder.CommissionByBettype;
    using Fanex.Razorbill.Module.Report.Services.Interface;
    using Fanex.Razorbill.Module.Report.ViewModels.Commission;

    /// <summary>
    /// Commission services.
    /// </summary>
    public class CommissionService : ServiceBase, ICommissionService
    {
        /// <summary>
        /// Get Commission By Bet Type.
        /// </summary>
        /// <param name="criteria">The criteria used to get data.</param>
        /// <returns>Commission By Bet Type List model.</returns>
        public CommissionByBettypeList GetCommissionByBettype(CommissionByBettypeCriteria criteria)
        {
            IEnumerable<CommissionByBettype> rawData = GetListData<CommissionByBettype>(criteria);
            CommissionByBettypeList commissionByBettype = new CommissionByBettypeList();
            CommissionByBettype total = new CommissionByBettype();

            if (rawData != null && rawData.Any())
            {
                foreach (CommissionByBettype item in rawData)
                {
                    var viewItem = new CommissionByBettypeItem(item);

                    commissionByBettype.AddItem(viewItem);

                    total.TotalComm_1 += item.TotalComm_1;
                    total.TotalComm_2 += item.TotalComm_2;
                    total.TotalComm_3 += item.TotalComm_3;
                    total.Casino_Comm += item.Casino_Comm;
                }

                commissionByBettype.Total = new CommissionByBettypeItem(total);
            }

            return commissionByBettype;
        }

        public MemoryStream ExportExcelCommissionByBettype(CommissionByBettypeCriteria criteria, CommissionByBettypeList data, bool isBold)
        {
            if (data == null)
            {
                data = this.GetCommissionByBettype(criteria);
            }

            return this.ExportReport(criteria, data, isBold);
        }

        private MemoryStream ExportReport(CommissionByBettypeCriteria criteria, CommissionByBettypeList data, bool isBold)
        {
            var builder = new CommissionByBetTypeReportBuilder(data, isBold, criteria.FromDate, criteria.ToDate, criteria.RoleId, criteria.CustId);

            return builder.Build();
        }
    }
}