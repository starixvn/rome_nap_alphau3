﻿function changeESportsStatus(evt, hasDownLine) {
    if ($('chkESports').disabled) return _stopEvent(evt);

    var isOpenPopup = false;
    var isDisabled = $('chkESports').checked;
    if (isDisabled) {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            if (!confirm(_page.strConfirmDisableESports)) {
                return _stopEvent(evt);
            }
            $('chkEnabled').checked = false;
            $('chkEnabledDL').checked = false;
            updateESportsStatus(true, true);
        } else { //for member
            if (!confirm(_page.lblAlertDisableESportsMember)) {
                return _stopEvent(evt);
            }
            updateESportsStatus(true, false);
        }
    } else {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            showESportsPopup();
            return _stopEvent(evt);
        } else { //for member
            if (!confirm(_page.strConfirmEnableESports)) {
                return _stopEvent(evt);
            }
            updateESportsStatus(false, false);
        }
    }

    function showESportsPopup() {
        // fill data for popup
        assignDefaultPopupValue();
        $('hdnTypeStatus').value = 'ESports'; //type popup
        $('lblEnableStatus').innerHTML = _page.lblEnableESports; //title popup
        $('lbldownline').innerHTML = _page.ESportsDownlines;
        $('btnUpdStatus').onclick = function () {
            var isDisabled = !$('chkEnabled').checked; // chkEnabled = true => isDisabled    =false;
            var isDisableDownline = !$('chkEnabledDL').checked; //chkEnabledDL = true => isDisableDownline =false
            updateESportsStatus(isDisabled, isDisableDownline);
        };
        isOpenPopup = true;
        showDivPopup('chkESports');
    }

    function updateESportsStatus(isDisabled, isDisableDownline) {
        ajax.PostJSON('Handlers/updateESportsStatus.ashx', 'custid=' + _page.UserId + '&isdisabled=' + isDisabled + '&disabledownline=' + isDisableDownline, onComplete);

        function onComplete(result) {
            if (result.errCode == 0) {
                $('chkESports').checked = !isDisabled;
            } else alert(result.errMsg);
            if (isOpenPopup) closeDivPopup();
        }
    }
}