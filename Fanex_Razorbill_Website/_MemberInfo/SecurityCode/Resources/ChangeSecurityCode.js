/*
* Created 20130415@kan: Javascript functions for change security code
* Revision ?@? - ... 
*/
function ChangeSecCode() {
	// Validate sec code
	var txtSecCode = $('txtSecCode');
	var txtConfirmSecCode = $('txtConfirmSecCode');

	var errMsg = null;

	var oldSecCode = $('inputSecCode') ? $('inputSecCode').value.trim() : null;
	var secCode = txtSecCode.value.trim();
	var confirmSecCode = txtConfirmSecCode.value.trim();
	if (oldSecCode != null && oldSecCode.length === 0) {
		errMsg = _page.alertOldSecCodeEmpty;
	    $('inputSecCode').focus();
    }
	else if (secCode.length === 0) {
	    errMsg = _page.alertSecCodeEmpty;
	    txtSecCode.focus();
	}
	else if (!IsSecurityCode(secCode)) {
	    errMsg = _page.alertSecCodeWrongRule;
	    txtSecCode.focus();
	}
	else if (secCode != confirmSecCode) {
	    errMsg = _page.alertErrorSecCodeConfirm;
	    txtConfirmSecCode.focus();
	}
	else if (oldSecCode != null && oldSecCode === secCode) {
		errMsg = _page.alertSameSecCode;
		txtSecCode.focus();
	}

	if (null != errMsg) {
		ageMsg.Show(errMsg);
		return;
	}

	txtSecCode.value = secCode.ec(top.ph).hc();
	txtConfirmSecCode.value = confirmSecCode.ec(top.ph).hc();
	var params = ajax.CreateParams('txtSecCode', 'txtConfirmSecCode');

	txtSecCode.value = secCode;
	txtConfirmSecCode.value = confirmSecCode;

	function OnComplete(result) {
		age.UnLock();
		var errCode = result.errCode;
		if (errCode === 0) {
			ageMsg.Show(_page.alertSecCodeSucc, true);
			if (_page.isForcedSecurityCode) {
				top.setTimeout(age.Refresh, 2000);
			}
			else if (oldSecCode != null) { // must be refresh page to make a valid random Token
				top.setTimeout(GotoSCPage, 5000);
			}
		}
		else {
			txtSecCode.focus();
			ageMsg.Show(result.errMsg);
		}
	}

	var url = 'Handlers/ChangeSecurityCode.ashx';
	if (_page.custid) {
	    url = age.GetBaseUrl() + '_AccountInfo/Handlers/ResetSecurityCode.ashx';
	    params += '&custid=' + _page.custid;
	}
	else if (_page.subaccid) {
	    url = age.GetBaseUrl() + '_SubAcc/Handlers/ResetSubAccSC.ashx';
	    params += '&subaccid=' + _page.subaccid;
	}
	else {
		if (_page.alreadyHasSecCode && oldSecCode != null) {
	        var sha1 = Sha1.hash(oldSecCode + _page.securitySalt);
	        var sha256 = Sha256.hash(sha1 + _page.securityCodeToken);
	        params += '&inputSecCode=' + sha256 + '&chkNotAskAgain=true';
	    }
    }

	// Post with return result in JSON format
	age.Lock(false);
	ajax.PostJSON(
        url,
        params,
        OnComplete
    );
}

function Reset() {
	$('txtSecCode').value = '';
	$('txtConfirmSecCode').value = '';
}

function CheckSecCode(secCodeElement, iconElementId) {
	var isSecCode = IsSecurityCode(secCodeElement.value);
	var iconElement = $(iconElementId);
	if (!iconElement) return false;

	if (isSecCode) {
		iconElement.className = 'PwdSuccess';
	}
	else {
		iconElement.className = 'PwdError';
	}

	return isSecCode;
}

function Initialize() {
	var pageMain = $('page_main');
	var div = document.createElement('DIV');
	div.style.width = '370px';
	div.style.padding = '2px';
	div.style.marginTop = '12px';
	div.innerHTML = '<pre style="padding: 4px; color:#dd0000; font-family: "Roboto",Tahoma, Arial, helvetica, sans-serif; line-height: 15px">' + _page.alertRequiredSecCode + '</pre>';

	// Show first logged message
	if (_page.isForcedSecurityCode) {
	    ageMsg.Show(_page.forceChangeSecCode);
	}
	else {
		div.innerHTML += '<hr size=1><a style="padding:4px" target="_blank" href="../../_HowToUse/Resources/HTML/SecurityCode/SecurityCode_' + _page.language + '.html">' + _page.lblLearnMore + '</a>';
	}

	$('txtSecCode').maxLength = $('txtConfirmSecCode').maxLength = 6;
	var inputSC = $('inputSecCode');
	if (inputSC) {
		inputSC.maxLength = 6;
	}
	pageMain.appendChild(div);
}

function GotoPwdPage() {
	if (top.menu && top.menu.ClearCurrentLeftMenu) {
		top.menu.ClearCurrentLeftMenu();
	}

	var pwdLink = '_MemberInfo/ChangePassword/ChangePassword.aspx';
	window.location = age.GetBaseUrl() + pwdLink;
}

function GotoSCPage() {
	if (top.menu && top.menu.ClearCurrentLeftMenu) {
		top.menu.ClearCurrentLeftMenu();
	}

	var pwdLink = '_MemberInfo/SecurityCode/ChangeSecurityCode.aspx';
	window.location = age.GetBaseUrl() + pwdLink;
}

RegisterStartUp(Initialize);
