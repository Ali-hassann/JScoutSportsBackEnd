using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Production
{
    public class Process : Entity
    {
        [Key]
        public int ProcessId { get; set; }
        public int ProcessTypeId { get; set; }
        public int ProductSizeId { get; set; }
        public long ProductId { get; set; }
        public long OrderMasterId { get; set; }
        public decimal ProcessRate { get; set; }
        public decimal OtherRate { get; set; }
        public string Description { get; set; }
    }
}