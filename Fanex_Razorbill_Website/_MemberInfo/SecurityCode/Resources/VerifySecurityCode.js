/*
* Created 20130422@kan: Javascript functions for change security code
* Revision ?@? - ... 
* 
*/
function VerifySecCode() {
	// Validate sec code
	var txtSecCode = $('inputSecCode');
	var errMsg = null;

	var secCode = txtSecCode.value.trim();
	if (secCode.length === 0) {
		errMsg = _page.alertSecCodeEmpty;
		txtSecCode.focus();
	}
	else if (!IsSecurityCode(secCode)) {
		errMsg = _page.alertSecCodeWrongRule;
		txtSecCode.focus();
	}

	if (null != errMsg) {
		ageMsg.Show(errMsg);
		return;
	}

	var sha1 = Sha1.hash(secCode + _page.securitySalt);
	var sha256 = Sha256.hash(sha1 + _page.securityCodeToken);

	txtSecCode.value = sha256;
	var params = ajax.CreateParams('inputSecCode');

	txtSecCode.value = secCode;
	function OnComplete(result) {
		age.UnLock();
		var errCode = result.errCode;
		if (errCode === 0) {
			ageMsg.Show(_page.alertSecCodeSucc, true);
			top.setTimeout(age.Refresh, 2000);
		}
		else {
			txtSecCode.focus();
			ageMsg.Show(result.errMsg);
		}
	}

	// Post with return result in JSON format
	age.Lock(false);
	ajax.PostJSON(
		'Handlers/VerifySecurityCode.ashx',
		params,
		OnComplete
    );
}

function Reset() {
	$('inputSecCode').value = '';
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

	$('inputSecCode').maxLength = 6;
	pageMain.appendChild(div);
}

RegisterStartUp(Initialize);