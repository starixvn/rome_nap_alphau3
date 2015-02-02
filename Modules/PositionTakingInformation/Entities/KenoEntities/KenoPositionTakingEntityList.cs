// --------------------------------------------------------------------------
// <copyright file="KenoPositionTakingEntityList.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Razorbill.Module.PositionTaking.Entities.KenoEntities
{
    using System.Collections.Generic;

    /// <summary>
    /// Keno Position Taking Entity List.
    /// </summary>
    public class KenoPositionTakingEntityList
    {
        public List<KenoPositionTakingEntity> CustPT { get; set; }

        public int TotalPage { get; set; }

        public Dictionary<int, bool> HasBetType { get; set; }
    }
}