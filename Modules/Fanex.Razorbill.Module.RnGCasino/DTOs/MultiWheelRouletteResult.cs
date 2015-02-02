// --------------------------------------------------------------------------
// <copyright file="MultiWheelRouletteResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.09.16">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.DTOs
{
    using System.Data;
    using Fanex.Common;
    using Fanex.Razorbill.Module.RnGCasino.Enums;

    /// <summary>
    /// Multi Wheel Roulette Result.
    /// </summary>
    public class MultiWheelRouletteResult : GameResult
    {
        public string RouletteResult { get; set; }

        // Total
        public string Summary { get; set; }

        public string Bet { get; set; }

        public string TotalBet { get; set; }

        public string WinLoss { get; set; }

        public string TotalWinLoss { get; set; }

        /// <summary>
        /// Builds the game result.
        /// </summary>
        /// <param name="drow">The data row game result.</param>
        /// <param name="code">The language code.</param>
        public override void BuildGameResult(DataRow drow, string code)
        {
            if (drow == null)
            {
                return;
            }

            base.BuildGameResult(drow, code);
            switch (ShowType)
            {
                case HandHistoryShowType.BasicInfo:
                    Summary = Converter.ToString2(drow["Summary" + code]);
                    Bet = Converter.ToString2(drow["Bet" + code]);
                    TotalBet = Converter.ToString2(drow["TotalBet"]);
                    WinLoss = Converter.ToString2(drow["WinLoss" + code]);
                    TotalWinLoss = Converter.ToString2(drow["TotalWinLoss"]);
                    break;

                case HandHistoryShowType.AddRow_EnableRoulette1:
                case HandHistoryShowType.AddRow_EnableRoulette2:
                case HandHistoryShowType.AddRow_EnableRoulette3:
                case HandHistoryShowType.AddRow_EnableRoulette4:
                    RouletteResult = Converter.ToString2(drow["RouletteResult" + code]);
                    break;

                default:
                    break;
            }
        }
    }
}
