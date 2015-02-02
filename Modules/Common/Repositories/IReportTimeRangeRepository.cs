// --------------------------------------------------------------------------
// <copyright file="IReportTimeRangeRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.09.26">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Common.Repositories
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;

    public interface IReportTimeRangeRepository
    {
        IDictionary<int, DateTime[]> GetReportTimeRange(ICriteria criteria);
    }
}