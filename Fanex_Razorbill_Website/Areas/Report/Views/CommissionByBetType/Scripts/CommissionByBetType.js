/* global _roleID, _custID, domain, resources, _userTypeMaster, _userTypeAgent */
/*jshint freeze: false */

// "use strict";
function $(id) {
    return document.getElementById(id);
}

String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] !== 'undefined' ? args[number] : match;
    });
};

var CommissionByBettypeReportManager = function () {
    var self = this;
    var _table = {
        btnSubmit: null,
        btnExcel: null,
        btnBold: null,
        domExcel: null,

        chkAll: null,
        lstCheckbox: null,
        rowNoInfo: null,
        frmMain: null,
        grdContainer: null,
        errCode: null,
        errMsg: null,
        dToday: null,
        dYesterday: null
    };

    var backStack = [];
    var currentStackParam = {
        RoleId: null,
        CustId: null
    };

    var backStackPush = function (roleId, custId) {
        var stackParam = {
            RoleId: roleId,
            CustId: custId
        };
        backStack.push(stackParam);
    };

    var cachedDOMElements = function () {
        _table.btnSubmit = jQuery('#dSubmit');
        _table.dToday = jQuery('#dToday');
        _table.dYesterday = jQuery('#dYesterday');
        _table.btnExcel = jQuery('#exporttoexcel');
        _table.btnBold = jQuery('#chk_rpt_bold');
        _table.frmMain = jQuery('#frmmain');
        _table.domExcel = '<input type="checkbox" id="chk_rpt_bold" name="BoldExcel" value="true" />'+
                        '<label for="chk_rpt_bold" style="color: #999; text-transform: none; font-weight: bold; line-height: 18px;">Bold</label>&nbsp;'+
                        '<img alt="Export to Excel" src="' +
                         jQuery('#RootURL').val() +
                         '_GlobalResources/Images/excel.gif" id="exporttoexcel" name="exporttoexcel" title="Export to Excel" class="hand" />';

        _table.grdContainer = jQuery('#GridContainer');
        _table.errCode = jQuery('#ErrCode');
        _table.errMsg = jQuery('#ErrMsg');
        _table.rowNoInfo = jQuery('<tr class="no_info"><td colspan="15">No information is available</td></tr>');
    };

    var setDateTimeRange = function(from, to) {
        $("fdate").value = from;
        $("tdate").value = to;
    };
    var bindingEvent = function () {
        _table.btnSubmit.click(function () {
            self.GenerateData(currentStackParam.RoleId, currentStackParam.CustId);
        });

        _table.dToday.click(function () {
            var value = _table.dToday.val();

            setDateTimeRange(value, value);

            self.GenerateData(_roleID, _custID);
        });

        _table.dYesterday.click(function () {
            var value = _table.dYesterday.val();

            setDateTimeRange(value, value);

            self.GenerateData(_roleID, _custID);
        });

        _table.btnExcel.click(function () {
            var excelParam = {
                custId:  currentStackParam.CustId,
                FromDate: jQuery("#fdate").val(),
                ToDate: jQuery("#tdate").val(),
                roleId:  currentStackParam.RoleId,
                isBold: _table.btnBold.prop('checked')
            };
            downloadFile(domain + 'CommissionByBettype/ExportExcel', excelParam);
        });

        _table.btnBold.click(function () {
            return boldAll(this.checked);
        });
        var downloadFile = function (url, parameters) {
            if (url && parameters) {
                var downloadUrl = url + '?';
                //var iframe = jQuery('#frm');
                var iframe = document.getElementById("frm");

                parameters = typeof parameters === 'string' ? parameters : jQuery.param(parameters);
                downloadUrl += parameters;

                //iframe.attr('src', downloadUrl);
                iframe.src = downloadUrl;
            }
        };
        _table.grdContainer.on('click', 'span.downline_link', function () {
            var $obj = jQuery(this);
            var roleId = $obj.attr('data-role-id');
            var custId = $obj.attr('data-cust-id');

            backStackPush(currentStackParam.RoleId, currentStackParam.CustId);
            currentStackParam.RoleId = roleId;
            currentStackParam.CustId = custId;

            self.GenerateData(roleId, custId);
        });
    };

    self.initPage = function (roleId, custId) {
        cachedDOMElements();
        bindingEvent();

        currentStackParam.RoleId = roleId;
        currentStackParam.CustId = custId;
    };

    var boldAll = function (isBold) {
        jQuery('.boldable').each(function () {
            var $this = jQuery(this);
            if (isBold) {
                $this.addClass('bold-td');
            } else {
                $this.removeClass('bold-td');
            }
        });
    };
    var disableButton = function() {
        var dSubmit = $('dSubmit');
        var dToday = $('dToday');
        var dYesterday = $('dYesterday');
    
        if (dSubmit) {
            dSubmit.disabled = true;
        }
        if (dToday) {
            dToday.disabled = true;
        }

        if (dYesterday) {
            dYesterday.disabled = true;
        }

        var loading = $("loading");
        if (loading) {
            loading.className = "loading";
        }
    };

    var enableButton = function() {
        var dSubmit = $('dSubmit');
        var dToday = $('dToday');
        var dYesterday = $('dYesterday');

        if (dSubmit) {
            dSubmit.disabled = false;
        }

        if (dToday) {
            dToday.disabled = false;
        }

        if (dYesterday) {
            dYesterday.disabled = false;
        }

        var loading = $("loading");
        if (loading) {
            loading.className = "";
        }
    };

    self.GenerateData = function (roleId, custId) {
        var getParam = {
            CustId:  custId,
            FromDate: jQuery("#fdate").val(),
            ToDate: jQuery("#tdate").val(),
            RoleId:  roleId
        };

        var params = jQuery.param(getParam);

        disableButton();
        jQuery.ajax({
            type: "POST",
            url: domain + "Report/CommissionByBettype/GetCommissionByBettype",
            dataType: "json",
            data: params,
            responseType: "json",
            success: function (rs) {
                renderGrid(rs.data, custId, rs.roleId);
                enableButton();
            },
            error: function (jqXhr) {
                if (jqXhr.status && jqXhr.status === 500) {
                    var r = jQuery.parseJSON(jqXhr.responseText);
                    alert(r.Message);
                }
            }
        });
    };

    var customSort = function(col1Value, col2Value, sortDirection) {
        var parseValCol1 = parseFloat(col1Value);
        var parseValCol2 = parseFloat(col2Value);
        if (sortDirection.toUpperCase() === 'ASC') {
            return parseValCol1 > parseValCol2 ? 1 : -1;
        }
        else {
            return parseValCol1 < parseValCol2 ? 1 : -1;
        }
    }
    var generateDataBack = function () {
        if ( backStack.length === 0) {
            return;
        }
        var stackVar = backStack.pop();
        currentStackParam.RoleId = stackVar.RoleId;
        currentStackParam.CustId = stackVar.CustId;

        self.GenerateData(stackVar.RoleId, stackVar.CustId);
    };
    var renderGrid = function (data, custId, roleId) {
        /*jshint camelcase: false */
        var titleString = "";
        var isShowBack = true;
        if (roleId === _userTypeMaster) {
            titleString = resources.MasterComm;
        }
        else if (roleId === _userTypeAgent) {
            titleString = resources.AgentComm;
        }
        else {
            titleString = resources.SuperComm;
        }

        if (roleId === _roleID)
        {
            isShowBack = false;
        }
        titleString += " " +jQuery("#fdate").val() + "-->" + jQuery("#tdate").val();

        var gridOptions =
        {
            columns: [
                { HeaderText: resources.Username, Width: 118, Name: "ColUserName", HeaderAlign: "center", CellAlign: "left", Sort: true, SortDirection: 'asc' },
                { HeaderText: resources.Commission_1, Width: 123, Name: "ColCommission1", HeaderAlign: "center", CellAlign: "right", Sort: true, SortType: 'number', CustomSortFunction: function(col1Value, col2Value, sortDirection){
                        return customSort(col1Value, col2Value, sortDirection);
                }},
                { HeaderText: resources.Commission_2, Width: 123, Name: "ColCommission2", HeaderAlign: "center", CellAlign: "right", Sort: true, SortType: 'number', CustomSortFunction: function(col1Value, col2Value, sortDirection){
                        return customSort(col1Value, col2Value, sortDirection);
                } },
                { HeaderText: resources.Commission_3, Width: 123, Name: "ColCommission3", HeaderAlign: "center", CellAlign: "right", Sort: true, SortType: 'number', CustomSortFunction: function(col1Value, col2Value, sortDirection){
                        return customSort(col1Value, col2Value, sortDirection);
                } },
                { HeaderText: resources.Gaming, Width: 123, Name: "ColGaming", HeaderAlign: "center", CellAlign: "right", Sort: true, SortType: 'number', CustomSortFunction: function(col1Value, col2Value, sortDirection){
                        return customSort(col1Value, col2Value, sortDirection);
                } },
            ],
            gridExpandHeight: 50,
            showFooter: true,
            showTitle: true,
            titleInfo: {
                text: titleString,
                showBackButton: isShowBack,
                backFunction: function () {
                    generateDataBack();
                },
            },
            noDataText: resources.NoInfoText
        };

        var nameColFormat = '<span class="downline_link link" data-role-id="{0}" data-cust-id="{1}">{2}</span>';

        var nameCol = "";
        var bodyData = [];
        var footerData = [];
        var row;
        var isBold = _table.btnBold.prop('checked');
        var tdFormat = "boldable";
        if (isBold)
        {
            tdFormat += " bold-td";
        }

        if (data.CommissionList && data.CommissionList.length > 0) {
            jQuery.each(data.CommissionList, function (index, item) {
                if (roleId === _userTypeAgent) {
                    nameCol = item.UserName;
                }
                else if (roleId === _userTypeMaster) {
                        nameCol = nameColFormat.format(_userTypeAgent, item.Recommend, item.UserName);
                    }
                else {
                    nameCol = nameColFormat.format(_userTypeMaster, item.Mrecommend, item.UserName);
                }

                row = {
                    Columns: [
                        { Name: "ColUserName", Value: nameCol, CellAlign: 'left' },
                        { Name: "ColCommission1", Value: parseFloat(item.TotalComm_1).toFixed(2), TdClass: tdFormat + " odd-column" },
                        { Name: "ColCommission2", Value: parseFloat(item.TotalComm_2).toFixed(2), TdClass: tdFormat },
                        { Name: "ColCommission3", Value: parseFloat(item.TotalComm_3).toFixed(2), TdClass: tdFormat + " odd-column" },
                        { Name: "ColGaming", Value: parseFloat(item.Casino_Comm).toFixed(2), TdClass: tdFormat }
                    ]
                };
                bodyData.push(row);
            });

            
            row = {
                Columns: [
                        { Name: "ColUserName", Value: resources.Total, CellAlign: 'left' },
                        { Name: "ColCommission1", Value: data.Total.TotalComm_1 },
                        { Name: "ColCommission2", Value: data.Total.TotalComm_2 },
                        { Name: "ColCommission3", Value: data.Total.TotalComm_3 },
                        { Name: "ColGaming", Value: data.Total.Casino_Comm }
                ]
            };
            footerData.push(row);
        }

        gridOptions.bodyRows = bodyData;
        gridOptions.footerRows = footerData;

        _table.grdContainer.fanexGrid(gridOptions);
    };
};

var commissionByBettypeReportManager;
jQuery.noConflict();
jQuery(document).ready(function () {
    jQuery(window).load(function(){
        commissionByBettypeReportManager = new CommissionByBettypeReportManager();
        commissionByBettypeReportManager.initPage(_roleID, _custID);
        commissionByBettypeReportManager.GenerateData(_roleID, _custID);
    });
});