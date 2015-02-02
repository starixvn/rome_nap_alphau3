'use strict';
/* global resources */
function $(id) {
    return document.getElementById(id);
}

// Manipulate style sheet class name
function HasClassName(element, className) {
    if (typeof element == 'string') element = $(element);
    var elementClassName = element.className;
    if (typeof elementClassName == 'undefined') return false;
    return (elementClassName.length > 0 && (elementClassName == className || new RegExp("(^|\\s)" + className + "(\\s|$)").test(elementClassName)));
}

function AddClassName(element, className) {
    if (typeof element == 'string') element = $(element);
    if (!HasClassName(element, className)) element.className += (element.className ? ' ' : '') + className;
    return element;
}

function RemoveClassName(element, className) {
    if (typeof element == 'string') element = $(element);
    var elementClassName = element.className;
    if (typeof elementClassName == 'undefined') return false;
    element.className = element.className.replace(
        new RegExp("(^|\\s+)" + className + "(\\s+|$)"), ' ');
    return element;
}

var FormatConstant = {
    INT_NUMBER: '0,0',
    TWO_DECIMAL_NUMBER: '0,0.00',
    FOUR_DECIMAL_NUMBER: '0,0.0000',
    TWO_DECIMAL_PERCENT_NUMBER: '0,0.00%',
    MMM_DD_YYYY_STRING: 'MMM DD, YYYY',
    M_DD_YY_STRING: 'M dd, yy',
    MM_DD_YYYY_WITH_SLASH_STRING: 'MM/dd/yyyy',
    MMM_YY_STRING: 'MMM YY',
    MMM_YY_STRING_MONTH_PICKER: 'MMM yyyy',
    MMM_DD_YY_STRING_MONTH_PICKER: 'MMM dd, yyyy',
    MM_DD_YYYY_WITH_SLASH_STRING_MOMENT: 'M/D/YYYY',
    MMM_DD_YYYY_STRING_MOMENT: 'MMM D, YYYY',
    MMM_DD_YYYY_H_M_STRING: 'MMM DD, YYYY HH:mm'
};


var FoobetManager = {
    GenerateData: function (fromDate, toDate) {
        DisableButtonById("submitBtn");
        DisableButtonById("todayBtn");
        DisableButtonById("yesterdayBtn");
        if (jQuery("#loading")) {
            jQuery("#loading").addClass("loading");
        }

        jQuery.ajax({
            type: "POST",
            url: domain + 'Report/Foobet/GetFoobetData?fromDate=' + fromDate + '&toDate=' + toDate,
            responseType: "json",
            success: function (rs) {
                FoobetManager.Render(rs);
                jQuery("#filter").width(jQuery("#GridContainer").width() + 22);
                EnableButtonById("submitBtn");
                EnableButtonById("todayBtn");
                EnableButtonById("yesterdayBtn");
                if (jQuery("#loading")) {
                    jQuery("#loading").removeClass("loading");
                }
            },
            error: function (jqXhr, event, ajaxSettings) {
                /*if (jqXhr.status && jqXhr.status == 500) {
                    var r = jQuery.parseJSON(jqXhr.responseText);
                }*/
                EnableButtonById("submitBtn");
                EnableButtonById("todayBtn");
                EnableButtonById("yesterdayBtn");
                if (jQuery("#loading")) {
                    jQuery("#loading").removeClass("loading");
                }
            }
        });
    },

    Render: function (data) {
        var bodyData = [];
        var footerData = [];
        if (data) {
            var foobet, currentRow;
            var length = data.FooBets ? data.FooBets.length : 0;
            for (var i = 0; i < length; i++) {
                foobet = data.FooBets[i];
                currentRow = { Columns: [
                    { Name: "Month-1", Value: foobet.Month },
                    { Name: "Turnover-2", Value: foobet.Turnover },
                    { Name: "BetCount-3", Value: foobet.BetCount },
                    { Name: "WinAmount-4", Value: foobet.WinAmount },
                    { Name: "LoseAmount-5", Value: foobet.LoseAmount },
                    { Name: "TotalAmount-6", Value: foobet.TotalAmount },
                    { Name: "LosingTax-7", Value: foobet.LosingTax },
                    { Name: "FBRebate-8", Value: foobet.FBRebate },
                    { Name: "CompanyIncome-9", Value: foobet.CompanyIncome}]
                };

                bodyData.push(currentRow);
            }

            var total = data.Total;
            footerData = [{ Columns: [
                { Name: "Month-1", Value: total.Month },
                { Name: "Turnover-2", Value: total.Turnover },
                { Name: "BetCount-3", Value: total.BetCount },
                { Name: "WinAmount-4", Value: total.WinAmount },
                { Name: "LoseAmount-5", Value: total.LoseAmount },
                { Name: "TotalAmount-6", Value: total.TotalAmount },
                { Name: "LosingTax-7", Value: total.LosingTax },
                { Name: "FBRebate-8", Value: total.FBRebate },
                { Name: "CompanyIncome-9", Value: total.CompanyIncome}]
            }];
        }

        FoobetManager.RenderGrid(bodyData, footerData, data.ReportTitle);
    },

    RenderGrid: function (bodyData, footerData, reportTitle) {
        var groupHeaders = [{ Columns: [
            { HeaderText: resources.Month, Name: "Month-1", MergeRowNumber: 2 },
            { HeaderText: resources.Turnover, Name: "Turnover-2", MergeRowNumber: 2 },
            { HeaderText: resources.BetCount, Name: "BetCount-3", MergeRowNumber: 2 },
            { HeaderText: resources.FBWinLoss, Name: "WinAmount-4", MergeColNumber: 3 },
            { HeaderText: resources.LosingTax, Name: "LosingTax-7", MergeRowNumber: 2 },
            { HeaderText: resources.FBRebate, Name: "FBRebate-8", MergeRowNumber: 2 },
            { HeaderText: resources.CompanyIncome, Name: "CompanyIncome-9", MergeRowNumber: 2}]
        }];

        var cols = [{
            HeaderText: resources.Month,
            Width: 90,
            Name: "Month-1",
            HeaderAlign: "center",
            CellAlign: "center"
        }, {
            HeaderText: resources.Turnover,
            Width: 110,
            Name: "Turnover-2",
            HeaderAlign: "center",
            CellAlign: "right",
            FormatNumber: "0,0.00"
        }, {
            HeaderText: resources.BetCount,
            Width: 110,
            Name: "BetCount-3",
            HeaderAlign: "center",
            CellAlign: "right",
            FormatNumber: "0,0"
        }, {
            HeaderText: resources.Win,
            Width: 110,
            Name: "WinAmount-4",
            HeaderAlign: "center",
            CellAlign: "right",
            FormatNumber: "0,0.00"
        }, {
            HeaderText: resources.Lose,
            Width: 110,
            Name: "LoseAmount-5",
            HeaderAlign: "center",
            CellAlign: "right",
            FormatNumber: "0,0.00"
        }, {
            HeaderText: resources.Total,
            Width: 110,
            Name: "TotalAmount-6",
            HeaderAlign: "center",
            CellAlign: "right",
            CellCssClass: "bold",
            FormatNumber: "0,0.00"
        }, {
            HeaderText: resources.LosingTax,
            Width: 110,
            Name: "LosingTax-7",
            HeaderAlign: "center",
            CellAlign: "right",
            FormatNumber: "0,0.00"
        }, {
            HeaderText: resources.FBRebate,
            Width: 110,
            Name: "FBRebate-8",
            HeaderAlign: "center",
            CellAlign: "right",
            FormatNumber: "0,0.00",
            TdClass: 'foobet'
        }, {
            HeaderText: resources.CompanyIncome,
            Width: 110,
            Name: "CompanyIncome-9",
            HeaderAlign: "center",
            CellAlign: "right",
            FormatNumber: "0,0.00",
            TdClass: 'foobet'
        }];

        var gridOptions = {
            customizeHeader: true,
            groupHeaderRows: groupHeaders,
            columns: cols,
            bodyRows: bodyData,
            //headerColumnHeight: 17,
            bodyRowHeight: 20,
            footerRowHeight: 17,
            showTitle: true,
            showFooter: true,
            titleInfo: {
                text: reportTitle,
                showExcelButton: false,
                showBackButton: false,
                showSendMailButton: false
            },
            language: _language != '' ? _language : 'en-US',
            showTree: false,
            footerRows: footerData,
            numberOfFrozenColumn: 0,
            gridExpandHeight: 180,
            showDistinctRow: true,
            showPager: false
        };

        jQuery('#GridContainer').fanexGrid(gridOptions);
    }
};
