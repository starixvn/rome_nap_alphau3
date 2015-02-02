function ShowFrmUpdKenoStatus(custid, uplineid, roleid, subaccid,userid, username) {
    if (parent) {
        parent.age.OpenPopUp('CustomerSetting/keno/enablestatus/index?id=' + custid + '&uplineId=' + uplineid + '&roleId=' + roleid + '&subAccountId=' + subaccid + '&userid=' + userid + '&username=' + username);
    }
}