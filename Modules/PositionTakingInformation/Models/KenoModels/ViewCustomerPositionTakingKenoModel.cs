// --------------------------------------------------------------------------
// <copyright file="ViewCustomerPositionTakingKenoModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Razorbill.Module.PositionTaking.Models.KenoModels
{
    using System.Collections.Generic;
    using Fanex.Razorbill.Module.PositionTaking.Models;

    /// <summary>
    /// View Customer Position Taking Keno Model.
    /// </summary>
    public class ViewCustomerPositionTakingKenoModel : ViewCustomerPositionTakingModel
    {
        public List<BetTypeModel> BetTypes { get; set; }
    }
}