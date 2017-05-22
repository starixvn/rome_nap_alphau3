// --------------------------------------------------------------------------
// <copyright file="OutRightResultDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Repositories.Impl.Result
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Razorbill.Module.Report.Criteria.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Interface.Result;
    using Fanex.Result.Core.Sportsbook.DTO;

    /// <summary>
    /// The class OutRightResultDataAccess.
    /// </summary>
    public class OutRightResultDataAccess : IOutRightResultDataAccess
    {
        /// <summary>
        /// Get Out Right result.
        /// </summary>
        /// <param name="criteria">The criterion.</param>
        /// <returns>The Out Right data.</returns>
        public IEnumerable<Outright> GetOutRightResult(GetOutRightResultCriteria criteria)
        {
            var result = new List<Outright>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(new Outright
                        {
                            EventDate = reader["eventdate"] != DBNull.Value ? DateTime.Parse(reader["eventdate"].ToString()) : DateTime.MinValue,
                            SportType = reader["sporttype"] != DBNull.Value ? int.Parse(reader["sporttype"].ToString()) : 0,
                            LeagueId = reader["leagueid"] != DBNull.Value ? int.Parse(reader["leagueid"].ToString()) : 0,
                            TeamId = reader["teamid"] != DBNull.Value ? int.Parse(reader["teamid"].ToString()) : 0,
                            Status = reader["status"] != DBNull.Value ? reader["status"].ToString() : string.Empty
                        });
                    }
                }
            }

            return result;
        }
    }
}
