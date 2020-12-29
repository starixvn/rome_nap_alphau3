namespace Fanex.BetList.Age.BetListCmd
{
    using System;
    using System.Data;
    using Fanex.BetList.Age.Definition;
    using Sunplus.Agent.Model.BL.BetList;
    using Sunplus.Agent.Model.BL.Reports;

    public class WLByBetTypeCmd : BetListCmd
    {
        private BetListBO betList = null;

        private int betType = 0;
        private DateTime fdate, tdate;

        public WLByBetTypeCmd(string sessionId)
        {
            this.betList = new BetListBO(sessionId);
        }

        public override void SetParams(System.Collections.Hashtable hash)
        {
            this.betType = (int)hash["bettype"];
            this.fdate = (DateTime)hash["fdate"];
            this.tdate = (DateTime)hash["tdate"];

            if (hash["DateIsNull"] != null)
            {
                Title = Resources.BetList.lblMemberBetList;
            }
            else
            {
                Title = string.Join(null, new string[] { Resources.BetList.lblMemberBetList, " ", this.fdate.ToShortDateString(), " --> ", this.tdate.ToShortDateString() });
            }

            BetListType = BetListType.Member | BetListType.Winlost;
        }

        public override object Execute()
            => betList.GetWinlossByBetType(this.fdate, this.tdate, ReportsBO.MoveBetsDate, betType);

        public override DataTable OutDataTable()
            => new DataTable();
    }
}