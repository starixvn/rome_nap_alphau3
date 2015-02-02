// --------------------------------------------------------------------------
// <copyright file="IHandHistoryService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.11">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.Services
{
    using System.Collections.Generic;
    using Fanex.Razorbill.Module.RnGCasino.Criteria;
    using Fanex.Razorbill.Module.RnGCasino.DTOs;

    public interface IHandHistoryService
    {
        IList<GroupHandHistory> GetHandHistory(HandHistoryCriteria criteria);

        HandHistoryDetail GetHandHistoryDetail(HandHistoryDetailCriteria criteria);
    }
}
