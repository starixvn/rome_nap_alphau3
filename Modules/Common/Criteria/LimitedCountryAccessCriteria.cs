// --------------------------------------------------------------------------
// <copyright file="LimitedCountryAccessCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Razorbill.Module.Common.Criteria
{
    using System.Collections;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Limited Country Access Criteria.
    /// </summary>
    public class LimitedCountryAccessCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Age_CheckValidCountry";
        }

        public override Hashtable GetSpParams()
        {
            return new Hashtable 
            { 
                { "ip", Helpers.IPHelper.CustomerIp() },
                { "countrycode", string.Empty }
            };
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}