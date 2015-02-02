// Edit HorseRacing's  PositionTaking
function CustomerEditHR(custid, actionOn, isMult) {
    var url = "";
    var popH = 750, popW = 1040;
    switch (_page.roleid) {
        case 4: // Super
            var roleId;
            if (actionOn == 'ViewAgent') {
                roleId = 2;
                popW = 780;
            }
            else if (actionOn == "Member") {
                roleId = 1;
                popW = 870;
            }
            else {
                roleId = 3;
            }
            if (isMult == 0) {
                url = "../../RaceBook/EditSingle.aspx?custid=" + custid + "&roleid=" + roleId;
                $("arrayCustID").value = custid;
                $("arrayUserName").value = "";
            }
            else {
                custid = GetCustID("chkid");
                url = "../../RaceBook/EditMultiple.aspx?custid=" + custid + "&roleid=" + roleId;
            }
            break;
        case 3: // Master
            url += "../../RaceBook/Master/";
            var roleId;
            if (actionOn == "Member") {
                roleId = 1;
                popW = 810;
            }
            else {
                roleId = 2;
            }
            if (isMult == 0) {
                url = "../../RaceBook/EditSingle.aspx?custid=" + custid + "&roleid=" + roleId;
                $("arrayCustID").value = custid;
                $("arrayUserName").value = "";
            }
            else {
                custid = GetCustID("chkid");
                url = "../../RaceBook/EditMultiple.aspx?custid=" + custid + "&roleid=" + roleId;
            }
            break;
        case 2: // Agent
            url += "../../RaceBook/Agent/";
            var roleId = 1;
            if (isMult == 0) {
                url = "../../RaceBook/EditSingle.aspx?custid=" + custid + "&roleid=" + roleId;
                $("arrayCustID").value = custid;
                $("arrayUserName").value = "";
            }
            else {
                custid = GetCustID("chkid");
                url = "../../RaceBook/EditMultiple.aspx?custid=" + custid + "&roleid=" + roleId;
            }
            break;
    }
    ageWnd.open(url, '', 0, 10, popW, popH);
}

function OpenEditMinMaxHR(custid, username) {
    // var url = "../MemberInfo/EditMinHR.aspx?custid=" + custid + "&username=" + username;
    var url = '';
    if (_page.roleid == 4) {
        url = '../../RaceBook/Super/EditMinPT.aspx';
    }

    else {
        url = '../../RaceBook/Master/EditMinPT.aspx';
    }

    url += '?custid=' + custid;
    $("arrayCustID").value = custid;
    var popW = 800, popH = 900;

    //url, title, left, top, width, height
    ageWnd.open(url, '', 0, 10, popW, popH);
}