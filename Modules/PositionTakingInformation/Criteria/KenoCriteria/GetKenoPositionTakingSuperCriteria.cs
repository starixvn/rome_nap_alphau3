// --------------------------------------------------------------------------
// <copyright file="GetKenoPositionTakingSuperCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Razorbill.Module.PositionTaking.Criteria.KenoCriteria
{
    using System.Collections;

    /// <summary>
    /// Get Keno Position Talking Super Criteria.
    /// </summary>
    public class GetKenoPositionTakingSuperCriteria : GetKenoPositionTakingCriteria
    {
        public static GetKenoPositionTakingSuperCriteria Create(GetKenoPositionTakingCriteria criteria)
        {
            return new GetKenoPositionTakingSuperCriteria
            {
                CustId = criteria.CustId,
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize
            };
        }

        public override Hashtable GetSpParam()
        {
            Hashtable htb = new Hashtable();
            htb.Add("srecommend", CustId);
            htb.Add("pageindex", PageIndex);
            htb.Add("pagesize", PageSize);
            htb.Add("totalpages", TotalPage);
            return htb;
        }

        public override string GetSettingKey()
        {
            return "Age_CustomerSelKenoPositionTakingBySAg";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}