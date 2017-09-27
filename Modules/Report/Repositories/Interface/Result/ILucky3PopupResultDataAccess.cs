namespace Fanex.Razorbill.Module.Report.Repositories.Interface.Result
{
    using System.Collections.Generic;
    using Fanex.Result.Core.Lucky3.DTO;
    using NAP.Framework.Data;

    public interface ILucky3PopupResultDataAccess
    {
        IList<Lucky3> GetLucky3PopupResult(ICriteria criteria);
    }
}
