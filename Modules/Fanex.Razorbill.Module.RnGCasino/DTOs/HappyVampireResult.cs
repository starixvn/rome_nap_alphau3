// --------------------------------------------------------------------------
// <copyright file="HappyVampireResult.cs" company="Nexcel Solutions Vietnam">
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
    /// Happy Vampire Result.
    /// </summary>
    public class HappyVampireResult : NinjaHeroResult
    {
        // AddRow_HitBonusGame, Result_HitBonusGame.
        public string BonusName { get; set; }

        // AddRow_HitFreeSpin.
        public string FreeSpinName { get; set; }

        // Only for Fotune Gods
        public string Wild { get; set; }

        public string WildResult { get; set; }

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
                case HandHistoryShowType.AddRow_HitBonusGame:
                    BonusName = Converter.ToString2(drow["BonusName" + code]);
                    break;

                case HandHistoryShowType.AddRow_HitBonusGame1:
                case HandHistoryShowType.AddRow_HitBonusGame2:
                case HandHistoryShowType.AddRow_HitBonusGame3:
                    BonusName = Converter.ToString2(drow["BonusName" + code]);
                    WinResult = Converter.ToString2(drow["Win"]);
                    break;

                case HandHistoryShowType.AddRow_HitFreeSpin:
                    FreeSpinName = Converter.ToString2(drow["FreeSpinName" + code]);
                    FreeSpin = Converter.ToString2(drow["FreeSpin" + code]);
                    FreeSpinResult = Converter.ToString2(drow["FreeSpinTimes_Result"]);
                    Multiplier = Converter.ToString2(drow["Multiplier" + code]);
                    MultiplierResult = Converter.ToDecimal(drow["Multiplier_Result"]);

                    if (drow.Table.Columns.Contains("Wild" + code))
                    {
                        Wild = Converter.ToString2(drow["Wild" + code]);
                        WildResult = Converter.ToString2(drow["Wild_Result"]);
                    }

                    break;

                default:
                    return;
            }
        }
    }
}
