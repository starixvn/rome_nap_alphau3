// --------------------------------------------------------------------------
// <copyright file="CowboyFeverBetTypeMoney.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.DTOs
{
    using System.Data;
    using Fanex.Common;

    /// <summary>
    /// Cowboy Fever BetType Money.
    /// </summary>
    public class CowboyFeverBetTypeMoney : BetTypeMoney
    {
        public decimal Odds { get; set; }

        public decimal Win { get; set; }

        /// <summary>
        /// Builds the bet type and money.
        /// </summary>
        /// <param name="drow">The data row bet type and money.</param>
        /// <param name="code">The language code.</param>
        public override void BuildBetTypeMoney(DataRow drow, string code)
        {
            if (drow == null)
            {
                return;
            }

            Odds = Converter.ToDecimal(drow["Odds"]);
            Win = Converter.ToDecimal(drow["Win"]);

            base.BuildBetTypeMoney(drow, code);
        }
    }
}
