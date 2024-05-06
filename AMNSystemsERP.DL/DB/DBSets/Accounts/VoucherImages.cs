using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Accounts
{
    public class VoucherImages
    {
        [Key]
        public long VoucherImagesId { get; set; }
        [Required]
        public long VouchersMasterId { get; set; }
        public string Imagepath { get; set; }

        public virtual VouchersMaster VouchersMaster { get; set; }
    }
}