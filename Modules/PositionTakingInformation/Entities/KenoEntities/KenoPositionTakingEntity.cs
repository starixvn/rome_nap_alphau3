// --------------------------------------------------------------------------
// <copyright file="KenoPositionTakingEntity.cs" company="Nexcel Solutions Vietnam">
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
    /// Keno Position Taking Entity.
    /// </summary>
    public class KenoPositionTakingEntity
    {
        public string Username { get; set; }

        public int CustId { get; set; }

        public Dictionary<int, decimal> PTValue { get; set; }

        public List<KenoPositionTakingEntity> UplineValue { get; set; }
    }
}