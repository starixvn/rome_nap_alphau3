// --------------------------------------------------------------------------
// <copyright file="ReportLinkSection.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Common.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Report Link Section.
    /// </summary>
    public class ReportLinkSection : ConfigurationSection
    {
        public static ReportLinkSection Instance
        {
            get
            {
                return ConfigurationManager.GetSection("ReportLinkSection") as ReportLinkSection;
            }
        }

        [ConfigurationProperty("Links", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ReportLinkCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ReportLinkCollection Links
        {
            get
            {
                return (ReportLinkCollection)base["Links"];
            }
        }
    }
}