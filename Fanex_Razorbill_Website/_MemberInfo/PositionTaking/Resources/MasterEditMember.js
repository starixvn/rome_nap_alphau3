Fanex.PTEngine.CURRENT_CONTEXT = context;
Fanex.PTEngine.CURRENT_CONTEXT.ia3 = !customer.statusSetting.disableAutoMPT; // Allow auto PT master on member

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
bettingLimit.Render(bettingLimitContainer, true);

var currentStatusClose = customer.statusSetting.closed;
var currentStatusSuspend = customer.statusSetting.suspended;

/*
Bind customer information to customer JSON object by knockout.js
Render PTGrid
*/
var CustomerModel = function (customer) {
    this.saveCustomer = function () {

        // EscapeHTML String
        customer.firstName = customer.firstName.escapeHtml();
        customer.lastName = customer.lastName.escapeHtml();
        customer.phone = customer.phone.escapeHtml();
        customer.mobilePhone = customer.mobilePhone.escapeHtml();
        customer.fax = customer.fax.escapeHtml();

        if (ptGrid) {
            customer.ptSetting = ptGrid.Save();
            customer.bettingLimit = null;
            var text = JSON.stringify(customer);
            $("txtCustomerObject").value = text;
            var params = ajax.CreateParams('txtCustomerObject');

            function MasterUpdateMemberComplete(result) {
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
                    age.UnLock();
                    ageMsg.Show(result.errMsg);
                    window.scroll(0, 0);
                    return;
                }
            }

            age.ReloadPage(30000);
            // Post with return result in JSON format
            var urlSubmit;
            if (context.targetCustId > 0 && context.targetRoleId == 1) { // Master update member
                urlSubmit = "../Handlers/MasterUpdateMember.ashx";
                OnComplete = MasterUpdateMemberComplete;
            }

            ajax.PostJSON(
				urlSubmit,
				params,
				OnComplete
			);
        }
    };
};

Fanex.EditPT = {
    // Edit PT Master on account Info
    OnRedirectToPage: function (agentId) {
        top.menu.reloadMenuUsrInfo();
        age.DelayReloadPage('../../../_MemberInfo/CustomerList/Master/MemberList.aspx?custid=' + agentId);
    },

    // Check or UnCheck Close Status
    Closed: function (checkBoxClose, currentStatusClose) {
        if (checkBoxClose.checked == true && checkBoxClose.checked != currentStatusClose) {
            if (!confirm(Language.ALERT_CONFIRM_CLOSE_MEMBER)) {
                customer.statusSetting.closed = false;
                checkBoxClose.checked = false;
            }
        }
        else if (checkBoxClose.checked == false && checkBoxClose.checked != currentStatusClose) {
            if (!confirm(Language.ALERT_CONFIRM_CLOSE_MEMBER)) {
                customer.statusSetting.closed = true;
                checkBoxClose.checked = true;
            }
        }
    },

    // Check or UnCheck Suspend Status
    Suspended: function (checkBoxSuspend, currentStatusSuspend) {
        if (checkBoxSuspend.checked == true && checkBoxSuspend.checked != currentStatusSuspend) {
            if (!confirm(Language.ALERT_CONFIRM_SUSPEND_MEMBER)) {
                customer.statusSetting.suspended = false;
                checkBoxSuspend.checked = false;
            }
        }
        else if (checkBoxSuspend.checked == false && checkBoxSuspend.checked != currentStatusSuspend) {
            if (!confirm(Language.ALERT_CONFIRM_UNSUSPEND_MEMBER)) {
                customer.statusSetting.suspended = true;
                checkBoxSuspend.checked = true;
            }
        }
    }
}

ko.applyBindings(new CustomerModel(customer));

// For Edit Position Taking
RegisterStartUp(function () {
    age.addEvent($('closed'), 'click', function () {
        Fanex.EditPT.Closed($('closed'), currentStatusClose);
    });

    age.addEvent($('suspended'), 'click', function () {
        Fanex.EditPT.Suspended($('suspended'), currentStatusSuspend);
    });
    Fanex.Customer.HideTitle();
});