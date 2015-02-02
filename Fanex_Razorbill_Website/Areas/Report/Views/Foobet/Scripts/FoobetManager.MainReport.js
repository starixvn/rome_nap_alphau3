var FoobetManager = FoobetManager || {};
FoobetManager.MainReport = {};

(function () {
    this.Submit = function () {
        var fromDate = jQuery('#fdate').val();
        var toDate = jQuery('#tdate').val();
        FoobetManager.GenerateData(fromDate, toDate);
    }

    this.ExportExcel = function () {
        var url = domain + 'Report/Foobet/ExportExcelFoobet?fromDate=' + jQuery('#fdate').val() + '&toDate=' + jQuery('#tdate').val();
        jQuery('#frm').attr('src', url);
    }
}).apply(FoobetManager.MainReport);
