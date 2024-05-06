namespace AMNSystemsERP.CL.Models.OrganizationModels
{
    public class AdminRegistration
    {
        public long OrganizationRequestId { get; set; }
        public string OrganizationName { get; set; }
        public string OwnerName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
    }
}
