// --------------------------------------------------------------------------
// <copyright file="FoobetAuthentication.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Razorbill.Module.Report.Security
{
    using Fanex.Razorbill.Module.Core.Security;
    using Fanex.Razorbill.Module.Report.Utils;

    /// <summary>
    /// FOOBET Authentication.
    /// </summary>
    public class FoobetAuthentication : AuthenticateAction
    {
        public FoobetAuthentication(AuthenticationType authenticationType, string mainAction)
            : base(authenticationType, mainAction)
        {
        }

        protected override bool GrantedRead()
        {
            return CustId == ReportUtils.FoobetId;
        }
    }
}