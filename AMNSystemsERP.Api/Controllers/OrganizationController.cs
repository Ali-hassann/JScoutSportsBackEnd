using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AMNSystemsERP.Api.Controllers;
using AMNSystemsERP.BL.Repositories.Organization;
using AMNSystemsERP.CL.Models.OrganizationModels;
using AMNSystemsERP.CL.Models.OrganizationModelsS;

namespace Organization.Api.Controllers
{
    [Route("v1/api/Organization")]
    public class OrganizationController : ApiController
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("OrganizationAndOutletCreation")]
        public async Task<BaseResponse> OrganizationAndOutletCreation([FromBody] OrganizationRegRequests request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request?.OrganizationName))
                {
                    return await _organizationService.OrganizationAndOutletCreation(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new BaseResponse();
        }

        [HttpPost]
        [Route("UpdateOrganization")]
        public async Task<OrganizationProfile> UpdateOrganization([FromBody] OrganizationProfile request)
        {
            try
            {
                if (request?.OrganizationId > 0)
                {
                    return await _organizationService.UpdateOrganization(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new OrganizationProfile();
        }        

        [HttpGet]
        [Route("GetOrganizationProfile")]
        public async Task<OrganizationProfile> GetOrganizationProfile(long organizationId, long outletId)
        {
            try
            {
                if (organizationId > 0 && outletId > 0)
                {
                    return await _organizationService.GetOrganizationProfile(organizationId, outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new OrganizationProfile();
        }

        [HttpPost]
        [Route("AddOutlet")]
        public async Task<OutletProfileRequest> AddOutlet([FromBody] OutletProfileRequest request)
        {
            try
            {
                if (request?.OrganizationId > 0)
                {
                    return await _organizationService.AddOutlet(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new OutletProfileRequest();
        }

        [HttpPost]
        [Route("UpdateOutlet")]
        public async Task<OutletProfileRequest> UpdateOutlet([FromBody] OutletProfileRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.OrganizationId > 0)
                {
                    return await _organizationService.UpdateOutlet(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new OutletProfileRequest();
        }

        [HttpGet]
        [Route("GetOutletList")]
        public async Task<List<OutletProfileRequest>> GetOutletList(long organizationId)
        {
            try
            {
                if (organizationId > 0)
                {
                    return await _organizationService.GetOutletList(organizationId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new List<OutletProfileRequest>();
        }

        [HttpPost]
        [Route("RemoveOutlet")]
        public async Task<bool> RemoveOutlet(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _organizationService.RemoveOutlet(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
    }
}
