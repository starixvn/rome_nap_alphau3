// --------------------------------------------------------------------------
// <copyright file="NinjaHeroResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.19">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.DTOs
{
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.Razorbill.Module.RnGCasino.Enums;

    /// <summary>
    /// Ninja Hero Result.
    /// </summary>
    public class NinjaHeroResult : GameResult
    {
        public string Mode { get; set; }

        public string ModeResult { get; set; }

        public string AutoSpin { get; set; }

        public string AutoSpinResult { get; set; }

        public string FreeSpin { get; set; }

        public string FreeSpinResult { get; set; }

        // AddRow_HitBetLine, Result_HitBetLine.
        // AddRow_HitScatter, Result_HitScatter.
        public string LineNo_TotalLineBet { get; set; }

        public string Symbol { get; set; }

        public string CoinSize { get; set; }

        public string BetPerLine { get; set; }

        public string Odds { get; set; }

        public string Win { get; set; }

        public string WinResult { get; set; }

        // MaxMultiply
        public string Multiplier { get; set; }

        public decimal MultiplierResult { get; set; }

        // Total
        public string Bet { get; set; }

        public string WinLoss { get; set; }

        /// <summary>
        /// Builds the game result.
        /// </summary>
        /// <param name="drow">The data row game result.</param>
        /// <param name="code">The language code.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
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
                    FreeSpin = Converter.ToString2(drow["FreeSpin" + code]);
                    FreeSpinResult = Converter.ToString2(drow["FreeSpinResult" + code]);
                    Bet = Converter.ToString2(drow["Bet" + code]);
                    WinLoss = Converter.ToString2(drow["WinLoss" + code]);
                    break;

                case HandHistoryShowType.Result_HitBetLine:
                case HandHistoryShowType.Result_HitScatter:
                    string columnName = ShowType == HandHistoryShowType.Result_HitBetLine
                        ? "LineNo"
                        : "TotalLineBet";
                    LineNo_TotalLineBet = Converter.ToString2(drow[columnName]);
                    Symbol = Converter.ToString2(drow["Symbol"]);
                    CoinSize = Converter.ToString2(drow["CoinSize"]);
                    BetPerLine = Converter.ToString2(drow["BetPerLine"]);
                    Odds = Converter.ToString2(drow["Odds"]);
                    WinResult = Converter.ToString2(drow["Win"]);
                    break;

                case HandHistoryShowType.AddRow_RollingReel:
                    ResultName = Converter.ToString2(drow["ResultName" + code]);
                    break;

                case HandHistoryShowType.AddRow_TotalLineWin:
                    Win = Converter.ToString2(drow["Win" + code]);
                    WinResult = Converter.ToString2(drow["Win"]);
                    break;

                case HandHistoryShowType.MaxMultiply:
                    Multiplier = Converter.ToString2(drow["Multiplier" + code]);
                    MultiplierResult = Converter.ToDecimal(drow["Multiplier_Result"]);
                    break;

                default:
                    return;
            }
        }
    }
}
