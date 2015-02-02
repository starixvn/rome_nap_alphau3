function EditMemberRacing(cid, user, agentid, Customer) {
    var URL = age.GetBaseUrl() + "_MemberInfo/RaceBook/EditSingle.aspx?custid=" + cid + "&roleid=" + 1;
    var popH = 950, popW = 1040;

    if ($("arrayCustID")) {
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
    }

    //url, title, left, top, width, height
    ageWnd.open(URL, '', 0, 10, popW, popH);
}