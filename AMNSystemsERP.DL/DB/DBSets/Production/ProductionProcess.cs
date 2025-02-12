﻿using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Production
{
    public class ProductionProcess : Entity
    {
        [Key]
        public long ProductionProcessId { get; set; }
        public long EmployeeId { get; set; }
        public long OrderMasterId { get; set; }
        public int ProcessTypeId { get; set; }
        public int Status { get; set; }
        public int IssuanceNo { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public long OutletId { get; set; }

        public long ProductId { get; set; }
        public int ProductSizeId { get; set; }
        public decimal IssueQuantity { get; set; }
        public decimal ReceiveQuantity { get; set; }
        public decimal ProcessRate { get; set; }
        public bool IsPosted { get; set; }
    }
}