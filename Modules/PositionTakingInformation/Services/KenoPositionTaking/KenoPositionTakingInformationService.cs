// --------------------------------------------------------------------------
// <copyright file="KenoPositionTakingInformationService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Razorbill.Module.PositionTaking.Services.KenoPositionTaking
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Razorbill.Module.Core.Services.Impl;
    using Fanex.Razorbill.Module.PositionTaking.Constants;
    using Fanex.Razorbill.Module.PositionTaking.Criteria.KenoCriteria;
    using Fanex.Razorbill.Module.PositionTaking.Entities.KenoEntities;
    using Fanex.Razorbill.Module.PositionTaking.Models;
    using Fanex.Razorbill.Module.PositionTaking.Models.KenoModels;
    using Fanex.Razorbill.Module.PositionTaking.Repositories.KenoPositionTaking;
    using Fanex.Razorbill.Module.PositionTakingInformation.Utils;

    /// <summary>
    /// Keno Position Taking Information Service.
    /// </summary>
    public class KenoPositionTakingInformationService : IKenoPositionTakingInformationService
    {
        private IKenoPositionTakingInformationRepository _kenoPositionTakingInformationRepository;

        public KenoPositionTakingInformationService()
            : this(null)
        {
        }

        public KenoPositionTakingInformationService(IKenoPositionTakingInformationRepository kenoPositionTakingInformationRepository)
        {
            _kenoPositionTakingInformationRepository = kenoPositionTakingInformationRepository ?? new KenoPositionTakingInformationRepository();
        }

        public List<int> GetPageSizeList()
        {
            return new List<int>() { 10, 50, 100, 500 };
        }

        /// <summary>
        /// Gets the keno position taking information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The keno position taking information.</returns>
        public ViewCustomerPositionTakingKenoModel GetKenoPositionTakingInformation(GetKenoPositionTakingCriteria criteria)
        {
            KenoPositionTakingEntityList rawData = GetKenoPositionTakingEntityList(criteria);
            ViewCustomerPositionTakingKenoModel positionTakingList = new ViewCustomerPositionTakingKenoModel
            {
                BetTypes = new List<BetTypeModel>(),
                CustomerInfo = new CustomerInfo
                {
                    CustId = criteria.CustId,
                    RoleId = criteria.RoleId
                },
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                CustomerList = new List<CustomerPositionTakingModel>()
            };
            if (rawData == null || !rawData.HasBetType.Any())
            {
                return positionTakingList;
            }

            positionTakingList.TotalPage = rawData.TotalPage;
            BettypesActive(rawData, positionTakingList);

            if (!rawData.CustPT.Any())
            {
                return positionTakingList;
            }

            CustomerPTInformation(criteria, rawData, positionTakingList);

            return positionTakingList;
        }

        /// <summary>
        /// Gets the keno position taking entity list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The keno position taking entity list.</returns>
        public KenoPositionTakingEntityList GetKenoPositionTakingEntityList(GetKenoPositionTakingCriteria criteria)
        {
            switch (criteria.RoleId)
            {
                case RoleIdConstants.Super:
                    return _kenoPositionTakingInformationRepository.GetKenoPositionTakingInformationSuper(GetKenoPositionTakingSuperCriteria.Create(criteria));
                case RoleIdConstants.Master:
                    return _kenoPositionTakingInformationRepository.GetKenoPositionTakingInformationMaster(GetKenoPositionTakingMasterCriteria.Create(criteria));
                case RoleIdConstants.Agent:
                    return _kenoPositionTakingInformationRepository.GetKenoPositionTakingInformationAgent(GetKenoPositionTakingAgentCriteria.Create(criteria));
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the up line information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="custPT">The customer PT.</param>
        /// <returns>The up line information.</returns>
        public List<CustomerPositionTakingModel> GetUplineInfo(
            GetKenoPositionTakingCriteria criteria,
            KenoPositionTakingEntity custPT)
        {
            List<CustomerPositionTakingModel> result = null;
            KenoPositionTakingEntity superPT;
            KenoPositionTakingEntity masterPT;
            if (SessionExtension.RoleId == RoleIdConstants.Agent)
            {
                return result;
            }

            switch (criteria.RoleId)
            {
                case RoleIdConstants.Master:
                    result = new List<CustomerPositionTakingModel>();
                    masterPT = custPT.UplineValue[0];
                    result.Add(new CustomerPositionTakingModel
                    {
                        PositionTakingList = GetPTList(masterPT.PTValue)
                    });
                    break;
                case RoleIdConstants.Agent:
                    result = new List<CustomerPositionTakingModel>();
                    superPT = custPT.UplineValue[0];
                    masterPT = custPT.UplineValue[1];
                    result.Add(new CustomerPositionTakingModel
                    {
                        PositionTakingList = GetPTList(superPT.PTValue)
                    });
                    result.Add(new CustomerPositionTakingModel
                    {
                        PositionTakingList = GetPTList(masterPT.PTValue)
                    });
                    break;
                default:
                    break;
            }

            return result;
        }

        public List<PositionTakingInformationModel> GetPTList(Dictionary<int, decimal> positionTakingValue)
        {
            return positionTakingValue.Select(keyPair => new PositionTakingInformationModel
                {
                    Value = keyPair.Value
                })
                .ToList();
        }

        /// <summary>
        /// Customer's PT information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="rawData">The raw data.</param>
        /// <param name="positionTakingList">The PT object.</param>
        private void CustomerPTInformation(
            GetKenoPositionTakingCriteria criteria,
            KenoPositionTakingEntityList rawData,
            ViewCustomerPositionTakingKenoModel positionTakingList)
        {
            foreach (var custPT in rawData.CustPT)
            {
                positionTakingList.CustomerList.Add(new CustomerPositionTakingModel
                {
                    CustomerInfo = new CustomerInfo
                    {
                        CustId = custPT.CustId,
                        CustName = custPT.Username
                    },
                    PositionTakingList = GetPTList(custPT.PTValue),
                    UplineInfo = GetUplineInfo(criteria, custPT)
                });
            }
        }

        /// <summary>
        /// Get bet-types active.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <param name="positionTakingList">The PT object.</param>
        private void BettypesActive(
            KenoPositionTakingEntityList rawData, 
            ViewCustomerPositionTakingKenoModel positionTakingList)
        {            
            var bettypesActive = from bettype in rawData.HasBetType
                                 where bettype.Value == true
                                 select new BetTypeModel()
                                 {
                                     Id = bettype.Key,
                                     Name = CacheRef.GetBettype(bettype.Key.ToString())
                                 };
            
            if (bettypesActive.Any())
            {
                positionTakingList.BetTypes = bettypesActive.ToList();
            }            
        }
    }
}