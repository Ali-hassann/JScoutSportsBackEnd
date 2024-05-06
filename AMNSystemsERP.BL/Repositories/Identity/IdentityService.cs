using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Core.CL.Enums;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AMNSystemsERP.DL.DB.DBSets.Identity;
using AMNSystemsERP.CL.Models.IdentityModels;
using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Enums;
using AMNSystemsERP.CL.Models.OrganizationModels;
using AMNSystemsERP.CL.Models.Commons.DocumentHelper;
using AMNSystemsERP.CL.Services.Documents;
using AMNSystemsERP.CL.Services.Configuration;

namespace AMNSystemsERP.BL.Repositories.Identity
{
    public class IdentityService : IIdentityService
    {
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unit;
        private readonly IDocumentHelperService _documentHelperService;
        private readonly IAppConfiguration _appConfiguration;
        private readonly IMapper _mapper;

        public UserManager<ApplicationUser> UserManager => _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager
        , IUnitOfWork unit
        , IDocumentHelperService documentHelperService
        , IAppConfiguration appConfiguration
        , IMapper mapper)
        {
            _userManager = userManager;
            _unit = unit;
            _documentHelperService = documentHelperService;
            _appConfiguration = appConfiguration;
            _mapper = mapper;
        }

        public string GenerateJwtToken(JwtTokenResponse request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(request.Secret);

            //tokenHandler.ValidateToken()

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserName", request.UserName),
                    new Claim("OrganizationId", $"{request.OrganizationId}"),
                    new Claim("OutletId", $"{request.OutletId}"),
                }),
                Expires = DateHelper.GetCurrentDate().AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }

        public async Task<object> ValidateToken(string token)
        {
            var errorResponse = new BaseResponse();
            errorResponse.Success = false;
            errorResponse.Message = $"Your session has expired. Please login again and retry";

            if (!string.IsNullOrEmpty(token))
            {
                var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration.Secret));

                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    ClaimsPrincipal principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidIssuer = "",
                        ValidAudience = "",
                        IssuerSigningKey = mySecurityKey,

                    }, out SecurityToken validatedToken);
                    if (principal != null)
                    {
                        if (principal?.Claims?.Count() > 0)
                        {
                            var user = principal?.Claims?.FirstOrDefault(c => c.Type == "UserName")?.Value;
                            if (!string.IsNullOrEmpty(user))
                            {
                                var nextRequest = new UserLoginRequest() { UserName = user, Password = "" };
                                return await Login(nextRequest, tokenVerification: true);
                            }
                        }
                    }


                }
                catch
                {
                    return errorResponse;
                }
            }
            return errorResponse;
        }

        public async Task<UserRegisterResponse> AddUser(ApplicationUserRequest request)
        {
            try
            {
                ApplicationUser applicationUser = _mapper.Map<ApplicationUser>(request);
                // Saving User Image
                if (applicationUser.OrganizationId > 0 && applicationUser.OutletId > 0)
                {
                    applicationUser.Id = Guid.NewGuid().ToString();
                    applicationUser.CreatedDate = DateHelper.GetCurrentDate();
                    applicationUser.ModifiedDate = DateHelper.GetCurrentDate();

                    var imagePath = applicationUser.ImagePath;
                    applicationUser.ImagePath = string.Empty;
                    applicationUser.LockoutEnabled = false;

                    applicationUser.PasswordHash = _userManager.PasswordHasher.HashPassword(applicationUser, applicationUser.Password);

                    //var checkverifypassword = IdentityHelper.VerifyHashedPassword(applicationUser.PasswordHash, applicationUser.Password);

                    applicationUser.ImagePath = _documentHelperService
                                                        .DeleteAndAddImage(Convert.ToInt64(applicationUser.OrganizationId),
                                                                            Convert.ToInt64(applicationUser.OutletId),
                                                                            applicationUser.Id,
                                                                            CL.Enums.EntityState.Inserted,
                                                                            PersonType.User,
                                                                            FileType.Images,
                                                                            FolderType.Profile,
                                                                            imagePath,
                                                                            false);
                    applicationUser.LockoutEnabled = false;
                    try
                    {
                        _unit.UserRepository.InsertSingle(applicationUser);
                        await AddUserRights(request, applicationUser);
                    }
                    catch (Exception)
                    {
                        _documentHelperService
                            .DeleteSingleDoc(new DocumentRequest()
                            {
                                OrganizationId = applicationUser.OrganizationId,
                                OutletId = applicationUser.OutletId,
                                PersonId = applicationUser.Id,
                                PersonType = PersonType.User.ToString(),
                                FileType = FileType.Images,
                                FolderType = FolderType.Profile,
                                FileName = $"{applicationUser.Id}.jpg"
                            });
                    }
                }

                if (applicationUser != null && await _unit.SaveAsync())
                {
                    return await GetUsersById(applicationUser.Id);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<UserRegisterResponse> UpdateUser(ApplicationUserRequest request)
        {
            try
            {
                var applicationUser = await _userManager.FindByIdAsync(request.Id);
                if (applicationUser != null)
                {
                    applicationUser.FirstName = request.FirstName;
                    applicationUser.LastName = request.LastName;
                    applicationUser.Email = request.Email;
                    applicationUser.OutletId = request.OutletId;
                    applicationUser.LockoutEnabled = request.LockoutEnabled;
                    applicationUser.PhoneNumber = request.PhoneNumber;
                    applicationUser.IsActive = request.IsActive;
                    applicationUser.LockoutEnabled = !request.IsActive;
                    applicationUser.OrganizationRoleId = request.OrganizationRoleId;
                    applicationUser.UserName = request.UserName;

                    // Updating User Image
                    applicationUser.ImagePath = _documentHelperService
                                                            .DeleteAndAddImage(Convert.ToInt64(applicationUser.OrganizationId),
                                                                                Convert.ToInt64(applicationUser.OutletId),
                                                                                applicationUser.Id,
                                                                                CL.Enums.EntityState.Updated,
                                                                                PersonType.User,
                                                                                FileType.Images,
                                                                                FolderType.Profile,
                                                                                request.ImagePath,
                                                                                request.IsToDeleteImage);
                    //

                    _unit.UserRepository.Update(applicationUser);
                    await AddUserRights(request, applicationUser);

                    if (await _unit.SaveAsync())
                    {
                        return await GetUsersById(applicationUser.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                return new UserRegisterResponse()
                {
                    Success = false,
                    Message = ex?.Message ?? "Something went wrong"
                };
            };
            return null;
        }

        private async Task<bool> AddUserRights(ApplicationUserRequest userRequest, ApplicationUser applicationUser)
        {
            try
            {
                if (userRequest?.OrganizationRoleId > 0 && applicationUser != null)
                {
                    if (userRequest.Id?.Length > 0)
                    {
                        var rightsToDelete = await _unit.UserRightsRepository.GetAsync(c => c.Id == userRequest.Id);
                        _unit.UserRightsRepository.DeleteRangeEntities(rightsToDelete.ToList());
                    }

                    var OrgRights = await _unit
                                          .OrgRoleRightsRepository
                                          .GetAsync(x => x.OrganizationRoleId == userRequest.OrganizationRoleId);
                    var userRightsToAdd = new List<UserRights>();
                    OrgRights.ToList().ForEach(s =>
                    {
                        var userRight = new UserRights()
                        {
                            HasAccess = true,
                            Id = applicationUser.Id,
                            RightsId = s.RightsId
                        };

                        userRightsToAdd.Add(userRight);
                    });

                    _unit.UserRightsRepository.InsertList(userRightsToAdd);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        public async Task<ApplicationUser> GetUserByUserName(string userName)
        {
            try
            {
                var user = await _userManager
                                 .Users
                                 .Where(x => x.UserName == userName.Replace(" ", ""))
                                 .FirstOrDefaultAsync() ?? new ApplicationUser();
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> Login(UserLoginRequest request, bool tokenVerification = false)
        {
            try
            {
                var errorResponse = new BaseResponse();

                var user = await GetUserByUserName(request.UserName);
                if (user == null
                    || (user?.IsDeleted ?? false))
                {
                    errorResponse.Success = false;
                    errorResponse.Message = $"Invalid user. System can not recongnize {request.UserName} as valid user. Please login with correct one";
                    return errorResponse;
                }

                if (!tokenVerification)
                {

                    var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
                    if (!passwordValid)
                    {
                        errorResponse.Success = false;
                        errorResponse.Message = $"Invalid password. Please enter correct password and try again";
                        return errorResponse;
                    }
                }

                if (user.LockoutEnabled)
                {
                    errorResponse.Success = false;
                    errorResponse.Message = $"we can not process your login request at this moment as your user has been inactivated by 'Admin'. Please contact your Organization administrator ";
                    return errorResponse;
                }

                var query = $@"SELECT                     
                                 USR.Email                
                                 , USR.UserName                
                                 , USR.PhoneNumber                
                                 , USR.OutletId                
                                 , USR.OrganizationId                
                                 , USR.Id              
                                 , USR.ImagePath            
                                 , USR.FirstName            
                                 , USR.LastName            
                                 , CONVERT(VARCHAR, USR.CreatedDate,106) AS Since                
                                 , ROLS.RoleName            
                                 , ROLS.NormalizedName AS RoleDescription            
                                 , USR.OrganizationRoleId          
                                FROM AspNetUsers AS USR                          
                                INNER JOIN OrganizationRole AS ROLS                    
                                 ON ROLS.OrganizationRoleId = USR.OrganizationRoleId                    
                                WHERE USR.Id = '{user.Id}'";

                var profile = await _unit.DapperRepository.GetSingleQueryAsync<UserProfile>(query);

                if (!string.IsNullOrEmpty(profile?.Id))
                {
                    profile.CurrentOutletId = profile.OutletId;

                    profile.Token = GenerateJwtToken
                    (
                        new JwtTokenResponse()
                        {
                            UserName = user.UserName,
                            OrganizationId = user.OrganizationId,
                            OutletId = user.OutletId,
                            Secret = _appConfiguration.Secret,
                        }
                    );
                }

                return profile;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteUser(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    var query = $"Update AspNetUsers Set IsDeleted = 1 WHERE Id = '{userId}'";
                    return await _unit.DapperRepository.ExecuteNonQuery(query, null, CommandType.Text) > 0;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }

        public async Task<bool> IsUserNameOrEmailAlreadyExist(string userName, string email)
        {
            bool resut = false;
            if (!string.IsNullOrEmpty(userName))
            {
                resut = await Task
                                .FromResult(_userManager
                                            .Users
                                            .Count(x => x.UserName.ToLower() == userName.ToLower()) > 0);
            }
            else if (!string.IsNullOrEmpty(email))
            {
                resut = await Task
                                .FromResult(_userManager
                                            .Users
                                            .Count(x => x.Email.ToLower() == email.ToLower()) > 0);
            }
            return resut;
        }

        public async Task<List<UserRegisterResponse>> GetUsers(long organizationId, long outletId)
        {
            try
            {
                var query = $@"SELECT               
                                 U.Email  
                                 , U.FirstName  
                                 , U.LastName  
                                 , U.Id  
                                 , U.PhoneNumber  
                                 , U.UserName  
                                 , U.ImagePath  
                                 , O.OutletName  
                                 , OROL.RoleName  
                                 , OROL.OrganizationRoleId
                                 , U.IsActive
                                FROM AspNetUsers AS U             
                                INNER JOIN OrganizationRole AS OROL              
                                 ON U.OrganizationRoleId = OROL.OrganizationRoleId            
                                INNER JOIN Outlet AS O  
                                 ON O.OutletId = U.OutletId  
                                 AND O.IsActive = 1  
                                WHERE U.OrganizationId = {organizationId}            
                                AND U.OutletId = {outletId}  
                                AND ISNULL(U.IsDeleted, 0) = 0";

                var usersList = await _unit
                                      .DapperRepository
                                      .GetListQueryAsync<UserRegisterResponse>(query);
                return usersList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserRegisterResponse> GetUsersById(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    var query = $@"SELECT TOP 1 
	                                    U.Id
	                                    , U.FirstName
	                                    , U.LastName
	                                    , U.UserName
	                                    , U.Email
	                                    , U.OutletId
	                                    , U.OrganizationId
	                                    , O.OutletName
	                                    , R.RoleName
                                        , U.PhoneNumber                                        
                                        , U.IsActive
                                        , R.OrganizationRoleId
                                    FROM AspNetUsers AS U
                                    INNER JOIN OrganizationRole AS R
	                                    ON U.OrganizationRoleId = R.OrganizationRoleId
                                    INNER JOIN Outlet AS O
	                                    ON O.OutletId = U.OutletId
	                                    AND O.IsActive = 1
                                    WHERE U.Id = '{userId}'";

                    return await _unit.DapperRepository.GetSingleQueryAsync<UserRegisterResponse>(query);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        public async Task<BaseResponse> ChangePassword(string username
        , string currentPassword
        , string newPassword)
        {
            try
            {
                var user = await GetUserByUserName(username);
                if (user == null)
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = "Invalid username. Please enter correct username."
                    };
                }

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (result.Succeeded)
                {
                    return new BaseResponse()
                    {
                        Success = true,
                        Message = "Password changed successfully."
                    };
                }
                else
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = string.Join(',', result.Errors.Select(x => x.Description).ToList())
                    };
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse> ResetPassword(string username, string newPasword)
        {
            try
            {
                var user = await GetUserByUserName(username);

                if (user == null)
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = "Invalid username. Please enter correct username."
                    };
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var response = await _userManager.ResetPasswordAsync(user, token, newPasword);

                return new BaseResponse()
                {
                    Success = response?.Succeeded ?? false,
                    Message = string.Join(',', response?.Errors?.Select(x => x.Description)?.ToList()) ?? ""
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse> ResetPasswordByToken(string username
        , string token
        , string newPassword)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = "Invalid username. Please enter correct username."
                    };
                }

                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (result.Succeeded)
                {
                    return new BaseResponse()
                    {
                        Success = true,
                        Message = "Password changed successfully."
                    };
                }
                else
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = string.Join(',', result.Errors.Select(x => x.Description).ToList())
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OrganizationRoleRequest> AddOrganizationRole(OrganizationRoleRequest request)
        {
            try
            {
                var organizationRole = _mapper.Map<OrganizationRole>(request);

                _unit.OrganizationRoleRepository.InsertSingle(organizationRole);
                var isSaved = await _unit.SaveAsync();

                if (isSaved)
                {
                    // Getting OrganizationRole
                    return await GetOrganizationRoleById(organizationRole.OrganizationId, organizationRole.OrganizationRoleId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<OrganizationRoleRequest> UpdateOrganizationRole(OrganizationRoleRequest request)
        {
            try
            {
                var organizationRole = _mapper.Map<OrganizationRole>(request);

                _unit.OrganizationRoleRepository.Update(organizationRole);
                var isSaved = await _unit.SaveAsync();

                if (isSaved)
                {
                    // Getting OrganizationRole
                    return await GetOrganizationRoleById(organizationRole.OrganizationId, organizationRole.OrganizationRoleId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<OrganizationRoleRequest> GetOrganizationRoleById(long organizationId, long organizationRoleId)
        {
            try
            {
                if (organizationId > 0 && organizationRoleId > 0)
                {
                    var query = $@"SELECT                     
                                     ORL.OrganizationRoleId                    
                                     , ORL.RoleName                    
                                     , ORL.NormalizedName                    
                                     , ORL.OrganizationId      
                                     , ORL.IsDefault        
                                     , COUNT(ORRL.OrgRoleRightsId) AS RightsCount           
                                    FROM OrganizationRole AS ORL          
                                    LEFT JOIN OrgRoleRights AS ORRL           
                                     ON ORL.OrganizationRoleId = ORRL.OrganizationRoleId           
                                    WHERE ORL.OrganizationId = {organizationId}          
                                    AND ORL.OrganizationRoleId = {organizationRoleId}          
                                    GROUP BY                    
                                     ORL.OrganizationRoleId          
                                     , ORL.RoleName                  
                                     , ORL.NormalizedName                    
                                     , ORL.OrganizationId       
                                     , ORL.IsDefault";

                    return await _unit.DapperRepository.GetSingleQueryAsync<OrganizationRoleRequest>(query);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<List<UserRightsBaseResponse>> GetUserGivenRightsById(string userId)
        {
            try
            {
                var query = $@"SELECT DISTINCT      
                                 UR.RightsId      
                                 , R.RightsName      
                                 , R.Description      
                                 , R.RightsArea      
                                 , R.ParentName      
                                 , UR.UserRightsId      
                                 , UR.Id      
                                 , UR.HasAccess
                                FROM UserRights AS UR      
                                INNER JOIN Rights AS R      
                                 ON UR.RightsId = R.RightsId      
                                INNER JOIN AspNetUsers AS U      
                                 ON U.Id = UR.Id      
                                 AND ISNULL(U.IsDeleted, 0) = 0       
                                 AND U.Id = '{userId}'      
                                WHERE UR.Id = '{userId}'";
                // Get User Rights
                return await _unit.DapperRepository.GetListQueryAsync<UserRightsBaseResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserRightsResponse>> GetUserAllRightsById(string userId)
        {
            try
            {
                var query = $@"SELECT DISTINCT      
	                                R.RightsId      
	                                , R.RightsName      
	                                , R.Description      
	                                , R.RightsArea      
	                                , R.ParentName      
	                                , ISNULL(UR.UserRightsId, 0) AS UserRightsId      
	                                , ISNULL(UR.Id, '{userId}') AS Id      
	                                , ISNULL(UR.HasAccess, 0) AS HasAccess      
	                                , CASE WHEN R.RightsAreaOrder = 0 AND R.RightsOrder = 0 THEN 1 ELSE 0 END AS HasMenuRights       
	                                , CASE WHEN R.Description = 'Has Access' AND R.RightsAreaOrder > 0 AND R.RightsOrder > 0 THEN 1 ELSE 0 END AS HasSubMenuRights       
                                FROM Rights AS R
                                INNER JOIN OrgRoleRights ORR
	                                ON ORR.RightsId = R.RightsId
                                LEFT JOIN UserRights AS UR      
                                 ON UR.RightsId = ORR.RightsId
                                 AND R.RightsId = UR.RightsId
                                 AND UR.Id = '{userId}'
                                LEFT JOIN AspNetUsers AS U      
                                 ON U.Id = UR.Id      
                                 AND ISNULL(U.IsDeleted, 0) = 0       
                                 AND U.Id = '{userId}'";

                // Get User Rights
                var result = await _unit
                                   .DapperRepository
                                   .GetListQueryAsync<UserRightsBaseResponse>(query);

                var response = new List<UserRightsResponse>();

                var rightGrp = result.GroupBy(x => x.ParentName);
                rightGrp?.ToList()?.ForEach(parentGrp =>
                {
                    var areaAllRights = parentGrp.ToList();
                    // 1st level 

                    var parentLevel = new UserRightsResponse();
                    parentLevel.MenuRightsArea = parentGrp.Key.ToString();
                    parentLevel.MenuRightsList = new List<SubMenuRightsResponse>();
                    parentLevel.MenuRight = parentGrp.FirstOrDefault(c => c.HasMenuRights);

                    // 2nd level

                    areaAllRights.Where(r => r.HasMenuRights == false)
                    ?.GroupBy(c => c.RightsArea)
                    ?.ToList()
                    .ForEach(childGrp =>
                    {
                        var allChildRights = childGrp.ToList();

                        var childLevel = new SubMenuRightsResponse();
                        childLevel.SubMenuRightsArea = childGrp.Key?.ToString();
                        childLevel.SubMenuSelectAll = allChildRights.All(x => x.HasAccess);
                        childLevel.SubMenuRightsList = allChildRights?.Where(d => d.HasSubMenuRights == false)?.ToList();
                        childLevel.SubMenuRight = childGrp.FirstOrDefault(c => c.HasSubMenuRights == true);

                        parentLevel.MenuRightsList.Add(childLevel);
                    });
                    parentLevel.MenuSelectAll = parentLevel.MenuRightsList.All(c => c.SubMenuSelectAll);
                    response.Add(parentLevel);

                });
                return response ?? new List<UserRightsResponse>();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SaveUserRightsList(List<UserRightsRequest> userRightsList)
        {
            try
            {
                var rightsToAdd = userRightsList
                                        .Where(x => x.HasAccess
                                                    && x.UserRightsId == 0
                                                    && !string.IsNullOrEmpty(x.Id)
                                               )
                                        ?.ToList() ?? new List<UserRightsRequest>();

                var rightsToUpdate = userRightsList
                                        .Where(x => x.UserRightsId > 0
                                                    && !string.IsNullOrEmpty(x.Id)
                                               )
                                        ?.ToList() ?? new List<UserRightsRequest>();

                if (rightsToAdd.Count > 0)
                {
                    _unit.UserRightsRepository.InsertList(_mapper.Map<List<UserRights>>(rightsToAdd));
                }

                if (rightsToUpdate.Count > 0)
                {
                    _unit.UserRightsRepository.UpdateList(_mapper.Map<List<UserRights>>(rightsToUpdate));
                }

                if (rightsToAdd.Count > 0 || rightsToUpdate.Count > 0)
                {
                    return await _unit.SaveAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public async Task<List<OrganizationRoleRequest>> GetOrganizationRoleList(long OrganizationId)
        {
            try
            {
                var query = $@"SELECT                   
                                 ORL.OrganizationRoleId                   
                                 , ORL.RoleName             
                                 , ORL.NormalizedName           
                                 , ORL.OrganizationId       
                                 , ORL.IsDefault      
                                 , COUNT(ORRL.OrgRoleRightsId) AS RightsCount           
                                FROM OrganizationRole AS ORL                
                                LEFT JOIN OrgRoleRights AS ORRL            
                                 ON ORL.OrganizationRoleId = ORRL.OrganizationRoleId          
                                WHERE ORL.OrganizationId = {OrganizationId}         
                                GROUP BY                   
                                 ORL.OrganizationRoleId            
                                 , ORL.RoleName                 
                                 , ORL.NormalizedName               
                                 , ORL.OrganizationId      
                                 , ORL.IsDefault";

                return await _unit.DapperRepository.GetListQueryAsync<OrganizationRoleRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OrgRoleRightsResponse>> GetOrgRoleRightsList(long organizationId, long organizationRoleId)
        {
            try
            {
                var query = @$"SELECT DISTINCT  
                                 R.Description AS RightsName  
                                 , RR.OrgRoleRightsId        
                                 , ISNULL(RR.OrganizationRoleId, {organizationRoleId}) AS OrganizationRoleId        
                                 , R.RightsId        
                                 , ISNULL(RR.OrganizationId, {organizationId}) AS OrganizationId  
                                 , R.RightsArea  
                                 , R.ParentName  
                                 , R.Description  
                                 , CASE WHEN R.RightsAreaOrder = 0 AND R.RightsOrder = 0 THEN 1 ELSE 0 END AS HasMenuRights  
                                 , CASE WHEN R.Description = 'Has Access' AND R.RightsAreaOrder > 0 AND R.RightsOrder > 0 THEN 1 ELSE 0 END AS HasSubMenuRights  
                                 , CASE WHEN ISNULL(RR.OrgRoleRightsId,0) > 0 THEN 1 ELSE 0 END AS HasAccess  
                                FROM Rights AS R        
                                LEFT JOIN OrgRoleRights AS RR        
                                 ON RR.RightsId = R.RightsId        
                                 AND RR.OrganizationId = {organizationId}        
                                 AND RR.OrganizationRoleId = {organizationRoleId}";

                var result = await _unit.DapperRepository.GetListQueryAsync<OrgRoleRightsBaseResponse>(query);

                var response = new List<OrgRoleRightsResponse>();

                var rightGrp = result.GroupBy(x => x.ParentName);
                rightGrp?.ToList()?.ForEach(parentGrp =>
                {
                    var areaAllRights = parentGrp.ToList();
                    // 1st level 

                    var parentLevel = new OrgRoleRightsResponse();
                    parentLevel.MenuRightsArea = parentGrp.Key.ToString();
                    parentLevel.MenuRightsList = new List<SubMenuOrgRoleRightsResponse>();
                    parentLevel.MenuRight = parentGrp.FirstOrDefault(c => c.HasMenuRights);

                    // 2nd level

                    areaAllRights.Where(r => r.HasMenuRights == false)
                    ?.GroupBy(c => c.RightsArea)
                    ?.ToList()
                    .ForEach(childGrp =>
                    {
                        var allChildRights = childGrp.ToList();

                        var childLevel = new SubMenuOrgRoleRightsResponse();
                        childLevel.SubMenuRightsArea = childGrp.Key?.ToString();
                        childLevel.SubMenuSelectAll = allChildRights.All(x => x.HasAccess);
                        childLevel.SubMenuRightsList = allChildRights?.Where(d => d.HasSubMenuRights == false)?.ToList();
                        childLevel.SubMenuRight = childGrp.FirstOrDefault(c => c.HasSubMenuRights == true);

                        parentLevel.MenuRightsList.Add(childLevel);
                    });
                    parentLevel.MenuSelectAll = parentLevel.MenuRightsList.All(c => c.SubMenuSelectAll);
                    response.Add(parentLevel);

                });
                return response ?? new List<OrgRoleRightsResponse>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OrganizationRoleRequest> AddRightsToOrganizationRoles(List<OrgRoleRightsRequest> request)
        {
            try
            {
                var roleRights = _mapper.Map<List<OrgRoleRights>>(request);
                _unit.OrgRoleRightsRepository.InsertList(roleRights);

                if (await _unit.SaveAsync())
                {
                    var singleRoleRight = request.FirstOrDefault();
                    return await GetOrganizationRoleById(singleRoleRight.OrganizationId, singleRoleRight.OrganizationRoleId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new OrganizationRoleRequest();
        }

        public async Task<OrganizationRoleRequest> UpdateRightsToOrganizationRoles(List<OrgRoleRightsRequest> request)
        {
            try
            {
                var defaultRoleRight = request.FirstOrDefault();
                var requestRoleRights = _mapper.Map<List<OrgRoleRights>>(request);

                // Getting RoleRights From DB
                var dBRoleRights = await _unit
                                         .OrgRoleRightsRepository
                                         .GetAsync(x => x.OrganizationRoleId == defaultRoleRight.OrganizationRoleId
                                                   && x.OrganizationId == defaultRoleRight.OrganizationId);
                //

                if ((dBRoleRights?.Count() ?? 0) > 0)
                {
                    // Compare Logic for CRUD Operation for (request and requestRoleRights)                
                    var dBRightsIds = dBRoleRights.Select(x => x.OrgRoleRightsId).ToList();
                    var requestRightsIdsToDelete = requestRoleRights
                                                            .Where(x => x.OrgRoleRightsId > 0)
                                                            ?.Select(x => x.OrgRoleRightsId)
                                                            ?.ToList() ?? new List<long>();


                    // DELETE Section
                    // Getting RoleRights to Delete by comparing dBRoleRights with requestRightsIds (Which are not Exits in request)
                    var roleRightsToDelete = dBRoleRights
                                                    .Where(x => !requestRightsIdsToDelete.Contains(x.OrgRoleRightsId))
                                                    ?.ToList() ?? new List<OrgRoleRights>();

                    //
                    if (roleRightsToDelete?.Count > 0)
                    {
                        await DeleteRightsFromRole(roleRightsToDelete);
                    }
                    //

                    // INSERT Section
                    // Getting RoleRights to Add by comparing request with dbRoleRights (Which are Exits in request)
                    var roleRightsToAdd = requestRoleRights
                                                    .Where(x => !dBRightsIds.Contains(x.OrgRoleRightsId)
                                                           || !requestRightsIdsToDelete.Contains(x.OrgRoleRightsId))
                                                    ?.ToList() ?? new List<OrgRoleRights>();

                    //
                    if (roleRightsToAdd?.Count > 0)
                    {
                        _unit.OrgRoleRightsRepository.InsertList(roleRightsToAdd);
                    }
                    //
                }

                if (await _unit.SaveAsync())
                {
                    var singleRoleRight = request.FirstOrDefault();
                    return await GetOrganizationRoleById(singleRoleRight.OrganizationId, singleRoleRight.OrganizationRoleId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new OrganizationRoleRequest();
        }

        public async Task<List<RightsRequest>> GetRightsList()
        {
            try
            {
                var rightsList = await _unit.RightsRepository.GetAsync();
                return _mapper.Map<List<RightsRequest>>(rightsList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> DeleteRightsFromRole(List<OrgRoleRights> requestList)
        {
            try
            {
                var rightIds = requestList.Select(x => x.RightsId).ToList();

                var orgRoleRights = await _unit
                                          .OrgRoleRightsRepository
                                          .GetAsync(x => x.OrganizationRoleId == requestList.FirstOrDefault().OrganizationRoleId
                                                    && x.OrganizationId == requestList.FirstOrDefault().OrganizationId
                                                    && rightIds.Contains(x.RightsId));


                _unit.OrgRoleRightsRepository.DeleteRangeEntities(orgRoleRights.ToList());

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
