// --------------------------------------------------------------------------
// <copyright file="GameResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.19">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.DTOs
{
    using System;
    using System.Data;
    using Fanex.Common;
    using Fanex.Razorbill.Module.RnGCasino.Enums;

    /// <summary>
    /// Game Result.
    /// </summary>
    public class GameResult
    {
        public HandHistoryShowType ShowType { get; set; }

        public string TableName { get; set; }

        public DateTime BetTime { get; set; }

        public string UserName { get; set; }

        public string ResultName { get; set; }

        public string Result { get; set; }

        public string PlayerHistoryNo { get; set; }

        public string PlayerHistoryNoResult { get; set; }

        /// <summary>
        /// Builds the game result.
        /// </summary>
        /// <param name="drow">The data row game result.</param>
        /// <param name="code">The language code.</param>
        public virtual void BuildGameResult(DataRow drow, string code)
        {
            if (drow == null)
            {
                return;
            }

            string showTypeValue = Converter.ToString2(drow["ShowType"]);
            
            // Sparkz doesn't has ShowType value.
            ShowType = string.IsNullOrWhiteSpace(showTypeValue)
                ? HandHistoryShowType.BasicInfo
                : (HandHistoryShowType)Enum.Parse(typeof(HandHistoryShowType), showTypeValue);
            if (ShowType == HandHistoryShowType.BasicInfo)
            {
                TableName = Converter.ToString2(drow["TableName" + code]);
                BetTime = Converter.ToDateTime(drow["BetTime"]);
                UserName = Converter.ToString2(drow["UserName"]);
                ResultName = Converter.ToString2(drow["ResultName" + code]);
                Result = Converter.ToString2(drow["Result"]);
                PlayerHistoryNo = Converter.ToString2(drow["PlayerHistoryNo" + code]);
                PlayerHistoryNoResult = Converter.ToString2(drow["PlayerHistoryNo_Result"]);
            }
        }
    }
}
