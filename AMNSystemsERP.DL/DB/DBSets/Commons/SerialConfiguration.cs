using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Commons
{
    public class SerialConfiguration
    {
        [Key]
        public int SerialConfigurationId { get; set; }
        public int Type { get; set; }
        public long SerialIndex { get; set; }
        public string SerialNumber { get; set; }
    }
}
