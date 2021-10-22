// --------------------------------------------------------------------------
// <copyright file="IIPService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Razorbill.Module.Common.Services.IP
{
    using System.Collections.Generic;
    using Fanex.Razorbill.Module.Common.Entities;

    public interface IIPService
    {
        IList<IPRangeEntity> GetIPVN();
        IPInformationModel GetIPInformation(string ip);
    }
}