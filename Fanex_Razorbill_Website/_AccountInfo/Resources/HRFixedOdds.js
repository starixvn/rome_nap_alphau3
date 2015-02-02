function changeHRFixedOddsStatus(evt, hasDownLine) {
    if ($('chkHRFixedOdds').disabled) return _stopEvent(evt);

    var isOpenPopup = false;
    var isDisabled = $('chkHRFixedOdds').checked;
    if (isDisabled) {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            if (!confirm(_page.strConfirmDisableHRFixedOdds)) {
                return _stopEvent(evt);
            }
            $('chkEnabled').checked = false;
            $('chkEnabledDL').checked = false;
            updateHRFixedOddsStatus(true, true);
        } else { //for member
            if (!confirm(_page.lblAlertDisableHRFixedOddsMember)) {
                return _stopEvent(evt);
            }
            updateHRFixedOddsStatus(true, false);
        }
    } else {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            showHRFixedOddsPopup();
            return _stopEvent(evt);
        } else { //for member
            if (!confirm(_page.strConfirmEnableHRFixedOdds)) {
                return _stopEvent(evt);
            }
            updateHRFixedOddsStatus(false, false);
        }
    }

    function showHRFixedOddsPopup() {
        // fill data for popup
        assignDefaultPopupValue();
        $('hdnTypeStatus').value = 'HR Fixed Odds'; //type popup
        $('lblEnableStatus').innerHTML = _page.lblHRFixedOdds; //title popup
        $('lbldownline').innerHTML = _page.HRFixedOddsDownlines;
        $('btnUpdStatus').onclick = function () {
            var isDisabled = !$('chkEnabled').checked; // chkEnabled = true => isDisabled    =false;
            var isDisableDownline = !$('chkEnabledDL').checked; //chkEnabledDL = true => isDisableDownline =false
            updateHRFixedOddsStatus(isDisabled, isDisableDownline);
        };
        isOpenPopup = true;
        showDivPopup('chkHRFixedOdds');
    }

    function updateHRFixedOddsStatus(isDisabled, isDisableDownline) {
        ajax.PostJSON('Handlers/UpdateHRFixedOddsStatus.ashx', 'custid=' + _page.UserId + '&isdisabled=' + isDisabled + '&disabledownline=' + isDisableDownline, onComplete);

        function onComplete(result) {
            if (result.errCode == 0) {
                $('chkHRFixedOdds').checked = !isDisabled;
            } else alert(result.errMsg);
            if (isOpenPopup) closeDivPopup();
        }
    }
}