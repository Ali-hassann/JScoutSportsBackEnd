using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.StockManagement
{
    public class PurchaseRequisitionMaster : Entity
    {
        public PurchaseRequisitionMaster()
        {
            PurchaseRequisitionDetail = new HashSet<PurchaseRequisitionDetail>();
        }

        [Key]
        public long PurchaseRequisitionMasterId { get; set; }
        [Required]
        public DateTime PurchaseRequisitionDate { get; set; }
        public string ReferenceNo { get; set; }  
        public string Remarks { get; set; }
        public int Status { get; set; }
        public long ProjectsId { get; set; }

        [Required]
        public long OutletId { get; set; }
        [Required]
        public long OrganizationId { get; set; }
        public virtual ICollection<PurchaseRequisitionDetail> PurchaseRequisitionDetail { get; set; }
    }
}
