// --------------------------------------------------------------------------
// <copyright file="RnGUtils.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.11">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.Utils
{
    using Sunplus.Agent.Model.BL.Casino;

    /// <summary>
    /// RNG Utils.
    /// </summary>
    public static class RnGUtils
    {
        public static string CasinoLang
        {
            get
            {
                return CasinoBO.CasinoLang(string.Empty);
            }
        }
    }
}
