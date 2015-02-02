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
                if (!ValidateBetSetting()) {
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
    var sportLength = customer.settings.sportTypes.length;

    // only update for checked tote
    otherSetting.isupdatebetsetting = document.getElementById("betsettingheader").firstChild.checked;
    otherSetting.isupdatecomm = document.getElementById("commheader").firstChild.checked;
    otherSetting.isupdatestatus = document.getElementById("statusheader").firstChild.checked;
    otherSetting.isupdatept = document.getElementById("ptheader").firstChild.checked;
    for (var i = 0; i < sportLength; i++) {
        var sportType = customer.settings.sportTypes[i];
        var toteLength = sportType.betTypeGroups.length;
        for (var j = 0; j < toteLength; j++) {
            var tote = sportType.betTypeGroups[j];
            if (!tote.isHide) {
                otherSetting["isupdatebettote_" + sportType.id + "_" + tote.id] = document.getElementById("divBettingComm_BettypeGroup_" + sportType.id + "_" + tote.id).firstChild.checked;
                otherSetting["isupdatepttote_" + sportType.id + "_" + tote.id] = document.getElementById("pttoteheader_" + sportType.id + "_" + tote.id).firstChild.checked;
            }
        }
    }

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
    var isMulti = IsMulti();
    var url = "Handlers/UpdateCustomerSetting.ashx?targetCustId=" + custid + "&targetRoleId=" + context.targetRoleId + "&isMulti=" + isMulti.toString() + "&id=" + id;
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
    'isupdateallmembers': false,
    'isupdatebetsetting': false,
    'isupdatecomm': false,
    'isupdatestatus': false,
    'isupdatept': false
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

EnhanceMultiplePT();

function InitUI() {
    if (context.formId == 4) {
        $("btnSubmit").parentNode.style.display = "none";
        $("btnMain").parentNode.style.display = "none";
    }
}

RegisterStartUp('InitUI()');

function EnhanceMultiplePT() {
    // bet settings
    if (context.roleId - context.targetRoleId == 1) {
        var element = document.getElementById("betsettingheader");
        InsertCheckBoxInFrontOf(element, BetSettingHeaderClicked);

        element = document.getElementById("commheader");
        InsertCheckBoxInFrontOf(element, BetCommHeaderClicked);

        element = document.getElementById("statusheader");
        InsertCheckBoxInFrontOf(element, BetStatusHeaderClicked);
    }

    // pt
    element = document.getElementById("ptheader");
    InsertCheckBoxInFrontOf(element, PTHeaderClicked);

    var sportLength = customer.settings.sportTypes.length;
    for (var i = 0; i < sportLength; i++) {
        var sportType = customer.settings.sportTypes[i];

        var toteLength = sportType.betTypeGroups.length;
        for (var j = 0; j < toteLength; j++) {
            var tote = sportType.betTypeGroups[j];
            // bet settings
            if (context.roleId - context.targetRoleId == 1) {
                element = document.getElementById("divBettingComm_BettypeGroup_" + sportType.id + "_" + tote.id);
                InsertCheckBoxInFrontOf(element, BetToteClicked);
            }

            // pt
            element = document.getElementById("pttoteheader_" + sportType.id + "_" + tote.id);
            InsertCheckBoxInFrontOf(element, PtToteClicked);
        }
    }

    // bet settings
    if (context.roleId - context.targetRoleId == 1) {
        var copyAllInput = document.getElementById("divBettingComm_BettypeGroup_151_41").childNodes[2].childNodes[0];
        copyAllInput.clickFunction = copyAllInput.onclick;
        copyAllInput.onclick = BettingCopyAllClicked;
        copyAllInput.disabled = true;
        

        var bettingCommEditAllInput = document.getElementById("commheader").childNodes[2].childNodes[0];
        bettingCommEditAllInput.clickFunction = bettingCommEditAllInput.onclick;
        bettingCommEditAllInput.onclick = BettingCommEditAllClicked;

        var bettingStatusEditAllInput = document.getElementById("statusheader").childNodes[2].childNodes[0];
        bettingStatusEditAllInput.clickFunction = bettingStatusEditAllInput.onclick;
        bettingStatusEditAllInput.onclick = BettingStatusEditAllClicked;

        ValidateBettingBoxUI(document.getElementById("betsettingheader"));
    }

    // pt
    var sportLength = customer.settings.sportTypes.length;
    for (var i = 0; i < sportLength; i++) {
        var sportType = customer.settings.sportTypes[i];
        var ptsportCopied = document.getElementById("ptsportheader_" + sportType.id).parentNode.childNodes[1];
        ptsportCopied.clickFunction = ptsportCopied.onclick;
        ptsportCopied.onclick = null;
    }

    ValidatePtBoxUI(document.getElementById("ptheader"));
}

function InsertCheckBoxInFrontOf(element, clickEvent) {    
    if (element) {
        var checkbox = document.createElement("INPUT");
        checkbox.type = "checkbox";
        checkbox.onclick = clickEvent;
        checkbox.parent = element;
        if (element.className == "bettingBox-tote") {
            checkbox.className = 'float-left';
        }
        if (element.id == 'commheader' && element.className == 'bettingBox-header-comm') {
            checkbox.className = 'float-left';
            checkbox.style.position = 'absolute';
            checkbox.style.left = '787px';
            checkbox.style.display = 'block';
            checkbox.style.zIndex = '999';
            checkbox.style.marginTop = '15px';
            
        }
        if (element.id == 'commheader' && element.className == 'bettingBox-header-comm2') {
            checkbox.className = 'float-left';
            checkbox.style.position = 'absolute';
            checkbox.style.left = '833px';
            checkbox.style.display = 'block';
            checkbox.style.zIndex = '999';
            
        }

        if (element.id == 'statusheader' && element.className == 'bettingBox-header-status') {
            checkbox.className = 'float-left';
            checkbox.style.position = 'absolute';
            checkbox.style.left = '900px';
            checkbox.style.display = 'block';
            checkbox.style.zIndex = '999';
            
        }
        if (element.id == 'statusheader' && element.className == 'bettingBox-header-status2') {
            checkbox.className = 'float-left';
            checkbox.style.position = 'absolute';
            checkbox.style.left = '932px';
            checkbox.style.display = 'block';
            checkbox.style.zIndex = '999';
            checkbox.style.marginTop = '15px';
        }

        var textNode = element.childNodes[0];
        element.insertBefore(checkbox, textNode);
        
    }
}

function ValidateBetSetting() {
    // no validating on cross level
    if (this.context.roleId - this.context.targetRoleId != 1) {
        return true;
    }

    var betsettingChecked = document.getElementById("betsettingheader").firstChild.checked;
    if (!betsettingChecked) {
        return true;
    }

    var copyAllInput = document.getElementById("divBettingComm_BettypeGroup_151_41").childNodes[2].childNodes[0];
    var isCopyAll = (copyAllInput.getAttribute('copyBettingComm') == "1");
   
    var minBet0 = parseFloat(document.getElementById("minbet_151_41").value, true);
    var maxBet0 = parseFloat(document.getElementById("maxbet_151_41").value, true);
    var maxPerMatch0 = parseFloat(document.getElementById("maxpermatch_151_41").value, true);

    var sportLength = customer.settings.sportTypes.length;
    for (var i = 0; i < sportLength; i++) {
        var sportType = customer.settings.sportTypes[i];
        var toteLength = sportType.betTypeGroups.length;
        for (var j = 0; j < toteLength; j++) {
            var tote = sportType.betTypeGroups[j];
            if (tote.isHide) {
                continue;
            }

            var toteChecked = document.getElementById("divBettingComm_BettypeGroup_" + sportType.id + "_" + tote.id).firstChild.checked;
            if (!toteChecked) {
                continue;
            }

            var minBetElement = document.getElementById("minbet_" + sportType.id + "_" + tote.id);
            var maxBetElement = document.getElementById("maxbet_" + sportType.id + "_" + tote.id);
            var maxPerMatchElement = document.getElementById("maxpermatch_" + sportType.id + "_" + tote.id);

            // Validate input text
            if ((isCopyAll && j == 0 && i == 0) || !isCopyAll) {
                if (validateExpr.test(minBetElement.value) == false) {
                    Fanex.PTRacing.BettingCommSetting.ValidateMessage(minBetElement, Language.INCORRECT_MIN_BET);
                    return false;
                }
                if (validateExpr.test(maxBetElement.value) == false) {
                    Fanex.PTRacing.BettingCommSetting.ValidateMessage(maxBetElement, Language.INCORRECT_MAX_BET);
                    return false;
                }
                if (validateExpr.test(maxPerMatchElement.value) == false) {
                    Fanex.PTRacing.BettingCommSetting.ValidateMessage(maxPerMatchElement, Language.INCORRECT_MAX_PER_MATCH);
                    return false;
                }
            }

            var minBet = parseFloat(document.getElementById("minbet_" + sportType.id + "_" + tote.id).value, true);
            var maxBet = parseFloat(document.getElementById("maxbet_" + sportType.id + "_" + tote.id).value, true);
            var maxPerMatch = parseFloat(document.getElementById("maxpermatch_" + sportType.id + "_" + tote.id).value, true);
            var minBetMin = tote.bettingCommSetting.minBetMin;
            var maxBetMax = tote.bettingCommSetting.maxBetMax;
            var maxPerMatchMax = tote.bettingCommSetting.maxPerMatchMax;

            if (isCopyAll) {
                minBet = minBet0;
                maxBet = maxBet0;
                maxPerMatch = maxPerMatch0;
            }

            if (minBet == maxBet && maxBet == maxPerMatch && maxPerMatch == 0) {
                continue;
            }

            // Check min bet < max bet < max per match
            if (minBet > maxBet) {
                this.ValidateMessage(minBetElement, Language.MIN_BET_LOWER_THAN_MIN_MAX_BET.Format(tote.name));
                return false;
            }
            if (minBet > maxPerMatch) {
                this.ValidateMessage(minBetElement, Language.MIN_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET.Format(tote.name));
                return false;
            }
            if (maxBet > maxPerMatch) {
                this.ValidateMessage(maxBetElement, Language.MAX_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET.Format(tote.name));
                return false;
            }

            // Check minbet < minbetmax
            if (typeof (minBetMin) != 'undefined' && minBetMin > 0 && minBet < minBetMin) {
                this.ValidateMessage(minBetElement, Language.MIN_BET_GREATER_THAN_MIN_MIN_BET.Format(''.FormatNumber(minBetMin)));
                return false;
            }
            // Check maxbet < maxbetmin
            if (typeof (maxBetMax) != 'undefined' && maxBet > maxBetMax && maxBetMax != 0) {
                this.ValidateMessage(maxBetElement, Language.MAX_BET_LOWER_THAN_MAX_MAX_BET.Format(''.FormatNumber(maxBetMax)));
                return false;
            }
            // Check maxpermatch_min < maxpermatch < maxpermatch_max
            if (typeof (maxPerMatchMax) != 'undefined' && maxPerMatch > maxPerMatchMax && maxPerMatchMax != 0) {
                this.ValidateMessage(maxPerMatchElement, Language.MAX_PER_MATCH_LOWER_THAN_MAX_PER_MATCH_MAX_BET.Format(''.FormatNumber(maxPerMatchMax)), -130, 160);
                return false;
            }
        }
    }

    // Success
    return true;
}

function ValidateBettingBoxUI(caller) {
    var copyAllInput = document.getElementById("divBettingComm_BettypeGroup_151_41").childNodes[2].childNodes[0];    
    var isCopyAll = (copyAllInput.getAttribute('copyBettingComm') == "1");

    var commEditAllInput = document.getElementById("commheader").childNodes[2].childNodes[0];
    var isCommEditAll = (commEditAllInput.getAttribute('copycomm') == "1");

    var statusEditAllInput = document.getElementById("statusheader").childNodes[2].childNodes[0];
    var isStatusEditAll = (statusEditAllInput.getAttribute('copystatus') == "1");

    var sportLength = customer.settings.sportTypes.length;
    for (var i = 0; i < sportLength; i++) {
        var sportType = customer.settings.sportTypes[i];
        var toteLength = sportType.betTypeGroups.length;
        for (var j = 0; j < toteLength; j++) {
            var tote = sportType.betTypeGroups[j];
            var element = document.getElementById("divBettingComm_BettypeGroup_" + sportType.id + "_" + tote.id);           
            if (element) {
                var isCopiedElement = (element.id == "divBettingComm_BettypeGroup_151_41");
                var toteInput = element.firstChild;
                var callerInput = caller.firstChild;
                if (caller.id == element.id && isCopiedElement) {
                    element.childNodes[2].childNodes[0].disabled = !toteInput.checked;
                    if (!callerInput.checked) {
                        if (isCopyAll) {
                            copyAllInput.onclick.apply(copyAllInput);
                            return;
                        }
                    }
                }

                // bet settings
                var minbetElement = document.getElementById("minbet_" + sportType.id + "_" + tote.id).parentNode;
                var maxbetElement = document.getElementById("maxbet_" + sportType.id + "_" + tote.id).parentNode;
                var maxpermatchElement = document.getElementById("maxpermatch_" + sportType.id + "_" + tote.id).parentNode;
                callerInput = document.getElementById("betsettingheader").firstChild;
                if (callerInput.checked && toteInput.checked) {
                    if (isCopyAll && !isCopiedElement) {
                        ElementStatusHandler.DisableElement(minbetElement);
                        ElementStatusHandler.DisableElement(maxbetElement);
                        ElementStatusHandler.DisableElement(maxpermatchElement);
                    }
                    else {
                        ElementStatusHandler.EnableElement(minbetElement);
                        ElementStatusHandler.EnableElement(maxbetElement);
                        ElementStatusHandler.EnableElement(maxpermatchElement);
                    }
                }
                else {
                    ElementStatusHandler.DisableElement(minbetElement);
                    ElementStatusHandler.DisableElement(maxbetElement);
                    ElementStatusHandler.DisableElement(maxpermatchElement);
                }

                // bet commission
                var commElement = document.getElementById("comm_" + sportType.id + "_" + tote.id).parentNode;
                callerInput = document.getElementById("commheader").firstChild;
                if (callerInput.checked && toteInput.checked) {
                    if (isCopyAll && isCommEditAll && !isCopiedElement) {
                        ElementStatusHandler.DisableElement(commElement);
                    }
                    else {
                        ElementStatusHandler.EnableElement(commElement);
                    }
                }
                else {
                    ElementStatusHandler.DisableElement(commElement);
                }

                // bet status
                var enableElement = document.getElementById("enable_" + sportType.id + "_" + tote.id).parentNode;
                if (context.targetRoleId != 1) {
                    var downlinesElement = document.getElementById("downlines_enable_" + sportType.id + "_" + tote.id).parentNode;
                }
                callerInput = document.getElementById("statusheader").firstChild;
                if (callerInput.checked && toteInput.checked) {
                    if (isCopyAll && isStatusEditAll && !isCopiedElement) {
                        ElementStatusHandler.DisableElement(enableElement);
                        if (context.targetRoleId != 1) {
                            ElementStatusHandler.DisableElement(downlinesElement);
                        }
                    }
                    else {
                        ElementStatusHandler.EnableElement(enableElement);
                        if (context.targetRoleId != 1) {
                            ElementStatusHandler.EnableElement(downlinesElement);
                        }
                    }
                }
                else {
                    ElementStatusHandler.DisableElement(enableElement);
                    if (context.targetRoleId != 1) {
                        ElementStatusHandler.DisableElement(downlinesElement);
                    }
                }
            }
        }
    }
}

function ValidatePtBoxUI(caller) {
    var copyAllInput = document.getElementById("ptsportheader_151").parentNode.childNodes[1];
    var isCopyAll = (copyAllInput.className == "PTSportCopiedActive");
    var ptheaderInput = document.getElementById("ptheader").childNodes[0];

    var sportLength = customer.settings.sportTypes.length;
    for (var i = 0; i < sportLength; i++) {
        var sportType = customer.settings.sportTypes[i];
        var toteLength = sportType.betTypeGroups.length;
        for (var j = 0; j < toteLength; j++) {
            var tote = sportType.betTypeGroups[j];
            var element = document.getElementById("pttoteheader_" + sportType.id + "_" + tote.id);
            if (element) {
                var isCopiedElement = (element.id == "pttoteheader_151_41");
                var elementInput = element.firstChild;

                if (caller.id == element.id) {
                    if (isCopiedElement) {
                        if (!elementInput.checked) {
                            if (isCopyAll) {
                                copyAllInput.onclick.apply(copyAllInput);
                                return;
                            }
                        }
                    }

                    if (!elementInput.checked) {
                        if (isCopyWinChecked) {
                            copyWinInput.onclick.apply(copyWinInput);
                            return;
                        }
                    }
                }

                if (isCopiedElement) {
                    if (ptheaderInput.checked && elementInput.checked) {
                        copyAllInput.onclick = PtCopyAllClicked;
                        var sportLength2 = customer.settings.sportTypes.length;
                        for (var k = 0; k < sportLength2; k++) {
                            var sportType2 = customer.settings.sportTypes[k];
                            if (sportType2.id != 151) {
                                var ptsportCopied = document.getElementById("ptsportheader_" + sportType2.id).parentNode.childNodes[1];
                                ptsportCopied.onclick = PtEditAllClicked;
                            }
                        }
                    }
                    else {
                        copyAllInput.onclick = null;
                        var sportLength2 = customer.settings.sportTypes.length;
                        for (var k = 0; k < sportLength2; k++) {
                            var sportType2 = customer.settings.sportTypes[k];
                            if (sportType2.id != 151) {
                                var ptsportCopied = document.getElementById("ptsportheader_" + sportType2.id).parentNode.childNodes[1];
                                ptsportCopied.onclick = null;
                            }
                        }
                    }
                }

                var ptBox = element.parentNode.childNodes[1];
                var copyWinInput = ptBox.childNodes[1].firstChild;
                var isCopyWinChecked = (copyWinInput.childNodes[1].className.indexOf("PTBetTypeCopiedActive") != -1);
                if (!copyWinInput.clickFunction) {
                    copyWinInput.clickFunction = copyWinInput.onclick;
                }

                if (elementInput.checked && ptheaderInput.checked) {
                    if (isCopiedElement || !isCopyAll) {
                        ElementStatusHandler.EnableElement(ptBox);
                        copyWinInput.onclick = PtCopyWinClicked;
                    }
                    else {
                        var ptsportEdited = document.getElementById("ptsportheader_" + sportType.id).parentNode.childNodes[1];
                        var isPtSportEdited = (ptsportEdited.className == "PTSportEditCopied");
                        if (sportType.id != 151 && isPtSportEdited) {
                            ElementStatusHandler.EnableElement(ptBox);
                            copyWinInput.onclick = PtCopyWinClicked;
                        }
                        else {
                            ElementStatusHandler.DisableElement(ptBox);
                            copyWinInput.onclick = null;
                        }
                    }

                    if ((isCopiedElement && isCopyAll) || isCopyWinChecked) {
                        ElementStatusHandler.DisableElement(ptBox.childNodes[2]);
                    }
                }
                else {
                    ElementStatusHandler.DisableElement(ptBox);
                    copyWinInput.onclick = null;
                }
            }
        }
    }
}

function BetSettingHeaderClicked() {
    ValidateBettingBoxUI(this.parentNode);
}

function BetCommHeaderClicked() {
    ValidateBettingBoxUI(this.parentNode);
}

function BetStatusHeaderClicked() {
    ValidateBettingBoxUI(this.parentNode);
}

function BetToteClicked() {
    ValidateBettingBoxUI(this.parentNode);
}

function BettingCopyAllClicked() {
    this.clickFunction();
    ValidateBettingBoxUI(this.parentNode);
}

function BettingCommEditAllClicked() {
    this.clickFunction();
    ValidateBettingBoxUI(this.parentNode);
}

function BettingStatusEditAllClicked() {
    this.clickFunction();
    ValidateBettingBoxUI(this.parentNode);
}

function PTHeaderClicked() {
    ValidatePtBoxUI(this.parentNode);
}

function PtToteClicked() {
    ValidatePtBoxUI(this.parentNode);
}

function PtCopyAllClicked() {
    this.clickFunction();
    ValidatePtBoxUI(this.parentNode);
}

function PtEditAllClicked() {
    this.clickFunction();
    ValidatePtBoxUI(this.parentNode);
}

function PtCopyWinClicked() {
    this.clickFunction();
    ValidatePtBoxUI(this.parentNode);
}