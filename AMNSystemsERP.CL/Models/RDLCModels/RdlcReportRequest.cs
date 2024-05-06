using System.Collections.Generic;

namespace AMNSystemsERP.CL.Models.RDLCModels
{
    public class RdlcReportRequest<T>
    {
        public List<T> ReportData { get; set; }
        public string ReportDataSourceName { get; set; }

        public List<T> ReportData2 { get; set; }
        public string ReportDataSourceName2 { get; set; }
        public string RdlcReportType { get; set; }
        public string RdlcReportName { get; set; }
        public string ReportFileName { get; set; }
        public string RenderType { get; set; }
        public string TempFolderPath { get; set; }
        public Dictionary<string, string> ReportParams { get; set; } = null;
        public List<RdlcSubReportRequest> SubReportData { get; set; }
    }

    public class RdlcSubReportRequest
    {
        public List<object> ReportData { get; set; }
        public string ReportDataSourceName { get; set; }
        public Dictionary<string, string> ReportParams { get; set; } = null;
    }
}
