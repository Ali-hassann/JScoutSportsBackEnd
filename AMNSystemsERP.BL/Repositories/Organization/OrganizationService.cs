using AMNSystemsERP.BL.Repositories.Identity;
using AMNSystemsERP.DL.DB.DBSets.Organization;
using AutoMapper;
using Core.CL.Enums;
using AMNSystemsERP.CL.Models.Commons.Base;
using DbOrganization = AMNSystemsERP.DL.DB.DBSets.Organization.Organization;
using AMNSystemsERP.CL.Services.Documents;
using AMNSystemsERP.CL.Models.OrganizationModels;
using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.IdentityModels;
using AMNSystemsERP.CL.Enums;
using AMNSystemsERP.CL.Models.OrganizationModelsS;

namespace AMNSystemsERP.BL.Repositories.Organization
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IDocumentHelperService _documentHelperService;
        private readonly IIdentityService _identityService;

        public OrganizationService(IUnitOfWork unit
        , IMapper mapper
        , IDocumentHelperService documentHelperService
        , IIdentityService identityService)
        {
            _unit = unit;
            _mapper = mapper;
            _documentHelperService = documentHelperService;
            _identityService = identityService;
        }

        public async Task<BaseResponse> OrganizationAndOutletCreation(OrganizationRegRequests request)
        {
            try
            {
                // Creating Organization
                var institute = _mapper.Map<DbOrganization>(request);
                institute.ImagePath = string.Empty;
                institute.CreatedBy = CommonHelper.SystemDefaultUser;
                institute.ModifiedBy = CommonHelper.SystemDefaultUser;
                //

                // Creating Default Outlet
                Outlet branch = new Outlet
                {
                    OutletName = institute.OrganizationName,
                    OutletOwnerName = institute.OwnerName,
                    ContactNumber = request.ContactNumber,
                    Address = string.Empty,
                    ImagePath = string.Empty,
                    Email = request.Email,
                    Country = request.Country,
                    City = request.City,
                    OrganizationId = institute.OrganizationId,
                    IsDefault = true,
                    IsActive = true,
                    CreatedDate = DateHelper.GetCurrentDate(),
                    ModifiedDate = DateHelper.GetCurrentDate(),
                    CreatedBy = CommonHelper.SystemDefaultUser,
                    ModifiedBy = CommonHelper.SystemDefaultUser,
                    StartingTime = DateHelper.GetCurrentDate(),
                    EndTime = DateHelper.GetCurrentDate()
                };
                institute.Outlets.Add(branch);
                //

                // Adding & Saving (Organization, Outlet) to DB 
                _unit.OrganizationRepository.InsertSingle(institute);
                var institutAndOutletResult = await _unit.SaveAsync();
                //

                if (institutAndOutletResult)
                {
                    await _unit.SaveAsync();

                    try
                    {
                        // Creating User
                        var applicationUserRequest = new ApplicationUserRequest()
                        {
                            OrganizationId = institute.OrganizationId,
                            OutletId = branch.OutletId,
                            UserName = request.UserName.Replace(" ", ""),
                            Password = request.Password,
                            Email = request.Email,
                            NormalizedEmail = request.Email,
                            RoleName = "Admin",
                            EmailConfirmed = true,
                            IsDeleted = false,
                            FirstName = request.UserName,
                            LastName = request.UserName,
                        };

                        var genericBaseModel = new CommonBaseModel()
                        {
                            OutletId = branch.OutletId,
                            OrganizationId = institute.OrganizationId,
                        };

                        var userResult = await _identityService.AddUser(applicationUserRequest);

                        if (userResult?.Success ?? false)
                        {
                            return new BaseResponse()
                            {
                                Success = true,
                                Message = $"Your account has been created successfully."
                            };
                        }
                        else if ((userResult?.Message?.Count() ?? 0) > 0)
                        {
                            return new BaseResponse()
                            {
                                Success = false,
                                Message = string.Join(',', userResult.Message.ToList())
                            };
                        }
                    }
                    catch (Exception)
                    {
                        return new BaseResponse()
                        {
                            Success = true,
                            Message = $"Your account has been created successfully. If you have any query please contact software team"
                        };
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        public async Task<OrganizationProfile> UpdateOrganization(OrganizationProfile instituteProfile)
        {
            try
            {
                var institute = await _unit.OrganizationRepository.GetByIDAsync(instituteProfile.OrganizationId);
                if (institute != null)
                {
                    // Organization Profile Updation
                    institute.OrganizationId = instituteProfile.OrganizationId;
                    institute.OrganizationName = instituteProfile.OrganizationName;
                    institute.OwnerName = instituteProfile.OwnerName;
                    institute.ImagePath = string.Empty;
                    institute.FoundedYear = instituteProfile.FoundedYear;
                    //
                    // Default Outlet Profile Updation
                    var branch = await _unit.OutletRepository.GetByIDAsync(instituteProfile.OutletId);

                    if (branch != null)
                    {
                        branch.OutletId = instituteProfile.OutletId;
                        branch.OutletName = instituteProfile.OutletName;
                        branch.OutletOwnerName = instituteProfile.OutletOwnerName;
                        branch.ContactNumber = instituteProfile.ContactNumber;
                        branch.Address = instituteProfile.Address;
                        branch.Email = instituteProfile.Email;
                        branch.City = instituteProfile.City;
                        branch.OrganizationId = instituteProfile.OrganizationId;
                        branch.IsActive = instituteProfile.IsActive;

                        // Saving Organization Image
                        branch.ImagePath = _documentHelperService
                                                        .DeleteAndAddImage(instituteProfile.OrganizationId,
                                                                            instituteProfile.OutletId,
                                                                            instituteProfile.OrganizationId.ToString(),
                                                                            EntityState.Updated,
                                                                            PersonType.Outlet,
                                                                            FileType.Images,
                                                                            FolderType.Profile,
                                                                            instituteProfile.ImagePath,
                                                                            instituteProfile.IsToDeleteImage);

                        _unit.OutletRepository.Update(branch);
                        _unit.OrganizationRepository.Update(institute);

                        var result = await _unit.SaveAsync(true);
                        if (result)
                        {
                            instituteProfile.ImagePath = branch.ImagePath;
                            return instituteProfile;
                        }
                    }
                    //
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        public async Task<OrganizationProfile> GetOrganizationProfile(long organizationId, long outletId)
        {
            try
            {
                var query = $@"SELECT                                
                                 O.OrganizationId                                  
                                 , O.OrganizationName                        
                                 , O.OwnerName                          
                                 , OT.OutletId                        
                                 , OT.OutletName                        
                                 , OT.OutletOwnerName                        
                                 , OT.ContactNumber                        
                                 , OT.Address                             
                                 , OT.Email                        
                                 , OT.Country                                          
                                 , OT.City                                          
                                 , OT.IsDefault                        
                                 , OT.IsActive                                  
                                 , OT.ImagePath   
                                FROM Organization AS O                                  
                                INNER JOIN Outlet AS OT                                 
                                 ON O.OrganizationId = OT.OrganizationId
                                WHERE ISNULL(OT.IsDeleted, 0) = 0                        
                                AND OT.IsActive = 1                
                                AND OT.OutletId = {outletId}                 
                                AND OT.OrganizationId = {organizationId}                      
                                AND O.OrganizationId = {organizationId}";

                return await _unit.DapperRepository.GetSingleQueryAsync<OrganizationProfile>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutletProfileRequest> AddOutlet(OutletProfileRequest branchProfile)
        {
            try
            {
                Outlet branch = _mapper.Map<Outlet>(branchProfile);

                branch.ImagePath = _documentHelperService
                                            .DeleteAndAddImage(branch.OrganizationId,
                                                                branch.OutletId,
                                                                branch.OutletId.ToString(),
                                                                EntityState.Updated,
                                                                PersonType.Outlet,
                                                                FileType.Images,
                                                                FolderType.Profile,
                                                                branchProfile.ImagePath,
                                                                branchProfile.IsToDeleteImage);

                _unit.OutletRepository.InsertSingle(branch);
                var result = await _unit.SaveAsync();
                return _mapper.Map<OutletProfileRequest>(branch);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutletProfileRequest> UpdateOutlet(OutletProfileRequest branchProfile)
        {
            try
            {
                Outlet branch = _mapper.Map<Outlet>(branchProfile);

                branch.ImagePath = _documentHelperService
                                            .DeleteAndAddImage(branch.OrganizationId,
                                                                branch.OutletId,
                                                                branch.OutletId.ToString(),
                                                                EntityState.Updated,
                                                                PersonType.Outlet,
                                                                FileType.Images,
                                                                FolderType.Profile,
                                                                branchProfile.ImagePath,
                                                                branchProfile.IsToDeleteImage);

                _unit.OutletRepository.Update(branch);
                var result = await _unit.SaveAsync();
                return _mapper.Map<OutletProfileRequest>(branch);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OutletProfileRequest>> GetOutletList(long OrganizationId)
        {
            try
            {
                var branchList = await _unit.OutletRepository.GetAsync(x => x.OrganizationId == OrganizationId);

                return _mapper.Map<List<OutletProfileRequest>>(branchList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutletProfileRequest> GetOutletById(long OutletId)
        {
            try
            {
                var branch = await _unit.OutletRepository.GetByIDAsync(OutletId) ?? new Outlet();

                return _mapper.Map<OutletProfileRequest>(branch);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveOutlet(long OutletId)
        {
            try
            {
                // Deleting Outlet
                _unit.OutletRepository.DeleteById(OutletId);
                //
                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
