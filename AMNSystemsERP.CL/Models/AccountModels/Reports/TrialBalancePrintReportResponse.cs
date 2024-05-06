﻿namespace AMNSystemsERP.CL.Models.AccountModels.Reports
{
    public class TrialBalancePrintReportResponse: AccountsReportBase
    {
        public long PostingAccountsId { get; set; }
        public string PostingAccountsName { get; set; }
        public long SubCategoriesId { get; set; }
        public string SubCategoriesName { get; set; }
        public long HeadCategoriesId { get; set; }
        public string HeadCategoriesName { get; set; }
        public long MainHeadsId { get; set; }
        public string MainHeadsName { get; set; }
        
    }
}