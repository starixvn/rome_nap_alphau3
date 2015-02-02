// --------------------------------------------------------------------------
// <copyright file="RacingPopupResultDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Services.Implement.Result
{
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Razorbill.Module.Report.Criteria.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Impl.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Interface.Result;
    using Fanex.Result.Core.DTO;

    /// <summary>
    /// The class RacingPopupResultDataAccess.
    /// </summary>
    public class RacingPopupResultDataAccess : IRacingPopupResultDataAccess
    {
        /// <summary>
        /// Get racing POPUP result.
        /// </summary>
        /// <param name="criteria">The criterion.</param>
        /// <returns>The racing POPUP data.</returns>
        public IList<Racing> GetRacingPopupResult(GetRacingPopupResultCriteria criteria)
        {
            var result = new List<Racing>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(BindingUtilities.BindRacing(reader));
                    }
                }
            }

            return result;
        }
    }
}
