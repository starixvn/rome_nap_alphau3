// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.26">New requirement #16452</change>
// </history>

namespace Fanex.Razorbill.Module.Report.App_Start
{
    using System.Web.Optimization;

    /// <summary>
    /// BundleConfig class.
    /// </summary>
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/_Libs/Jquery").Include(
                "~/Scripts/_Libs/jQuery/jquery-{version}.js"));

            RegisterFanexGridBundles(ref bundles);
            RegisterDateBundles(ref bundles);
            RegisterFBFinancialBundles(ref bundles);
            RegisterCommmissionByBettypeBundles(ref bundles);
            RegisterResultBundles(ref bundles);
        }

        /// <summary>
        /// Registers the FANEX.Grid bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterFanexGridBundles(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/_Libs/FanexGrid/FanexGridScripts").Include(
                "~/Scripts/_Libs/FanexGrid-MultiLang/Fanex.Grid.js",
                "~/Scripts/_Libs/FanexGrid-MultiLang/Language/Fanex.Gird.Lang.en-US.js",
                "~/Scripts/_Libs/FanexGrid-MultiLang/Language/Fanex.Gird.Lang.ja-JP.js",
                "~/Scripts/_Libs/FanexGrid-MultiLang/Language/Fanex.Gird.Lang.ko-KR.js",
                "~/Scripts/_Libs/FanexGrid-MultiLang/Language/Fanex.Gird.Lang.th-TH.js",
                "~/Scripts/_Libs/FanexGrid-MultiLang/Language/Fanex.Gird.Lang.vi-VN.js",
                "~/Scripts/_Libs/FanexGrid-MultiLang/Language/Fanex.Gird.Lang.zh-CN.js",
                "~/Scripts/_Libs/FanexGrid-MultiLang/Language/Fanex.Gird.Lang.zh-TW.js",
                "~/Scripts/_Libs/FanexGrid/Fanex.Pagination.js",
                "~/Scripts/_Libs/FanexGrid/moment.js",
                "~/Scripts/_Libs/FanexGrid/numeral.js"));

            bundles.Add(new StyleBundle("~/Scripts/_Libs/FanexGrid/FanexGridStyles").Include(
                "~/Scripts/_Libs/FanexGrid/Fanex.Grid.css"));
        }

        /// <summary>
        /// Registers the date bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterDateBundles(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/_Components/DateTimeControl/JSCalendar/JSCalendarScripts").Include(
                "~/_Components/DateRangeSelect/DateRangeSelect.js",
                "~/_Components/DateTimeControl/JSCalendar/js/jscal2.js",
                "~/_Components/DateTimeControl/JSCalendar/js/lang/en.js"));

            bundles.Add(new StyleBundle("~/_Components/DateRangeSelect/DateRangeSelectStyles").Include(
                "~/_Components/DateRangeSelect/DateRangeSelect.css"));

            bundles.Add(new StyleBundle("~/_Components/DateTimeControl/JSCalendar/css/JSCalendarStyles").Include(
                "~/_Components/DateTimeControl/JSCalendar/css/jscal2.css"));

            bundles.Add(new StyleBundle("~/_Components/DateTimeControl/JSCalendar/css/steel/JSCalendarSteelStyles").Include(
                "~/_Components/DateTimeControl/JSCalendar/css/steel/steel.css"));
        }

        /// <summary>
        /// Registers the FB financial bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterFBFinancialBundles(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/Foobet/FoobetScripts").Include(
                "~/Areas/Report/Views/Foobet/Scripts/FoobetManager.js",
                "~/Areas/Report/Views/Foobet/Scripts/FoobetManager.MainReport.js"));

            bundles.Add(new StyleBundle("~/Areas/Report/Foobet/FoobetStyles").Include(
                "~/Areas/Report/Views/Foobet/Style/Foobet.css"));
        }

        public static void RegisterCommmissionByBettypeBundles(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/CommissionByBetType/CommissionByBettypeScripts").Include(
                "~/Areas/Report/Views/CommissionByBetType/Scripts/CommissionByBetType.js"));

            bundles.Add(new StyleBundle("~/Areas/Report/CommissionByBetType/CommissionByBettypeStyles").Include(
                "~/Areas/Report/Views/CommissionByBetType/Style/CommissionByBetType.css"));
        }

        public static void RegisterResultBundles(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/Result/ResultScripts").Include(
                "~/Areas/Report/Views/Result/Scripts/Result.js"));
        }
    }
}