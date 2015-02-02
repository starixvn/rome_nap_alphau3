// Overwrite DelayReloadPage in old Core.js
function DelayReloadPage(Url) {
    age.DelayReloadPage(Url);
}

//Start: View Downline
function ViewSB(type, custId, userName) {
    window.location.href = 'SportbookPositionTakingList.aspx?type=' + type + '&custid=' + custId + '&USER=' + userName;
}

function ViewRacing(type, custId, userName) {
    window.location.href = 'RacingPositionTakingList.aspx?type=' + type + '&custid=' + custId + '&USER=' + userName;
}

function ViewNG(type, custId, userName) {
    window.location.href = 'NumberGamePositionTakingList.aspx?type=' + type + '&custid=' + custId + '&USER=' + userName;
}

function ViewCS(type, custid, user) {
    document.location.href = 'CasinoPositionTakingList.aspx?type=' + type + '&custid=' + custid + '&USER=' + user;
}

function ViewBG(type, custid, user) {
    document.location.href = 'BingoPositionTakingList.aspx?type=' + type + '&custid=' + custid + '&USER=' + user;
}

function ViewLC(type, custid, user) {
    document.location.href = 'LiveCasinoPositionTakingList.aspx?type=' + type + '&custid=' + custid + '&USER=' + user;
}

function ViewVS(type, custid, user) {
    document.location.href = 'VirtualSportsPositionTakingList.aspx?type=' + type + '&custid=' + custid + '&USER=' + user;
}
//End: View Downline

//For Sportbook & Number Game
function EditMember_Single(cid, user, agentid, roleId) {
    var URL = age.GetBaseUrl() + "_MemberInfo/PositionTaking/Agent/EditMember.aspx?"; //V1
    var popH = 950;
    var popW = 890;
    if (roleId == '4') {
        URL = age.GetBaseUrl() + "_MemberInfo/PositionTaking/Super/EditMember.aspx?";
    }

    if (roleId == '3') {
        URL = age.GetBaseUrl() + "_MemberInfo/PositionTaking/Master/EditMember.aspx?";
    }

    URL += "m=0";
    URL += "&custid=" + cid;
    URL += "&USER=" + user;
    URL += '&agentid=' + agentid;

    if ($("arrayCustID")) {
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
    }

    //url, title, left, top, width, height
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function EditMemberRacing(cid, user, agentid, roleId) {
    var URL = age.GetBaseUrl() + "_MemberInfo/RaceBook/EditSingle.aspx?custid=" + cid + "&roleid=" + 1;
    var popH = 950, popW = 1040;

    if ($("arrayCustID")) {
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
    }

    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function EditCS(cid) {
    var URL = age.GetBaseUrl() + "MemberInfo/PositionTakingCS.aspx?custid=" + cid + "&isupdate=false";
    var popH = 600, popW = 980;
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function EditBG(cid) {
    var URL = age.GetBaseUrl() + "MemberInfo/PositionTakingBingo.aspx?custid=" + cid + "&isupdate=false";
    var popH = 600, popW = 980;
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function EditMemberLiveCasino(custids, username, roleId) {
    var URL = "../../../LiveCasinoSetting/Setting/EditSetting?uplineId=" + _page.loginId + "&parentId=" + _page.loginId + "&uplineLevel="
            + _page.RoleId + "&actorId=" + _page.loginId + "&actorUsername=" + _page.loginUsername + "&customerId=" + custids + "&customerLevel=1&subAccId=" + _page.subAccId 
            + "&language=" + _page.langKey + "&subAccUsername=" + _page.subUserName;
    ageWnd.open(URL, '', 0, 10, 880, 580);
}

function EditMemberVS(custids, username, roleId) {
    var URL = age.GetBaseUrl() + "_MemberInfo/VirtualSports/EditSingle.aspx?";
    URL += "custid=" + custids;
    URL += "&username=" + username;
    URL += "&roleid=1&isMult=0";

    var popH = 500, popW = 880;
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

RegisterStartUp(function () {
    if (_page.error == true) {
        ageMsg.Show(_page.messageError);
    }
});