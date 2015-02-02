function $(id) {
    return document.getElementById(id);
}
function ChangeDate(el) {
    var val = el.value;
    GetDateTime(val);
}
function ChangeCurrency() {
}
function SetDateTimeRange(from, to) {
    $("fdate").value = from;
    $("tdate").value = to;
}
function GetDateTime(_value) {
    var now = new Date();
    var cmonth = now.getMonth() + 1;
    var cday = now.getDate();
    var cyear = now.getFullYear();
    var $date = new DateTime(cyear, cmonth, cday);
    switch (parseInt(_value)) {
        case 0:
            return;
            break;
        case 1:
            SetDateTimeRange($date.ToString(), $date.ToString());
            break;
        case 2:
            var yesterdate = $date.Yesterday();
            SetDateTimeRange(yesterdate, yesterdate);
            break;
        case 3:
            SetDateTimeRange($date.CurrentWeek(), $date.ToString());
            break;
        case 4:
            SetDateTimeRange($date.LastWeekStart(), $date.LastWeekEnd());
            break;
        case 5:
            SetDateTimeRange($date.CurrentMonth(), $date.ToString());
            break;
        case 6:
            SetDateTimeRange($date.LastMonthStart(), $date.LastMonthEnd());
            break;
        case 7:
            SetDateTimeRange($date.LastMonthStart(), $date.ToString());
            break;
        case 8:
            return;
            break;
    }
    return;
}
//define a DateTime Class
function DateTime(year, month, day) {
    //field
    this.year = parseInt(year, 10);
    this.month = parseInt(month, 10);
    this.day = parseInt(day, 10);
    //end field
    //////////
    //Method
    this.ToString = function () {
        return (this.month < 10 ? ("0" + this.month) : this.month) + "/" + (this.day < 10 ? ("0" + this.day) : this.day) + "/" + this.year;
    };
    this.DaysInMonth = function () {
        var m = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (this.month != 2) return m[this.month - 1];
        if (this.year % 4 != 0) return m[1];
        if (this.year % 100 == 0 && this.year % 400 != 0) return m[1];
        return m[1] + 1;
    };
    //
    this.Reset = function () {
        this.year = parseInt(year, 10);
        this.month = parseInt(month, 10);
        this.day = parseInt(day, 10);
    };
    this.Yesterday = function () {
        this.AddDay(-1);
        var temp = this.ToString();
        this.Reset();
        return temp;
    };
    this.AddDay = function (seek) {
        var max = this.DaysInMonth();
        var newday = this.day + seek;
        if (newday <= max && newday > 0) {
            this.day = newday;
        }
        else {
            if (newday < 0) {
                if (this.month == 1) {
                    this.year--;
                    this.month = 12;
                }
                else {
                    this.month--;
                }
                this.day = this.DaysInMonth() + newday;
            }
            else {
                //newday>max
                if (this.month == 12) {
                    this.year++;
                    this.month = 1;
                }
                else {
                    this.month++;
                }
                this.day = this.DaysInMonth() - newday;
            }
        }
    };
    this.CurrentWeek = function () {
        var d = new Date();
        d.setFullYear(this.year, this.month - 1, this.day);
        var dayofweek = parseInt(d.getDay());
        this.AddDay(-dayofweek);
        var temp = this.ToString();
        this.Reset();
        return temp;
    };
    this.LastWeekStart = function () {
        var d = new Date();
        d.setFullYear(this.year, this.month - 1, this.day);
        var dayofweek = parseInt(d.getDay());
        this.AddDay(-(dayofweek + 7));
        var temp = this.ToString();
        this.Reset();
        return temp;
    };
    this.LastWeekEnd = function () {
        var d = new Date();
        d.setFullYear(this.year, this.month - 1, this.day);
        var dayofweek = parseInt(d.getDay());
        this.AddDay(-(dayofweek + 1));
        var temp = this.ToString();
        this.Reset();
        return temp;
    };
    this.CurrentMonth = function () {
        this.day = 1;
        var temp = this.ToString();
        this.Reset();
        return temp;
    };
    this.LastMonthStart = function () {
        if (this.month == 1) {
            this.year--;
            this.month = 12;
        }
        else {
            this.month--;
        }
        this.day = 1;
        var temp = this.ToString();
        this.Reset();
        return temp;
    };
    this.LastMonthEnd = function () {
        if (this.month == 1) {
            this.year--;
            this.month = 12;
        }
        else {
            this.month--;
        }
        this.day = this.DaysInMonth();
        var temp = this.ToString();
        this.Reset();
        return temp;
    };
    //end Method
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

var consolidationReportManager;
jQuery.noConflict();
jQuery(document).ready(function () {
    consolidationReportManager = new ConsolidationReportManager();
    consolidationReportManager.initPage();
});


ConsolidationReportManager = function () {
    var self = this;
    var _table = {
        btnSubmit: null,
        btnExcel: null,
        btnBold: null,
        domExcel: null,
        ddlCurrency: null,
        grdReport: null,
        grdDetails: null,
        chkAll: null,
        lstCheckbox: null,
        rowNoInfo: null,
        frmMain: null,
        grdContainer: null,
        errCode: null,
        errMsg: null
    };

    var _mappingColumns = {
        PartyID: 0,
        PartyName: 1,
        PartyType: 2,
        Description: 3,
        PartyFullName: 4,
        AccountNumber: 5,
        AccountName: 6,
        BankName: 7,
        Benificiary: 8,
        IBAN: 9,
        SwiftCode: 10,
        CorrespondentBank: 11,
        AttnName: 12,
        To1: 13,
        To2: 14
    }

    var gridOptions = {};

    var cachedDOMElements = function () {
        _table.btnSubmit = jQuery('#dSubmit');
        _table.btnExcel = jQuery('#exporttoexcel');
        _table.btnBold = jQuery('#chk_rpt_bold');
        _table.frmMain = jQuery('#frmmain');
        _table.domExcel = '<input type="checkbox" id="chk_rpt_bold" name="BoldExcel" value="true" /><label for="chk_rpt_bold" style="color: #999; text-transform: none; font-weight: bold; line-height: 18px;">Bold</label>&nbsp;<img alt="Export to Excel" src="' + jQuery('#RootURL').val() + '_GlobalResources/Images/excel.gif" id="exporttoexcel" name="exporttoexcel" title="Export to Excel" class="hand" />';
        _table.ddlCurrency = jQuery('#ddlCurrency');
        _table.grdReport = jQuery('#tbl-rpt');
        _table.grdDetails = jQuery('#tbl-rpt-detail');
        _table.grdContainer = jQuery('#GridContainer');
        _table.chkAll = jQuery('#chk_all');
        _table.lstCheckbox = jQuery('input[id^="chk_"]:not("#chk_all")', "#reportFilter");
        _table.errCode = jQuery('#ErrCode');
        _table.errMsg = jQuery('#ErrMsg');
        _table.rowNoInfo = jQuery('<tr class="no_info"><td colspan="15">No information is available</td></tr>');
    }

    var bindingEvent = function () {
        _table.btnSubmit.click(function () {
            self.GenerateData();
        });

        _table.btnExcel.click(function () {
            _table.frmMain.attr('Action', 'Consolidate/ExportExcel?')
        });

        _table.lstCheckbox.each(function () {
            jQuery(this).click(function () { return IsCheck(this); });
        });

        _table.chkAll.click(function () { return CheckAll(this); });

        _table.btnBold.click(function () {
            return BoldAll(this.checked);
        });
    }

    self.initPage = function () {
        ChangeDate($("ddlSelectDate"));
        cachedDOMElements();
        bindingEvent();
        GenerateFilterData();
        renderData([]);
    }

    var BoldAll = function (isBold) {
        jQuery('.chk-bold').each(function () {
            $this = jQuery(this);
            if (isBold) {
                $this.addClass('b');
            } else {
                $this.removeClass('b');
            }
        });
    };

    // Generate Data for controls in filter area
    var GenerateFilterData = function () {
        jQuery.ajax({
            type: "POST",
            url: domain + "Report/Consolidate/GetCurrency",
            dataType: "json",
            data: null,
            responseType: "json",
            success: function (rs) {
                _table.ddlCurrency.html("");
                _table.ddlCurrency.append(jQuery('<option value="0">' + resources.All + '</option>'));
                for (var i = 0; i < rs.length; i++) {
                    _table.ddlCurrency.append(jQuery('<option value="' + rs[i].Value + ' ' + ((rs[i].Selected == true) ? "selected=selected" : " ") + '">' + rs[i].Text + '</option>'));
                }
            },
            error: function (jqXhr, event, ajaxSettings) {
                if (jqXhr.status && jqXhr.status == 500) {
                    var r = jQuery.parseJSON(jqXhr.responseText);
                    alert(r.Message);
                }
            }
        });
    };

    self.GenerateData = function () {
        DisableButton();
        jQuery.ajax({
            type: "POST",
            url: domain + "Report/Consolidate/GetData",
            dataType: "json",
            data: _table.frmMain.serialize(),
            responseType: "json",
            success: function (rs) {
                renderData(rs);
                _table.btnBold.attr('checked', false);
                EnableButton();
            },
            error: function (jqXhr, event, ajaxSettings) {
                if (jqXhr.status && jqXhr.status == 500) {
                    var r = jQuery.parseJSON(jqXhr.responseText);
                    alert(r.Message);
                }
            }
        });
    };

    var renderData = function (data) {
        var reportType = jQuery('input[name=ReportType]:checked').val();
        if (reportType == "1") {
            //UserName
            renderDataUserName(data);
        } else if (reportType == "2") {
            //Daily
            renderDataDaily(data);
        } else if (reportType == "3") {
            //Sport
            renderDataSport(data);
        }

        //bindingEvent();
    };

    var renderDataUserName = function (data) {
        var bodyData = [];
        if (data && data.length > 0) {
            var dataLength = data.length;
            for (var i = 0; i < dataLength; i++) {
                var item = data[i];
                var currentRow = {
                    Columns: [
                                { Name: "Level", Value: item.Level },
                                { Name: "UserName", Value: item.UserName },
                                { Name: "CurrencyName", Value: item.CurrencyName },
                                { Name: "Turnover", Value: item.Turnover },
                                { Name: "MemberWinloss", Value: item.MemberWinloss },
                                { Name: "MemberComm", Value: item.MemberComm },
                                { Name: "MemberTotal", Value: item.MemberTotal },
                                { Name: "OwnerTurnover", Value: item.OwnerTurnover },
                                { Name: "OwnerWinloss", Value: item.OwnerWinloss },
                                { Name: "OwnerComm", Value: item.OwnerComm },
                                { Name: "OwnerTotal", Value: item.OwnerTotal },
                                { Name: "Company", Value: item.Company },
                                { Name: "BetCount", Value: item.BetCount }
                            ]
                };

                bodyData.push(currentRow);
            }
        }

        var cols = [
                    { HeaderText: resources.Level, Width: 40, Name: "Level", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: resources.UserName, Width: 100, Name: "UserName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: resources.Currency, Width: 50, Name: "CurrencyName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: resources.Turnover, Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", TdClass: "chk-bold" },
                    { HeaderText: resources.WinLoss, Width: 100, Name: "MemberWinloss", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.Comm, Width: 70, Name: "MemberComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.Total, Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "b" },
                    { HeaderText: resources.Turnover, Width: 100, Name: "OwnerTurnover", HeaderAlign: "center", TdClass: "altercol", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.WinLoss, Width: 100, Name: "OwnerWinloss", HeaderAlign: "center", TdClass: "altercol", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.Comm, Width: 70, Name: "OwnerComm", HeaderAlign: "center", TdClass: "altercol", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.Total, Width: 100, Name: "OwnerTotal", HeaderAlign: "center", TdClass: "altercol", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "b" },
                    { HeaderText: resources.Company, Width: 100, Name: "Company", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", TdClass: "chk-bold" },
                    { HeaderText: resources.BetCount, Width: 100, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0", TdClass: "chk-bold"}];

        gridOptions = {
            customizeHeader: true,
            groupHeaderRows: [
                {
                    Columns: [
                                { HeaderText: resources.Level, Name: "Level", MergeRowNumber: 2 },
                                { HeaderText: resources.UserName, Name: "UserName", MergeRowNumber: 2 },
                                { HeaderText: resources.Currency, Name: "CurrencyName", MergeRowNumber: 2 },
                                { HeaderText: resources.Turnover, Name: "Turnover", MergeRowNumber: 2 },
                                { HeaderText: resources.Member, Name: "MemberWinloss", MergeColNumber: 3 },
                                { HeaderText: resources.Owner, Name: "OwnerTurnover", MergeColNumber: 4 },
                                { HeaderText: resources.Company, Name: "Company", MergeRowNumber: 2 },
                                { HeaderText: resources.BetCount, Name: "BetCount", MergeRowNumber: 2 }
                            ]
                }
            ],
            showTitle: true,
            titleInfo:
            {
                text: resources.UserNameConsolidatedReport + " " + jQuery('#fdate').val() + "-->" + jQuery('#tdate').val() + " - " + (jQuery('#ddlCurrency').val() == "0" || jQuery('#ddlCurrency').val() == undefined ? resources.AllCurrencies : resources.ViewUnder + " " + jQuery('#ddlCurrency').find('option:checked').text()),
                showExcelButton: false,
                showBackButton: false,
                showSendMailButton: false
                //customButtons: _table.domExcel
            },
            columns: cols,
            bodyRows: bodyData,
            numberOfFrozenColumn: 2,
            gridExpandHeight: 200,
            noDataText: resources.GridNoDataText ,
            showDistinctRow: false
        };

        _table.grdContainer.fanexGrid(gridOptions);
    };

    var renderDataDaily = function (data) {
        var bodyData = [];
        if (data && data.length > 0) {
            var dataLength = data.length;
            var mergeRowNumber = 0;
            var oldValue = "";
            var newValue = "";
            var firstMergeRow = -1;
            for (var i = 0; i < dataLength; i++) {
                var item = data[i];
                var currentRow = {};
                
                newValue = item.Daily;
                if (oldValue != newValue){
                    if (mergeRowNumber > 1) {
                        bodyData[firstMergeRow].Columns[0].MergeRowNumber = mergeRowNumber;
                    }

                    firstMergeRow = i;
                    mergeRowNumber = 1;
                    
                    currentRow = {
                        Columns: [
                                    { Name: "Daily", Value: item.Daily },
                                    { Name: "Level", Value: item.Level },
                                    { Name: "UserName", Value: item.UserName },
                                    { Name: "CurrencyName", Value: item.CurrencyName },
                                    { Name: "Turnover", Value: item.Turnover },
                                    { Name: "MemberWinloss", Value: item.MemberWinloss },
                                    { Name: "MemberComm", Value: item.MemberComm },
                                    { Name: "MemberTotal", Value: item.MemberTotal },
                                    { Name: "OwnerTurnover", Value: item.OwnerTurnover },
                                    { Name: "OwnerWinloss", Value: item.OwnerWinloss },
                                    { Name: "OwnerComm", Value: item.OwnerComm },
                                    { Name: "OwnerTotal", Value: item.OwnerTotal },
                                    { Name: "Company", Value: item.Company },
                                    { Name: "BetCount", Value: item.BetCount }
                                ]
                    };

                    bodyData.push(currentRow);
                    oldValue = newValue;
                } else {
                    // New same row
                    mergeRowNumber++;
                    currentRow = {
                        Columns: [
                                    { Name: "Level", Value: item.Level },
                                    { Name: "UserName", Value: item.UserName },
                                    { Name: "CurrencyName", Value: item.CurrencyName },
                                    { Name: "Turnover", Value: item.Turnover },
                                    { Name: "MemberWinloss", Value: item.MemberWinloss },
                                    { Name: "MemberComm", Value: item.MemberComm },
                                    { Name: "MemberTotal", Value: item.MemberTotal },
                                    { Name: "OwnerTurnover", Value: item.OwnerTurnover },
                                    { Name: "OwnerWinloss", Value: item.OwnerWinloss },
                                    { Name: "OwnerComm", Value: item.OwnerComm },
                                    { Name: "OwnerTotal", Value: item.OwnerTotal },
                                    { Name: "Company", Value: item.Company },
                                    { Name: "BetCount", Value: item.BetCount }
                                ]
                    };

                    bodyData.push(currentRow);
                }
            }

            if (mergeRowNumber > 1) {
                bodyData[firstMergeRow].Columns[0].MergeRowNumber = mergeRowNumber;
            }
        }

        var cols = [
                    { HeaderText: resources.Daily, Width: 70, Name: "Daily", HeaderAlign: "center", CellAlign: "left", FormatDateTime: "MM/DD/YYYY" },
                    { HeaderText: resources.Level, Width: 40, Name: "Level", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: resources.UserName, Width: 100, Name: "UserName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: resources.Currency, Width: 50, Name: "CurrencyName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: resources.Turnover, Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", TdClass: "chk-bold" },
                    { HeaderText: resources.WinLoss, Width: 100, Name: "MemberWinloss", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.Comm, Width: 70, Name: "MemberComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.Total, Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "b" },
                    { HeaderText: resources.Turnover, Width: 100, Name: "OwnerTurnover", HeaderAlign: "center", TdClass: "altercol", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.WinLoss, Width: 100, Name: "OwnerWinloss", HeaderAlign: "center", TdClass: "altercol", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.Comm, Width: 70, Name: "OwnerComm", HeaderAlign: "center", TdClass: "altercol", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.Total, Width: 100, Name: "OwnerTotal", HeaderAlign: "center", TdClass: "altercol", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "b" },
                    { HeaderText: resources.Company, Width: 100, Name: "Company", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", TdClass: "chk-bold" },
                    { HeaderText: resources.BetCount, Width: 100, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0", TdClass: "chk-bold"}];

        gridOptions = {
            customizeHeader: true,
            groupHeaderRows: [
                {
                    Columns: [
                                { HeaderText: resources.Daily, Name: "Daily", MergeRowNumber: 2 },
                                { HeaderText: resources.Level, Name: "Level", MergeRowNumber: 2 },
                                { HeaderText: resources.UserName, Name: "UserName", MergeRowNumber: 2 },
                                { HeaderText: resources.Currency, Name: "CurrencyName", MergeRowNumber: 2 },
                                { HeaderText: resources.Turnover, Name: "Turnover", MergeRowNumber: 2 },
                                { HeaderText: resources.Member, Name: "MemberWinloss", MergeColNumber: 3 },
                                { HeaderText: resources.Owner, Name: "OwnerTurnover", MergeColNumber: 4 },
                                { HeaderText: resources.Company, Name: "Company", MergeRowNumber: 2 },
                                { HeaderText: resources.BetCount, Name: "BetCount", MergeRowNumber: 2 }
                            ]
                }
            ],
            showTitle: true,
            titleInfo:
            {
                text: resources.DailyConsolidatedReport + " " + jQuery('#fdate').val() + "-->" + jQuery('#tdate').val() + " - " + (jQuery('#ddlCurrency').val() == "0" || jQuery('#ddlCurrency').val() == undefined ? resources.AllCurrencies : resources.ViewUnder + " " + jQuery('#ddlCurrency').find('option:checked').text()),
                showExcelButton: false,
                showBackButton: false,
                showSendMailButton: false
                //customButtons: '<span>aaaa</span>'
            },
            columns: cols,
            bodyRows: bodyData,
            numberOfFrozenColumn: 3,
            gridExpandHeight: 200,
            noDataText: resources.GridNoDataText,
            showDistinctRow: false
        };

        _table.grdContainer.fanexGrid(gridOptions);
    };

    var renderDataSport = function (data) {
        var bodyData = [];
        if (data && data.length > 0) {
            var dataLength = data.length;
            var mergeRowNumber = 0;
            var oldValue = "";
            var newValue = "";
            var firstMergeRow = -1;
            for (var i = 0; i < dataLength; i++) {
                var item = data[i];
                var currentRow = {};
                newValue = item.SportName;
                if (oldValue != newValue){
                    if (mergeRowNumber > 1) {
                        bodyData[firstMergeRow].Columns[0].MergeRowNumber = mergeRowNumber;
                    }

                    firstMergeRow = i;
                    mergeRowNumber = 1;
                    
                    currentRow = {
                        Columns: [
                                    { Name: "SportName", Value: item.SportName},
                                    { Name: "Level", Value: item.Level },
                                    { Name: "UserName", Value: item.UserName },
                                    { Name: "CurrencyName", Value: item.CurrencyName },
                                    { Name: "Turnover", Value: item.Turnover },
                                    { Name: "MemberWinloss", Value: item.MemberWinloss },
                                    { Name: "MemberComm", Value: item.MemberComm },
                                    { Name: "MemberTotal", Value: item.MemberTotal },
                                    { Name: "OwnerTurnover", Value: item.OwnerTurnover },
                                    { Name: "OwnerWinloss", Value: item.OwnerWinloss },
                                    { Name: "OwnerComm", Value: item.OwnerComm },
                                    { Name: "OwnerTotal", Value: item.OwnerTotal },
                                    { Name: "Company", Value: item.Company },
                                    { Name: "BetCount", Value: item.BetCount }
                                ]
                    };

                    bodyData.push(currentRow);
                    oldValue = newValue;
                } else {
                    // New same row
                    mergeRowNumber++;
                    currentRow = {
                        Columns: [
                                    { Name: "Level", Value: item.Level },
                                    { Name: "UserName", Value: item.UserName },
                                    { Name: "CurrencyName", Value: item.CurrencyName },
                                    { Name: "Turnover", Value: item.Turnover },
                                    { Name: "MemberWinloss", Value: item.MemberWinloss },
                                    { Name: "MemberComm", Value: item.MemberComm },
                                    { Name: "MemberTotal", Value: item.MemberTotal },
                                    { Name: "OwnerTurnover", Value: item.OwnerTurnover },
                                    { Name: "OwnerWinloss", Value: item.OwnerWinloss },
                                    { Name: "OwnerComm", Value: item.OwnerComm },
                                    { Name: "OwnerTotal", Value: item.OwnerTotal },
                                    { Name: "Company", Value: item.Company },
                                    { Name: "BetCount", Value: item.BetCount }
                                ]
                    };

                    bodyData.push(currentRow);
                }
            }

            if (mergeRowNumber > 1) {
                bodyData[firstMergeRow].Columns[0].MergeRowNumber = mergeRowNumber;
            }
        
        }

        var cols = [
                    { HeaderText: resources.Sport, Width: 80, Name: "SportName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: resources.Level, Width: 40, Name: "Level", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: resources.UserName, Width: 100, Name: "UserName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: resources.Currency, Width: 50, Name: "CurrencyName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: resources.Turnover, Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", TdClass: "chk-bold" },
                    { HeaderText: resources.WinLoss, Width: 100, Name: "MemberWinloss", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.Comm, Width: 70, Name: "MemberComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.Total, Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "b" },
                    { HeaderText: resources.Turnover, Width: 100, Name: "OwnerTurnover", HeaderAlign: "center", TdClass: "altercol", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.WinLoss, Width: 100, Name: "OwnerWinloss", HeaderAlign: "center", TdClass: "altercol", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.Comm, Width: 70, Name: "OwnerComm", HeaderAlign: "center", TdClass: "altercol", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "chk-bold" },
                    { HeaderText: resources.Total, Width: 100, Name: "OwnerTotal", HeaderAlign: "center", TdClass: "altercol", CellAlign: "right", FormatNumber: "0,0.00", ThClass: "RptHeader02", TdClass: "b" },
                    { HeaderText: resources.Company, Width: 100, Name: "Company", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", TdClass: "chk-bold" },
                    { HeaderText: resources.BetCount, Width: 100, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0", TdClass: "chk-bold"}];

        gridOptions = {
            customizeHeader: true,
            groupHeaderRows: [
                {
                    Columns: [
                                { HeaderText: resources.Sport, Name: "SportName", MergeRowNumber: 2 },
                                { HeaderText: resources.Level, Name: "Level", MergeRowNumber: 2 },
                                { HeaderText: resources.UserName, Name: "UserName", MergeRowNumber: 2 },
                                { HeaderText: resources.Currency, Name: "CurrencyName", MergeRowNumber: 2 },
                                { HeaderText: resources.Turnover, Name: "Turnover", MergeRowNumber: 2 },
                                { HeaderText: resources.Member, Name: "MemberWinloss", MergeColNumber: 3 },
                                { HeaderText: resources.Owner, Name: "OwnerTurnover", MergeColNumber: 4 },
                                { HeaderText: resources.Company, Name: "Company", MergeRowNumber: 2 },
                                { HeaderText: resources.BetCount, Name: "BetCount", MergeRowNumber: 2 }
                            ]
                }
            ],
            showTitle: true,
            titleInfo:
            {
                text: resources.SportConsolidatedReport + " " + jQuery('#fdate').val() + "-->" + jQuery('#tdate').val() + " - " + (jQuery('#ddlCurrency').val() == "0" || jQuery('#ddlCurrency').val() == undefined ? resources.AllCurrencies : resources.ViewUnder + " " + jQuery('#ddlCurrency').find('option:checked').text()),
                showExcelButton: false,
                showBackButton: false,
                showSendMailButton: false,
                //customButtons: '<span>aaaa</span>'
            },
            columns: cols,
            bodyRows: bodyData,
            numberOfFrozenColumn: 3,
            gridExpandHeight: 200,
            noDataText: resources.GridNoDataText,
            showDistinctRow: false
        };

        _table.grdContainer.fanexGrid(gridOptions);
    };

    var CheckAll = function (ele) {
        var isCheck = jQuery(ele).attr('checked') == "checked";
        _table.lstCheckbox.each(function () {
            jQuery(this).attr('checked', isCheck);
        });
    };

    var IsCheck = function (ele) {
        var isCheck = jQuery(ele).attr('checked') == "checked";
        if (isCheck) {
            var isCheckAll = true;
            _table.lstCheckbox.each(function () {
                if (jQuery(this).attr('checked') != "checked") {
                    isCheckAll = false;
                    return false;
                }
            });

            _table.chkAll.attr('checked', isCheckAll);
        } else {
            _table.chkAll.attr('checked', false);
        }
    };
}