Fanex.PTEngine.CURRENT_CONTEXT = context;
Fanex.PTEngine.CURRENT_CONTEXT.ia3 = !customer.statusSetting.disableAutoMPT; // Allow auto PT master on member

var isChangeM2 = false;
var isShowPopUpSetPTMember = false;
var newValueMemberPT = "";
var oldPTSetting = customer.ptSetting.clone();

var ptGrid = null;
ptGrid = new Fanex.PTEngine.PTGrid(customer.ptSetting);
ptGrid.ShowCloneHint = function () {
    if (this.copiedItem == null) {
        return;
    }
    var copiedElements = [0, this.items[0].items[0].items[0].items[0].element,
                                        this.items[1].items[0].items[0].items[0].element]; // Handicap of Soccer and Basketball
    var hint = Language.COPY_ALL_HINT.Format('<b>' + this.activeCopiedTitle + '</b>');
    var otherBetTypesOf = Language.OTHER_BET_TYPE_OF.Format(this.items[this.copiedItem.sportId - 1].name);
    hint = hint + '<div class="ul-position"><ul><li>' + otherBetTypesOf + '</li>';
    // Find all copied items to generate tooltip
    var count = this.items.length;
    var top = 0;
    for (var i = this.copiedItem.sportId; i < count; i++) {
        if (this.items[i].copiedItem != null && this.items[i].copiedItem.sportId == this.copiedItem.sportId) {
            hint += '<li>' + this.items[i].name + '</li>';
            top += 18;
        }
    }
    hint = hint + '</ul></div>';
    top = top + 40;
    // Show tooltip
    var hintclass = 'hint-pointer';
    var xpos = 0;
    var pointerXPos = 10;
    var pointerYPos = top + 20;
    var ypos = -top - 20;
    show(copiedElements[this.copiedItem.sportId], hint, xpos, pointerXPos, ypos, pointerYPos, top, 30000);
};
function show(elementInput, message, xPos, pointerXPos, yPos, pointerYPos, height, timeout) {
    // <span class="hint"><span>This is the message.<span><span class="hint-pointer">&nbsp;</span></span>
    var hint = document.getElementById('fHint');
    var hintPointer = null;
    if (hint == null) {
        var hint = document.createElement("SPAN");
        hint.className = 'hint';
        var hintMsg = document.createElement("SPAN");
        hint.appendChild(hintMsg);
        hintPointer = document.createElement("SPAN");
        hint.appendChild(hintPointer);
        hint.id = 'fHint';
        hint.style.top = '10px';
        hint.style.left = '10px';
        hint.style.zIndex = 999999;
        hint.onclick = function () { // Hide onclick
            hint.style.display = 'none';
        };
        document.body.appendChild(hint);
    };
    hintPointer = hint.lastChild;
    if (typeof hintclass == 'undefined') {
        hintclass = 'hint-pointer';
    }
    hintPointer.className = hintclass;
    hint.firstChild.innerHTML = message;
    if (this.timeOutHideTooltip) {
        clearTimeout(this.timeOutHideTooltip);
        this.timeOutHideTooltip = null;
    };
    if (typeof (timeout) == 'undefined') {
        timeout = 3000;
    }
    this.timeOutHideTooltip = setTimeout(function () {
        if (hint == null) {
            return;
        }
        hint.style.display = 'none';
    }, timeout); // Auto hide in 10s
    if (elementInput != null) {
        if (typeof xPos == 'undefined') {
            xPos = 0;
        }
        if (typeof pointerXPos == 'undefined') {
            pointerXPos = 10;
        }
        if (typeof yPos == 'undefined') {
            yPos = -45;
        }
        if (typeof (pointerYPos) == 'undefined') {
            pointerYPos = 38;
        }
        if (typeof (height) == 'undefined') {
            height = 18;
        }
        hint.style.height = height + 'px';
        hintPointer.style.left = (pointerXPos + 'px');
        hintPointer.style.top = (pointerYPos + 'px');
        var pos = Validators.utils.findPos(elementInput);
        hint.style.left = (pos[0] + xPos) + 'px';
        hint.style.top = (pos[1] + yPos) + 'px';
        elementInput.onchange = function () {
            hint.style.display = 'none';
        };
        elementInput.focus();
    };
    hint.style.display = 'block';
}
var div = $('ptGridContainer');
ptGrid.Render(div);

var bettingLimit = null;
bettingLimit = new Fanex.PTEngine.BettingLimit(customer.bettingLimit)
var bettingLimitContainer = $('bettingLimitContainer');
bettingLimit.Render(bettingLimitContainer);

/* Render Commission to selected*/
var commission = customer.commissionSetting;
Fanex.PTEngine.PT_SetSelect($("groupA"), 0, commission.groupAMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupA);
Fanex.PTEngine.PT_SetSelect($("groupB"), 0, commission.groupBMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupB);
Fanex.PTEngine.PT_SetSelect($("groupC"), 0, commission.groupCMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupC);
Fanex.PTEngine.PT_SetSelect($("groupD"), 0, commission.groupDMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupD);

Fanex.PTEngine.PT_SetSelect($("discount1x2"), 0, commission.discount1x2Max, Fanex.PTEngine.COMMISSION_STEP, commission.discount1x2);
Fanex.PTEngine.PT_SetSelect($("discountCS"), 0, commission.discountCSMax, Fanex.PTEngine.COMMISSION_STEP, commission.discountCS);
Fanex.PTEngine.PT_SetSelect($("discountNumber"), 0, commission.discountNumberMax, Fanex.PTEngine.COMMISSION_STEP, commission.discountNumber);
if (parseBool2(isOfferHRFixedOdds)) {
    Fanex.PTEngine.PT_SetSelect($("discountHRFixOdds"), 0, commission.discountHRFixedOddsMax, Fanex.PTEngine.COMMISSION_STEP, commission.discountHRFixedOdds);
}
Fanex.Customer.SetNextCustomer($("selectedOne"), $("selectedTwo"), null, customer.newUserName);

// For Check Edit Postion Taking
if (context.targetCustId > 0) {
    var currentStatusClose = customer.statusSetting.closed;
    var currentStatusSuspend = customer.statusSetting.suspended;
    $("minCreditId").className = "";
    customer.minCreditTemp = String.prototype.FormatNumber(customer.minCredit);
    $("minMemberMaxCreditId").className = "";
    customer.memberMinCredit = String.prototype.FormatNumber(customer.memberMinCredit);
}
else {
    $("minCreditId").className = "hide";
    $("minMemberMaxCreditId").className = "hide";
    customer.minCreditTemp = 0;
}

//console.log(customer);
/*
Bind customer information to customer JSON object by knockout.js
Render PTGrid
*/
var CustomerModel = function (customer) {
    var uplineUserName = customer.userName;
    // Check Transfer condition
    displayTransferCondition = Fanex.Customer.DisplayTransferCondition(customer.transferOption);

    // Set label MaxCredit
    customer.maxCredit = String.prototype.FormatNumber(customer.maxCredit);
    customer.credit = String.prototype.FormatNumber(customer.credit);
    customer.memberMaxCredit = String.prototype.FormatNumber(customer.memberMaxCredit);

    if (context.targetCustId == 0) {
        if (customer.credit == "0") customer.credit = "";
        if (customer.memberMaxCredit == "0") customer.memberMaxCredit = "";
    }

    this.saveCustomer = function () {
        if (!Fanex.Customer.CheckInfoValue()) return;
        if (!bettingLimit.Validate()) return;

        // Set value to transfer option
        customer.transferOption = Fanex.Customer.TransferOption(customer.transferOption);
        customer.commissionSetting = Fanex.Customer.SetValueCommissionSettings(customer.commissionSetting);
        customer.credit = Fanex.RoundNumber(parseFloat($("credit").value, true), 1); // Fix bug for IE7
        customer.memberMaxCredit = Fanex.RoundNumber(parseFloat($("memberMaxCredit").value, true), 1); // Fix bug for IE7

        // For Check Edit Postion Taking
        if (context.targetCustId == 0) {
            // Set next to userName
            customer.userName = uplineUserName + Fanex.Customer.SetAutoUserName();
        }

        // EscapeHTML String
        customer.firstName = $("firstName").value.escapeHtml();
        customer.lastName = $("lastName").value.escapeHtml();
        customer.phone = $("phone").value.escapeHtml();
        customer.mobilePhone = $("mobilePhone").value.escapeHtml();
        customer.fax = $("fax").value.escapeHtml();
        try {
            customer.password = $("password").value.ec(top.ph).hc();
        }
        catch (ex) {

        }
        // Add more .....

        if (ptGrid) {
            customer.ptSetting = ptGrid.Save();
            customer.bettingLimit = bettingLimit.Save();
            var text = JSON.stringify(customer);
            $("txtCustomerObject").value = text;
            var params = ajax.CreateParams('txtCustomerObject');

            // For Check Edit Postion Taking
            if (context.targetCustId > 0) {
                if (!isShowPopUpSetPTMember) {
                    isChangeM2 = Fanex.EditPT.CheckChangeM2(oldPTSetting, customer.ptSetting);
                }
                if (isChangeM2 && !isShowPopUpSetPTMember) {
                    Popups.Confirm(Fanex.EditPT.MemberMaximumPT(), Language.CONFIRM_AGENT_NOT_TO_TAKE_AUTO_POSITION_TAKING);
                    return;
                }
                else if (isChangeM2 && isShowPopUpSetPTMember) {
                    Popups.Close();
                }
            }

            function AddNewAgentComplete(result) {
                var errMsg = result.errMsg;
                if (result.errCode == 0) {
                    ageMsg.Hide();
                    var splitName = errMsg.split(',');
                    var username = splitName[0];
                    var custid = splitName[1];
                    var isFI = splitName[2];
                    var isCS = splitName[3];
                    var isHR = splitName[4];
                    var isBG = splitName[5];
                    var isLiveCS = splitName[6];
                    var isDisableKeno = splitName[7];
                    var userId = splitName[8];
                    var subAccId = splitName[9];

                    if (isFI == 0 || isCS == 0 || isHR == 0 || isBG == 0 || isDisableKeno == 0) {
                        url = age.GetBaseUrl() + "MemberInfo/RedirectPT.aspx?username=" + username + "&custid=" + custid 
                        + "&userId=" + userId + "&subAccId=" + subAccId + "&isfi=" + isFI + "&iscs=" + isCS + "&ishr=" + isHR + "&isbg="
                        + isBG + "&islivecs=" + isLiveCS + "&isDisableKeno" + isDisableKeno 
                        + "&isedit=false" + "&insert=true";
                    }
                    setTimeout(function () { parent.location.href = url; }, 4000);
                }
                else {
                    age.UnLock();
                    ageMsg.Show(errMsg);
                    window.scroll(0, 0);
                    return;
                }
            }

            function MasterUpdateAgentComplete(result) {
                if (result.errCode == 0) {
                    age.UnLock();
                    ageMsg.Show(result.errMsg, 1);

                    if (_page.CallBack) {
                        eval(_page.CallBack);
                    }
                    else {
                        top.main.DelayReloadPage();
                    }
                }
                else {
                    isChangeM2 = false;
                    isShowPopUpSetPTMember = false;
                    newValueMemberPT = "";
                    age.UnLock();
                    ageMsg.Show(result.errMsg);
                    window.scroll(0, 0);
                    return;
                }
            }

            age.ReloadPage(30000);
            // Post with return result in JSON format
            var submitUrl = "";
            if (context.targetCustId > 0 && context.targetRoleId == 2) { // Master update Agent
                if (newValueMemberPT != "") {
                    submitUrl = "../Handlers/MasterUpdateAgent.ashx?memberPositionTaking=" + newValueMemberPT;
                }
                else {
                    submitUrl = "../Handlers/MasterUpdateAgent.ashx";
                }
                OnComplete = MasterUpdateAgentComplete;
            }
            else {
                submitUrl = "../Handlers/SaveNewAgent.ashx";
                OnComplete = AddNewAgentComplete;
            }
            ajax.PostJSON(
				submitUrl,
				params,
				OnComplete
			);
        }
    };
};

ko.applyBindings(new CustomerModel(customer));

Fanex.EditPT = {
    // Edit PT Master on account Info
    OnRedirectToPage: function () {
        top.menu.reloadMenuUsrInfo();
        age.DelayReloadPage('../../../_MemberInfo/CustomerList/Master/AgentList.aspx');
    },

    // Check or UnCheck Close Status
    Closed: function (checkBoxClose, currentStatusClose) {
        if (checkBoxClose.checked == true && checkBoxClose.checked != currentStatusClose) {
            if (!confirm(Language.ALERT_CONFIRM_CLOSE)) {
                customer.statusSetting.closed = false;
                checkBoxClose.checked = false;
            }
        }
        else if (checkBoxClose.checked == false && checkBoxClose.checked != currentStatusClose) {
            if (!confirm(Language.ALERT_CONFIRM_OPEN)) {
                customer.statusSetting.closed = true;
                checkBoxClose.checked = true;
            }
        }
    },

    // Check or UnCheck Suspend Status
    Suspended: function (checkBoxSuspend, currentStatusSuspend) {
        if (checkBoxSuspend.checked == true && checkBoxSuspend.checked != currentStatusSuspend) {
            if (!confirm(Language.ALERT_CONFIRM_SUSPEND)) {
                customer.statusSetting.suspended = false;
                checkBoxSuspend.checked = false;
            }
        }
        else if (checkBoxSuspend.checked == false && checkBoxSuspend.checked != currentStatusSuspend) {
            if (!confirm(Language.ALERT_CONFIRM_UNSUSPEND)) {
                customer.statusSetting.suspended = true;
                checkBoxSuspend.checked = true;

            }
        }
    },

    // We need check have any PT value change from auto to none auto. And Set new value to Member
    CheckChangeM2: function (ptSettingOld, ptSettingNew) {
        var sportTypes = ptSettingOld.sportTypes;
        for (var sportTypeIndex = 0; sportTypeIndex < sportTypes.length; sportTypeIndex++) {
            var sportTypesItem = sportTypes[sportTypeIndex]
            var betTypeGroupSets = sportTypesItem.betTypeGroupSets;

            for (var groupSetIndex = 0; groupSetIndex < betTypeGroupSets.length; groupSetIndex++) {
                var betTypeGroupSetsItem = betTypeGroupSets[groupSetIndex];
                var betTypeGroups = betTypeGroupSetsItem.betTypeGroups;

                for (var groupIndex = 0; groupIndex < betTypeGroups.length; groupIndex++) {
                    var betTypeGroupItem = betTypeGroups[groupIndex];
                    var betTypes = betTypeGroupItem.betTypes;

                    for (var betTypeIndex = 0; betTypeIndex < betTypes.length; betTypeIndex++) {
                        betTypeItem = betTypes[betTypeIndex];
                        var isAutoOld = betTypeItem.pt.im2;
                        var isAutoNew = ptSettingNew.sportTypes[sportTypeIndex].betTypeGroupSets[groupSetIndex].betTypeGroups[groupIndex].betTypes[betTypeIndex].pt.im2;

                        if (isAutoOld && !isAutoNew) {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    },

    // Get selected and m4 value of Soccer HdpD Master Preset
    MemberMaximumPT: function () {
        var ptSetting = customer.ptSetting;
        var l3 = ptSetting.sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].pt.l3;
        var a2 = ptSetting.sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].pt.a2;
        return Math.max(0, Fanex.Round(l3 - a2));
    },

    // Confirm new value for all member before transfer to server
    SetNewValueToMember: function (isUpdateAllMember) {
        isShowPopUpSetPTMember = true;
        if (isUpdateAllMember) {
            var valueMemberPositionTaking = $("selectedMemberPositionTaking").value;
            newValueMemberPT = valueMemberPositionTaking + ";" + isUpdateAllMember;
        }
        var customerModel = new CustomerModel(customer);
        customerModel.saveCustomer();
    }
}

function ConfirmYes() {
    Fanex.EditPT.SetNewValueToMember(1);
}

function ConfirmNo() {
    Fanex.EditPT.SetNewValueToMember(0);
}

// For Edit Position Taking
RegisterStartUp(function () {
    if (context.targetCustId > 0) {
        age.addEvent($('closed'), 'click', function () {
            Fanex.EditPT.Closed($('closed'), currentStatusClose);
        });

        age.addEvent($('suspended'), 'click', function () {
            Fanex.EditPT.Suspended($('suspended'), currentStatusSuspend);
        });
    }

    var divPwd = $('divPassword');
    if (divPwd) {
        divPwd.title = '';
        $('password').maxLength = 16;
    }

    if ($('lblMemberMaxCredit') && $('lblMemberMaxCredit').innerHTML == '') {
        $('lblMemberMaxCredit').innerHTML = 0;
    }

    if ($('maxCredit') && $('maxCredit').innerHTML == '') {
        $('maxCredit').innerHTML = 0;
    }
    
    Fanex.Customer.CheckUser(customer.userName, 'selectedOne', 'selectedOne', 'selectedTwo', null, 'msgIcon');
    Fanex.Customer.CheckUser(customer.userName, 'selectedTwo', 'selectedOne', 'selectedTwo', null, 'msgIcon');
});