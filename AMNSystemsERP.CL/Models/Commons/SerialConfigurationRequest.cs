namespace AMNSystemsERP.CL.Models.Commons
{
    public class SerialConfigurationRequest
    {
        public int SerialConfigurationId { get; set; }
        public int Type { get; set; }
        public long SerialIndex { get; set; }
        public string SerialNumber { get; set; }
    }
}
