// --------------------------------------------------------------------------
// <copyright file="CowboyFeverResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.19">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.DTOs
{
    using System.Data;
    using Fanex.Common;
    using Fanex.Razorbill.Module.RnGCasino.Enums;

    /// <summary>
    /// Cowboy Fever Result.
    /// </summary>
    public class CowboyFeverResult : GameResult
    {
        // BasicInfo
        public string TotalBet { get; set; }

        public string TotalWinLoss { get; set; }

        public string AutoSpin { get; set; }

        public string AutoSpinResult { get; set; }

        // AddRow_HitBetLine, Result_HitBetLine
        public string LineNo { get; set; }

        public string Symbol { get; set; }

        public string CoinSize { get; set; }

        public string Odds { get; set; }

        public string Win { get; set; }

        // Add_TotalLineWinLoss
        public string WinLoss { get; set; }

        public string WinLossResult { get; set; }

        // Add_Bonus
        public string Bonus { get; set; }

        public string BonusResult { get; set; }

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
                    // Add more properties when ShowType = BasicInfo.
                    AutoSpin = Converter.ToString2(drow["AutoSpin" + code]);
                    AutoSpinResult = Converter.ToString2(drow["AutoSpinResult" + code]);

                    TotalBet = Converter.ToString2(drow["TotalBet" + code]);
                    TotalWinLoss = Converter.ToString2(drow["TotalWinLoss" + code]);
                    break;

                case HandHistoryShowType.Result_HitBetLine:
                    LineNo = Converter.ToString2(drow["LineNo"]);
                    Symbol = Converter.ToString2(drow["Symbol"]);
                    CoinSize = Converter.ToString2(drow["CoinSize"]);
                    Odds = Converter.ToString2(drow["Odds"]);
                    Win = Converter.ToString2(drow["Win"]);
                    break;

                case HandHistoryShowType.Add_TotalLineWinLoss:
                    WinLoss = Converter.ToString2(drow["WinLoss" + code]);
                    WinLossResult = Converter.ToString2(drow["WinLoss_Result"]);
                    break;

                case HandHistoryShowType.Add_Bonus:
                    Bonus = Converter.ToString2(drow["Bonus" + code]);
                    BonusResult = Converter.ToString2(drow["Bonus_Result"]);
                    break;

                case HandHistoryShowType.AddRow_HitBetLine:
                case HandHistoryShowType.None:
                default:
                    return;
            }
        }
    }
}
