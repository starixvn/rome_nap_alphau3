// --------------------------------------------------------------------------
// <copyright file="BetTypeMoney.cs" company="Nexcel Solutions Vietnam">
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
    /// Bet Type Money.
    /// </summary>
    public class BetTypeMoney
    {
        public string BetTypeName { get; set; }

        public decimal Bet { get; set; }

        public decimal WinLoss { get; set; }

        public string Currency { get; set; }

        /// <summary>
        /// Builds the bet type and money.
        /// </summary>
        /// <param name="drow">The data row bet type and money.</param>
        /// <param name="code">The language code.</param>
        public virtual void BuildBetTypeMoney(DataRow drow, string code)
        {
            if (drow == null)
            {
                return;
            }

            BetTypeName = Converter.ToString2(drow["BetTypeName" + code]);
            Bet = Converter.ToDecimal(drow["Bet"]);
            WinLoss = Converter.ToDecimal(drow["WinLoss"]);
            Currency = Converter.ToString2(drow["Currency"]);
        }
    }
}
