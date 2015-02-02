// --------------------------------------------------------------------------
// <copyright file="ConsolidateAuthentication.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Razorbill.Module.Report.Security
{
    using Fanex.Razorbill.Module.Core.Security;

    /// <summary>
    /// Consolidate Authentication.
    /// </summary>
    public class ConsolidateAuthentication : AuthenticateAction
    {
        public ConsolidateAuthentication(AuthenticationType authenticationType, string mainAction)
            : base(authenticationType, mainAction)
        {
        }

        protected override bool GrantedRead()
        {
            return IsShowConsolidatedReport;
        }
    }
}