// --------------------------------------------------------------------------
// <copyright file="IKenoPositionTakingInformationService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Razorbill.Module.PositionTaking.Services.KenoPositionTaking
{
    using System.Collections.Generic;
    using Fanex.Razorbill.Module.PositionTaking.Criteria.KenoCriteria;
    using Fanex.Razorbill.Module.PositionTaking.Models.KenoModels;

    public interface IKenoPositionTakingInformationService
    {
        List<int> GetPageSizeList();

        ViewCustomerPositionTakingKenoModel GetKenoPositionTakingInformation(GetKenoPositionTakingCriteria criteria);
    }
}
