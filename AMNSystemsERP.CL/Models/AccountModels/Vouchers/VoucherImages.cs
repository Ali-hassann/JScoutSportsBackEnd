namespace AMNSystemsERP.CL.Models.AccountModels.Vouchers
{
    public class VoucherImagesRequest
    {
        public long VoucherImagesId { get; set; }
        public long VouchersMasterId { get; set; }
        public string Imagepath { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class VoucherImagesResponse : VoucherImagesRequest
    {
    }
}