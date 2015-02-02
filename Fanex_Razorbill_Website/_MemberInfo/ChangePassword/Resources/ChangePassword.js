/*
* Created 20100226@Terry, Lee - Javascript functions for change password
* Revision ?@? - ... 
* 
*/
function ChangePassword() {
    // Validate password
    var txtOldPwd = $('txtOldPwd');
    var txtNewPwd = $('txtNewPwd');
    var txtConfirmPwd = $('txtConfirmPwd');

    var errMsg = null;

    if (txtOldPwd.value.length == 0) {
        errMsg = _page.alertInputOldPwd;
        txtOldPwd.focus();
    }
    else if (txtNewPwd.value != txtConfirmPwd.value) {
        errMsg = _page.alertErrorConfirm;
        txtNewPwd.focus();
    }
    else {
        errMsg = ValidatePassword(txtNewPwd.value);
        txtNewPwd.focus();
    }

    if (null != errMsg) {
        ageMsg.Show(errMsg);
        return;
    }

    var txtOldPwd_b = txtOldPwd.value;
    var txtNewPwd_b = txtNewPwd.value;
    var txtConfirmPwd_b = txtConfirmPwd.value;

    txtOldPwd.value = txtOldPwd.value.ec(top.ph).hc();
    txtNewPwd.value = txtNewPwd.value.ec(top.ph).hc();
    txtConfirmPwd.value = txtConfirmPwd.value.ec(top.ph).hc();

    var params = ajax.CreateParams('txtNewPwd', 'txtOldPwd', 'txtConfirmPwd');

    txtOldPwd.value = txtOldPwd_b;
    txtNewPwd.value = txtNewPwd_b;
    txtConfirmPwd.value = txtConfirmPwd_b;

    function OnComplete(result) {
        age.UnLock();
        var errCode = result.errCode;
        if (errCode == 0) {
            ageMsg.Show(_page.alertPasswordSucc, true);
            if (_page.firstLogged || _page.pwdExpired) {
                top.setTimeout(age.Refresh, 2000);
            }
        }
        else {
            txtOldPwd.focus();
            ageMsg.Show(result.errMsg);
        }
    }

    // Post with return result in JSON format
    age.Lock(false);
    ajax.PostJSON(
        'Handlers/ChangePassword.ashx',
        params,
        OnComplete
    );
}

function Reset() {
    $('txtOldPwd').value = '';
    $('txtNewPwd').value = '';
    $('txtConfirmPwd').value = '';
}

// Check pass call in keyup event
function CheckPass(pwdElement, iconElementId) {
    var isPwd = IsPassword(pwdElement.value);
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
    if (str.length == 0) return _page.alertPasswordEmpty;
    else if (!IsPassword(str)) return _page.alertWrongPasswordRule;
    else if ($('txtOldPwd').value == $('txtNewPwd').value) return _page.pwdOldHint;

    return null;
}

function ChangePasswordOnLoad() {
    // Set active tab
    if (top.frHeader && top.frHeader.SelectTopMenu) {
        top.frHeader.SelectTopMenu('ChangePassword.aspx');
    }

    var pageMain = $('page_main');
    var div = document.createElement('DIV');
    div.style.width = '370px';
    div.style.padding = '2px';
    div.style.marginTop = '12px';
    div.innerHTML = '<pre style="padding: 4px; color:#dd0000; font-family: "Roboto",Tahoma, Arial, helvetica, sans-serif; line-height: 15px">' + _page.alertPasswordRequire + '</pre>';

    // Show first logged message
    if (_page.firstLogged) {
        ageMsg.Show(_page.forcechangepwd);
    }
    else if (_page.pwdExpired) {
        div.innerHTML += _page.pwdHint;
        ageMsg.Show(_page.pwdExpiredMsg);
    }

    $('txtOldPwd').maxLength = $('txtNewPwd').maxLength = $('txtConfirmPwd').maxLength = 16;
    pageMain.appendChild(div);
}

function GotoSCPage() {
    if (top.menu && top.menu.ClearCurrentLeftMenu) {
        top.menu.ClearCurrentLeftMenu();
    }

    var secCodeLink = !_page.isNewSC
        ? '_MemberInfo/SecurityCode/ChangeSecurityCode.aspx'
        : 'Security/SecurityCode/ChangeSecurityCode';
    window.location = age.GetBaseUrl() + secCodeLink;
}

RegisterStartUp(ChangePasswordOnLoad);