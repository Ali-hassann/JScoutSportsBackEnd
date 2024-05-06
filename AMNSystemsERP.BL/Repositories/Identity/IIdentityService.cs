using AMNSystemsERP.CL.Models.IdentityModels;
using AMNSystemsERP.CL.Models.OrganizationModels;
using AMNSystemsERP.DL.DB.DBSets.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.BL.Repositories.Identity
{
    public interface IIdentityService
    {
        public UserManager<ApplicationUser> UserManager { get; }
        Task<UserRegisterResponse> AddUser(ApplicationUserRequest request);
        Task<UserRegisterResponse> UpdateUser(ApplicationUserRequest request);
        Task<bool> DeleteUser(string userId);
        string GenerateJwtToken(JwtTokenResponse jwtTokenResponse);
        Task<object> ValidateToken(string token);
        Task<object> Login(UserLoginRequest request, bool tokenVerified);
        Task<ApplicationUser> GetUserByUserName(string userName);
        Task<List<UserRegisterResponse>> GetUsers(long organizationId, long outletId);
        Task<UserRegisterResponse> GetUsersById(string userId);
        Task<bool> IsUserNameOrEmailAlreadyExist(string userName, string email);
        Task<BaseResponse> ChangePassword(string username, string currentPassword, string newPassword);
        Task<BaseResponse> ResetPassword(string username, string newPasword);
        Task<BaseResponse> ResetPasswordByToken(string username, string token, string newPassword);
        Task<List<UserRightsBaseResponse>> GetUserGivenRightsById(string userId);
        Task<List<UserRightsResponse>> GetUserAllRightsById(string userId);
        Task<bool> SaveUserRightsList(List<UserRightsRequest> userRightsList);
        // Rights Work
        Task<OrganizationRoleRequest> AddOrganizationRole(OrganizationRoleRequest request);
        Task<OrganizationRoleRequest> UpdateOrganizationRole(OrganizationRoleRequest request);
        Task<OrganizationRoleRequest> GetOrganizationRoleById(long organizationId, long organizationRoleId);
        Task<List<OrganizationRoleRequest>> GetOrganizationRoleList(long OrganizationId);
        Task<List<OrgRoleRightsResponse>> GetOrgRoleRightsList(long organizationId, long organizationRoleId);
        Task<OrganizationRoleRequest> AddRightsToOrganizationRoles(List<OrgRoleRightsRequest> request);
        Task<OrganizationRoleRequest> UpdateRightsToOrganizationRoles(List<OrgRoleRightsRequest> request);

        Task<List<RightsRequest>> GetRightsList();
    }
}