using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.OrganizationModels
{
    public class OrganizationProfile : CommonBaseModel
    {
        public string OrganizationName { get; set; }
        public string OwnerName { get; set; }
        public string FoundedYear { get; set; }
        public string OutletOwnerName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public long City { get; set; }
        public string CityFullName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public bool IsToDeleteImage { get; set; }
        public string OutletName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string ImagePath { get; set; }
    }
}