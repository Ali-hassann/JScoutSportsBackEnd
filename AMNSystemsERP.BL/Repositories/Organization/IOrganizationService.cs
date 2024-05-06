using AMNSystemsERP.CL.Models.OrganizationModels;
using AMNSystemsERP.CL.Models.OrganizationModelsS;

namespace AMNSystemsERP.BL.Repositories.Organization
{
    public interface IOrganizationService
    {
        Task<BaseResponse> OrganizationAndOutletCreation(OrganizationRegRequests request);
        Task<OrganizationProfile> UpdateOrganization(OrganizationProfile instituteProfile);
        Task<OrganizationProfile> GetOrganizationProfile(long organizationId,  long outletId);
        Task<OutletProfileRequest> AddOutlet(OutletProfileRequest branchProfile);
        Task<OutletProfileRequest> UpdateOutlet(OutletProfileRequest branchProfile);
        Task<List<OutletProfileRequest>> GetOutletList(long organizationId);
        Task<bool> RemoveOutlet(long outletId);
    }
}