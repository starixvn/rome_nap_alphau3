// --------------------------------------------------------------------------
// <copyright file="HandHistory.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.11">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.DTOs
{
    using System;

    /// <summary>
    /// Hand History.
    /// </summary>
    public class HandHistory
    {
        public int GameId { get; set; }

        public string PlayerHistoryNo { get; set; }

        public string GameTypeNo { get; set; }

        public string GameType { get; set; }

        public string GameGroup { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public decimal StartBalance { get; set; }

        public decimal EndBalance { get; set; }

        public decimal Stake { get; set; }

        public decimal Bet { get; set; }

        public decimal WinLoss { get; set; }

        public decimal RollingComm { get; set; }

        public int UserBetId { get; set; }

        public string Result { get; set; }

        public SMAStruct PositionTaking { get; set; }

        public SMAStruct CommissionRate { get; set; }

        public int Status { get; set; }

        public string Url { get; set; }

        public string Width { get; set; }

        public static HandHistory operator +(HandHistory casino1, HandHistory casino2)
        {
            return new HandHistory 
            {
                Stake = casino1.Stake + casino2.Stake,
                Bet = casino1.Bet + casino2.Bet,
                WinLoss = casino1.WinLoss + casino2.WinLoss,
                RollingComm = casino1.RollingComm + casino2.RollingComm
            };
        }
    }
}
