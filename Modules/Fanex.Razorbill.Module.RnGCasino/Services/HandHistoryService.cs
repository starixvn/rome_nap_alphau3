// --------------------------------------------------------------------------
// <copyright file="HandHistoryService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.11">Created</change>
// </history>

namespace Fanex.Razorbill.Module.RnGCasino.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using CasinoLibrary.SBAgent;
    using Fanex.Common;
    using Fanex.Razorbill.Module.RnGCasino.Configurations;
    using Fanex.Razorbill.Module.RnGCasino.Criteria;
    using Fanex.Razorbill.Module.RnGCasino.DTOs;
    using Fanex.Razorbill.Module.RnGCasino.Enums;
    using Fanex.Razorbill.Module.RnGCasino.Utils;
    using Sunplus.Agent.Model.BL.Casino;

    /// <summary>
    /// Hand History Service.
    /// </summary>
    public class HandHistoryService : IHandHistoryService
    {
        private readonly Report _casinoReport;

        public HandHistoryService()
            : this(null)
        { 
        }

        public HandHistoryService(Report casinoReport)
        {
            _casinoReport = casinoReport ?? new Report();
        }

        /// <summary>
        /// Gets the hand history.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The group of Hand History.</returns>
        public IList<GroupHandHistory> GetHandHistory(HandHistoryCriteria criteria)
        {
            DataTable datatable = HandHistoryResult(criteria);
            if (datatable == null || datatable.Rows.Count == 0)
            {
                return null;
            }

            // Realtime WinLoss page.
            bool isRealTimeWinLoss = criteria.IBCCustId != 0;

            string sessionId;
            HandHistory handHistory;
            GroupHandHistory groupHandHistory;
            HandHistoryStatus status;

            string lang = RnGUtils.CasinoLang;
            IDictionary<string, GroupHandHistory> groupHash = new Dictionary<string, GroupHandHistory>();
            foreach (DataRow row in datatable.Rows)
            {
                sessionId = Converter.ToString2(row["SessionLogID"]);
                if (!groupHash.TryGetValue(sessionId, out groupHandHistory))
                {
                    groupHandHistory = new GroupHandHistory(sessionId);
                    groupHash.Add(sessionId, groupHandHistory);
                }

                handHistory = ParseHandHistory(row, lang);
                groupHandHistory.HandHistories.Add(handHistory);

                status = (HandHistoryStatus)handHistory.Status;
                if (isRealTimeWinLoss)
                {
                    if (status != HandHistoryStatus.Playing && status != HandHistoryStatus.BrokenFinish)
                    {
                        groupHandHistory.Total += handHistory;
                    }
                }
                else
                {
                    if (status != HandHistoryStatus.Broken && status != HandHistoryStatus.BrokenFinish)
                    {
                        groupHandHistory.Total += handHistory;   
                    }
                }
            }

            return groupHash.Values.ToList();
        }

        /// <summary>
        /// Gets the hand history detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The HandHistoryDetail object.</returns>
        public HandHistoryDetail GetHandHistoryDetail(HandHistoryDetailCriteria criteria)
        {
            DataTable[] tables = GetWinLossDetail(criteria.UserBetId);
            if (tables == null ||
                tables.Length != 2 ||
                (tables[0] == null || tables[0].Rows.Count == 0) ||
                (tables[1] == null || tables[1].Rows.Count == 0))
            {
                return null;
            }

            var element = GameTypeConfig.GetByGameType(criteria.GameType);
            HandHistoryDetail handHistory = new HandHistoryDetail();
            string languageCode = RnGUtils.CasinoLang;

            GameResult result;
            foreach (DataRow drow in tables[0].Rows)
            {
                result = GetGameResult(element.ResultType);
                result.BuildGameResult(drow, languageCode);

                handHistory.GameResults.Add(result);
            }

            BetTypeMoney betMoney;
            foreach (DataRow drow in tables[1].Rows)
            {
                betMoney = GetBetTypeMoney(element.BetType);
                betMoney.BuildBetTypeMoney(drow, languageCode);

                handHistory.BetTypeMoneys.Add(betMoney);
            }

            return handHistory;
        }

        /// <summary>
        /// Hands the history result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The data table.</returns>
        private DataTable HandHistoryResult(HandHistoryCriteria criteria)
        {
            // Hardcode CasinoNo = 01 for tuning performance.
            string casinoGroup1 = CasinoBO.CASINO_GROUP1;
            if (criteria.IBCRefNo != 0)
            {
                return _casinoReport.WinLossHandHistory(criteria.IBCRefNo, criteria.BetType, casinoGroup1, criteria.ShowPic);
            }

            if (criteria.IBCCustId != 0)
            {
                return _casinoReport.RealTimeWinLossHandHistory(criteria.FDate, criteria.TDate, criteria.IBCCustId, casinoGroup1, criteria.ShowPic);                
            }

            return null;
        }

        /// <summary>
        /// Parses the hand history.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <param name="lang">The language code.</param>
        /// <returns>
        /// The Hand History object.
        /// </returns>
        private HandHistory ParseHandHistory(DataRow row, string lang)
        {
            HandHistory handHistory = new HandHistory
            {
                GameId = Converter.ToInt(row["GameID"]),
                PlayerHistoryNo = Converter.ToString2(row["PlayerHistoryNo"]),
                GameTypeNo = Converter.ToString2(row["GameTypeNo"]),
                GameType = Converter.ToString2(row["GameTypeName" + lang]),
                GameGroup = Converter.ToString2(row["GameGroupName" + lang]),
                StartDate = Converter.ToString2(row["StartTime"]),
                EndDate = Converter.ToString2(row["EndTime"]),
                StartBalance = Converter.ToDecimal(row["BeginBalance"]),
                EndBalance = Converter.ToDecimal(row["EndBalance"]),
                Bet = Converter.ToDecimal(row["Bet"]),
                Stake = Converter.ToDecimal(row["Stake"]),
                WinLoss = Converter.ToDecimal(row["WinLoss"]),
                RollingComm = Converter.ToDecimal(row["RollComm"]),
                UserBetId = Converter.ToInt(row["UserBetID"]),
                Result = Converter.ToString2(row["Result" + lang]),
                PositionTaking = new SMAStruct 
                {
                    AG = Converter.ToDecimal(row["AG%"]) * 100,
                    MA = Converter.ToDecimal(row["MA%"]) * 100,
                    SA = Converter.ToDecimal(row["SH%"]) * 100
                },
                CommissionRate = new SMAStruct 
                {
                    AG = Converter.ToDecimal(row["AgentRollingRate"]) * 100,
                    MA = Converter.ToDecimal(row["MAgentRollingRate"]) * 100,
                    SA = Converter.ToDecimal(row["SMAgentRollingRate"]) * 100
                },
                Status = Converter.ToInt(row["Status"])
            };

            HandHistoryElement handHistoryElement = GameTypeConfig.GetByGameType(handHistory.GameTypeNo);
            if (handHistoryElement == null)
            {
                // Get the default page.
                handHistoryElement = GameTypeConfig.GetByGameType("default");    
            }

            string url = handHistoryElement.Url.Contains(".aspx") ? handHistoryElement.Url : "RnGCasino/HandHistory/ViewDetail";
            handHistory.Url = string.Format("{0}?userbetid={1}&gametype={2}", url, handHistory.UserBetId, handHistory.GameTypeNo);
            handHistory.Width = handHistoryElement.Width;
            
            return handHistory;
        }

        private DataTable[] GetWinLossDetail(int userBetId)
        {
            return _casinoReport.WinLossDetail(userBetId);
        }

        private GameResult GetGameResult(string type)
        {
            Type objectType = Type.GetType("Fanex.Razorbill.Module.RnGCasino.DTOs." + type);
            return (GameResult)Activator.CreateInstance(objectType);
        }

        private BetTypeMoney GetBetTypeMoney(string type)
        {
            Type objectType = Type.GetType("Fanex.Razorbill.Module.RnGCasino.DTOs." + type);
            return (BetTypeMoney)Activator.CreateInstance(objectType);
        }
    }
}
