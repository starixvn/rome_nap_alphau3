using Fanex.BetListDetail.Lucky3.Criteria;
using Fanex.BetListDetail.Lucky3.Services;
using Fanex.Razorbill.Module.Core.Web.Controllers;
using Fanex.Razorbill.Module.Report.Repositories.Impl;
using System.Web.Mvc;

namespace Fanex.Razorbill.Module.Report.Controllers
{
    public class Lucky3Controller : BaseController
    {
        private readonly ILucky3Service lucky3Service;

        public Lucky3Controller() : this(null)
        {
        }

        public Lucky3Controller(ILucky3Service lucky3Service)
        {
            this.lucky3Service = lucky3Service ?? new Lucky3Service(new Lucky3Repository());
        }

        public ActionResult ViewDetail(GetLucky3DetailParameter parameter)
        {
            var result = lucky3Service.GetBetListDetail(parameter);

            return View(result);
        }
    }
}
