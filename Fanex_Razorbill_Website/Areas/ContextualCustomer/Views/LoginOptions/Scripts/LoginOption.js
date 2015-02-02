var loginOption = {
    init: function () {
        $('#textNickName').keypress(function (e) {
            loginOption.processRequestWhenPressingEnter(e);
        });
    },
    validateNickname: function () {
        var nickname = $('#textNickName').val();
        var isLoginByNicknameOnly = $('#chkNickName').prop('checked');
        var alphaExp = /^[0-9a-zA-Z]+$/;

        if (nickname.match(alphaExp) && (nickname.length >= 5) && (nickname.length < 16)) {
            return true;
        }
        else {
            return false;
        }
    },
    updatingLoginOption: function () {
        if (_isInternal == 'True' || _isSubAcc == 'True') {
            ShowMsgs(resources._notAuthorized, false);

            return false;
        }

        var validateNickname = this.validateNickname();

        if (!validateNickname) {
            ShowMsgs(msgNicknameValid, false);
            $('#textNickName').focus();
            return false;
        }

        $('#loading').css({ display: '' });
        var params = "{'nickname':'" + $('#textNickName').val() + "','isLoginByNicknameOnly':" + $('#chkNickName').prop('checked') + "}";

        $.ajax({
            type: "POST",
            url: window.rootUrl + "ContextualCustomer/LoginOption/UpdatingLoginOption",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: params,
            success: function (rs) {
                if (rs) {
                    $('#loading').css({ display: 'none' });
                    if (rs.ErrorCode != 0) {
                        $('#textNickName').focus();
                        ShowMsgs(rs.ErrorMessage, false);
                    }
                    else {
                        ShowMsgs(rs.ErrorMessage, true);
                        setTimeout(function () { loginOption.refeshInfoAfterUpdateSuccessfull(); }, 2000);
                    }
                }
            },
            error: function (event, jqXHR, ajaxSettings) {
            },
            complete: function () {
            }
        });
    },
    processRequestWhenPressingEnter: function (event) {
        if ((event.which == 13) || (event.keyCode == 13)) {
            this.updatingLoginOption();
        }
    },
    refeshInfoAfterUpdateSuccessfull: function (event) {
        $('#msgError').empty().removeClass('success error warning');
        if (top.frHeader != undefined) {

            top.frHeader.$('iconSetNickname').style.display = "";
            top.frHeader.$('labelNickname').innerHTML = $('#textNickName').val().toUpperCase();
            top.frHeader.$('linkNickNameID').className = "linkNickName";
            if (top.frHeader.$('spanNickNameID') != null) top.frHeader.$('spanNickNameID').innerHTML = "";
            location.reload();
        }
    }
};

$(document).ready(function () {
    loginOption.init();
});