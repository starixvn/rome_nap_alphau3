// --------------------------------------------------------------------------
// <copyright file="GetKenoPositionTakingCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get Keno Position Talking Criteria.
    /// </summary>
    public class GetKenoPositionTakingCriteria
    {
        public int CustId { get; set; }

        public int RoleId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPage { get; set; }

        public virtual Hashtable GetSpParam()
        {
            Hashtable htb = new Hashtable();
            return htb;
        }

        public virtual string GetSettingKey()
        {
            return string.Empty;
        }

        public virtual bool IsValid()
        {
            return true;
        }
    }
}