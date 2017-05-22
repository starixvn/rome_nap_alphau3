// --------------------------------------------------------------------------
// <copyright file="SportsbookResultDataAccess.cs" company="Nexcel Solutions Vietnam">
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
    using System.Data;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Razorbill.Module.Report.Criteria.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Interface.Result;
    using Fanex.Result.Core.Sportsbook.DTO;

    /// <summary>
    /// The class SportsBookResultDataAccess.
    /// </summary>
    public class SportsbookResultDataAccess : ISportsbookResultDataAccess
    {
        /// <summary>
        /// Get SPORTBOOK result.
        /// </summary>
        /// <param name="criteria">The criterion.</param>
        /// <returns>The SPORTBOOK data.</returns>
        public IEnumerable<Sportsbook> GetSportbookResult(GetSportsbookResultCriteria criteria)
        {
            var results = new List<Sportsbook>();
            var gameDetails = new List<GameDetail>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        results.Add(BindSportbook(reader));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            gameDetails.Add(BindGameDetail(reader));
                        }
                    }
                }
            }

            foreach (var result in results)
            {
                result.GameDetails = gameDetails.Where(i => i.MatchId == result.MatchId).ToList();
            }

            return results;
        }

        /// <summary>
        /// Bind data from IDATAREADER to DTO.
        /// </summary>
        /// <param name="reader">The IDATAREADER.</param>
        /// <returns>The SPORTBOOK DTO.</returns>
        private static Sportsbook BindSportbook(IDataReader reader)
        {
            var item = new Sportsbook
            {
                HomeId = reader["homeID"] != DBNull.Value ? Convert.ToInt32(reader["homeID"]) : 0,
                AwayId = reader["awayID"] != DBNull.Value ? Convert.ToInt32(reader["awayID"]) : 0,
                BetStatus =
                    reader["betstatus"] != DBNull.Value ? reader["betstatus"].ToString() : string.Empty,
                LeagueId = reader["leagueid"] != DBNull.Value ? int.Parse(reader["leagueid"].ToString()) : 0,
                MatchId = reader["matchid"] != DBNull.Value ? int.Parse(reader["matchid"].ToString()) : 0,
                EventStatus = reader["eventstatus"] != DBNull.Value ? reader["eventstatus"].ToString() : string.Empty,
                FinalAwayScore =
                    reader["finalawayscore"] != DBNull.Value
                        ? int.Parse(reader["finalawayscore"].ToString())
                        : new int?(),
                HtAwayScore =
                    reader["htawayscore"] != DBNull.Value ? int.Parse(reader["htawayscore"].ToString()) : new int?(),
                Color = BindingUtilities.ColumnExisted(reader, "lcolor") && reader["lcolor"] != DBNull.Value ? reader["lcolor"].ToString() : string.Empty,
                FinalHomeScore =
                    reader["finalhomescore"] != DBNull.Value
                        ? int.Parse(reader["finalhomescore"].ToString())
                        : new int?(),
                HtHomeScore =
                    reader["hthomescore"] != DBNull.Value
                        ? int.Parse(reader["hthomescore"].ToString())
                        : new int?(),
                EventDate = reader["eventdate"] != DBNull.Value ? DateTime.Parse(reader["eventdate"].ToString()) : DateTime.MinValue,
                IsNeutral = reader["IsNeutral"] != DBNull.Value ? bool.Parse(reader["IsNeutral"].ToString()) : false,
                IsPending = reader["IsPending"] != DBNull.Value ? bool.Parse(reader["IsPending"].ToString()) : false,
                KickOffTime = reader["kickofftime"] != DBNull.Value ? DateTime.Parse(reader["kickofftime"].ToString()) : DateTime.MinValue,
                MatchCode = reader["matchcode"] != DBNull.Value ? reader["matchcode"].ToString() : string.Empty,
                GoalRecord = reader["goalrecord"] != DBNull.Value ? reader["goalrecord"].ToString() : string.Empty,
                IsHTCal = reader["IsHTCal"] != DBNull.Value ? Convert.ToBoolean(reader["IsHTCal"]) : false,
                Calculate = reader["Calculate"] != DBNull.Value ? Convert.ToBoolean(reader["Calculate"]) : false,
                RefNo = string.Empty,
                GameDetails = new List<GameDetail>(),
                Bettype = GetBetTypeFromToken(reader),
                BetID = GetBetIDFromToken(reader)
            };

            return item;
        }

        /// <summary>
        /// Bind game detail value.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <returns>The game detail value.</returns>
        private static GameDetail BindGameDetail(IDataReader reader)
        {
            return new GameDetail
            {
                MatchId = reader["matchid"] != DBNull.Value ? int.Parse(reader["matchid"].ToString()) : 0,
                ResourceId = reader["ResourceId"] != DBNull.Value ? reader["ResourceId"].ToString() : string.Empty,
                HomeValue = reader["HomeValue"] != DBNull.Value ? reader["HomeValue"].ToString() : string.Empty,
                AwayValue = reader["AwayValue"] != DBNull.Value ? reader["AwayValue"].ToString() : string.Empty,
                SettlementStatusID = reader["SettlementStatusID"] != DBNull.Value ? int.Parse(reader["SettlementStatusID"].ToString()) : 0
            };
        }

        private static int GetBetTypeFromToken(IDataReader reader)
        {
            const string BettypeColumn = "bettype";
            if (BindingUtilities.ColumnExisted(reader, BettypeColumn) && reader[BettypeColumn] != DBNull.Value)
            {
                return int.Parse(reader[BettypeColumn].ToString());
            }

            return 0;
        }

        private static string GetBetIDFromToken(IDataReader reader)
        {
            const string BetcheckColumn = "betcheck";
            if (BindingUtilities.ColumnExisted(reader, BetcheckColumn) && reader[BetcheckColumn] != DBNull.Value)
            {
                return reader[BetcheckColumn].ToString();
            }

            return string.Empty;
        }
    }
}
