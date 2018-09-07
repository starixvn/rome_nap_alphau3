<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Http" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.SessionState" %>
<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Sunplus.Agent.Utilities" %>
<%@ Import Namespace="Sunplus.Agent.Model.Entities" %>
<%@ Import Namespace="Sunplus.Agent.Model.BL.Authorization" %>
<%@ Import Namespace="CaptchaMvc.Infrastructure" %>
<%@ Import Namespace="CaptchaMvc.Interface" %>
<%@ Import Namespace="Fanex.BetList.Core" %>
<%@ Import Namespace="Fanex.BetList.Core.RemotingRef" %>
<%@ Import Namespace="Fanex.Result.Core.Base" %>
<%@ Import Namespace="Fanex.Result.Core.RemotingRef" %>
<script RunAt="server">

    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        routes.MapRoute(
            "Default1", // Route name
            "{controller}/{action}/{id}", // URL with parameters
            new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
    }
    void Application_Start(object sender, EventArgs e)
    {
        Sunplus.Agent.Configurations.AppSettings.LoadComplete += new Sunplus.Agent.Configurations.AppSettings.UpdateEventHandler(AppSettings_LoadComplete);
        Sunplus.Agent.Configurations.AppSettings.Configuration.Load();
        // Code that runs on application startup
        Fanex.Data.Configuration.DbSettingProviderManager.Current.Start();
        AreaRegistration.RegisterAllAreas();
        RegisterRoutes(RouteTable.Routes);
        RegisterGlobalFilters(GlobalFilters.Filters);
        Fanex.RemotingRef.RefDataTypeContainer
            .ConnectTo(new Uri(ConfigurationManager.AppSettings["Fanex.RemotingRef.Service"]))
            .FromApplication("Alpha U3")
            .SupportOnlyDefaultLanguage();
        BetListManager.RegisterRemotingRef(new Fanex.BetList.Core.RemotingRef.CacheRef());
        ResultCoreManager.RegisterRefDataHelper(new RefDataHelper());

    }



    private static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
        filters.Add(new HandleErrorAttribute());
    }

    void AppSettings_LoadComplete(object sender, bool error)
    {
        if (error) return;

        string[] keys = Sunplus.Agent.Configurations.AppSettings.Configuration.GetKeys();
        foreach (string key in keys)
        {
            ConfigurationManager.AppSettings[key] = Sunplus.Agent.Configurations.AppSettings.Configuration.Get(key);
        }

        // Domains.ini
        Sunplus.Agent.Configurations.IConfiguration domainConfiguration = Sunplus.Agent.Configurations.Domains.Configuration;
        string[] domainKeys = domainConfiguration.GetKeys();
        foreach (string key in domainKeys)
        {
            ConfigurationManager.AppSettings[key] = domainConfiguration.Get(key);
        }

        TplCore.TplSettings.UrlVersion = Sunplus.Agent.Configurations.AppSettings.Configuration.Get("JsVersion");
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        Exception ex = Server.GetLastError().GetBaseException();
        if (ex is HttpException)
        {
            if (((HttpException)ex).GetHttpCode() == 404)
            {
                Response.Redirect("~/Index.aspx");
            }

            return;
        }

        // hide sensitive information in POST or GET parameters
        string formString = string.Empty, queryString = string.Empty;
        if (Request.Form.Count > 0)
        {
            formString = Request.Form.ToString();
            formString = Common.ReplaceSensitiveInformation(formString, Globals.SensitiveParameters);
        }

        if (Request.QueryString.Count > 0)
        {
            queryString = Request.QueryString.ToString();
            queryString = Common.ReplaceSensitiveInformation(queryString, Globals.SensitiveParameters);
        }

        string message = ex.Message +
                        "\r\nSOURCE: " + ex.Source +
                        "\r\nDOMAIN: " + HttpContext.Current.Request.Headers["Host"] +
                         "\r\nREFERRER: " + HttpContext.Current.Request.UrlReferrer +
                        "\r\nFORM: " + formString +
                        "\r\nQUERYSTRING: " + queryString +
                        "\r\nTARGETSITE: " + ex.TargetSite +
                        "\r\nSTACKTRACE: " + ex.StackTrace;

        if (HttpContext.Current.Handler is IRequiresSessionState)
        {
            IUserSession session = UserSession.Current;
            message += "\r\nCUSTID:" + session.CustId;
        }

        Logs.Write(message);

        if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.LocalTest)
        {
            Server.ClearError();
            Server.Transfer("~/Error.aspx");
        }
    }



    void Application_AcquireRequestState(object sender, EventArgs e)
    {
        if (!(HttpContext.Current.Handler is IRequiresSessionState)) return;

        // Validate request & session
        string executeFileName = Request.AppRelativeCurrentExecutionFilePath;
        bool isNewSession = executeFileName.EndsWith("SignIn.aspx")
            || executeFileName.EndsWith("ProcessAdminLogin.ashx")
            || executeFileName.EndsWith("ProcessAfterLogin.ashx")
            || executeFileName.EndsWith("Captcha.ashx")
            || executeFileName.EndsWith("ChartImg.axd");
        ;

        if (UserSession.Current.CustId == 0 && !isNewSession)
        {
            Response.Redirect("~/_Authorization/SignIn.aspx", true);
            return;
        }

        // Validate url and request parameters
        int custId = 0; long transId = 0;
        int masterId = 0;

        bool isDataWarehouse;
        string url;
        GetRequestParams(ref custId, ref transId, ref masterId, out isDataWarehouse, out url);

        AuthorizationBO authorization = new AuthorizationBO(Session.SessionID);

        // For master level: MemberCommission.aspx
        bool isValidMasterId = true;
        if (!isValidMasterId || !authorization.AuthorizeUrl(custId, transId, isDataWarehouse, url))
        {
            string errUrl = (Sunplus.Agent.Model.BL.Definitions.AppSettings.LocalTest) ? "~/AccessNoData.aspx" : "~/_Authorization/SignIn.aspx";
            Server.Transfer(errUrl);
        }
    }

    private void GetRequestParams(ref int custId, ref long transId, ref int masterId, out bool isDataWarehouse, out string url)
    {
        int.TryParse(Request["custid"], out custId);
        if (custId == 0) int.TryParse(Request["cid"], out custId);
        if (custId == 0) int.TryParse(Request["recommend"], out custId);
        if (custId == 0) int.TryParse(Request["mrecommend"], out custId);
        if (custId == 0) int.TryParse(Request["idagent"], out custId);
        long.TryParse(Request["transid"], out transId);

        if (transId == 0) long.TryParse(Request["refno"], out transId);
        int.TryParse(Request["idmaster"], out masterId);

        isDataWarehouse = (Request["iswhcheck"] == "1");

        // Get file path (remove /)
        url = Request.Path.Remove(0, 1);
        url = url.ToLower();
    }



    void Session_Start(object sender, EventArgs e)
    {

    }

    void Session_End(object sender, EventArgs e)
    {

    }
  

</script>
