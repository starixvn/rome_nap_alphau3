// --------------------------------------------------------------------------
// <copyright file="ViewCustomerPositionTakingModel.cs" company="Nexcel Solutions Vietnam">
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
    /// View Customer Position Taking Model.
    /// </summary>
    public class ViewCustomerPositionTakingModel
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPage { get; set; }

        public CustomerInfo CustomerInfo { get; set; }

        public List<CustomerPositionTakingModel> CustomerList { get; set; }
    }
}