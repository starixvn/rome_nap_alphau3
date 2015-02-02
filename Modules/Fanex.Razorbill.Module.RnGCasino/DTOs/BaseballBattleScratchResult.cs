// --------------------------------------------------------------------------
// <copyright file="BaseballBattleScratchResult.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// Baseball Battle Scratch Result.
    /// </summary>
    public class BaseballBattleScratchResult : GameResult
    {
        public string Bet { get; set; }

        public string Odds { get; set; }

        public string WinLoss { get; set; }

        public string Multiplier { get; set; }

        public decimal MultiplierResult { get; set; }

        public string AutoPlay { get; set; }

        public string AutoPlayResult { get; set; }

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

            // Custom build properties here.
            Bet = Converter.ToString2(drow["Bet" + code]);
            Odds = Converter.ToString2(drow["Odds" + code]);
            WinLoss = Converter.ToString2(drow["WinLoss" + code]);

            Multiplier = Converter.ToString2(drow["Multiplier" + code]);
            MultiplierResult = Converter.ToDecimal(drow["Multiplier"]);

            AutoPlay = Converter.ToString2(drow["AutoPlay" + code]);
            AutoPlayResult = Converter.ToString2(drow["AutoPlayResult" + code]);
        }
    }
}
