/*
* Created 20100203@Lee - Left menu page functions
* Revision ?@? - ... 
* 
*/


var currentMenuItem = null;
function ClearCurrentLeftMenu() {
    if (currentMenuItem != null) {
        currentMenuItem.className = 'Bleft_Sub';
    }
}

function MenuItemSelect(index) {
    ClearCurrentLeftMenu();

    // Clear header tab
    if (top.frHeader) {
        if (top.frHeader.ClearActiveTab) {
            top.frHeader.ClearActiveTab();
        }
    }
    
    var menuItem = $('menuitem' + index);
    if(!menuItem) return;
    menuItem.className = 'Bleft_Sub_2';
    currentMenuItem = menuItem;
}

function MenuToggle(index) {
    var subMenu = $('div' + index);
    subMenu.style.display = (subMenu.style.display == 'none') ? 'block' : 'none';
    var aMenu = $('a' + index);
    aMenu.className = (aMenu.className == 'Bleft_Parent') ? 'Bleft_ParentAc' : 'Bleft_Parent';
}

function clearBackground() {
    document.body.style.background = "#f2f4f6 url(../_GlobalResources/Images/bg_conts.jpg) repeat-x top";
}

function StopBlinkAccInfoTab() {

}

function ShowFlashAccInfoMaster() {

}

function OpenPage(url, width, height) {
    var wnd = window.open(age.GetBaseUrl() + url, 'flashuserguide', 'height=' + height + ',width=' + width + ',menubar=no,status=no,scrollbars=yes,resizable=yes');
    if (typeof window.focus != 'undefined' && wnd) wnd.focus();
    return false;
}

function AddIconNew() {
    var imageNew = "&nbsp;&nbsp;<img border='0' src='Resources/Images/new3.gif' />";
    if ($("menu3")) $("menu3").innerHTML += imageNew;
}

function iniMenu() {
    AddIconNew();

    if (_page.clearContent) {
        clearBackground();
    }
}


RegisterStartUp("iniMenu()");