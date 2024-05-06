using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Accounts
{
    public class PostingAccounts : Entity
    {
        [Key]
        public long PostingAccountsId { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public long SubCategoriesId { get; set; }
        public decimal? OpeningDebit { get; set; }
        public decimal? OpeningCredit { get; set; }
        public DateTime? OpeningDate { get; set; }
        [Required]
        public long OutletId { get; set; }
        public long? EmployeeId { get; set; }
        public long? ParticularId { get; set; }

        public virtual SubCategories SubCategories { get; set; }
    }
}