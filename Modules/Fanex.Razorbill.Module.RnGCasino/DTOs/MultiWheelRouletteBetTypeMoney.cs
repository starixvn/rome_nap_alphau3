// --------------------------------------------------------------------------
// <copyright file="MultiWheelRouletteBetTypeMoney.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// Multi Wheel Roulette BetType and Money.
    /// </summary>
    public class MultiWheelRouletteBetTypeMoney : BetTypeMoney
    {
        public string BetContent { get; set; }

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

            base.BuildBetTypeMoney(drow, code);
            BetContent = Converter.ToString2(drow["BetContent"]);
        }
    }
}
