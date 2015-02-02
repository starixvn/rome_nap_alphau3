// --------------------------------------------------------------------------
// <copyright file="CustomerPositionTakingModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Razorbill.Module.PositionTaking.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Customer Position Taking Model.
    /// </summary>
    public class CustomerPositionTakingModel
    {
        public CustomerInfo CustomerInfo { get; set; }

        public List<PositionTakingInformationModel> PositionTakingList { get; set; }

        public List<CustomerPositionTakingModel> UplineInfo { get; set; }
    }
}