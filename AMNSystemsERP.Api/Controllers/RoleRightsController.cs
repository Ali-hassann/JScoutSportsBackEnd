using AMNSystemsERP.BL.Repositories.Identity;
using AMNSystemsERP.CL.Models.IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/RoleRights")]
    public class RoleRightsController : ApiController
    {
        private readonly IIdentityService _identity;

        public RoleRightsController(IIdentityService identity)
        {
            _identity = identity;
        }

        // ------------------ OrganizationRoles Section Start -----------------------------

        [HttpPost]
        [AllowAnonymous]
        [Route("AddOrganizationRole")]
        public async Task<OrganizationRoleRequest> AddOrganizationRole([FromBody] OrganizationRoleRequest request)
        {
            try
            {
                if (request?.OrganizationId > 0
                    && !string.IsNullOrEmpty(request.RoleName)
                    && !string.IsNullOrEmpty(request.NormalizedName))
                {
                    return await _identity.AddOrganizationRole(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("UpdateOrganizationRole")]
        public async Task<OrganizationRoleRequest> UpdateOrganizationRole([FromBody] OrganizationRoleRequest request)
        {
            try
            {
                if (request?.OrganizationId > 0
                    && request.OrganizationRoleId > 0
                    && !string.IsNullOrEmpty(request.RoleName)
                    && !string.IsNullOrEmpty(request.NormalizedName))
                {
                    return await _identity.UpdateOrganizationRole(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetOrganizationRoleById")]
        public async Task<OrganizationRoleRequest> GetOrganizationRoleById(long organizationId, long organizationRoleId)
        {
            try
            {
                if (organizationId > 0 && organizationRoleId > 0)
                {
                    return await _identity.GetOrganizationRoleById(organizationId, organizationRoleId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetOrganizationRoleList")]
        public async Task<List<OrganizationRoleRequest>> GetOrganizationRoleList(long OrganizationId)
        {
            try
            {
                if (OrganizationId > 0)
                {
                    return await _identity.GetOrganizationRoleList(OrganizationId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        // ------------------ Rights Section Start -----------------------------

        [HttpPost]
        [AllowAnonymous]
        [Route("AddRightsToOrganizationRoles")]
        public async Task<OrganizationRoleRequest> AddRightsToOrganizationRoles([FromBody] List<OrgRoleRightsRequest> request)
        {
            try
            {
                if (request?.Count > 0)
                {
                    return await _identity.AddRightsToOrganizationRoles(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new OrganizationRoleRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("UpdateRightsToOrganizationRoles")]
        public async Task<OrganizationRoleRequest> UpdateRightsToOrganizationRoles([FromBody] List<OrgRoleRightsRequest> request)
        {
            try
            {
                if (request?.Count > 0)
                {
                    return await _identity.UpdateRightsToOrganizationRoles(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new OrganizationRoleRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetOrgRoleRightsList")]
        public async Task<List<OrgRoleRightsResponse>> GetOrgRoleRightsList(long OrganizationId, long OrganizationRoleId)
        {
            try
            {
                if (OrganizationId > 0 && OrganizationRoleId > 0)
                {
                    return await _identity.GetOrgRoleRightsList(OrganizationId, OrganizationRoleId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new List<OrgRoleRightsResponse>();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetRightsList")]
        public async Task<List<RightsRequest>> GetRightsList()
        {
            try
            {
                return await _identity.GetRightsList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetUserGivenRightsById")]
        public async Task<List<UserRightsBaseResponse>> GetUserGivenRightsById(string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    return await _identity.GetUserGivenRightsById(userId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        
        [HttpGet]
        [AllowAnonymous]
        [Route("GetUserAllRightsById")]
        public async Task<List<UserRightsResponse>> GetUserAllRightsById(string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    return await _identity.GetUserAllRightsById(userId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SaveUserRightsList")]
        public async Task<bool> SaveUserRightsList([FromBody] List<UserRightsRequest> userRightsList)
        {
            try
            {
                if (userRightsList?.Count > 0)
                {
                    return await _identity.SaveUserRightsList(userRightsList);
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