// --------------------------------------------------------------------------
// <copyright file="PositionTakingInformationModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Razorbill.Module.PositionTaking.Models
{
    /// <summary>
    /// Position Taking Information Model.
    /// </summary>
    public class PositionTakingInformationModel
    {
        public decimal Value { get; set; }

        public int PositionTakingTargetId { get; set; }

        public string PositionTakingTargetName { get; set; }
    }
}