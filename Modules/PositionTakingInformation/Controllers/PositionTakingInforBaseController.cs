// --------------------------------------------------------------------------
// <copyright file="PositionTakingInforBaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Razorbill.Module.PositionTaking.Controllers
{
    using System.Globalization;
    using System.Threading;
    using Fanex.Razorbill.Module.Core.Web.Controllers;
    using Fanex.Razorbill.Module.PositionTakingInformation.Utils;

    /// <summary>
    /// Position Taking Information Base Controller.
    /// </summary>
    public class PositionTakingInforBaseController : BaseController
    {
        public PositionTakingInforBaseController()
        {
            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new CultureInfo(SessionExtension.Language);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
    }
}
