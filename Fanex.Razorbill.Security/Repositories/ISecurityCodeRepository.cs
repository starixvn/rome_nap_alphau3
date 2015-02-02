// ************************************************************************************
// <copyright file="ISecurityCodeRepository.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ************************************************************************************
// <history>
//     <change who="kan.nguyen">Create/Run StyleCop.</change>
// </history>
// ************************************************************************************
namespace Fanex.Razorbill.Security.Repositories
{
    using Fanex.Razorbill.Security.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISecurityCodeRepository
    {
        string GetSecurityCodeOfMainAccount(int custId);

        string GetSecurityCodeOfSubAccount(int custId, int subAccountId);

        Result UpdateSecurityCodeStatusOfMainAccount(
            int custId,
            bool validSecurityCode,
            string language,
            string clientIP);

        Result UpdateSecurityCodeStatusOfSubAccount(
            int custId,
            int subAccountId,
            bool validSecurityCode,
            string language,
            string clientIP);

        Result ChangeSecurityCodeOfMainAccount(
            int custId,
            int userId,
            int subAccountId,
            string securityCode,
            string language,
            string clientIP);

        Result ChangeSecurityCodeOfSubAccount(
            int custId,
            int userId,
            int subAccountId,
            string securityCode,
            string language,
            string clientIP);

        Result ChangeSecurityCodeOfSubAccount2(
            int custId,
            int userId,
            int subAccountId,
            string securityCode,
            string language,
            string clientIP);
    }
}
