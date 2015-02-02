Fanex.PTRacing.CURRENT_CONTEXT = context;
Fanex.PTRacing.CURRENT_CONTEXT.ia3 = !customer.disableAutoMasterPt;
Fanex.PTRacing.CURRENT_CONTEXT.isAllowHRCurrency = !customer.isAllowHRCurrency;

var oldCustomer = customer.clone();
oldCustomer.firstName = null;
oldCustomer.lastName = null;
oldCustomer.phone = null;
oldCustomer.mobilePhone = null;
oldCustomer.fax = null;
var newCustomer = oldCustomer.clone();

// Customer Info
var custids;
var usernames;

function InitArrays() {
    if (custids == null || custids == undefined) {
        if (parent.$("arrayCustID") != null) {
            custids = parent.$("arrayCustID").value.split('^');
            custids.pop();
            usernames = parent.$("arrayUserName").value.split('^');
            usernames.pop();
        }
        else {
            custids = [context.targetCustId];
            usernames = [customer.userName];
        }

        if (custids.length == 0) {
            custids = [context.targetCustId];
            usernames = [customer.userName];
        }
    }
}

var CustomerModel = function (customer) {
    InitArrays();
    customer.userNames = usernames.join(", ");
    customer.targetLevel = function () {
        switch (context.targetRoleId) {
            case 4:
                return Language.SUPER;
            case 3:
                return Language.MASTER;
            case 2:
                return Language.AGENT;
            case 1:
                return Language.MEMBER;
            default:
                return "Unknown";
        }
    } ();

    this.saveCustomer = function () {
        newCustomer = oldCustomer.clone();

        if (ptGrid != null) {
            // get pt setting
            if (context.roleId - context.targetRoleId == 1) {
                if (!bettingLimit1.Validate()) {
                    return;
                }
            }

            var isupdateallmembers = ptGrid.Validate(newCustomer.settings);
            if (!isupdateallmembers) {
                updateSettings();
            }
        }
    }
}

var isSuccess = true;
function updateSettings() {
    isSuccess = true;
    age.ReloadPage(30000);
    newCustomer.settings = ptGrid.Save();
    updateCustSettings(0);
}

function IsMulti() {
    if (custids != null && custids != undefined && custids.length > 1)
        return true;

    return false;
}

function updateCustSettings(id) {
    var custid = custids[id];
    var username = usernames[id];
    var isMulti = false;
    if (id > 0) {
        isMulti = true;
    }
    var url = "Handlers/UpdateCustomerSetting.ashx?targetCustId=" + custid + "&targetRoleId=" + context.targetRoleId + "&isMulti=" + isMulti.toString();
    var parameters = ['oldCustomer=', JSON.stringify(oldCustomer), '&newCustomer=', JSON.stringify(newCustomer), '&otherSetting=', JSON.stringify(otherSetting)].join('');

    ajax.PostJSON(url, parameters, function onComplete(result) {
        age.UnLock();
        var ArrCodes = new Array();
        var ArrMsgs = new Array();
        var ArrUserName = new Array();
        ArrUserName.push(username);

        if (result instanceof Array) {
            var len = result.length;
            for (var i = 0; i < len; i++) {
                var item = result[i];
                ArrCodes.push(item.errCode);
                ArrMsgs.push(item.errMsg);
                if (item.errCode >= 0) {
                    isSuccess = false;
                }
                else if (item.errCode == -2) { // update status successfully
                    reloadCheckboxes();
                }
            }
        }
        else {
            ArrCodes.push(result.errCode);
            ArrMsgs.push(result.errMsg);
            if (result.errCode >= 0) {
                isSuccess = false;
            }
        }

        GenerateMessage(ArrCodes, ArrMsgs, ArrUserName, id != 0);

        id++;
        if (id < custids.length) {
            updateCustSettings(id);
        }
        else {
            if (isSuccess) {
                oldCustomer = newCustomer;
                if (parent.ageWnd) {
                    setTimeout('top.main.DelayReloadPage();', 1000);
                    //top.main.DelayReloadPage();
                }
                return;
            }
            age.UnLock();
            window.scroll(0, 0);
        }
        age.HideMaskDiv();
    });
}

var otherSetting = {
    'isupdateallmembers': false
};

function ConfirmYes() {
    setNewValueToMember(true);
}

function ConfirmNo() {
    setNewValueToMember(false);
}

function setNewValueToMember(isupdateallmembers) {
    otherSetting.isupdateallmembers = isupdateallmembers;
    if (context.targetRoleId == 3) {
        otherSetting.newspositiontaking = Popups.GetNewPt();
    }
    else if (context.targetRoleId == 2) {
        otherSetting.newapositiontaking = Popups.GetNewPt();
    }

    updateSettings();
    Popups.Close();
}

ko.applyBindings(new CustomerModel(customer));

// Betting Commission Status
if (customer.settings.sportTypes.length > 0) {
    var bettingLimit1 = new Fanex.PTRacing.BettingCommSetting(customer.settings.sportTypes);
    bettingLimit1.Render($('bettingLimitContainer'), context.targetRoleId);
}
else {
    $("bettingLimitContainer").className = "noInfoBlock";
    $("bettingLimitContainer").innerHTML = Language.NOINFO;
    $("btnSubmit").disabled = true;
}

// PT Setting
if (customer.settings.sportTypes.length > 0) {
    var ptGrid = new Fanex.PTRacing.PTGrid(customer.settings);
    ptGrid.Render($("ptGridContainer"));
}
else {
}

function InitUI() {
    if (context.formId == 4) {
        $("btnSubmit").parentNode.style.display = "none";
        $("btnMain").parentNode.style.display = "none";
    }
}

RegisterStartUp('InitUI()');