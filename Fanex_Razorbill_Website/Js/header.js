// Scroller message section ------------------
AGE.prototype.ApplyScroller = function()
{
    var text_news = $('text_news');

    this.scroller = $('scroller');
    this.topnews = $('topnews');

    this.topnews.onmouseover = function(){this.scrollerPaused = true;};
    this.topnews.onmouseout = function(){this.scrollerPaused = false;};

    this.maskWidth = parseInt(text_news.offsetWidth);
    this.scrollInterval = setInterval(AGE.Scrolling, 20);

    AGE.scroller_left = parseInt(AGE.scroller.style.left);
    AGE.scroller_width = parseInt(AGE.topnews.offsetWidth);
}

AGE.prototype.Scrolling = function()
{
    if(AGE.scrollerPaused) return;

    if(AGE.scroller_left + AGE.scroller_width < -5)
    {
        AGE.scroller_left = AGE.maskWidth + 5;
        AGE.scroller_width = parseInt(AGE.topnews.offsetWidth);
    }
    AGE.scroller.style.left = (AGE.scroller_left--) + 'px';
}

var checkOnunload;
function ReCheckin() {
    Ajax.Request("Handlers/ReCheckin.ashx",{
        asynchronous:true,
        method:'get',
        parameters:'onid='+ OnUserID,
        onComplete:ProcessData
    });
}

function ProcessData(data)
{
    eval(data.responseText);
}

function CheckLogin()
{
    setInterval(ReCheckin, 30000);
    reSelectTopMenu();
    //showIntroLetter(ShowLetter, 'divPopupLetter');
}

function ChangeLang(strLang)
{
    Ajax.Request("Handlers/SetLanguage.ashx",{
        asynchronous:true,
        method:'get',
        parameters:'lang=' + strLang,
        onComplete:UpdateSuccess
    });

    createCookie('ibclang', strLang, 30);
}

function UpdateSuccess(data)
{
    var val = data.responseText + "";
    if(val == "1")
    {
        top.menu.location = RemoveBookmarksInUrl(top.menu.location.href);
        top.frHeader.location = RemoveBookmarksInUrl(top.frHeader.location.href);
        top.main.location = RemoveBookmarksInUrl(top.main.location.href);
        top.frFooter.location = RemoveBookmarksInUrl(top.frFooter.location.href);
    }
}

function ClearActiveTab()
{
    setClassName("transfer", "");
    setClassName("balance", "");
    setClassName("changepass", "");
    setClassName("logout", "");
}

function change2Menu(url)
{
    top.menu.ClearCurrentLeftMenu();
    ClearActiveTab();
    parent.main.location.href = url;
}

function privateMsgLink(url)
{
    change2Menu(url);

    var open = "(", close = ")";
    var countText = $('privateMsgLnk').innerHTML;
    var countPriMsg = countText.substring(countText.indexOf(open) + 1, countText.indexOf(close));
    if (parseInt(countPriMsg) > 0)
    {
        updateCountPriMsg();
    }
}

function completeCountPriMsg(data) {
    var val = data.responseText + "";
    if (val == "Success")
    {
        var open = "(", noMsg = "0";
        var countText = $('privateMsgLnk').innerHTML;
        $('privateMsgLnk').innerHTML = countText.substring(0, countText.indexOf(open) + 1) + noMsg + ")";
    }
}

function updateCountPriMsg()
{
    Ajax.Request("Handlers/UpdateCountPriMsg.ashx",{
        asynchronous:true,
        method:'get',
        onComplete:completeCountPriMsg
    });
}

function setClassName(id, newValue)
{
    var el = $(id);
    if (el) el.className = newValue;
}

//for keeping top menu selected correctly
function reSelectTopMenu()
{
    if (top.main)
    {
        var mainUrl = top.main.location.href;
        var startPos = mainUrl.lastIndexOf('/');
        var endPos = mainUrl.lastIndexOf('?');
        if (endPos == -1) endPos = mainUrl.length
        var page = mainUrl.substring(startPos+1, endPos);

        ClearActiveTab();
        switch (page.toLowerCase())
        {
            case 'creditbalance.aspx':
                setClassName("balance", "current");
                break;
            case 'multipletransfer.aspx':
                setClassName("transfer", "current");
                break;
            case 'changepass.aspx':
                setClassName("changepass", "current");
                break;
        }
    }
}

// Balloon section -----------------
// Returns the string with all the beginning
// and ending whitespace removed
String.prototype.trim = function() {
  return this.replace(/^\s+/, '').replace(/\s+$/, '');
};

var timeOutID1, timeOutID2;
timeOutID1 = setTimeout(InitBalloonTip, (5*1000));
function InitBalloonTip()
{
    GetBalloonTip();
    setInterval (GetBalloonTip, (60*1000));
}

function GetBalloonTip()
{
    Ajax.Request("Handlers/GetBalloonTip.ashx",
        {   asynchronous:true,
            method:'get',
            onComplete:ProcessResponseBalloonTip,
            onFailure: function(){ return false; }
        });
}

function ProcessResponseBalloonTip(rs)
{
    var result = rs.responseText;
    if (result != "Nothing") {
        // ---------- process pageQuery value ----------
        var normal = "<--normal-->";
        var special = "<--allSpecial-->";
        var url = "Info/NewMessageForm.aspx?pageQuery=";
        var priMsg = "<--Count:";

        // update count of private messages
        if (result.indexOf(priMsg) >= 0) {
            var open = "(", close = ")";
            var groupPriMsg = result.substring(result.indexOf(priMsg));
            var countPriMsg = groupPriMsg.substring(groupPriMsg.indexOf(open) + 1, groupPriMsg.indexOf(close));

            var countText = $('privateMsgLnk').innerHTML;
            if (countText.indexOf(open) >= 0) {
                $('privateMsgLnk').innerHTML = countText.substring(0, countText.indexOf(open) + 1) + countPriMsg + ")";
            }
        }

        if (result.indexOf(special) >= 0) {
            result = result.substring(0, result.indexOf(special))
            url += special.substr(3, 10);
        }
        if (result.indexOf(normal) >= 0) {
            result = result.substring(0, result.indexOf(normal))
            url += normal.substr(3, 6);
        }
        // ---------- /process pageQuery value ----------

        var oldNews = $('topnews');
        var marqueeContent = oldNews.innerHTML.trim();
        var theDiv = AGE.createElement('div');
        theDiv.innerHTML = result.trim();

        var prefixPriMsg = ". Private message: ";
        var theDiv2 = AGE.createElement('div');
        theDiv2.innerHTML = "<font color=\"green\">. Private message: ";
        var prefixPriMsg2 = theDiv2.innerHTML;
        if ((prefixPriMsg2.indexOf("</FONT>") >= 0) || (prefixPriMsg2.indexOf("</font>") >= 0)) {
            prefixPriMsg2 = prefixPriMsg2.substring(0, prefixPriMsg2.length - 7);
        }
        var oldPub = "", oldPri = "", newPub = "", newPri = "";
        oldPub = marqueeContent;
        if (marqueeContent.indexOf(prefixPriMsg) >= 0) {
            oldPub = marqueeContent.substring(0, marqueeContent.indexOf(prefixPriMsg2));
            oldPri = marqueeContent.substr(marqueeContent.indexOf(prefixPriMsg));
            var tmpIndex = oldPri.indexOf("</font>");
            if (tmpIndex == -1) {
                tmpIndex = oldPri.indexOf("</FONT>");
            }
            oldPri = oldPri.substring(0, tmpIndex);
        }
        newPub = theDiv.innerHTML;
        if (newPub.indexOf(prefixPriMsg) >= 0) {
            newPri = newPub.substr(newPub.indexOf(prefixPriMsg));
            newPri = newPri.substring(newPri.indexOf("</font>"));
            newPub = newPub.substring(0, newPub.indexOf(prefixPriMsg));
        }
        if (newPri == "") {
            oldNews.innerHTML = newPub;
        }
        else if ((newPri != "") && (oldPri.trim() != newPri.trim())) {
            $('text_news_out').onclick = new Function("change2Menu('" + url + "')"); //Work with FF, IE6-7
            oldNews.innerHTML = newPub + "<font color=\"green\">" + newPri + "</font>";
            setContentPopUpTop(newPri.substr(prefixPriMsg.length));
            closePopUpTop(0);
            timeOutID2 = setTimeout("closePopUpTop(1)", (30 * 1000));
        }
        else {
            if ((newPub != "") && (oldPub.trim() != newPub.trim())) {
                $('text_news_out').onclick = new Function("change2Menu('" + url + "')"); //Work with FF, IE6-7
                oldNews.innerHTML = newPub + (newPri != "" ? "<font color=green>" + newPri + "</font>" : "");
                setContentPopUpTop(newPub);
                closePopUpTop(0);
                timeOutID2 = setTimeout("closePopUpTop(1)", (30 * 1000));
            }
        }
    }
}

function showHotMsg() {
    if (arrayHotMsg == null) {
        var temp = $('hdSecondMsg').value;
        if (temp != '') {
            arrayHotMsg = temp.split(";");
        }
    }
    if (arrayHotMsg != null) {
        if (countHotMsg < arrayHotMsg.length) {
            closePopUpTop(0);
            setContentPopUpTop("");
            if (countHotMsg == 0) {
                setContentPopUpTop(arrayHotMsg[countHotMsg]);
            }
            else {
                setContentPopUpTop("<blink>" + arrayHotMsg[countHotMsg] + "</blink>");
                idxBlinkText = countHotMsg;
                if (IE) {
                    timeOutIETextEffect = setInterval(ieTextEffect, 1000);
                }

                var timeOutRemoveEffect = setTimeout(removeTextEffect, (6 * 1000));
            }
            countHotMsg = countHotMsg + 1;
            var timeOutNextMsg = setTimeout(showHotMsg, (10 * 1000));
        }
    }
}

function ieTextEffect() {
    for (var idx = 0; idx < document.all.tags('blink').length; idx++) {
        var element = document.all.tags('blink')[idx];
        element.style.visibility = (element.style.visibility == '' ? 'hidden' : '');
    }
}

function removeTextEffect() {
    setContentPopUpTop(arrayHotMsg[idxBlinkText]);
    if (IE) {
        clearInterval(timeOutIETextEffect);
    }
}

var timeOutIETextEffect;
var idxBlinkText = 0;
var timeOutHotMsg = setTimeout(showHotMsg, (3 * 1000));
var countHotMsg = 0;
var arrayHotMsg = null;

//@DateTime section -------------------
function isDate(TDY, TDM, TDD) //input 3 string
{
    var BegDate = new Date(parseFloat(TDY) + "/" + TDM + "/" + TDD);
    if (BegDate.getYear() < 200)
        var TBegYear = BegDate.getYear() + 1900
    else var TBegYear = BegDate.getYear()
    if (TBegYear != parseFloat(TDY) || BegDate.getMonth() + 1 != parseFloat(TDM) || BegDate.getDate() != parseFloat(TDD))
        return false;
    else return true;
}

function clock() {
    sec++;
    if (sec == 60) {
        sec = 0;
        min++;
    }
    if (min == 60) {
        min = 0;
        hrs++;
    }
    if (hrs == 24) {
        hrs = 0;
        sec = 0;
        min = 0;
        day++;
    }
    if (min < 10) {
        strmin = "0" + min;
    }
    else {
        strmin = min;
    }
    if (sec < 10) {
        strsec = "0" + sec;
    }
    else {
        strsec = sec;
    }

    if (!isDate(year, month, day))    //to next month
    {
        month++;
        day = 1;
    }
    if (!isDate(year, month, day))    //to next year
    {
        year++;
        month = 1;
        day = 1;
    }

    if (hrs >= 12) {
        stra = PM;
    }
    else {
        stra = AM;
    }
    if (hrs >= 12) {
        strhrs = hrs - 12;
    }
    else {
        strhrs = hrs;
    }
    var str = strhrs + ":" + strmin + ":" + strsec + " " + stra + " " + strMonth[month - 1] + " " + day + ", " + year + " GMT-4";
    $("clock").innerHTML = str;

    setTimeout("clock()", 1000);
}

function closePopUpTop(close)
{
    if (close == 1)
    {
        $("Popup_top").style.display = "none";
    }
    else
    {
        $("Popup_top").style.display = "block";
    }
    return false;
}

function setContentPopUpTop(content)
{
    $("Popup_contents").innerHTML = content;
}

function setCurrentLanguageValue()
{
    $('selectLanguage').value = currentLanguage;
}

// Start up section
if(typeof AGE != 'undefined') AGE = null; // free memory
AGE = new AGE();
AGE.addEvent(window, 'load', function() {
    setCurrentLanguageValue();
    AGE.ApplyScroller();
    clock();
    CheckLogin();
},
    true
);