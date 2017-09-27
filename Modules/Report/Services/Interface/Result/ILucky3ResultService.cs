namespace Fanex.Razorbill.Module.Report.Services.Interface.Result
{
    using System;
    using System.IO;

    public interface ILucky3ResultService
    {
        string GetResult(DateTime fromDate, int leagueId, int sportType);

        MemoryStream ExportExcel(DateTime fromDate, int sportType, int leagueId, string excelTemplatePath);
    }
}