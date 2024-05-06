using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Production
{
    public class ProcessType
    {
        [Key]
        public int ProcessTypeId { get; set; }
        public int MainProcessTypeId { get; set; }
        public string ProcessTypeName { get; set; }
        public int SortOrder { get; set; }
    }
}