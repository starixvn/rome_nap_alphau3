<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._Authorization.ProcessAfterLogin_Handler" %>

/*
 * Created 20100223@Lee - Process after login (convert from code of Don).
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.UI._Authorization
{
    using System;
    using System.Web;
    using System.Collections;
    using System.Web.Security;
    using RSACryptoPad;

    using Fanex.Age.UI;

    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Utilities;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Services;

    public class ProcessAfterLogin_Handler : Fanex.Age.UI.TplHandler
    {
        const int dwKeySize = 1024;
        private void RedirectToBeforeLoginPage()
        {
            Session.Abandon();

            // Redirect to before login page
            string loginUrl = Sunplus.Agent.Model.BL.Definitions.AppSettings.BeforeLoginUrl;

            Response.Redirect(loginUrl, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void RedirectToDefaultPage()
        {
            Request.Cookies.Remove(CookieCurrentURLKey);
            FormsAuthentication.RedirectFromLoginPage(CurrentUserSession.UserName, false);
        }

        public string CookieCurrentURLKey
        {
            get
            {
                string name = string.Join("_", new string[] { 
                   CurrentUserSession.CustId.ToString(), 
                   CurrentUserSession.SubAccId.ToString(),
                   "URL"
                });

                return Sunplus.Agent.Utilities.Encryption.PearsonEncode(name);
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            Response.Cache.SetNoStore();

            string keyPath = Server.MapPath("../../IbcKeys/ibckey.snk");

            // Get encrypted parameters: custid, subAccId, login time, passwordSeed, nick name, isNickName, __utms, language, no cross cookie
            string parameter = Request["params"] + string.Empty;
            parameter = parameter.Replace(' ', '+');
            EncryptionThread encryption = new EncryptionThread();

            try
            {
                parameter = encryption.DecryptString(parameter, dwKeySize, keyPath);
            }
            catch
            {
                parameter = null;
                Logs.WriteWithFullFormat(new Exception("Cannot verify request key " + Request.RawUrl));
            }

            if (null == parameter)
            {
                RedirectToBeforeLoginPage();
                return;
            }

            string[] ps = parameter.Split('&');
            p.Add("parameter", ps);
        }


        int custId, subAccId;
        DateTime loginTime;
        string language, password, nickname;
        bool isnickname;


        // Under maintenance checking
        static object _locker = new object();
        static DateTime _lastCheckUM = DateTime.Now;
        static bool _isUM = false;


        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            string startTime, endTime;

            // Double check and lock
            if (DateTime.Now.Subtract(_lastCheckUM) > TimeSpan.FromSeconds(5)) // Check down time each 5 seconds
            {
                lock (_locker)
                {
                    if (DateTime.Now.Subtract(_lastCheckUM) > TimeSpan.FromSeconds(5))
                    {
                        // Check under maintenance time
                        _isUM = MaintenanceService.GetDownTime(out startTime, out endTime);
                        _lastCheckUM = DateTime.Now;
                    }
                }
            }

            if (_isUM)
            {
                Logs.Write("Under maintenance.");
                RedirectToBeforeLoginPage();
                return new Result(ErrorType.AccessDenied, Resources.Lang.undermaintenance);
            }

            string[] ps = p["parameter"] as string[];

            long timeTicks;

            // Verify customer information
            if (ps == null 
                || ps.Length < 9 
                || !int.TryParse(ps[0], out custId) 
                || !int.TryParse(ps[1], out subAccId) 
                || !long.TryParse(ps[2], out timeTicks)
                )
            {
                Logs.WriteWithFullFormat(new Exception("Verify customer information failed." + (ps == null ? string.Empty : String.Join(";", ps))));
                RedirectToBeforeLoginPage();
                return new Result(ErrorType.AccessDenied, Resources.Lang.strNotAccessPage);
            }

            // Verify cross cookie ticket
            bool isVerifiedTicket = true;

            //try
            //{
            //    HttpCookie cookie = Request.Cookies.Get("__utms");
            //    if ((cookie != null && 
            //        cookie.Value != Encryption.CfsEnCode((custId + loginTime.Minute).ToString())) || 
            //        (cookie == null && ps[8] == "False"))
            //    {
            //        isVerifiedTicket = false;
            //    }
            //    else
            //    {
            //        isVerifiedTicket = true;
            //    }
            //}
            //catch {
            //    isVerifiedTicket = false;
            //    Logs.Write("Verify customer information failed: " + ps[0] + ";" + ps[1]);
            //}

            if (!isVerifiedTicket)
            {
                RedirectToBeforeLoginPage();
                return new Result(ErrorType.AccessDenied, Resources.Lang.strNotAccessPage);
            }

            language = ps[7] as string;
            nickname = ps[4];
            bool.TryParse(ps[5], out isnickname);
            loginTime = new DateTime(timeTicks);
            password = RC4(loginTime.Minute.ToString(), hexDecode(ps[3]));
            if (Validation.CheckValidLanguage(language))
            {
                CurrentUserSession.Language = language;
                CurrentUserSession.LangKey = Sunplus.Agent.Model.Common.Language.LangKeyFromCulture(language);
            }
            loginTime = new DateTime(timeTicks);

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AuthorizationBO bo = new AuthorizationBO(CurrentUserSession.NetSessionId);

            int errCode = 0;
            string errMsg = null;

            IUserSession session = CurrentUserSession;
            //Get Browser
            Hashtable requestInfo = Sunplus.Agent.Model.Services.DeviceService.GetRequestInfo(Request);

            try
            {
                bool isSuccess = bo.AfterSignIn(custId, password, subAccId, nickname, isnickname, loginTime, requestInfo, out errCode, out errMsg, ref session);
                if (!isSuccess)
                {
                    RedirectToBeforeLoginPage();
                }
                else
                {

                    if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.LocalTest)
                    {
                        string[] domains = Sunplus.Agent.Model.BL.Definitions.AppSettings.CookielessDomainCollection;
                        TplCore.TplSettings.SetCookielessDomains(domains);
                    }

                    RedirectToDefaultPage();
                }
            }
            catch (Exception e)
            {
                Logs.WriteWithFullFormat(new Exception("Verify customer information failed." + String.Join(";", new object[]{
                    custId
                })));
                Logs.Write(e);
            }

            return null;
        }

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {

        }

        #region Encode

        public const string HEX_DIGIT = "0123456789abcdef";
        private static string hexEncode(string data)
        {
            var b16_digits = HEX_DIGIT;
            string[] b16_map = new string[256];
            for (var i = 0; i < 256; i++)
            {
                b16_map[i] = string.Concat(b16_digits[i >> 4], b16_digits[i & 15]);
            }

            int c = data.Length;
            System.Collections.Generic.List<string> result = new System.Collections.Generic.List<string>();

            for (var i = 0; i < c; i++)
            {
                result.Add(b16_map[Convert.ToInt32(data[i])]);
            }

            return string.Join(null, result);
        }

        private static string hexDecode(string data)
        {
            string b16_digits = HEX_DIGIT;
            char[] b16_map = new char[256];
            for (int i = 0; i < 256; i++)
            {
                b16_map[Convert.ToInt32(string.Concat(b16_digits[i >> 4], b16_digits[i & 15]), 16)] = Convert.ToChar(i);
            }
            int c = data.Length;
            if (c % 2 != 0) data = '0' + data;
            System.Collections.Generic.List<char> result = new System.Collections.Generic.List<char>();


            for (int i = 0; i < c; i += 2)
            {
                result.Add(b16_map[Convert.ToInt32(data.Substring(i, 2), 16)]);
            }
            return new string(result.ToArray());
        }

        private static string RC4(string key, string value)
        {
            char[] ks = key.ToCharArray();
            char[] vs = value.ToCharArray();
            int c = key.Length;
            int d = value.Length;
            int[] s = new int[256];
            int i = 0;
            for (i = 0; i < 256; i++)
            {
                s[i] = i;
            }
            int j = 0;
            int x;
            for (i = 0; i < 256; i++)
            {
                j = (j + s[i] + (int)ks[i % c]) % 256;
                x = s[i];
                s[i] = s[j];
                s[j] = x;
            }
            i = 0;
            j = 0;
            string ct = string.Empty;
            for (int y = 0; y < d; y++)
            {
                i = (i + 1) % 256;
                j = (j + s[i]) % 256;
                x = s[i];
                s[i] = s[j];
                s[j] = x;
                ct += Convert.ToChar((int)vs[y] ^ s[(s[i] + s[j]) % 256]);
            }
            return ct;
        }

        #endregion
    }
}