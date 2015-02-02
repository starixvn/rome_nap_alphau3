// --------------------------------------------------------------------------
// <copyright file="KenoBetTypesSection.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Razorbill.Module.PositionTaking.Configurations.KenoBetTypes
{
    using System.Configuration;

    /// <summary>
    /// Keno Bet Types Section.
    /// </summary>
    public class KenoBetTypesSection : ConfigurationSection
    {
        public static KenoBetTypesSection Instance
        {
            get
            {
                var kenoSportsSection =
                 ConfigurationManager.GetSection("KenoBetTypesSection") as KenoBetTypesSection;
                return kenoSportsSection;
            }
        }

        [ConfigurationProperty("active", IsRequired = true)]
        public bool Active
        {
            get { return (bool)this["active"]; }
        }

        [ConfigurationProperty("BetTypes", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(KenoBetTypesCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public KenoBetTypesCollection BetTypes
        {
            get
            {
                return (KenoBetTypesCollection)base["BetTypes"];
            }
        }
    }
}
