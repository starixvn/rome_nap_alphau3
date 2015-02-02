//Edit Live Casino's Positontaking
function EditLiveCS(custid, directUplineId, uplineId, subAccId ) {
    var URL = "../../../LiveCasinoSetting/Setting/EditSetting?uplineId=" + uplineId + "&parentId=" + directUplineId + "&uplineLevel=" + _page.roleid + "&actorId=" + uplineId + "&actorUsername=" + _page.loginUsername + "&customerId=" + custid + "&customerLevel=" + _page.roleidLiveCasino + "&subAccId=" + subAccId + "&language=" + _page.langKey + "&subAccUsername=" + _page.subUserName;
    ageWnd.open(URL, '', 0, 10, 880, 580);
}

function EditMultiLiveCasino(directUplineId, uplineId, subAccId) {
    var SelStatus = "";
    var SelCID = "";
    var SelUser = "";
    var arCID = document.getElementsByName("chkid");

    if (!arCID) return;

    var lengthOfArray = arCID.length;
    for (var i = 0; i < lengthOfArray; i++) {
        if (arCID[i].checked) {
            var linkChar = i == lengthOfArray - 1 ? "" : ",";
            SelCID += arCID[i].id.split('_')[1] + linkChar;
            SelUser += arCID[i].value.substring(arCID[i].value.indexOf(";") + 1) + linkChar;
            SelStatus += arCID[i].getAttribute("statusLiveCasino") + linkChar;
        }
    }

    document.getElementById('parentId').value = uplineId;
    document.getElementById('uplineId').value = uplineId;
    document.getElementById('customerIds').value = SelCID;
    document.getElementById('syncStatuses').value = SelStatus;
    document.getElementById('usernames').value = SelUser;
    document.getElementById('uplineLevel').value = _page.roleid;
    document.getElementById('customerLevel').value = _page.roleidLiveCasino;
    document.getElementById('actorId').value = uplineId;
    document.getElementById('actorIdUsername').value = _page.loginUsername;
    document.getElementById('subAccId').value = subAccId;
    document.getElementById('subAccUsername').value = _page.subUserName;
    document.getElementById('language').value = _page.langKey;

    ageWnd.open('', '', 0, 10, 880, 580);

    document.getElementById("target").action = "../../../LiveCasinoSetting/Setting/EditMultipleSetting";
    document.getElementById("target").submit();
}

window.actionAfterUpdatedLiveCasinoSettingMultiple = function (isUpdatePartial) {
        window.top.frames["main"].location.reload();
};

    window.actionAfterUpdatedLiveCasinoSettingSingle = function (isUpdatePartial) {
        window.top.frames["main"].location.reload();
};
