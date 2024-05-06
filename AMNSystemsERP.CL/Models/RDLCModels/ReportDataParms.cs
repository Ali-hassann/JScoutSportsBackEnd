namespace AMNSystemsERP.CL.Models.RDLCModels
{
    public class ReportDataParms<T>
    {
        public List<T> Data { get; set; }
        public List<T> Data2 { get; set; }
        public Dictionary<string, string> Parms { get; set; }
        public RdlcReportConfiguration ReportConfig { get; set; }
        public string DownloadFileName { get; set; }
    }
}
