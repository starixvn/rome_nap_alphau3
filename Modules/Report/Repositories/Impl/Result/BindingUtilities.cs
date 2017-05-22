// --------------------------------------------------------------------------
// <copyright file="BindingUtilities.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Repositories.Impl.Result
{
    using System;
    using System.Data;
    using Fanex.Result.Core.Racing.DTO;

    /// <summary>
    /// The class BindingUtilities.
    /// </summary>
    public static class BindingUtilities
    {
        /// <summary>
        /// Bind data from IDATAREADER to DTO.
        /// </summary>
        /// <param name="reader">The IDATAREADER.</param>
        /// <returns>The Racing DTO.</returns>
        public static Racing BindRacing(IDataReader reader)
        {
            return new Racing
            {
                LeagueId = reader["meetingid"] != DBNull.Value ? int.Parse(reader["meetingid"].ToString()) : 0,
                EngineId = reader["HorseId"] != DBNull.Value ? reader["HorseId"].ToString() : string.Empty,
                EngineNo = reader["HorseNo"] != DBNull.Value ? reader["HorseNo"].ToString() : string.Empty,
                IsFavor = reader["IsFavor"] != DBNull.Value && reader["IsFavor"].ToString().Equals("1") ? true : false,
                IsSCR = reader["IsSCR"] != DBNull.Value && reader["IsSCR"].ToString().Equals("1") ? true : false,
                Place = reader["place"] != DBNull.Value ? int.Parse(reader["place"].ToString()) : 0,
                PlaceOdds = reader["placeodds"] != DBNull.Value ? decimal.Parse(reader["placeodds"].ToString()) : 0,
                WinOdds = reader["odds"] != DBNull.Value ? decimal.Parse(reader["odds"].ToString()) : 0,
                RaceName = reader["racename"] != DBNull.Value ? int.Parse(reader["racename"].ToString()) : 0,
                KickOffTime = reader["kickofftime"] != DBNull.Value ? DateTime.Parse(reader["kickofftime"].ToString()) : DateTime.MinValue,
                Favourite = reader["IsFavor"] != DBNull.Value ? reader["IsFavor"].ToString() : string.Empty,
                RowCount = reader["rowcount"] != DBNull.Value ? int.Parse(reader["rowcount"].ToString()) : 0,
                SCROfMatch = GetSRCValue(reader),
                TFOfMatch = reader["TF"] != DBNull.Value ? reader["TF"].ToString() : string.Empty
            };
        }

        /// <summary>
        /// Check the column existed in data reader.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>True or False.</returns>
        public static bool ColumnExisted(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the source value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The value of SRC.</returns>
        private static string GetSRCValue(IDataReader reader)
        {
            if (reader["SCR"] != DBNull.Value)
            {
                string value = reader["SCR"].ToString();

                if (value.EndsWith(","))
                {
                    value = value.Remove(value.Length - 1);
                }

                return value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
