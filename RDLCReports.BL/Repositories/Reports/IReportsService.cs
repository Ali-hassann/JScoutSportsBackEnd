using AMNSystemsERP.CL.Models.RDLCModels;
using Microsoft.AspNetCore.Mvc;

namespace SMSRdlcReports.BL.Repositories.Reports
{
    public interface ICommonRDLCReportsService
    {
        Task<byte[]> GenerateRdlcReport<T>(RdlcReportRequest<T> request);
    }
}
