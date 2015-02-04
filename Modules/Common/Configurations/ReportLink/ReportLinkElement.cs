// --------------------------------------------------------------------------
// <copyright file="ReportLinkElement.cs" company="Nexcel Solutions Vietnam">
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
    /// Report Link Element.
    /// </summary> 
    public class ReportLinkElement : ConfigurationElement
    {
        [ConfigurationProperty("id", DefaultValue = "0", IsRequired = true, IsKey = true)]
        public string Id
        {
            get
            {
                return (string)this["id"];
            }

            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("name", DefaultValue = "NA", IsRequired = true, IsKey = false)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }

            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("value", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Value
        {
            get
            {
                return (string)this["value"];
            }

            set
            {
                this["value"] = value;
            }
        }
    }
}