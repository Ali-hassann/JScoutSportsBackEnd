using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.RDLCModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.WinForms;
using System.Text;

namespace SMSRdlcReports.BL.Repositories.Reports
{
    public class CommonRDLCReportsService : ICommonRDLCReportsService
    {
        public async Task<byte[]> GenerateRdlcReport<T>(RdlcReportRequest<T> request)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding.GetEncoding("windows-1252");

                LocalReport localReport = new LocalReport();
                localReport.EnableExternalImages = true;
                localReport.ReportPath = @$"{DocumentHelper.GetReportPath(request.RdlcReportType)}\{request.RdlcReportName}";

                ReportDataSource reportDataSource = new ReportDataSource
                (
                    request.ReportDataSourceName,
                    request.ReportData
                );

                localReport.DataSources.Add(reportDataSource);

                /******* 2nd datasource***********/
                ReportDataSource reportDataSource2 = new ReportDataSource
                (
                    request.ReportDataSourceName2,
                    request.ReportData2
                );

                localReport.DataSources.Add(reportDataSource2);

                /************* parameters***********/
                if (request.ReportParams?.Count > 0)
                {
                    foreach (string key in request.ReportParams.Keys)
                    {
                        localReport.SetParameters(new ReportParameter(key, request.ReportParams[key]));
                    }
                }

                string mimeType;
                string encoding;
                string fileNameExtension;
                string[] streams;
                Warning[] warnings;
                byte[] renderedbyte;

                renderedbyte = localReport.Render
                (
                    request.RenderType,
                    null,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings
                );

                var reportSavedFilePath = request.TempFolderPath;
                var extension = "";
                switch (request.RenderType.ToLower())
                {
                    case "excel":
                        extension = "xls";
                        break;
                    case "pdf":
                        extension = "pdf";
                        break;
                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(request.ReportFileName))
                {
                    reportSavedFilePath = @$"{reportSavedFilePath}\{request.ReportFileName}.{extension}";
                }

                return renderedbyte;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
