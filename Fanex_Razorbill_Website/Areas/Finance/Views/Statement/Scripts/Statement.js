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

var statementManager;
jQuery.noConflict();
jQuery(document).ready(function () {
    jQuery('#dSubmit').click(function () {
        statementManager.GenerateData();
    });
    jQuery('#exporttoexcel').click(function () {
        statementManager.ExportExcel();
    });
    statementManager = new StatementManager();
    statementManager.GenerateData();
});


StatementManager = function () {
    var self = this;
    self.initPage = function () {
        ChangeDate($("ddlSelectDate"));
        cachedDOMElements();
        renderData([]);
    }

    self.ExportExcel = function () {
        var url = '';
        if (_roleID == 1) {
            url = domain + 'Finance/StatementMember/ExportExcelStatementMember?FromDate=' + jQuery('#fdate').val() + '&ToDate=' + jQuery('#tdate').val() + '&CustID=' + _custID + "&RoleID=" + _roleID;
        } else {
            url = domain + 'Finance/StatementSMA/ExportExcelStatementSMA?FromDate=' + jQuery('#fdate').val() + '&ToDate=' + jQuery('#tdate').val() + '&CustID=' + _custID + "&RoleID=" + _roleID ;
        }
        jQuery('#frm').attr('src', url);
    }

    self.GenerateData = function () {
        var url = '';
        if (_roleID == 1) {
            url = domain + 'Finance/StatementMember/GetStatementMember?Username=' + _username;
        } else {
            url = domain + 'Finance/StatementSMA/GetStatementSMA';
        }
        DisableButton();
        jQuery.ajax({
            type: "POST",
            url: url,
            data: 'FromDate=' + jQuery('#fdate').val() + '&ToDate=' + jQuery('#tdate').val() + '&CustID=' + _custID + "&RoleID=" + _roleID ,
            responseType: "json",
            success: function (rs) {
                if (_roleID == 1) {
                    renderMemberStatement(rs);
                } else {
                    renderData(rs);
                }
                jQuery("#filter").width(jQuery("#GridContainer").width() + 22);
                EnableButton();
            },
            error: function (jqXhr, event, ajaxSettings) {
                if (jqXhr.status && jqXhr.status == 500) {
                    var r = jQuery.parseJSON(jqXhr.responseText);
                    alert(r.Message);
                }
            }
        });
        EnableButton();
    };

    var renderData = function (data) {
        var bodyData = [];
        if (data && data.length > 0) {
            var dataLength = data.length;
            for (var i = 0; i < dataLength; i++) {
                var item = data[i];
                var currentRow = {
                    Columns: [{
                        Name: "No",
                        Value: item.No
                    }, {
                        Name: "Date",
                        Value: item.WinlossDate
                    }, {
                        Name: "Remark",
                        Value: item.TransType + '<br/> -' + item.TransDesc
                    }, {
                        Name: "Total",
                        Value: item.Total
                    }, {
                        Name: "AccummulatedTotal",
                        Value: (item.IsOpeningBalance == true ? item.AccumulatedTotalString : item.AccumulatedTotalNumber)
                    }]
                };
                bodyData.push(currentRow);
            }
        }

        var cols = [{
            HeaderText: "#",
            Width: 20,
            Name: "No",
            HeaderAlign: "center",
            CellAlign: "right"
        }, {
            HeaderText: resources.Date,
            Width: 150,
            Name: "Date",
            HeaderAlign: "center",
            CellAlign: "center",
            CellCssClass: "sizeTime"
        }, {
            HeaderText: resources.Remark,
            Width: 300,
            Name: "Remark",
            HeaderAlign: "center",
            CellAlign: "left",
            CellCssClass: "bold"
        }, {
            HeaderText: resources.Total,
            Width: 120,
            Name: "Total",
            HeaderAlign: "center",
            CellAlign: "right",
            FormatNumber: "0,0.00",
            FormatNegative: '$',
            CellCssClass: "bold"
        }, {
            HeaderText: resources.AccummulatedTotal,
            Width: 120,
            Name: "AccummulatedTotal",
            HeaderAlign: "center",
            CellAlign: "right",
            FormatNumber: "0,0.00",
            FormatNegative: '$',
            CellCssClass: "bold"
        }];

        gridOptions = {
            columns: cols,
            bodyRows: bodyData,
            headerColumnHeight: 24,
            bodyRowHeight: 34,
            numberOfFrozenColumn: 0,
            gridExpandHeight: 100,
            showPager: true,
            customPager: false,
            pagerOption: {
                itemsPerPage: 50,
                maxItemsPerPage: 500,
                itemsPerPageArray: [50, 100, 200, 500],
                currentPage: 0,
                startPerPage: 50,
                pageStep: 50
            }
        };

        jQuery('#GridContainer').fanexGrid(gridOptions);
    };

    var renderMemberStatement = function (data) {
        var bodyData = [];
        if (data && data.length > 0) {
            var dataLength = data.length;
            for (var i = 0; i < dataLength; i++) {
                var item = data[i];
                var currentRow = {
                    Columns: [{
                        Name: "No",
                        Value: item.No
                    }, {
                        Name: "Date",
                        Value: item
                    }, {
                        Name: "Remark",
                        Value: item.TransType + '<br/> -' + item.TransDesc
                    }, {
                        Name: "Turnover",
                        Value: item.TurnOver
                    }, {
                        Name: "Credit",
                        Value: item.Credit
                    }, {
                        Name: "Commission",
                        Value: item.Commission
                    }, {
                        Name: "Total",
                        Value: item.Total
                    }, {
                        Name: "AccummulatedTotal",
                        Value: (item.IsOpeningBalance == true ? item.AccumulatedTotalString : item.AccumulatedTotalNumber)
                    }]
                };
                bodyData.push(currentRow);
            }
        }
        //        href="../../_BetList/BetList.aspx?custid={custid}&fdate={fdate}&transtype={transtype}&type={type}&username={uname}
        var cols = [{
            HeaderText: "#",
            Width: 20,
            Name: "No",
            HeaderAlign: "center",
            CellAlign: "right"
        }, {
            HeaderText: resources.Date,
            Width: 120,
            Name: "Date",
            HeaderAlign: "center",
            CellAlign: "center",
            CellCssClass: "sizeTime",
            CustomCellRender: function (currentCell) {
                if (currentCell.Value.BelistLink == '') {
                    return '<div>' + currentCell.Value.WinlossDate + '</div>';
                }
                else {
                    return '<a href=' + domain + currentCell.Value.BelistLink + '>' + currentCell.Value.WinlossDate + '</a>';
                }
            }
        }, {
            HeaderText: resources.Remark,
            Width: 250,
            Name: "Remark",
            HeaderAlign: "center",
            CellAlign: "left",
            CellCssClass: "bold"
        }, {
            HeaderText: resources.Turnover,
            Width: 120,
            Name: "Turnover",
            HeaderAlign: "center",
            CellAlign: "right",
            FormatNumber: "0,0.00",
            FormatNegative: '$',
            CellCssClass: "bold"
        }, {
            HeaderText: resources.CreditDebit,
            Width: 80,
            Name: "Credit",
            HeaderAlign: "center",
            CellAlign: "right",
            FormatNumber: "0,0.00",
            FormatNegative: '$',
            CellCssClass: "bold"
        }, {
            HeaderText: resources.Commission,
            Width: 120,
            Name: "Commission",
            HeaderAlign: "center",
            CellAlign: "right",
            FormatNumber: "0,0.00",
            FormatNegative: '$',
            CellCssClass: "bold"
        }, {
            HeaderText: resources.Total,
            Width: 120,
            Name: "Total",
            HeaderAlign: "center",
            CellAlign: "right",
            FormatNumber: "0,0.00",
            FormatNegative: '$',
            CellCssClass: "bold"
        }, {
            HeaderText: resources.AccummulatedTotal,
            Width: 120,
            Name: "AccummulatedTotal",
            HeaderAlign: "center",
            CellAlign: "right",
            FormatNumber: "0,0.00",
            FormatNegative: '$',
            CellCssClass: "bold"
        }];

        gridOptions = {
            columns: cols,
            bodyRows: bodyData,
            headerColumnHeight: 24,
            bodyRowHeight: 34,
            numberOfFrozenColumn: 0,
            gridExpandHeight: 100,
            showPager: true,
            pagerOption: {
                itemsPerPage: 50,
                maxItemsPerPage: 500,
                itemsPerPageArray: [50, 100, 200, 500],
                currentPage: 0,
                startPerPage: 50,
                pageStep: 50
            }
        };

        jQuery('#GridContainer').fanexGrid(gridOptions);
    };
}


function OnchangeData(param) {
    var par = param.value.split('^');
    var now = new Date();
    if (par[0] == 0)
        return;
    if (par[0] >= 1 && par[0] < 5) {
        if ($('tdate'))
            $('tdate').value = (now.getMonth() + 1) + "/" + now.getDate() + "/" + now.getFullYear();
        if ($('fdate'))
            $('fdate').value = (now.getMonth() + 1) + "/" + (now.getDate() - 1) + "/" + now.getFullYear();
    }
    if (par[0] >= 5) {
        if ($('tdate'))
            $('tdate').value = (now.getMonth() + 1) + "/" + now.getDate() + "/" + now.getFullYear();
        if ($('fdate'))
            $('fdate').value = (now.getMonth() + 1) + "/" + (now.getDate() - 8) + "/" + now.getFullYear();
    }

    var url = "#";
    if (par[0] == 1) {
        //       url='DailyStatement.aspx?username=' + par[3] +'&option=' + par[0] + '&custid=' + par[2] + '&fdate=' + $('fdate').value + '&tdate=' + $('tdate').value + '&page=1' + '&chkgetall=on' ;
        url = domain + "Finance/StatementMember?CustID=" + par[2] + "&RoleID=" + par[4] + "&username=" + par[3];
        url += urlFiltering();
    } else if (par[0] == 3) {
        url = domain + '_Reports/BetList/BetSummary.aspx?username=' + par[3] + '&option=' + par[0] + '&custid=' + par[2] + '&page=1' + '&chkgetall=on';
        url += urlFiltering();
        url += '&FilterPostback=postback';
    } else {
        url = domain + '_Reports/BetList/OutstandingDetail.aspx?username=' + par[3] + '&option=' + par[0] + '&custid=' + par[2] + '&fdate=' + $('fdate').value + '&tdate=' + $('tdate').value + '&page=1' + '&chkgetall=on';
        url += urlFiltering();
        url += '&FilterPostback=postback';
    }

    location.href = url;
}

function urlFiltering() {
    var url = '';
    url += '&chk_showsb=' + _chk_showsb + '&_chk_showsb=' + _chk_showcasino + '&chk_showrb=' + _chk_showrb + '&chk_showng=' + _chk_showng + '&chk_showbi=' + _chk_showbi;
    return url;
}