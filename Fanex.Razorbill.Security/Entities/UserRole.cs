// ************************************************************************************
// <copyright file="UserRole.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ************************************************************************************
// <history>
//     <change who="kan.nguyen">Create/Run StyleCop.</change>
// </history>
// ************************************************************************************
namespace Fanex.Razorbill.Security.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum UserRole : int
    {
        /// <summary>
        /// The administrator.
        /// </summary>
        Admin = 5,
        
        /// <summary>
        /// The super role.
        /// </summary>
        Super = 4,
        
        /// <summary>
        /// The master role.
        /// </summary>
        Master = 3,
       
        /// <summary>
        /// The agent role.
        /// </summary>
        Agent = 2,
        
        /// <summary>
        /// The member role.
        /// </summary>
        Member = 1,
        
        /// <summary>
        /// The unknown role.
        /// </summary>
        Unknown = 0
    }
}
