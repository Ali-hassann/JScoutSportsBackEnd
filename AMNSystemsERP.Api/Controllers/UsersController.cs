using AMNSystemsERP.BL.Repositories.Identity;
using AMNSystemsERP.CL.Models.IdentityModels;
using AMNSystemsERP.CL.Models.OrganizationModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/Users")]
    public class UsersController : ApiController
    {
        private readonly IIdentityService _identity;

        public UsersController(IIdentityService identity)
        {
            _identity = identity;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("AddUser")]
        public async Task<UserRegisterResponse> AddUser([FromBody] ApplicationUserRequest request)
        {
            try
            {
                if (request != null)
                {
                    return await _identity.AddUser(request);
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
        [Route("GetUsers")]
        public async Task<List<UserRegisterResponse>> GetUsers(long organizationId, long outletId)
        {
            try
            {
                if (organizationId > 0 && outletId > 0)
                {
                    return await _identity.GetUsers(organizationId, outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return new List<UserRegisterResponse>();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetUsersById")]
        public async Task<UserRegisterResponse> GetUsersById(string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    return await _identity.GetUsersById(userId);
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
        [Route("UpdateUser")]
        public async Task<UserRegisterResponse> UpdateUser([FromBody] ApplicationUserRequest request)
        {
            try
            {
                if (request != null)
                {
                    return await _identity.UpdateUser(request);
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
        [Route("UserLogin")]
        public async Task<object> Login([FromBody] UserLoginRequest request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request?.UserName)
                    && !string.IsNullOrEmpty(request?.Password))
                {
                    return await _identity.Login(request, tokenVerified: false);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BadRequest(request);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("CheckExistingUserNameOrEmail")]
        public async Task<ActionResult<bool>> CheckExistingUserNameOrEmail(string userName, string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(email))
                {
                    return Ok(await _identity.IsUserNameOrEmailAlreadyExist(userName, email));
                }

            }
            catch (Exception)
            {
                throw;
            }
            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ChangePassword")]
        public async Task<BaseResponse> ChangePassword(string username
        , string currentPassword
        , string newPassword)
        {
            if (string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(currentPassword)
                || string.IsNullOrEmpty(newPassword))
            {
                return new BaseResponse() { Success = false, Message = "Invalid parameters. Please enter correct data." };
            }
            try
            {
                return await _identity.ChangePassword(username, currentPassword, newPassword);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<BaseResponse> ResetPassword(string username, string newPasword)
        {
            try
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(newPasword))
                {
                    return await _identity.ResetPassword(username, newPasword);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new BaseResponse()
            {
                Success = false,
                Message = "Invalid username. Please enter correct username."
            };
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("DeleteUser")]
        public async Task<BaseResponse> DeleteUser(string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var res = new BaseResponse()
                    {
                        Message = "Deleted Succesfully",
                        Success = await _identity.DeleteUser(userId)
                    };

                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new BaseResponse()
            {
                Success = false,
                Message = "Invalid user. Please try again"
            };
        }
    }
}