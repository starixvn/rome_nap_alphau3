// --------------------------------------------------------------------------
// <copyright file="VirtualRacingResultDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Repositories.Impl.Result
{
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Razorbill.Module.Report.Criteria.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Interface.Result;
    using Fanex.Result.Core.DTO;

    /// <summary>
    /// The class VirtualRacingResultDataAccess.
    /// </summary>
    public class VirtualRacingResultDataAccess : IVirtualRacingResultDataAccess
    {
        /// <summary>
        /// Get racing result.
        /// </summary>
        /// <param name="criteria">The criterion.</param>
        /// <returns>The racing data.</returns>
        public IEnumerable<Racing> GetVirtualRacingResult(GetVirtualRacingResultCriteria criteria)
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
