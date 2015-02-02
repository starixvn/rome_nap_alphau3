'use strict';
/* global rootUrl, ageWnd */
var handhistoryManager = (function ($) {
    function init() {
        $('#tblHandHistoryResult').find('.hand').click(function () {
            var $this = $(this);
            var width = parseInt($this.data('width'));
            var url = rootUrl + $this.data('url');
            if (ageWnd) {
                ageWnd.open(url, '', 50, 100, width, 600);
            }
        });

        $('#chkResultsWithPics').click(function () {
            var checked = $(this).prop('checked');
            var url = location.href;
            var index = url.indexOf('&showPic=');
            if (index !== -1) {
                location.href = url.substr(0, index);
            }
            else {
                location.href += '&showPic=' + checked;
            }
        });
    }

    return {
        init: function () { init(); }
    };
})(jQuery);

handhistoryManager.init();