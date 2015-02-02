/*
* Created 20130416@kan: Javascript functions for change security code
* Revision ?@? - ... 
* 
*/
function SubmitSecCode() {
	// Validate sec code
	var txtSecCode = $('inputSecCode');
	var errMsg = null;

	var secCode = txtSecCode.value.trim();
	if (secCode.length === 0) {
		errMsg = _page.alertSecCodeEmpty;
		txtSecCode.focus();
	}
	
	if (null != errMsg) {
		ageMsg.Show(errMsg);
		return;
	}

	var sha1 = Sha1.hash(secCode + _page.securitySalt);
	var sha256 = Sha256.hash(sha1 + _page.securityCodeToken);
	function OnComplete(result) {
		var errCode = result.errCode;
		if (errCode === 9009) { // wrong sc
			txtSecCode.focus();
			ageMsg.Show(result.errMsg);
		}
		else if (errCode === 2) { // account is closed
			alert(result.errMsg);
			top.age.SignOut();
		}
		else {
			top.ageWnd.onClosing = function () {
				top.age.UnLock(); return true;
			};
			top.ageWnd.close();
			top[_page.url].callback(result);
			top[_page.url] = null;
		}
	}

	var chkNotAskAgain = $('chkNotAskAgain').checked;

    // Post with return result in JSON format
	ajax.PostJSON(
		unescape(_page.url),
		top[_page.url].param + '&inputSecCode=' + sha256 + '&chkNotAskAgain=' + chkNotAskAgain,
		OnComplete
    );
}

function Reset() {
	$('inputSecCode').value = '';
}

function NotAskAgainClick() {
	var chkNotAskAgain = $('chkNotAskAgain');
	chkNotAskAgain.checked = !chkNotAskAgain.checked;
}

RegisterStartUp(function () {
    $('inputSecCode').maxLength = 6;
    ageMsg.Show(_page.informSecCode);
});
