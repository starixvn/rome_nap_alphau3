// --------------------------------------------------------------------------
// <copyright file="KenoPositionTakingInformationController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Razorbill.Module.PositionTaking.Controllers
{
    using System.Web.Mvc;
    using Fanex.Razorbill.Module.PositionTaking.Criteria.KenoCriteria;
    using Fanex.Razorbill.Module.PositionTaking.Services.KenoPositionTaking;
    using Fanex.Razorbill.Module.PositionTakingInformation.Utils;

    /// <summary>
    /// Keno Position Taking Information Controller.
    /// </summary>
    public class KenoPositionTakingInformationController : PositionTakingInforBaseController
    {
        private IKenoPositionTakingInformationService _kenoPositionTakingInformationService;

        public KenoPositionTakingInformationController()
            : this(null)
        {
        }

        public KenoPositionTakingInformationController(KenoPositionTakingInformationService kenoPositionTakingInformationService)
        {
            _kenoPositionTakingInformationService = kenoPositionTakingInformationService ?? new KenoPositionTakingInformationService();
        }

        /// <summary>
        /// Gets the position taking.
        /// </summary>
        /// <returns>The position taking.</returns>
        public ActionResult GetPositionTaking()
        {
            var pageSizeList = _kenoPositionTakingInformationService.GetPageSizeList();
            var maxPageSize = pageSizeList != null && pageSizeList.Count > 0 ?
                pageSizeList[pageSizeList.Count - 1] :
                0;
            var getPositionInfoCriteria = new GetKenoPositionTakingCriteria
            {
                CustId = SessionExtension.CustId,
                PageIndex = 1,
                PageSize = maxPageSize,
                RoleId = SessionExtension.RoleId
            };

            var bootstrapData = new
            {
                PageSizeList = pageSizeList,
                PositionInfo = _kenoPositionTakingInformationService.GetKenoPositionTakingInformation(getPositionInfoCriteria)
            };

            bootstrapData.PositionInfo.CustomerInfo = new Models.CustomerInfo()
            {
                CustId = getPositionInfoCriteria.CustId,
                RoleId = getPositionInfoCriteria.RoleId
            };
            
            return View("~/Areas/PositionTakingInformation/Views/Keno/Index.cshtml", bootstrapData);
        }

        [HttpPost]
        public JsonResult GetPositionTaking(GetKenoPositionTakingCriteria getPositionInfoCriteria)
        {
            getPositionInfoCriteria = getPositionInfoCriteria ?? new GetKenoPositionTakingCriteria();
            return Json(_kenoPositionTakingInformationService.GetKenoPositionTakingInformation(getPositionInfoCriteria), JsonRequestBehavior.AllowGet);
        }
    }
}
