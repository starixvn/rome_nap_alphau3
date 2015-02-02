// --------------------------------------------------------------------------
// <copyright file="HtmlExtension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.19">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.Extensions
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Html Extension.
    /// </summary>
    public static class HtmlExtension
    {
        public static IHtmlString FormatNumber(this HtmlHelper helper, object number, string currency)
        {
            decimal dec = decimal.Round((decimal)number, 2);
            string spanClass = dec >= 0 ? "bl_underdog" : "bl_favorite";
            string htmlString = string.Format("{0} <span class='{1}'>{2}</span>", currency, spanClass, dec.ToString());

            return new HtmlString(htmlString);
        }

        public static IHtmlString FormatNumber(this HtmlHelper helper, string winlossResult, string currency)
        {
            if (string.IsNullOrEmpty(winlossResult))
            {
                return new HtmlString(currency);
            }

            decimal dec = decimal.Parse(winlossResult.Replace(currency, string.Empty).Trim());
            return FormatNumber(helper, dec, currency);
        }

        public static IHtmlString FormatNumber(this HtmlHelper helper, decimal number, int digits)
        {
            return new HtmlString(number.ToString("N" + digits));
        }
    }
}
