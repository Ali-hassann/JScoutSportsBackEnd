using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.StockManagementModels
{
    public class PurchaseRequisitionMasterRequest : CommonBaseModel
    {
        public long PurchaseRequisitionMasterId { get; set; }

        public DateTime PurchaseRequisitionDate { get; set; }

        public string ReferenceNo { get; set; }

        public string Remarks { get; set; }

        public int Status { get; set; }

        public long ProjectsId { get; set; }

        public virtual List<PurchaseRequisitionDetailRequest> PurchaseRequisitionDetailRequest { get; set; }
    }
}