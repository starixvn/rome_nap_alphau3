namespace Fanex.Razorbill.Module.Common.Criteria
{
    using System.Collections;
    using Fanex.NAP.Framework.Data;

    public class GetIPInformationCriteria : CriteriaBase
    {
        public string Ip { get; set; }

        public override string GetSettingKey()
            => "Acc_GetIPInfo_Full";
    }
}