// --------------------------------------------------------------------------
// <copyright file="FoobetDTO.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.22">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Entities.FooBet
{
    using System;

    /// <summary>
    /// FooBetDTO class.
    /// </summary>
    [Serializable]
    public class FooBetDTO
    {
        public string Month { get; set; }

        public decimal Turnover { get; set; }

        public int BetCount { get; set; }

        public decimal WinAmount { get; set; }

        public decimal LoseAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal LosingTax { get; set; }

        public decimal FBRebate { get; set; }

        public decimal CompanyIncome { get; set; }

        /// <summary>
        /// Operator + for FooBet.
        /// </summary>
        /// <param name="x">The x FooBet.</param>
        /// <param name="y">The y FooBet.</param>
        /// <returns>The FooBet object.</returns>
        public static FooBetDTO operator +(FooBetDTO x, FooBetDTO y)
        {
            return new FooBetDTO
            {
                Turnover = x.Turnover + y.Turnover,
                BetCount = x.BetCount + y.BetCount,
                WinAmount = x.WinAmount + y.WinAmount,
                LoseAmount = x.LoseAmount + y.LoseAmount,
                TotalAmount = x.TotalAmount + y.TotalAmount,
                LosingTax = x.LosingTax + y.LosingTax,
                FBRebate = x.FBRebate + y.FBRebate,
                CompanyIncome = x.CompanyIncome + y.CompanyIncome
            };
        }

        /// <summary>
        /// Formats the FooBet object to 2 digit.
        /// </summary>
        /// <param name="foobet">The FooBet.</param>
        /// <returns>The FooBet object.</returns>
        public FooBetDTO Format2Digits(FooBetDTO foobet)
        {
            return new FooBetDTO
            {
                Month = foobet.Month,
                Turnover = Format2Digits(foobet.Turnover),
                BetCount = foobet.BetCount,
                WinAmount = Format2Digits(foobet.WinAmount),
                LoseAmount = Format2Digits(foobet.LoseAmount),
                TotalAmount = Format2Digits(foobet.TotalAmount),
                LosingTax = Format2Digits(foobet.LosingTax),
                FBRebate = Format2Digits(foobet.FBRebate),
                CompanyIncome = Format2Digits(foobet.CompanyIncome)
            };
        }

        private decimal Format2Digits(decimal value)
        {
            return Math.Round(value, 2);
        }
    }
}