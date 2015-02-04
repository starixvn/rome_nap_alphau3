<%@ Page Language="C#" AutoEventWireup="false" EnableSessionState="True" CodeFile="Index.aspx.cs"
    Inherits="Index" %>

<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <script type="text/javascript">
        function $(id) {
            return document.getElementById(id);
        }

        function setURL() {
            var url = '';
            try {
                url = top.main.location;
            } catch (e) {
            }

            var start = url.toString().lastIndexOf(')/') + 2;
            var currentURL = url.toString().substring(start);

            if (currentURL == null) return;
            setCookie('<%=CookieCurrentURLKey %>', currentURL, 1);
        }

        var url = window.location.href;
        var start = url.indexOf('//');
        var end = url.indexOf('/(');

        window.document.title = url.substr(start + 2, end - start - 2);
        window.ph = '<%= Session["ph"] %>';

        // For caching in child frames
        window.cachePage = new Array();
        window.cacheData = new Array();

        // For showing message in child frames
        window.errCodes = new Array();
        window.errMsgs = new Array();
        window.userNames = new Array();

    </script>
    <link href="../_MemberInfo/SetNickName/Resources/SetNickName.css?<%=TplCore.TplSettings.UrlVersion %>"
        rel="stylesheet" type="text/css" />
    <link href="../_Announcement/Resources/Announcement.css?<%=TplCore.TplSettings.UrlVersion %>"
        rel="stylesheet" type="text/css" />
    <link href="../_GlobalResources/css/tooltip.css?<%=TplCore.TplSettings.UrlVersion %>"
        rel="stylesheet" type="text/css" />
    <link href="../_Components/AGEWnd/AGEWnd.css?<%=TplCore.TplSettings.UrlVersion %>"
        rel="stylesheet" type="text/css" />
</head>
<body scroll='no'>
    <form id="agentSite" runat="server" style="height: 100%; width: 100%;">
    <table border="0" cellpadding="0" cellspacing="0" class="content">
        <tr>
            <td class="mainpage" valign="top">
                <iframe src="<%=mainPage%>" onload="setURL();" frameborder="0" id="main" scrolling="auto"
                    name="main" width="100%" height="100%" marginwidth="0" marginheight="0" allowtransparency="yes">
                </iframe>
            </td>
        </tr>
        <tr>
            <td class="footer">
                <iframe src="../Footer.html" frameborder="0" width="100%" scrolling="no" height="22px"
                    id="frFooter" name="frFooter"></iframe>
            </td>
        </tr>
    </table>
    </form>
</body>
<script src="../_Components/AGEWnd/AGEWnd.js?<%=TplCore.TplSettings.UrlVersion %>"
    type="text/javascript"></script>
<script src="../_GlobalResources/Js/Core.js?<%=TplCore.TplSettings.UrlVersion %>"
    type="text/javascript"></script>
<script src="../_MemberInfo/SetNickName/Resources/SetNickName.js?<%=TplCore.TplSettings.UrlVersion %>"
    type="text/javascript"></script>
<script src="../_Announcement/Resources/Announcement.js?<%=TplCore.TplSettings.UrlVersion %>"
    type="text/javascript"></script>
<script type="text/javascript">
    AGE.PERMISSIONS = '<%= (string)(Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess) %>';
    AGE.prototype._SignOut = AGE.prototype.SignOut;
    // SignOut
    AGE.prototype.SignOut = function () {
        setCookie('<%=CookieCurrentURLKey %>', '', -1);
        this._SignOut();
    }
    AGE.prototype.OpenPopUp = function (url) {
        var popH = 360, popW = 480;
        ageWnd.open(url, '', 200, 200, popW, popH);
    }
    AGE.prototype.ClosePopUp = function () {
        ageWnd.close()
    }
    
</script>
</html>
