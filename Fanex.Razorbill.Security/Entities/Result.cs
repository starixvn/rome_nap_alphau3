// ************************************************************************************
// <copyright file="Result.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ************************************************************************************
// <history>
//     <change who="kan.nguyen">Create/Run StyleCop.</change>
// </history>
// ************************************************************************************
namespace Fanex.Razorbill.Security.Entities
{
    using System;

    /// <summary>
    /// Result Entity.
    /// </summary>
    [Serializable]
    public class Result
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}