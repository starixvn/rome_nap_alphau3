// --------------------------------------------------------------------------
// <copyright file="FoobetService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2013.12.10">Created</change>
//   <change who="Kan.Nguyen" date="2014.05.22">New requirement #16452</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Foobet.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.Razorbill.Module.Core.Extensions;
    using Fanex.Razorbill.Module.Core.Services.Impl;
    using Fanex.Razorbill.Module.Core.Web;
    using Fanex.Razorbill.Module.Report.Criteria.Foobet;
    using Fanex.Razorbill.Module.Report.Entities.FooBet;
    using Fanex.Razorbill.Module.Report.Services.Foobet.Interface;

    /// <summary>
    /// FOOBET services.
    /// </summary>
    public class FoobetService : ServiceBase, IFoobetService
    {
        private const string ResourceClass = "Report";
        private const string SessionKeyFoobet = "Fanex.Razorbill.Report.Foobet";
        private const string DateFormat = "MM/dd/yyyy";

        /// <summary>
        /// Gets the FooBet report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The FooBetReport object.</returns>
        public FooBetReport GetFoobetReport(FoobetCriteria criteria)
        {
            FooBetReport report = null;
            try
            {
               report = ParseFooBetReport(criteria);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return report;
        }

        public MemoryStream ExportFoobetReport(FoobetCriteria criteria)
        {
            criteria.IsCached = true;
            FooBetReport report = GetFoobetReport(criteria);

            FoobetReportExcelService excelFooBet = new FoobetReportExcelService();
            return excelFooBet.Generate(report);
        }

        #region Helper methods

        /// <summary>
        /// Gets the FooBet DTO.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of FooBetDTO.</returns>
        private FooBetReport ParseFooBetReport(FoobetCriteria criteria)
        {
            FooBetReport report = null;
            string key = SessionKeyFoobet;
            try
            {
                report = criteria.IsCached ? SessionManager.Get<FooBetReport>(key) : null;
                if (report == null)
                {
                    FooBetDTO total = new FooBetDTO();
                    IList<FooBetDTO> foobets = new List<FooBetDTO>();

                    var rawData = GetListData<FooBetDTO>(criteria);
                    foreach (var fb in rawData)
                    {
                        foobets.Add(fb.Format2Digits(fb));
                        total += fb;
                    }

                    total.Month = ResourceExtension.GetResource(ResourceClass, "Total");
                    string reportTitle = string.Format(
                        "{0} {1} --> {2}",
                        ResourceExtension.GetResource(ResourceClass, "FoobetTitle"),
                        criteria.FromDate.ToString(DateFormat),
                        criteria.ToDate.ToString(DateFormat));
                    report = new FooBetReport 
                    {
                        ReportTitle = reportTitle,
                        FooBets = foobets,
                        Total = total.Format2Digits(total)
                    };

                    SessionManager.Set(key, report);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return report;
        }

        #endregion
    }
}