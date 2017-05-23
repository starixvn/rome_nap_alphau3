using Fanex.BetListDetail.Lucky3.Criteria;
using Fanex.NAP.Framework.Data;

namespace Fanex.Razorbill.Module.Report.Criteria.Lucky3
{
    public class GetLucky3DetailCriteria : CriteriaBase
    {
        public GetLucky3DetailCriteria(IGetLucky3DetailParameter commandData)
        {
            Criteria = commandData;
        }

        [SpParam(Ignored = true)]
        private IGetLucky3DetailParameter Criteria { get; set; }

        public long CustId => Criteria.CustomerId;

        public long TransId => Criteria.TransactionId;

        public override string GetSettingKey() => "Acc_BetList_Lucky3_Detail";

        public override bool IsValid() => Criteria.IsValid();
    }
}
