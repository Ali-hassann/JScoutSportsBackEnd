using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMNSystemsERP.CL.Models.ProductionModels
{
    public class OrderConsumptionRequest
    {
        public long OrderMasterId { get; set; }
        public string OrderName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public long ArticleId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductColorName { get; set; }
        public string ProductSizeName { get; set; }
        public decimal ProductQuantity { get; set; }

        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemQuantity { get; set; }
        public decimal ItemPrice { get; set; }
        public string UnitName { get; set; }
        public bool IsManualPrice { get; set; }
        public string ProcessTypeName { get; set; }
        public decimal ProcessRate { get; set; }
        public decimal RequiredMaterialQuantity { get; set; }
        public decimal StockBalanceQuantity { get; set; }
        public decimal OtherCost { get; set; }
    }
}
