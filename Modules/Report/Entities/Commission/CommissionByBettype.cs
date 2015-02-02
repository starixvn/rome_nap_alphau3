// --------------------------------------------------------------------------
// <copyright file="CommissionByBettype.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.09.30">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Entities.CommissionByBettype
{
    using System;

    /// <summary>
    /// FooBetReport class.
    /// </summary>
    [Serializable]
    public class CommissionByBettype
    {
        public int CustId { get; set; }

        public int Recommend { get; set; }

        public int Mrecommend { get; set; }

        public string UserName { get; set; }

        public decimal TotalComm_1 { get; set; }

        public decimal TotalComm_2 { get; set; }

        public decimal TotalComm_3 { get; set; }

        public decimal Casino_Comm { get; set; }
    }
}