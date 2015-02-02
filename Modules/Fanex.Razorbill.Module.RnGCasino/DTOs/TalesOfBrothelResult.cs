// --------------------------------------------------------------------------
// <copyright file="TalesOfBrothelResult.cs" company="Nexcel Solutions Vietnam">
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
    /// Tales Of Brothel Result.
    /// </summary>
    public class TalesOfBrothelResult : GameResult
    {
        public string Mode { get; set; }

        public string ModeResult { get; set; }

        public string AutoSpin { get; set; }

        public string AutoSpinResult { get; set; }

        // AddRow_HitBetLine, Result_HitBetLine.
        public string LineNo { get; set; }

        public string Symbol { get; set; }

        public string CoinSize { get; set; }

        public string BetPerLine { get; set; }

        public string Odds { get; set; }

        // Win field is common using for AddRow_HitBetLine/AddRow_TotalLineWin/AddRow_HitBonusGame.
        public string Win { get; set; }

        public string WinResult { get; set; }

        // AddRow_HitBonusGame, Result_HitBonusGame.
        public string BonusName { get; set; }

        // Total
        public string Bet { get; set; }

        public string WinLoss { get; set; }

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
                    Mode = Converter.ToString2(drow["Mode" + code]);
                    ModeResult = Converter.ToString2(drow["ModeResult" + code]);
                    AutoSpin = Converter.ToString2(drow["AutoSpin" + code]);
                    AutoSpinResult = Converter.ToString2(drow["AutoSpinResult" + code]);
                    Bet = Converter.ToString2(drow["Bet" + code]);
                    WinLoss = Converter.ToString2(drow["WinLoss" + code]);
                    break;

                case HandHistoryShowType.Result_HitBetLine:
                    LineNo = Converter.ToString2(drow["LineNo"]);
                    Symbol = Converter.ToString2(drow["Symbol"]);
                    CoinSize = Converter.ToString2(drow["CoinSize"]);
                    BetPerLine = Converter.ToString2(drow["BetPerLine"]);
                    Odds = Converter.ToString2(drow["Odds"]);
                    WinResult = Converter.ToString2(drow["Win"]);
                    break;

                case HandHistoryShowType.AddRow_TotalLineWin:
                    Win = Converter.ToString2(drow["Win" + code]);
                    WinResult = Converter.ToString2(drow["Win"]);
                    break;

                case HandHistoryShowType.AddRow_HitBonusGame:
                    BonusName = Converter.ToString2(drow["BonusName" + code]);
                    break;

                case HandHistoryShowType.AddRow_HitBonusGame1:
                case HandHistoryShowType.AddRow_HitBonusGame2:
                case HandHistoryShowType.AddRow_HitBonusGame3:
                    BonusName = Converter.ToString2(drow["BonusName" + code]);
                    WinResult = Converter.ToString2(drow["Win"]);
                    break;

                default:
                    return;
            }
        }
    }
}
