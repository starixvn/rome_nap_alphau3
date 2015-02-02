// --------------------------------------------------------------------------
// <copyright file="IReportTimeRangeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.09.26">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Common.Services
{
    using System;
    using System.Collections.Generic;

    public interface IReportTimeRangeService
    {
        IDictionary<int, DateTime[]> GetHashReportTimeRange();
    }
}