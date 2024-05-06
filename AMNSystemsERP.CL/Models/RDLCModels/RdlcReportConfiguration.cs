namespace AMNSystemsERP.CL.Models.RDLCModels
{
    public class RdlcReportConfiguration
    {
        public string Url { get; set; }
        public string RDLCReportType { get; set; }
        public string RenderType { get; set; }
        public string ReportName { get; set; }
        public string DataSource { get; set; }
        public string DataSource2 { get; set; }
    }

    public class ReportsConfigs
    {
        public List<RdlcReportConfiguration> Configs { get; set; }
    }
}