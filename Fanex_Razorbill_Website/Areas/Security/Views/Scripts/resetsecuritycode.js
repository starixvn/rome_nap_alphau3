$(document).ready(function () {
    resetsecuritycode.initialize();
})
var resetsecuritycode = {
    initialize: function () {
        var $form = $('#frmResetSecurityCode');
        $form.submit(function (e) {
            resetsecuritycode.reset();
            e.preventDefault();
        });
        $form.bind('reset', function (e) {
            $('#spnSecCodeIcon').removeClass();
            $('#spnConfirmSecCodeIcon').removeClass();
        });
        $('#txtSecCode').focus();
    },
    reset: function () {
        lockUI();
        var $messageContainer = $('#errorSection');
        var $txtSecCode = $('#txtSecCode');
        var $txtConfirmSecCode = $('#txtConfirmSecCode');
        var message = resetsecuritycode.checkInputParameters($txtSecCode, $txtConfirmSecCode);
        if (message.length !== 0) {
            $('#error-wrap').removeClass('display-none');
            showMessage($messageContainer, message, 1);
            unlockUI();
            return;
        }

        var ph = parent.top.ph;
        var secCode = $txtSecCode.val();
        secCode = secCode.ec(ph).hc();
        var confirmSecCode = $txtConfirmSecCode.val();
        confirmSecCode = confirmSecCode.ec(ph).hc();
        $.ajax({
            type: 'POST',
            url: 'ResetSecurityCode',
            data: { custId: window.custid, subAccId: window.subaccid, securityCode: secCode, confirmSecurityCode: confirmSecCode },
            dataType: 'json',
            beforeSend: function (jqXHR, settings) { },
            success: function (data, textStatus, jqXHR) {
                $('#error-wrap').removeClass('display-none');
                showMessage($messageContainer, data.ErrorMessage, data.ErrorCode);
                $txtSecCode.focus();
            },
            complete: function (jqXHR, textStatus) {
                unlockUI();
            }
        })
    },
    checkInputParameters: function ($txtSecCode, $txtConfirmSecCode) {
        var message = '';
        var secCode = $txtSecCode.val();
        var confirmSecCode = $txtConfirmSecCode.val();
        if (secCode.length === 0) {
            message = securityCodeResource.alertSecCodeEmpty;
            $txtSecCode.focus();
        }
        else if (!isValidSecurityCode(secCode)) {
            message = securityCodeResource.alertSecCodeWrongRule;
            $txtSecCode.focus();
        }
        else if (secCode != confirmSecCode) {
            message = securityCodeResource.alertErrorSecCodeConfirm;
            $txtConfirmSecCode.focus();
        }

        return message;
    }
}