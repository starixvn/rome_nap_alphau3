var FoobetManager = FoobetManager || {};
FoobetManager.HistoryReport = {};

(function () {
     this.Submit = function () {
        var fromDate = jQuery('#ddlMonth').monPickr("getFromDate", FormatConstant.MM_DD_YYYY_WITH_SLASH_STRING);
        var toDate = jQuery('#ddlMonth').monPickr("getToDate", FormatConstant.MM_DD_YYYY_WITH_SLASH_STRING);
        FoobetManager.GenerateData(fromDate, toDate);
    }

    this.ExportExcel = function () {
        var fromDate = jQuery('#ddlMonth').monPickr("getFromDate", FormatConstant.MM_DD_YYYY_WITH_SLASH_STRING);
        var toDate = jQuery('#ddlMonth').monPickr("getToDate", FormatConstant.MM_DD_YYYY_WITH_SLASH_STRING);
        var url = domain + 'Report/Foobet/ExportExcelFoobet?fromDate=' + fromDate + '&toDate=' + toDate;
        jQuery('#frm1').attr('src', url);
    }
}).apply(FoobetManager.HistoryReport);
