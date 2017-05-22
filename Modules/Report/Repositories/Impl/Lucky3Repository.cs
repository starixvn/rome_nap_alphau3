using Fanex.BetListDetail.Lucky3.Criteria;
using Fanex.BetListDetail.Lucky3.DataAccesses;
using Fanex.BetListDetail.Lucky3.Models;
using Fanex.NAP.Framework.Data;
using Fanex.Razorbill.Module.Report.Criteria.Lucky3;
using System.Collections.Generic;

namespace Fanex.Razorbill.Module.Report.Repositories.Impl
{
    public class Lucky3Repository : ILucky3DataAccess
    {
        private readonly IDynamicRepository repository;

        public Lucky3Repository() : this(null)
        {
        }

        public Lucky3Repository(IDynamicRepository repository)
        {
            this.repository = repository ?? new DynamicRepository();
        }

        public IEnumerable<Lucky3DetailModel> GetLucky3Detail(GetLucky3DetailParameter parameter)
        {
            var criteria = new GetLucky3DetailCriteria(parameter);

            return repository.Fetch<Lucky3DetailModel>(criteria);
        }
    }
}
