// --------------------------------------------------------------------------
// <copyright file="SportsbookPopupResultDataAccess.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.Data;
    using Fanex.Razorbill.Module.Report.Criteria.Result;
    using Fanex.Razorbill.Module.Report.Repositories.Interface.Result;
    using Fanex.Result.Core.DTO;

    /// <summary>
    /// The class SportsBookPopupResultDataAccess.
    /// </summary>
    public class SportsbookPopupResultDataAccess : ISportsbookPopupResultDataAccess
    {
        /// <summary>
        /// Gets the score information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of SPORTSBOOKS.</returns>
        public List<Sportsbook> GetScoreInformation(ref GetSportsbookPopupResultCriteria criteria)
        {
            var result = new List<Sportsbook>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        result.Add(BindSportsbook(reader, ref criteria));

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                result[0].GameDetails.Add(BindGameDetail(reader, criteria.MatchId));
                            }
                        }

                        if (result[0].GameDetails.Count == 0)
                        {
                            result[0].GameDetails.Add(new GameDetail());
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Bind game detail value.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="matchId">The match identifier.</param>
        /// <returns>
        /// The game detail value.
        /// </returns>
        private static GameDetail BindGameDetail(IDataReader reader, int matchId)
        {
            return new GameDetail
            {
                MatchId = matchId,
                ResourceId = reader["ResourceId"] != DBNull.Value ? reader["ResourceId"].ToString() : string.Empty,
                HomeValue = reader["HomeValue"] != DBNull.Value ? reader["HomeValue"].ToString() : string.Empty,
                AwayValue = reader["AwayValue"] != DBNull.Value ? reader["AwayValue"].ToString() : string.Empty,
                ResultStatusID = reader["ResultStatusID"] != DBNull.Value ? int.Parse(reader["ResultStatusID"].ToString()) : 0,
                SettlementStatusID = reader["SettlementStatusID"] != DBNull.Value ? int.Parse(reader["SettlementStatusID"].ToString()) : 0,
                OtherInfo = reader["OtherInfo"] != DBNull.Value ? reader["OtherInfo"].ToString() : string.Empty
            };
        }

        /// <summary>
        /// Binds the SPORTSBOOK.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The object of SPORTSBOOK.
        /// </returns>
        private static Sportsbook BindSportsbook(IDataReader reader, ref GetSportsbookPopupResultCriteria criteria)
        {
            criteria.SportType = reader["sporttype"] != DBNull.Value ? Convert.ToInt32(reader["sporttype"]) : 0;

            return new Sportsbook()
            {
                HomeId = reader["homeID"] != DBNull.Value ? Convert.ToInt32(reader["homeID"]) : 0,
                AwayId = reader["awayID"] != DBNull.Value ? Convert.ToInt32(reader["awayID"]) : 0,
                HtHomeScore = reader["hthomescore"] != DBNull.Value ? Convert.ToInt32(reader["hthomescore"]) : new int?(),
                FinalHomeScore = reader["finalhomescore"] != DBNull.Value ? Convert.ToInt32(reader["finalhomescore"]) : new int?(),
                HtAwayScore = reader["htawayscore"] != DBNull.Value ? Convert.ToInt32(reader["htawayscore"]) : new int?(),
                FinalAwayScore = reader["finalawayscore"] != DBNull.Value ? Convert.ToInt32(reader["finalawayscore"]) : new int?(),
                EventStatus = reader["eventstatus"] != DBNull.Value ? Convert.ToString(reader["eventstatus"]) : string.Empty,
                KickOffTime = reader["kickofftime"] != DBNull.Value ? Convert.ToDateTime(reader["kickofftime"].ToString()) : DateTime.MinValue,
                EventDate = reader["eventdate"] != DBNull.Value ? Convert.ToDateTime(reader["eventdate"].ToString()) : DateTime.MinValue,
                IsNeutral = reader["IsNeutral"] != DBNull.Value ? Convert.ToBoolean(reader["IsNeutral"]) : false,
                BetStatus = reader["betstatus"] != DBNull.Value ? Convert.ToString(reader["betstatus"]) : string.Empty,
                IsPending = reader["IsPending"] != DBNull.Value ? Convert.ToBoolean(reader["IsPending"]) : false,
                LeagueId = reader["LeagueID"] != DBNull.Value ? Convert.ToInt32(reader["LeagueID"]) : 0,
                MatchId = criteria.MatchId,
                Color = string.Empty,
                MatchCode = reader["matchcode"] != DBNull.Value ? Convert.ToString(reader["matchcode"]) : string.Empty,
                GoalRecord = string.Empty,
                IsHTCal = false,
                Calculate = false,
                RefNo = string.Empty,
                BetID = criteria.BetId.ToString(),
                Bettype = criteria.BetType,
                GameDetails = new List<GameDetail>()
            };
        }
    }
}
