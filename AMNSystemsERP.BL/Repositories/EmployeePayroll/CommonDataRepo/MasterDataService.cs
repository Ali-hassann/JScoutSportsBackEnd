using AutoMapper;
using System.Data;
using AMNSystemsERP.DL.DB.DBSets.EmployeePayroll;
using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Base;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Allowances;

namespace AMNSystemsERP.BL.Repositories.EmployeePayroll.CommonDataRepo
{
    public class CommonDataService : ICommonDataService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        public CommonDataService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        // ----------------- BenefitType Section Start -----------------------
        public async Task<List<AllowanceTypeRequest>> GetAllowanceTypeList(long organizationId, long outletId)
        {
            try
            {
                var query = $@"SELECT                              
                                 AT.AllowanceTypeId                    
                                 , AT.OrganizationId                            
                                 , AT.OutletId         
                                 , AT.Name       
                                FROM AllowanceType AS AT 
                                WHERE AT.OrganizationId = {organizationId}
                                AND AT.OutletId = {outletId}";

                return await _unit.DapperRepository.GetListQueryAsync<AllowanceTypeRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AllowanceTypeRequest> AddAllowanceType(AllowanceTypeRequest request)
        {
            try
            {
                AllowanceType benefitTypeToAdd = _mapper.Map<AllowanceType>(request);
                _unit.AllowanceTypeRepository.InsertSingle(benefitTypeToAdd);

                if (await _unit.SaveAsync())
                {
                    request.AllowanceTypeId = benefitTypeToAdd.AllowanceTypeId;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<AllowanceTypeRequest> UpdateAllowanceType(AllowanceTypeRequest request)
        {
            try
            {
                AllowanceType benefitTypeToUpdate = _mapper.Map<AllowanceType>(request);
                _unit.AllowanceTypeRepository.Update(benefitTypeToUpdate);

                if (await _unit.SaveAsync())
                {
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public Task<bool> RemoveAllowanceType(long allowanceTypeId)
        {
            _unit.AllowanceTypeRepository.DeleteById(allowanceTypeId);
            return _unit.SaveAsync();
        }

        #region Departments CRUD
        // ------------------ Departments Section Start ------------------
        public async Task<List<DepartmentsRequest>> GetDepartmentsList(long organizationId)
        {
            try
            {
                var query = $@"SELECT * FROM Departments 
                               WHERE ISNULL(IsDeleted , 0) = 0
                               AND OrganizationId = {organizationId}";

                return await _unit.DapperRepository.GetListQueryAsync<DepartmentsRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DepartmentsRequest> AddDepartment(DepartmentsRequest request)
        {
            try
            {
                var department = _mapper.Map<Departments>(request);
                _unit.DepartmentsRepository.InsertSingle(department);

                if (await _unit.SaveAsync())
                {
                    request.DepartmentsId = department.DepartmentsId;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<DepartmentsRequest> UpdateDepartment(DepartmentsRequest request)
        {
            try
            {
                var department = _mapper.Map<Departments>(request);
                _unit.DepartmentsRepository.Update(department);
                if (await _unit.SaveAsync())
                {
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<bool> RemoveDepartment(long departmentsId)
        {
            try
            {
                var query = $@"UPDATE Departments SET IsDeleted = 1 WHERE DepartmentsId = {departmentsId}";
                var res = await _unit.DapperRepository.ExecuteNonQuery(query, null, CommandType.Text);
                return res > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Designation CRUD     
        // ------------------ Designation Section Start ------------------
        public async Task<List<DesignationRequest>> GetDesignationList(long organizationId)
        {
            try
            {
                var query = $@"SELECT * FROM Designation 
                               WHERE ISNULL(IsDeleted , 0) = 0
                               AND OrganizationId = {organizationId}";

                return await _unit.DapperRepository.GetListQueryAsync<DesignationRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DesignationRequest> AddDesignation(DesignationRequest request)
        {
            try
            {
                var designation = _mapper.Map<Designation>(request);

                _unit.DesignationRepository.InsertSingle(designation);

                if (await _unit.SaveAsync())
                {
                    request.DesignationId = designation.DesignationId;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<DesignationRequest> UpdateDesignation(DesignationRequest request)
        {
            try
            {
                var designation = _mapper.Map<Designation>(request);
                _unit.DesignationRepository.Update(designation);

                var result = await _unit.SaveAsync();
                if (result)
                {
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<bool> RemoveDesignation(long designationId)
        {
            try
            {
                var query = $@"UPDATE Designation SET IsDeleted = 1 WHERE DesignationId = {designationId}";
                var res = await _unit.DapperRepository.ExecuteNonQuery(query, null, CommandType.Text);
                return res > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}