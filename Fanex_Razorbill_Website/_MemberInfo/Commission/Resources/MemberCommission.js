
function ShowInfoCommissionSportsbook() {
    var popH = 230, popW = 600;
    var url = 'InfoCommission.aspx?isSportsbook=true';

    ageWnd.open(url, '', 150, 55, popW, popH, '', 'MemberCommissionInfoTitle', 'MemberCommissionInfoCloseButton');
}

function ShowInfoCommission() {
    var popH = 70, popW = 300;
    var url = 'InfoCommission.aspx?isSportsbook=false';

    ageWnd.open(url, '', 320, 55, popW, popH, '', 'MemberCommissionInfoTitle', 'MemberCommissionInfoCloseButton');
}

document.onmousedown = function (event) {
    if (ageWnd.createdElement) {
        ageWnd.close();
    }
};