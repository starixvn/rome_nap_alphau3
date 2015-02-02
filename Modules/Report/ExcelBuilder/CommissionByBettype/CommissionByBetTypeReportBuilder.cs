// --------------------------------------------------------------------------
// <copyright file="CommissionByBetTypeReportBuilder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.10.03">Created</change>
// </history>
namespace Fanex.Razorbill.Module.Report.ExcelBuilder.CommissionByBettype
{
    using System;
    using System.IO;
    using Fanex.Age.Excel;
    using Fanex.Age.Excel.Reports.Commission;
    using Fanex.Razorbill.Module.Report.ViewModels.Commission;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// Report builder for Commission By Bet Type.
    /// </summary>
    public class CommissionByBetTypeReportBuilder
    {
        public CommissionByBetTypeReportBuilder(CommissionByBettypeList data, bool isBold, DateTime fromDate, DateTime toDate, int roleId, int custId)
        {
            Data = data;
            IsBold = isBold;
            FromDate = fromDate;
            ToDate = toDate;
            RoleId = roleId;
            CustId = custId;
        }

        public CommissionByBettypeList Data { get; private set; }

        public bool IsBold { get; private set; }

        public DateTime FromDate { get; private set; }

        public DateTime ToDate { get; private set; }

        public int RoleId { get; private set; }

        public int CustId { get; private set; }

        /// <summary>
        /// Build Report.
        /// </summary>
        /// <returns>Result in memory stream.</returns>
        public MemoryStream Build()
        {
            TemplateReport report = null;
            TemplateSettings settings = new TemplateSettings();
            string filePath = settings.CommissionByBetTypeNewPath();

            report = new CommissionByBetTypeReport(filePath, this);

            if (report != null)
            {
                if (report is IReportBuilder)
                {
                    MemoryStream mem = new MemoryStream();
                    HSSFWorkbook hssfWorkbook = ((IReportBuilder)report).Build();
                    if (hssfWorkbook != null)
                    {
                        hssfWorkbook.Write(mem);
                        return mem;
                    }

                    report.Dispose();
                }
            }

            return null;
        }
    }
}