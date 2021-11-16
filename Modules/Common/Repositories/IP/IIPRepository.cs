// --------------------------------------------------------------------------
// <copyright file="IIPRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Razorbill.Module.Common.Repositories.IP
{
    using System.Collections.Generic;
    using Fanex.Razorbill.Module.Common.Entities;

    public interface IIPRepository
    {
        IList<IPRangeEntity> GetVNIPs();
        IPInformationModel GetIPInformation(string ip);
    }
}