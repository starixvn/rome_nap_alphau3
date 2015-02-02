// --------------------------------------------------------------------------
// <copyright file="GameTypeConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.18">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.Configurations
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Game Type Config.
    /// </summary>
    public static class GameTypeConfig
    {
        private static readonly Dictionary<string, HandHistoryElement> GameTypes;

        static GameTypeConfig()
        {
            GameTypes = InitializeHandHistory();
        }

        public static HandHistoryElement GetByGameType(string type)
        {
            HandHistoryElement element = null;
            GameTypes.TryGetValue(type, out element);

            return element;
        }

        /// <summary>
        /// Initializes the hand history.
        /// </summary>
        /// <returns>The hash of HandHistoryElement.</returns>
        private static Dictionary<string, HandHistoryElement> InitializeHandHistory()
        {
            Dictionary<string, HandHistoryElement> gameTypeHash = new Dictionary<string, HandHistoryElement>();

            char[] splitChars = new char[] { ',', ';' };
            string[] gameTypes;
            HandHistoryElement handHistoryElement;
            foreach (var item in RnGCasinoSection.Instance.HandHistories)
            {
                handHistoryElement = (HandHistoryElement)item;
                gameTypes = handHistoryElement.GameType.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                foreach (var type in gameTypes)
                {
                    gameTypeHash.Add(type, handHistoryElement);
                }
            }

            return gameTypeHash;
        }
    }
}
