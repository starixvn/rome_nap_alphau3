// Edit Keno's  PositionTaking
function CustomerEditKeno(custid, actionOn, isMultipleEdit, username, userId, subAccId) {
    var url = "../../../customersetting/kenosetting/kenosetting/";
    var popH = 750, popW = 1040;

    if (isMultipleEdit == 0) {
        $("arrayCustID").value = custid;
        $("arrayUserName").value = "";
    }
    else {
        custid = GetCustID("chkid");
    }

    if ($("arrayCustID").value.indexOf('^') == -1) {
        isMultipleEdit = 0;
    }

    switch (_page.roleid) {
        case 4: // Super
            if (actionOn == "Master") {
                url += "EditMaster?custid=" + custid + "&username=" + username + "&isMultipleEdit=" + isMultipleEdit + "&userId=" + userId + "&subAccid=" + subAccId;
            }
            else if (actionOn == "Member") {
                url += "SuperEditMember?custid=" + custid + "&username=" + username + "&isMultipleEdit=" + isMultipleEdit + "&userId=" + userId + "&subAccid=" + subAccId;
            }
            else {
                return;
            }

            break;
        case 3: // Master
            if (actionOn == "Agent") {
                url += "EditAgent?custid=" + custid + "&username=" + username + "&isMultipleEdit=" + isMultipleEdit + "&userId=" + userId + "&subAccid=" + subAccId;
            }
            else if (actionOn == "Member") {
                url += "MasterEditMember?custid=" + custid + "&username=" + username + "&isMultipleEdit=" + isMultipleEdit + "&userId=" + userId + "&subAccid=" + subAccId;
            }
            else {
                return;
            }

            break;
        case 2: // Agent
            if (actionOn == "Member") {
                url += "AgentEditMember?custid=" + custid + "&username=" + username + "&isMultipleEdit=" + isMultipleEdit + "&userId=" + userId + "&subAccid=" + subAccId;
            }
            else {
                return;
            }

            break;
    }

    ageWnd.open(url, '', 0, 10, popW, popH);
}