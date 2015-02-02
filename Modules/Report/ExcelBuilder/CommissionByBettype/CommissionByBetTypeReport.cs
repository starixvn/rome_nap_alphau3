// --------------------------------------------------------------------------
// <copyright file="CommissionByBetTypeReport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.ExcelBuilder.CommissionByBettype
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Age.Excel;
    using Fanex.Age.Utility.NPOI;
    using Fanex.Razorbill.Module.Core.Extensions;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Sunplus.Agent.Utilities;    

    /// <summary>
    /// Commission By Bet Type report.
    /// </summary>
    public class CommissionByBetTypeReport : TemplateReport, IReportBuilder
    {
        private const string ResourceClass = "Lang";
        private CommissionByBetTypeReportBuilder _builder;

        public CommissionByBetTypeReport(string filePath, CommissionByBetTypeReportBuilder builder)
            : base(filePath)
        {
            _builder = builder;
        }

        /// <summary>
        /// Build Report.
        /// </summary>
        /// <returns>HSSF Work book.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public HSSFWorkbook Build()
        {
            var data = _builder.Data.CommissionList;
            if (data == null)
            {
                return null;
            }

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("CommissionByBetType");

            // apply Bold on all fonts
            if (_builder.IsBold)
            {
                BoldAllFonts();
            }

            int rowIdContent = 2;
            int rowId = 0;
            int colId = 0;
            hssfWorkbook.SetSheetName(0, ResourceExtension.GetResource(ResourceClass, "lblCommissionByBettype"));

            // header
            short totalColumn = 5;
            short totalRemoveColumn = 0;

            string currentUserType = string.Empty;
            
            if (_builder.RoleId == (int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Super)
            {
                currentUserType = ResourceExtension.GetResource(ResourceClass, "superCommissionByBetType");
            }
            else if (_builder.RoleId == (int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Master)
            {
                currentUserType = ResourceExtension.GetResource(ResourceClass, "masterCommissionByBetType");
            }
            else
            {
                currentUserType = ResourceExtension.GetResource(ResourceClass, "agentCommissionByBetType");
            }

            resultSheet.GetCell(rowId, colId).SetCellValue(currentUserType + " " +
                Formater.ShortDateFormat(_builder.FromDate) + "-->" + Formater.ShortDateFormat(_builder.ToDate));
            rowId = 1;

            var values = new List<object>
            {
                ResourceExtension.GetResource(ResourceClass, "username"),
                ResourceExtension.GetResource(ResourceClass, "commission") + " 1",
                ResourceExtension.GetResource(ResourceClass, "commission") + " 2",
                ResourceExtension.GetResource(ResourceClass, "commission") + " 3",
                ResourceExtension.GetResource(ResourceClass, "Gaming")
            };

            resultSheet.GetRow(rowId).WriteCells(colId, values);

            // content
            rowId = rowIdContent;
            int id = 1;

            if (data.Count > 0)
            {
                int lastColumnIndex = colId + totalColumn - totalRemoveColumn - 1;

                foreach (var item in data)
                {
                    if (id % 2 == 0)
                    {
                        resultSheet.CopyRow(templateSheet, 1, rowId, colId, lastColumnIndex);
                    }
                    else
                    {
                        resultSheet.CopyRow(templateSheet, 3, rowId, colId, lastColumnIndex);
                    }

                    values = new List<object>
                    {
                        item.UserName,
                        Convert.ToDecimal(item.TotalComm_1),
                        Convert.ToDecimal(item.TotalComm_2),
                        Convert.ToDecimal(item.TotalComm_3),
                        Convert.ToDecimal(item.Casino_Comm)
                    };

                    resultSheet.GetRow(rowId).WriteCells(colId, values);

                    id++;
                    rowId++;
                }

                resultSheet.CopyRow(templateSheet, 5, rowId, colId, lastColumnIndex);
                var totalRow = _builder.Data.Total;
                values = new List<object>
                {
                    ResourceExtension.GetResource(ResourceClass, "total"),
                    Convert.ToDecimal(totalRow.TotalComm_1),
                    Convert.ToDecimal(totalRow.TotalComm_2),
                    Convert.ToDecimal(totalRow.TotalComm_3),
                    Convert.ToDecimal(totalRow.Casino_Comm)
                };

                resultSheet.GetRow(rowId).WriteCells(colId, values);
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));

            return hssfWorkbook;
        }
    }
}