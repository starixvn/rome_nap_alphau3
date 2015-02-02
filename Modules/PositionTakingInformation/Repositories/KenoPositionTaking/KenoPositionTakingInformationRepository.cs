// --------------------------------------------------------------------------
// <copyright file="KenoPositionTakingInformationRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Razorbill.Module.PositionTaking.Repositories.KenoPositionTaking
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Razorbill.Module.PositionTaking.Configurations.KenoBetTypes;
    using Fanex.Razorbill.Module.PositionTaking.Criteria.KenoCriteria;
    using Fanex.Razorbill.Module.PositionTaking.Entities.KenoEntities;

    /// <summary>
    /// Keno Position Taking Information Repository.
    /// </summary>
    public class KenoPositionTakingInformationRepository : IKenoPositionTakingInformationRepository
    {
        private const int DefaultTimeOut = 600;
        private const char SplitChar = '_';
        private const string SuperPTPrefix = "spositiontaking";
        private const string MasterPTPrefix = "mpositiontaking";
        private const string AgentPTPrefix = "apositiontaking";

        /// <summary>
        /// Gets the super keno position taking information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The super keno position taking information.</returns>
        public KenoPositionTakingEntityList GetKenoPositionTakingInformationSuper(GetKenoPositionTakingSuperCriteria criteria)
        {
            KenoPositionTakingEntityList result = null;
            using (ObjectDb objectDB = new ObjectDb(criteria.GetSettingKey(), DefaultTimeOut))
            {
                objectDB.AssignParamValues(criteria.GetSpParam());
                var customerPTInforReader = objectDB.ExecuteReader();
                if (customerPTInforReader != null)
                {
                    result = new KenoPositionTakingEntityList
                    {
                        CustPT = new List<KenoPositionTakingEntity>(),
                        TotalPage = Converter.ToInt(objectDB.GetOuputParams()["totalpages"])
                    };

                    var positionTakingPrefix = string.Format("{0}{1}", SuperPTPrefix, SplitChar);
                    result.HasBetType = HasBetType(positionTakingPrefix, customerPTInforReader);
                    while (customerPTInforReader.Read())
                    {
                        var positionTakingInfo = new KenoPositionTakingEntity
                        {
                            Username = Converter.ToString2(customerPTInforReader["username"]),
                            CustId = Converter.ToInt(customerPTInforReader["custid"]),
                            PTValue = new Dictionary<int, decimal>()
                        };

                        foreach (var betType in result.HasBetType)
                        {
                            if (!betType.Value)
                            {
                                continue;
                            }

                            positionTakingInfo.PTValue.Add(
                                betType.Key, 
                                Converter.ToDecimal(customerPTInforReader[string.Format("{0}{1}", positionTakingPrefix, betType.Key)]));
                        }

                        result.CustPT.Add(positionTakingInfo);                        
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the master keno position taking information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The master keno position taking information.</returns>
        public KenoPositionTakingEntityList GetKenoPositionTakingInformationMaster(GetKenoPositionTakingMasterCriteria criteria)
        {
            KenoPositionTakingEntityList result = null;
            using (ObjectDb objectDB = new ObjectDb(criteria.GetSettingKey(), DefaultTimeOut))
            {
                objectDB.AssignParamValues(criteria.GetSpParam());
                var customerPTInforReader = objectDB.ExecuteReader();
                if (customerPTInforReader != null)
                {
                    result = new KenoPositionTakingEntityList
                    {
                        CustPT = new List<KenoPositionTakingEntity>(),
                        TotalPage = Converter.ToInt(objectDB.GetOuputParams()["totalpages"])
                    };

                    var positionTakingPrefix = string.Format("{0}{1}", AgentPTPrefix, SplitChar);
                    result.HasBetType = HasBetType(positionTakingPrefix, customerPTInforReader);
                    while (customerPTInforReader.Read())
                    {
                        var positionTakingInfo = new KenoPositionTakingEntity
                        {
                            Username = Converter.ToString2(customerPTInforReader["username"]),
                            CustId = Converter.ToInt(customerPTInforReader["custid"]),
                            PTValue = new Dictionary<int, decimal>(),
                            UplineValue = new List<KenoPositionTakingEntity>()
                        };
                        var uplineInfo = new KenoPositionTakingEntity
                        {
                            PTValue = new Dictionary<int, decimal>()
                        };

                        foreach (var betType in result.HasBetType)
                        {
                            if (!betType.Value)
                            {
                                continue;
                            }

                            positionTakingInfo.PTValue.Add(betType.Key, Converter.ToDecimal(customerPTInforReader[string.Format("{0}{1}", positionTakingPrefix, betType.Key)]));
                            uplineInfo.PTValue.Add(betType.Key, Converter.ToDecimal(customerPTInforReader[string.Format("{0}{1}{2}", MasterPTPrefix, SplitChar, betType.Key)]));
                        }

                        positionTakingInfo.UplineValue.Add(uplineInfo);
                        result.CustPT.Add(positionTakingInfo);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the agent keno position taking information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The agent keno position taking information.</returns>
        public KenoPositionTakingEntityList GetKenoPositionTakingInformationAgent(GetKenoPositionTakingAgentCriteria criteria)
        {
            KenoPositionTakingEntityList result = null;
            using (ObjectDb objectDB = new ObjectDb(criteria.GetSettingKey(), DefaultTimeOut))
            {
                objectDB.AssignParamValues(criteria.GetSpParam());
                var customerPTInforReader = objectDB.ExecuteReader();
                if (customerPTInforReader != null)
                {
                    result = new KenoPositionTakingEntityList
                    {
                        CustPT = new List<KenoPositionTakingEntity>(),
                        TotalPage = Converter.ToInt(objectDB.GetOuputParams()["totalpages"])
                    };
                    
                    var positionTakingPrefix = string.Format("{0}{1}", AgentPTPrefix, SplitChar);
                    result.HasBetType = HasBetType(positionTakingPrefix, customerPTInforReader);
                    while (customerPTInforReader.Read())
                    {
                        var positionTakingInfo = new KenoPositionTakingEntity
                        {
                            Username = Converter.ToString2(customerPTInforReader["username"]),
                            CustId = Converter.ToInt(customerPTInforReader["custid"]),
                            PTValue = new Dictionary<int, decimal>()
                        };
                        var masterPT = new KenoPositionTakingEntity
                        {
                            PTValue = new Dictionary<int, decimal>()
                        };
                        var superPT = new KenoPositionTakingEntity
                        {
                            PTValue = new Dictionary<int, decimal>()
                        };
                        foreach (var betType in result.HasBetType)
                        {
                            if (betType.Value)
                            {
                                positionTakingInfo.PTValue.Add(betType.Key, Converter.ToDecimal(customerPTInforReader[string.Format("{0}{1}", positionTakingPrefix, betType.Key)]));
                                superPT.PTValue.Add(betType.Key, Converter.ToDecimal(customerPTInforReader[string.Format("{0}{1}{2}", SuperPTPrefix, SplitChar, betType.Key)]));
                                masterPT.PTValue.Add(betType.Key, Converter.ToDecimal(customerPTInforReader[string.Format("{0}{1}{2}", MasterPTPrefix, SplitChar, betType.Key)]));
                            }
                        }

                        positionTakingInfo.UplineValue = new List<KenoPositionTakingEntity>() { superPT, masterPT };
                        result.CustPT.Add(positionTakingInfo);
                    }
                }
            }

            return result;
        }

        private bool ColumnExists(IDataReader reader, string columnName)
        {
            reader.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", columnName);
            return reader.GetSchemaTable().DefaultView.Count > 0;
        }

        /// <summary>
        /// Determines whether [has bet type] [the specified position taking prefix].
        /// </summary>
        /// <param name="positionTakingPrefix">The position taking prefix.</param>
        /// <param name="customerPTInforReader">The data reader.</param>
        /// <returns>Bet type active indicator list.</returns>        
        private Dictionary<int, bool> HasBetType(
            string positionTakingPrefix,
            IDataReader customerPTInforReader)
        {
            Dictionary<int, bool> hasBetType = new Dictionary<int, bool>();
            var kenoSection = KenoBetTypesSection.Instance;
            if (kenoSection.Active &&
                kenoSection.BetTypes.Count > 0)
            {
                var kenoBetTypes = kenoSection.BetTypes;
                foreach (KenoBetTypesElement betType in kenoBetTypes)
                {
                    if (!betType.Active)
                    {
                        continue;
                    }
                    
                    hasBetType.Add(
                        Converter.ToInt(betType.Id),
                        ColumnExists(customerPTInforReader, string.Format("{0}{1}", positionTakingPrefix, betType.Id)));
                }
            }
            else
            {
                var totalColumns = customerPTInforReader.FieldCount;
                for (int i = 0; i < totalColumns; i++)
                {
                    var currentColumnName = customerPTInforReader.GetName(i);
                    if (currentColumnName.IndexOf(positionTakingPrefix, StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        continue;
                    }

                    hasBetType.Add(Converter.ToInt(currentColumnName.Split(SplitChar)[1]), true);
                }
            }

            return hasBetType;
        }
    }
}