namespace AMNSystemsERP.CL.Models.OrganizationModelsS
{
    public class OutletProfileRequest
    {
        public long OutletId { get; set; }
        public long OrganizationId { get; set; }
        public string OutletName { get; set; }
        public string OutletOwnerName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public long City { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public string ImagePath { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsToDeleteImage { get; set; }
    }
}
