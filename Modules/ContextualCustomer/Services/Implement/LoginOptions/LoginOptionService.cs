// -----------------------------------------------------------------------
// <copyright file="LoginOptionService.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Razorbill.Module.ContextualCustomer.Services.Implement
{
    using System.Web;
    using Fanex.Common;
    using Fanex.Razorbill.Module.ContextualCustomer.Criteria.LoginOptions;
    using Fanex.Razorbill.Module.ContextualCustomer.Entities;
    using Fanex.Razorbill.Module.ContextualCustomer.Repositories.Impl;
    using Fanex.Razorbill.Module.ContextualCustomer.Repositories.Interface;
    using Fanex.Razorbill.Module.ContextualCustomer.Services.Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LoginOptionService : ILoginOptionService
    {
        private ILoginOptionRepository _loginOptionRepository;
        private string _nickname = string.Empty;
        private int _custID = 0;
        private string _language = string.Empty;
        private int _adminID = 0;
        private dynamic _pageAccess = null;
        private bool _isLoginByNickname = false;
        private string _sessionID = string.Empty;

        public LoginOptionService() : this(null)
        {
        }

        public LoginOptionService(ILoginOptionRepository loginOptionRepository)
        {
            _loginOptionRepository = loginOptionRepository ?? new LoginOptionRepository();
            _sessionID = HttpContext.Current.Session.SessionID;
            _nickname = Converter.ToString2(HttpContext.Current.Session[string.Format("{0}_NICKNAME", _sessionID)]);
            _custID = Converter.ToInt(HttpContext.Current.Session[string.Format("{0}_CUSTID", _sessionID)]);
            _language = Converter.ToString2(HttpContext.Current.Session[string.Format("{0}_LANGKEY", _sessionID)]);
            _adminID = Converter.ToInt(HttpContext.Current.Session[string.Format("{0}_ADMINID", _sessionID)]);
            _pageAccess = HttpContext.Current.Session[string.Format("{0}_SUBPAGEACCESS", _sessionID)];
            _isLoginByNickname = Converter.ToBool(HttpContext.Current.Session[string.Format("{0}_ISNICKNAMELOGINONLY", _sessionID)]);
        }

        /// <summary>
        /// Setting nickname.
        /// </summary>
        /// <param name="nickname">Nick name.</param>
        /// <param name="customerIp">Customer IP address.</param>
        /// <returns>Update result.</returns>
        public Result SettingNickname(string nickname, string customerIp)
        {
            if (_custID == 0 ||
                string.IsNullOrEmpty(nickname) ||
                nickname.Length < 5 ||
                nickname.Length > 15)
            {
                return new Result()
                {
                    ErrorCode = 100,
                    ErrorMessage = Resources.LoginOptions.msgValidNickName
                };
            }

            var nicknameCriteria = new SettingNicknameCriteria()
            {
                CustId = _custID,
                Nickname = nickname,
                IP = customerIp,
                Language = _language,
                ErrCode = 0,
                ErrMsg = string.Empty
            };

           return _loginOptionRepository.SettingNickname(nicknameCriteria);
        }

        /// <summary>
        /// Updating login option.
        /// </summary>
        /// <param name="isLoginByNickname">Login by nick-name.</param>
        /// <param name="nickname">Nick name.</param>
        /// <param name="customerIp">Customer IP address.</param>
        /// <returns>Update result.</returns>
        public Result UpdatingLoginOption(bool isLoginByNickname, string nickname, string customerIp)
        {
            if (_adminID > 0 || _pageAccess == null || _pageAccess.UpdateAccount == null || !_pageAccess.UpdateAccount)
            {
                return new Result()
                {
                    ErrorCode = 100,
                    ErrorMessage = Resources.LoginOptions.lblNotAuthorized
                };
            }

            var result = new Result()
            {
                ErrorCode = 1,
                ErrorMessage = Resources.LoginOptions.noChangeToUpdate
            };

            bool isFirstTimeSettingNickname = false;
            if (_nickname == null || string.IsNullOrEmpty(_nickname))
            {
                result = SettingNickname(nickname, customerIp);
                if (result.ErrorCode != 0)
                {
                    return result;
                }

                isFirstTimeSettingNickname = true;
                _nickname = nickname.ToUpper();
                HttpContext.Current.Session[string.Format("{0}_NICKNAME", _sessionID)] = nickname.ToUpper();
            }
            
            if (isLoginByNickname != _isLoginByNickname)
            {
                HttpContext.Current.Session[string.Format("{0}_ISNICKNAMELOGINONLY", _sessionID)] = isLoginByNickname;
                result = _loginOptionRepository.UpdatingLoginOption(new UpdatingLoginOptionCriteria()
                {
                    CustId = _custID,
                    Language = _language,
                    IP = customerIp,
                    NicknameLoginOnly = isLoginByNickname,
                    ErrCode = 0,
                    ErrMsg = string.Empty
                });
            }

            result.ErrorMessage = result.ErrorCode != 0 ? 
                result.ErrorMessage :
                (isFirstTimeSettingNickname ? Resources.LoginOptions.configuredNicknameSuccessfully : Resources.LoginOptions.updatesuccessfull);
            
            return result;
        }
    }
}