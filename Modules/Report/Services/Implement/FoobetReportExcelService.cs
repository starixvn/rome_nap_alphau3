// --------------------------------------------------------------------------
// <copyright file="FoobetReportExcelService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2013.12.10">Created</change>
//   <change who="Kan.Nguyen" date="2014.05.23">New requirement #16452</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Foobet.Impl
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Fanex.Razorbill.Module.Core.Extensions;
    using Fanex.Razorbill.Module.Report.Entities.FooBet;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// FOOBET report service.
    /// </summary>
    public class FoobetReportExcelService
    {
        private const string _ResourceClass = "Report";

        private IFont _fontBlackNormal;
        private IFont _fontBlackBold;
        private IFont _fontWhiteNormal;
        private IFont _fontWhiteBold;

        private ICellStyle _cell;

        private HSSFWorkbook _workbook;

        private ICellStyle _titleStyle;

        private HSSFPalette _palette;

        private short _colorIndex;

        private short _borderColorIndex;

        private int _startRowNumber = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoobetReportExcelService" /> class.
        /// </summary>
        public FoobetReportExcelService()
        {
            this._workbook = new HSSFWorkbook();
            this._palette = _workbook.GetCustomPalette();
            this._colorIndex = _palette.FindSimilarColor(255, 255, 204).GetIndex();
            this._borderColorIndex = _palette.FindSimilarColor(106, 105, 105).GetIndex();

            this._fontBlackBold = _workbook.CreateFont();
            this._fontBlackNormal = _workbook.CreateFont();
            this._fontWhiteNormal = _workbook.CreateFont();
            this._fontWhiteBold = _workbook.CreateFont();

            this._fontBlackBold.FontName = "Tahoma";
            this._fontBlackBold.FontHeightInPoints = 9;
            this._fontBlackBold.Color = HSSFColor.Black.Index;
            this._fontBlackBold.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

            this._fontBlackNormal.FontName = "Tahoma";
            this._fontBlackNormal.FontHeightInPoints = 9;
            this._fontBlackNormal.Color = HSSFColor.Black.Index;

            this._fontWhiteNormal.FontName = "Tahoma";
            this._fontWhiteNormal.FontHeightInPoints = 9;
            this._fontWhiteNormal.Color = HSSFColor.White.Index;

            this._fontWhiteBold.FontName = "Tahoma";
            this._fontWhiteBold.FontHeightInPoints = 9;
            this._fontWhiteBold.Color = HSSFColor.White.Index;
            this._fontWhiteBold.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

            this._cell = _workbook.CreateCellStyle();
            this._cell.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            this._cell.BottomBorderColor = _borderColorIndex;
            this._cell.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            this._cell.TopBorderColor = _borderColorIndex;
            this._cell.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            this._cell.LeftBorderColor = _borderColorIndex;
            this._cell.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            this._cell.RightBorderColor = _borderColorIndex;
            this._cell.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            this._cell.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

            // Title Style
            this._titleStyle = _workbook.CreateCellStyle();
            this._titleStyle.Indention = 2;
            this._titleStyle.FillForegroundColor = _colorIndex;
            this._titleStyle.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            this._titleStyle.SetFont(_fontBlackBold);
        }

        /// <summary>
        /// Generates the specified report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns>The MemoryStream object.</returns>
        public MemoryStream Generate(FooBetReport report)
        {
            var sheetName = ResourceExtension.GetResource(_ResourceClass, "FoobetTitle");
            var worksheet = _workbook.CreateSheet(sheetName);
            RenderReportTitle(ref worksheet, report.ReportTitle);
            RenderHeader(ref worksheet);
            if (report.FooBets != null && report.FooBets.Count > 0)
            {
                RenderBody(ref worksheet, report.FooBets);
                RenderFooter(ref worksheet, report.Total);
            }
            else
            {
                RenderNoDataFound(ref worksheet);
            }

            var result = new MemoryStream();
            _workbook.Write(result);
            
            return result;
        }

        private void RenderReportTitle(ref ISheet worksheet, string title)
        { 
            var titleRow = worksheet.CreateRow(0);
            var titleCell = titleRow.CreateCell(0);
            worksheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 8));
            titleCell.SetCellValue(title);
            titleCell.CellStyle = _titleStyle;
            _startRowNumber++;
        }

        /// <summary>
        /// Create header for EXCEL file.
        /// </summary>
        /// <param name="worksheet">Work sheet reference.</param>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void RenderHeader(ref ISheet worksheet)
        {
            ICellStyle headerStyleNormal = _workbook.CreateCellStyle();
            ICellStyle headerStyleBold = _workbook.CreateCellStyle();

            headerStyleNormal.CloneStyleFrom(this._cell);
            headerStyleNormal.SetFont(_fontWhiteBold);
            headerStyleNormal.FillForegroundColor = _palette.FindSimilarColor(128, 128, 128).GetIndex();
            headerStyleNormal.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            headerStyleBold.CloneStyleFrom(this._cell);
            headerStyleBold.SetFont(_fontWhiteBold);
            headerStyleBold.FillForegroundColor = _palette.FindSimilarColor(64, 64, 64).GetIndex();
            headerStyleBold.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            int standardWidth = 20 * 256;
            var headerRow1 = worksheet.CreateRow(_startRowNumber);
            headerRow1.Sheet.SetColumnWidth(0, 12 * 256);
            headerRow1.Sheet.SetColumnWidth(1, standardWidth);
            headerRow1.Sheet.SetColumnWidth(2, standardWidth);
            headerRow1.Sheet.SetColumnWidth(3, standardWidth);
            headerRow1.Sheet.SetColumnWidth(4, standardWidth);
            headerRow1.Sheet.SetColumnWidth(5, standardWidth);
            headerRow1.Sheet.SetColumnWidth(6, standardWidth);
            headerRow1.Sheet.SetColumnWidth(7, standardWidth);
            headerRow1.Sheet.SetColumnWidth(8, standardWidth);

            headerRow1.CreateCell(0).SetCellValue(ResourceExtension.GetResource(_ResourceClass, "Month"));
            headerRow1.GetCell(0).CellStyle = headerStyleNormal;
            headerRow1.CreateCell(1).SetCellValue(ResourceExtension.GetResource(_ResourceClass, "Turnover"));
            headerRow1.GetCell(1).CellStyle = headerStyleNormal;
            headerRow1.CreateCell(2).SetCellValue(ResourceExtension.GetResource(_ResourceClass, "BetCount"));
            headerRow1.GetCell(2).CellStyle = headerStyleNormal;
            headerRow1.CreateCell(3).SetCellValue(ResourceExtension.GetResource(_ResourceClass, "FBWinLoss"));
            headerRow1.GetCell(3).CellStyle = headerStyleNormal;
            headerRow1.CreateCell(4);
            headerRow1.GetCell(4).CellStyle = headerStyleNormal;
            headerRow1.CreateCell(5);
            headerRow1.GetCell(5).CellStyle = headerStyleNormal;
            headerRow1.CreateCell(6).SetCellValue(ResourceExtension.GetResource(_ResourceClass, "LosingTax"));
            headerRow1.GetCell(6).CellStyle = headerStyleNormal;
            headerRow1.CreateCell(7).SetCellValue(ResourceExtension.GetResource(_ResourceClass, "FBRebate"));
            headerRow1.GetCell(7).CellStyle = headerStyleNormal;
            headerRow1.CreateCell(8).SetCellValue(ResourceExtension.GetResource(_ResourceClass, "CompanyIncome"));
            headerRow1.GetCell(8).CellStyle = headerStyleNormal;
            _startRowNumber++;

            var headerRow2 = worksheet.CreateRow(_startRowNumber);
            headerRow2.CreateCell(0);
            headerRow2.GetCell(0).CellStyle = headerStyleNormal;
            headerRow2.CreateCell(1);
            headerRow2.GetCell(1).CellStyle = headerStyleNormal;
            headerRow2.CreateCell(2);
            headerRow2.GetCell(2).CellStyle = headerStyleNormal;
            headerRow2.CreateCell(3).SetCellValue(ResourceExtension.GetResource(_ResourceClass, "Win"));
            headerRow2.GetCell(3).CellStyle = headerStyleNormal;
            headerRow2.CreateCell(4).SetCellValue(ResourceExtension.GetResource(_ResourceClass, "Lose"));
            headerRow2.GetCell(4).CellStyle = headerStyleNormal;
            headerRow2.CreateCell(5).SetCellValue(ResourceExtension.GetResource(_ResourceClass, "Total"));
            headerRow2.GetCell(5).CellStyle = headerStyleNormal;
            headerRow2.CreateCell(6);
            headerRow2.GetCell(6).CellStyle = headerStyleNormal;
            headerRow2.CreateCell(7);
            headerRow2.GetCell(7).CellStyle = headerStyleNormal;
            headerRow2.CreateCell(8);
            headerRow2.GetCell(8).CellStyle = headerStyleNormal;
            _startRowNumber++;

            // Merge header (first row, last row, first col, last col)
            worksheet.AddMergedRegion(new CellRangeAddress(1, 2, 0, 0));
            worksheet.AddMergedRegion(new CellRangeAddress(1, 2, 1, 1));
            worksheet.AddMergedRegion(new CellRangeAddress(1, 2, 2, 2));
            worksheet.AddMergedRegion(new CellRangeAddress(1, 1, 3, 5));
            worksheet.AddMergedRegion(new CellRangeAddress(1, 2, 6, 6));
            worksheet.AddMergedRegion(new CellRangeAddress(1, 2, 7, 7));
            worksheet.AddMergedRegion(new CellRangeAddress(1, 2, 8, 8));
        }

        /// <summary>
        /// Render body for EXCEL file.
        /// </summary>
        /// <param name="worksheet">Work sheet reference.</param>
        /// <param name="foobets">List FOOBET model.</param>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private void RenderBody(ref ISheet worksheet, IList<FooBetDTO> foobets)
        {
            short decimalFormat = _workbook.CreateDataFormat().GetFormat("#,##0.00");
            short intergerFormat = _workbook.CreateDataFormat().GetFormat("#,##0");

            ICellStyle whiteStyleNormal = _workbook.CreateCellStyle();
            ICellStyle whiteStyleNormalCenter = _workbook.CreateCellStyle();
            ICellStyle whiteStyleBold = _workbook.CreateCellStyle();
            ICellStyle yellowStyleNormal = _workbook.CreateCellStyle();
            ICellStyle yellowStyleBold = _workbook.CreateCellStyle();

            ICellStyle betCountStyle = _workbook.CreateCellStyle();
            betCountStyle.CloneStyleFrom(this._cell);
            betCountStyle.SetFont(this._fontBlackNormal);
            betCountStyle.FillForegroundColor = _palette.FindSimilarColor(255, 255, 255).GetIndex();
            betCountStyle.DataFormat = intergerFormat;

            whiteStyleNormal.CloneStyleFrom(this._cell);
            whiteStyleNormal.SetFont(this._fontBlackNormal);
            whiteStyleNormal.FillForegroundColor = _palette.FindSimilarColor(255, 255, 255).GetIndex();

            whiteStyleNormalCenter.CloneStyleFrom(this._cell);
            whiteStyleNormalCenter.SetFont(this._fontBlackNormal);
            whiteStyleNormalCenter.FillForegroundColor = _palette.FindSimilarColor(255, 255, 255).GetIndex();
            whiteStyleNormalCenter.Alignment = HorizontalAlignment.Center;

            whiteStyleBold.CloneStyleFrom(this._cell);
            whiteStyleBold.SetFont(this._fontBlackBold);
            whiteStyleBold.FillForegroundColor = _palette.FindSimilarColor(255, 255, 255).GetIndex();

            yellowStyleNormal.CloneStyleFrom(this._cell);
            yellowStyleNormal.SetFont(this._fontBlackNormal);
            yellowStyleNormal.FillForegroundColor = _palette.FindSimilarColor(255, 242, 205).GetIndex();

            yellowStyleBold.CloneStyleFrom(this._cell);
            yellowStyleBold.SetFont(this._fontBlackBold);
            yellowStyleBold.FillForegroundColor = _palette.FindSimilarColor(255, 242, 205).GetIndex();

            FooBetDTO foobet;
            IRow newRow;
            var foobetsLenght = foobets.Count;
            for (int i = 0; i < foobetsLenght; i++)
            {
                foobet = foobets[i];
                newRow = worksheet.CreateRow(_startRowNumber);

                var cell = newRow.CreateCell(0);
                cell.SetCellValue(foobet.Month);
                cell.CellStyle = whiteStyleNormalCenter;

                cell = newRow.CreateCell(1);
                cell.SetCellValue(ConvertToDouble(foobet.Turnover));
                cell.CellStyle = whiteStyleNormal;
                cell.CellStyle.DataFormat = decimalFormat;

                cell = newRow.CreateCell(2);
                cell.SetCellValue(ConvertToDouble(foobet.BetCount));
                cell.CellStyle = betCountStyle;

                cell = newRow.CreateCell(3);
                cell.SetCellValue(ConvertToDouble(foobet.WinAmount));
                cell.CellStyle = whiteStyleNormal;
                cell.CellStyle.DataFormat = decimalFormat;

                cell = newRow.CreateCell(4);
                cell.SetCellValue(ConvertToDouble(foobet.LoseAmount));
                cell.CellStyle = whiteStyleNormal;
                cell.CellStyle.DataFormat = decimalFormat;

                cell = newRow.CreateCell(5);
                cell.SetCellValue(ConvertToDouble(foobet.TotalAmount));
                cell.CellStyle = whiteStyleBold;
                cell.CellStyle.DataFormat = decimalFormat;

                cell = newRow.CreateCell(6);
                cell.SetCellValue(ConvertToDouble(foobet.LosingTax));
                cell.CellStyle = whiteStyleNormal;
                cell.CellStyle.DataFormat = decimalFormat;

                cell = newRow.CreateCell(7);
                cell.SetCellValue(ConvertToDouble(foobet.FBRebate));
                cell.CellStyle = yellowStyleNormal;
                cell.CellStyle.DataFormat = decimalFormat;

                cell = newRow.CreateCell(8);
                cell.SetCellValue(ConvertToDouble(foobet.CompanyIncome));
                cell.CellStyle = yellowStyleNormal;
                cell.CellStyle.DataFormat = decimalFormat;

                _startRowNumber++;
            }
        }

        /// <summary>
        /// Render footer for EXCEL file.
        /// </summary>
        /// <param name="worksheet">Work sheet reference.</param>
        /// <param name="total">The total.</param>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private void RenderFooter(ref ISheet worksheet, FooBetDTO total)
        {
            ICellStyle totalStyleRight = _workbook.CreateCellStyle();
            totalStyleRight.CloneStyleFrom(this._cell);
            totalStyleRight.SetFont(this._fontWhiteBold);
            totalStyleRight.FillForegroundColor = _palette.FindSimilarColor(131, 61, 12).GetIndex();

            ICellStyle totalStyleCenter = _workbook.CreateCellStyle();
            totalStyleCenter.CloneStyleFrom(this._cell);
            totalStyleCenter.SetFont(this._fontWhiteBold);
            totalStyleCenter.FillForegroundColor = _palette.FindSimilarColor(131, 61, 12).GetIndex();
            totalStyleCenter.Alignment = HorizontalAlignment.Center;

            short decimalFormat = _workbook.CreateDataFormat().GetFormat("#,##0.00");
            short intergerFormat = _workbook.CreateDataFormat().GetFormat("#,##0");

            var totalRow = worksheet.CreateRow(_startRowNumber);
            var totalCell = totalRow.CreateCell(0);
            totalCell.SetCellValue(total.Month);
            totalCell.CellStyle = totalStyleCenter;

            totalCell = totalRow.CreateCell(1);
            totalCell.SetCellValue(ConvertToDouble(total.Turnover));
            totalCell.CellStyle = totalStyleRight;
            totalCell.CellStyle.DataFormat = decimalFormat;

            totalCell = totalRow.CreateCell(2);
            totalCell.SetCellValue(total.BetCount);
            totalCell.CellStyle = totalStyleRight;
            totalCell.CellStyle.DataFormat = intergerFormat;

            totalCell = totalRow.CreateCell(3);
            totalCell.SetCellValue(ConvertToDouble(total.WinAmount));
            totalCell.CellStyle = totalStyleRight;
            totalCell.CellStyle.DataFormat = decimalFormat;

            totalCell = totalRow.CreateCell(4);
            totalCell.SetCellValue(ConvertToDouble(total.LoseAmount));
            totalCell.CellStyle = totalStyleRight;
            totalCell.CellStyle.DataFormat = decimalFormat;

            totalCell = totalRow.CreateCell(5);
            totalCell.SetCellValue(ConvertToDouble(total.TotalAmount));
            totalCell.CellStyle = totalStyleRight;
            totalCell.CellStyle.DataFormat = decimalFormat;

            totalCell = totalRow.CreateCell(6);
            totalCell.SetCellValue(ConvertToDouble(total.LosingTax));
            totalCell.CellStyle = totalStyleRight;
            totalCell.CellStyle.DataFormat = decimalFormat;

            totalCell = totalRow.CreateCell(7);
            totalCell.SetCellValue(ConvertToDouble(total.FBRebate));
            totalCell.CellStyle = totalStyleRight;
            totalCell.CellStyle.DataFormat = decimalFormat;

            totalCell = totalRow.CreateCell(8);
            totalCell.SetCellValue(ConvertToDouble(total.CompanyIncome));
            totalCell.CellStyle = totalStyleRight;
            totalCell.CellStyle.DataFormat = decimalFormat;
        }

        private void RenderNoDataFound(ref ISheet worksheet)
        {
            ICellStyle nodataStyle = _workbook.CreateCellStyle();
            nodataStyle.Alignment = HorizontalAlignment.Center;

            var newRow = worksheet.CreateRow(_startRowNumber);
            var cell = newRow.CreateCell(0);
            cell.CellStyle = nodataStyle;
            cell.SetCellValue(ResourceExtension.GetResource(_ResourceClass, "NoData"));
            worksheet.AddMergedRegion(new CellRangeAddress(_startRowNumber, _startRowNumber, 0, 8));
        }

        private double ConvertToDouble(decimal value)
        {
            return (double)value;
        }
    }
}
