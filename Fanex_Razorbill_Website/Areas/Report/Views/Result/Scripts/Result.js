/* global _roleID, _custID, domain, resources, _userTypeMaster, _userTypeAgent */
/*jshint freeze: false */

// "use strict";

var ResultReportManager = function () {
    var self = this;
    var _table = {
        btnSubmit: null,
        dToday: null,
        dYesterday: null,
        btnExcel: null,
        frmMain: null,
        ddlLeague: null,
        ddlSport: null,
        isOutright: null,
        isChangeSportType: false,
        rootURL: null,
        btnOutRight: null,
        btnNormal: null,
        detailLink: null
    };

    var cachedDOMElements = function () {
        _table.btnSubmit = jQuery('#dSubmit');
        _table.dToday = jQuery('#dToday');
        _table.dYesterday = jQuery('#dYesterday');
        _table.btnExcel = jQuery('#exporttoexcel');
        _table.frmMain = jQuery('#frmmain');
        _table.ddlLeague = jQuery('#ddlLeague');
        _table.ddlSport = jQuery('#ddlSport');
        _table.isOutright = jQuery('#isOutright');
        _table.rootURL = jQuery('#rootURL');
        _table.btnOutRight = jQuery('#btnOutRight');
        _table.btnNormal = jQuery('#btnNormal');
        _table.detailLink = jQuery('.detail-link');
    };

    var setDateTimeRange = function (from, to) {
        jQuery("#fromDate").val(from);

        if (_table.isOutright.val().toLowerCase() == "true") {
            jQuery("#toDate").val(to);
        }
    };

    var bindingEvent = function () {
        _table.btnSubmit.click(function () {
            self.GenerateData(false);
        });

        _table.dToday.click(function () {
            var value = _table.dToday.val();
            setDateTimeRange(value, value);
            self.GenerateData(false);
        });

        _table.dYesterday.click(function () {
            var value = _table.dYesterday.val();
            setDateTimeRange(value, value);
            self.GenerateData(false);
        });

        _table.btnOutRight.click(function () {
            self.GenerateDataForOutright();
        });

        _table.btnNormal.click(function () {
            self.GenerateDataForNormal();
        });

        _table.btnExcel.click(function () {
            self.GenerateData(true);
            return false;
        });

        _table.detailLink.click(function () {
            self.ViewDetail($(this));
        });

        _table.ddlSport.change(function (event, ui) {
            _table.ddlLeague.selectedIndex = 0;
            _table.ddlLeague.val('0');
            _table.isChangeSportType = 'true';
            self.GenerateData(false);
        });

        _table.ddlLeague.change(function () {
            _table.isChangeSportType = 'true';
            self.GenerateData(false);
        });
    };

    self.initPage = function () {
        cachedDOMElements();
        bindingEvent();
        setDateTimeRange(fromDate, toDate);

        if (_table.isOutright.val().toLowerCase() == "false") {
            jQuery("#tdtdatetext").css({ "display": "none" });
            jQuery("#tdToDateCal").css({ "display": "none" });
        }

        showSoccer5Result = function (matchId, balls) {
            var url = 'Result/Soccer5Result';
            var sportid = _table.ddlSport.val();
            url += '?matchid=' + matchId + '&balls=' + balls + '&sportid=' + sportid;

            if (parent.ageWnd) {
                parent.ageWnd.open(url, '', 50, 30, 800, 600);
            }
            else {
                ageWnd.open(url, '', 10, 10, 800, 600);
            }
        };
    };

    self.GenerateDataForNormal = function () {
        location.href = "GetResult?IsOutRight=false";
    };

    self.GenerateDataForOutright = function () {
        location.href = "GetResult?IsOutRight=true";
    };

    self.GenerateData = function (isExportExcel) {
        var url = "GetResult?FromDate=" + jQuery("#fromDate").val() + (_table.isOutright.val().toLowerCase() == "true" ? "&toDate=" + jQuery("#toDate").val() : "")
        + "&SportId=" + _table.ddlSport.val() + "&LeagueId=" + _table.ddlLeague.val() + "&IsOutRight=" + _table.isOutright.val()
        + "&IsChangeSportType=" + _table.isChangeSportType;
        location.href = isExportExcel ? url + "&IsExportExcel=true" : url;
    };

    self.ViewDetail = function (me) {
        var sportid = _table.ddlSport.val();
        var url = 'Report/Result/GetResult';
        url += '?matchid=' + me.attr("data-match-id") + '&leagueid=' + me.attr("data-league-id") + '&sportid=' + sportid;
        if (sportid == 1 || sportid == 5) { // Soccer + Tennis
            url += '&viewDetail=true';
        }

        if (parent.ageWnd) {
            parent.ageWnd.open(url, '', 50, 30, 800, 600);
        }
        else {
            ageWnd.open(url, '', 10, 10, 800, 600);
        }
    };

    
};

var resultReportManager;
var showSoccer5Result;
jQuery(document).ready(function () {
    resultReportManager = new ResultReportManager();
    resultReportManager.initPage();
});