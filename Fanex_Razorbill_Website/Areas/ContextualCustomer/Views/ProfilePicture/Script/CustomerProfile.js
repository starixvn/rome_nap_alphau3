var customerProfle = {
    HideTooltipIntroNickname: function () {
        if ($('tooltip_box') != null) {
            var element = $('tooltip_box');
            element.parentNode.removeChild(element);
        }
    },

    HideProfileTooltip: function () {
        if ($('profileTooltip') != null) {
            var element = $('profileTooltip');
            element.parentNode.removeChild(element);
        }
    },

    ShowTooltipProfilePicture: function () {
        if ($('profileTooltip') != null) {
            $('profileTooltip').style.display = '';
            setTimeout(function () { customerProfle.HideProfileTooltip(); }, 30000);
        }
    },

    ClickToHide: function () {
        if ($('tooltip_box') != null) {
            customerProfle.HideTooltipIntroNickname();
            customerProfle.ShowTooltipProfilePicture();
        }
        else {
            if ($('profileTooltip') != null) {
                customerProfle.HideProfileTooltip();
            }
        }
    },

    loadCustomerProfile: function (url) {
        var xmlhttp;

        if (window.XMLHttpRequest) {
            // code for IE7+, Firefox, Chrome, Opera, Safari
            xmlhttp = new XMLHttpRequest();
        } else {
            // code for IE6, IE5
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        }

        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                document.getElementById("logo").innerHTML = xmlhttp.responseText;

                var numerousFrames = window.parent.frames.length;

                for (var i = 0; i < numerousFrames; i++) {
                    window.parent.frames[i].window.document.onclick = function () {
                        customerProfle.ClickToHide();
                    }
                }

                if ($('tooltip_box') != null) {
                    setTimeout(function () {
                        customerProfle.HideTooltipIntroNickname();
                        customerProfle.ShowTooltipProfilePicture();
                    }, 10000);
                }
                else {
                    customerProfle.ShowTooltipProfilePicture();
                }
            }
        }

        xmlhttp.open("POST", url, true);
        xmlhttp.send();
    }
}