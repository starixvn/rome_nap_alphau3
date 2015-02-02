// --------------------------------------------------------------------------
// <copyright file="ISportsbookPopupResultDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Repositories.Interface.Result
{
    using System.Collections.Generic;
    using Fanex.Razorbill.Module.Report.Criteria.Result;
    using Fanex.Result.Core.DTO;

    /// <summary>
    /// The interface ISportsBookPopupResultDataAccess.
    /// </summary>
    public interface ISportsbookPopupResultDataAccess
    {
        List<Sportsbook> GetScoreInformation(ref GetSportsbookPopupResultCriteria criteria);
    }
}
