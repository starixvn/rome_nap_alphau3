// --------------------------------------------------------------------------
// <copyright file="IKenoPositionTakingInformationRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Razorbill.Module.PositionTaking.Repositories.KenoPositionTaking
{
    using Fanex.Razorbill.Module.PositionTaking.Criteria.KenoCriteria;
    using Fanex.Razorbill.Module.PositionTaking.Entities.KenoEntities;

    public interface IKenoPositionTakingInformationRepository
    {
        KenoPositionTakingEntityList GetKenoPositionTakingInformationSuper(GetKenoPositionTakingSuperCriteria criteria);

        KenoPositionTakingEntityList GetKenoPositionTakingInformationMaster(GetKenoPositionTakingMasterCriteria criteria);

        KenoPositionTakingEntityList GetKenoPositionTakingInformationAgent(GetKenoPositionTakingAgentCriteria criteria);
    }
}
