// --------------------------------------------------------------------------
// <copyright file="GetKenoPositionTakingAgentCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get Keno Position Talking Agent Criteria.
    /// </summary>
    public class GetKenoPositionTakingAgentCriteria : GetKenoPositionTakingCriteria
    {
        public static GetKenoPositionTakingAgentCriteria Create(GetKenoPositionTakingCriteria criteria)
        {
            return new GetKenoPositionTakingAgentCriteria
            {
                CustId = criteria.CustId,
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize
            };
        }

        public override Hashtable GetSpParam()
        {
            Hashtable htb = new Hashtable();
            htb.Add("recommend", CustId);
            htb.Add("pageindex", PageIndex);
            htb.Add("pagesize", PageSize);
            htb.Add("totalpages", TotalPage);
            return htb;
        }

        public override string GetSettingKey()
        {
            return "Age_CustomerSelKenoPositionTakingByAg";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}