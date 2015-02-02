
function disableCommission(isDisable) {
    $("groupA").disabled = isDisable;
    $("groupB").disabled = isDisable;
    $("groupC").disabled = isDisable;
    $("groupD").disabled = isDisable;
    $("discount1x2").disabled = isDisable;
    $("discountCS").disabled = isDisable;
    $("discountNumber").disabled = isDisable;
    if (parseBool2(isOfferHRFixedOdds)) {
        $("discountHRFixOdds").disabled = isDisable;
    }
}

function initCommission() {
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

    currentStatusClose = customer.statusSetting.closed;
}

function saveCommissionSettings(commissionSettings) {
    commissionSettings.groupA = $("groupA").value;
    commissionSettings.groupB = $("groupB").value;
    commissionSettings.groupC = $("groupC").value;
    commissionSettings.groupD = $("groupD").value;
    commissionSettings.discount1x2 = $("discount1x2").value;
    commissionSettings.discountCS = $("discountCS").value;
    commissionSettings.discountNumber = $("discountNumber").value;
    if (parseBool2(isOfferHRFixedOdds)) {
        commissionSettings.discountHRFixedOdds = $("discountHRFixOdds").value;
    }
}

function CheckChangeM3(ptSettingOld, ptSettingNew) {
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
                        var isAutoOld = betTypeItem.pt.is3;
                        var isAutoNew = ptSettingNew.sportTypes[sportTypeIndex].betTypeGroupSets[groupSetIndex].betTypeGroups[groupIndex].betTypes[betTypeIndex].pt.is3;

                        if (isAutoOld && !isAutoNew) {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
}

function MemberMaximumPT() {
        var ptSetting = customer.ptSetting;
        var s4 = ptSetting.sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].pt.s4;
        var m3 = ptSetting.sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].pt.m3;
        return Fanex.Round(s4 - m3);
}

function SetNewValueToMember(isUpdateAllMember) {
        isShowPopUpSetPTMember = true;
        if (isUpdateAllMember) {
            var valueMemberPositionTaking = $("selectedMemberPositionTaking").value;
            newValueMemberPT = valueMemberPositionTaking + ";" + isUpdateAllMember;
        }
        var customerModel = new CustomerModel(customer);
        customerModel.saveCustomer();
}

function ConfirmYes() {
    SetNewValueToMember(1);
}

function ConfirmNo() {
    SetNewValueToMember(0);
}

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