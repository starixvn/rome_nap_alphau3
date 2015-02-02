// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.11">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.App_Start
{
    using System.Web.Optimization;

    /// <summary>
    /// Bundle Config.
    /// </summary>
    internal static class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterHandHistoryBundles(bundles);
            RegisterHandHistoryDetailBundles(bundles);
        }

        /// <summary>
        /// Registers the hand history bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterHandHistoryBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Areas/RnGCasino/HandHistoryScripts").Include(
                   "~/Areas/RnGCasino/Views/HandHistory/Scripts/handhistory.js"));

            bundles.Add(new StyleBundle("~/App_Themes/AgencyV2/AgencyV2Styles").Include(
                "~/App_Themes/AgencyV2/common.css",
                "~/App_Themes/AgencyV2/main.css",
                "~/Areas/RnGCasino/Styles/handhistory.css"));
        }

        private static void RegisterHandHistoryDetailBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/_GlobalResources/Css/HandHistoryDetailStyles").Include(
                "~/_GlobalResources/Css/Agent.css",
                "~/_BetList/Resources/BetList.css"));
        }
    }
}
