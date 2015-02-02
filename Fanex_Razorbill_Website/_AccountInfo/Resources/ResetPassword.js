/*
* Created 20120727@Terry, Lee - Javascript functions for reset password
* Revision ?@? - ... 
* 
*/

function ChangePassword() {
    // Validate password
    var txtPwd = $('password');
    var txtConfirmPwd = $('confirmPassword');

    var errMsg = null;

    if (txtPwd.value.length == 0) {
        errMsg = _page.errPasswordEmpty;
        AddPopupHeight(70);
        txtPwd.focus();
    }
    else if (txtPwd.value != txtConfirmPwd.value) {
        errMsg = _page.errConfirm;
        AddPopupHeight(70);
        txtConfirmPwd.focus();
    }
    else {
        errMsg = ValidatePassword(txtPwd.value);
        txtPwd.focus();
    }

    if (null != errMsg) {
        ageMsg.Show(errMsg);
        AddPopupHeight(70);
        return;
    }

    var params = ajax.CreateParams();
    params += '&password=' + txtPwd.value.ec(top.ph).hc();
    params += '&custId=' + _page.custId;
    params += '&username=' + _page.username;
    params += '&isSyncCasino=' + _page.isSyncCasino;
    params += '&role=' + _page.role;

    function OnComplete(result) {
        age.UnLock();
        var errCode = result.errCode;
        AddPopupHeight(70);
        if (errCode == 0) {
            ageMsg.Show(_page.infoSuccess, true);
            age.ReloadPage(500);
        }
        else {           
            txtPwd.focus();
            ageMsg.Show(result.errMsg);
        }
    }

    // Post with return result in JSON format
    age.Lock(false);
    ajax.PostJSON(
        'Handlers/ResetPassword.ashx',
        params,
        OnComplete
    );
}

function Reset() {
    $('password').value = '';
    $('confirmPassword').value = '';
    $('password').focus();
}

// Check pass call in keyup event
function CheckPass(pwdElement, iconElementId) {
    var isPwd = _page.role == 1 ? IsMemberPassword(pwdElement.value) : IsPassword(pwdElement.value);
    var iconElement = $(iconElementId);

    if (!iconElement) return false;

    if (isPwd) {
        iconElement.className = 'PwdSuccess';
    }
    else {
        iconElement.className = 'PwdError';
    }

    return isPwd;
}

function ValidatePassword(str) {
    if (str.length == 0) return _page.errPasswordEmpty;
    else if (!IsMemberPassword(str) && _page.role == 1) return _page.errWrongPasswordRule;
    else if (!IsPassword(str) && _page.role > 1) return _page.errWrongPasswordRule;
    else if (str == _page.username) return _page.pwdDAccountName;

    return null;
}


function ChangePasswordOnLoad() {
    if (_page.isPopup == true) {
        $('header_main').style.display = 'none';
    }

    var page_popup = $('page_popup');
    var div = document.createElement('DIV');
    div.style.width = '350px';
    div.style.padding = '2px';
    div.style.marginTop = '12px';
    div.innerHTML = '<pre style="padding: 4px; color:#dd0000; white-space: pre-wrap; font-family: "Roboto",Tahoma, Arial, helvetica, sans-serif; line-height: 15px">' + _page.errPasswordRequire + '</pre>';

    page_popup.appendChild(div);

    $('password').maxLength = $('confirmPassword').maxLength = _page.role == 1 ? 10 : 16;
}

RegisterStartUp(ChangePasswordOnLoad);