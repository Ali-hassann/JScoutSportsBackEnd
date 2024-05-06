using AutoMapper;
using System.Data;
using AMNSystemsERP.DL.DB.DBSets.EmployeePayroll;
using Core.CL.Enums;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee;
using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.Commons.DocumentHelper;
using AMNSystemsERP.CL.Services.Documents;
using AMNSystemsERP.BL.Repositories.ChartOfAccounts;
using AMNSystemsERP.DL.DB.DBSets.Accounts;
using AMNSystemsERP.CL.Models.AccountModels.ChartOfAccounts;
using AMNSystemsERP.CL.Enums;
using System;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Enums;

namespace AMNSystemsERP.BL.Repositories.EmployeePayroll.EmployeeRepo
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IDocumentHelperService _documentHelperService;
        private readonly IChartOfAccountsService _chartOfAccountsService;
        // private readonly IDocumentHelper _documentHelper;

        public EmployeeService(IUnitOfWork unit, IMapper mapper
            , IDocumentHelperService documentHelperService
            , IChartOfAccountsService chartOfAccountsService)
        {
            _unit = unit;
            _mapper = mapper;
            _documentHelperService = documentHelperService;
            _chartOfAccountsService = chartOfAccountsService;
        }

        // ------------------ Employee Section Start ------------------
        public async Task<EmployeeBasicRequest> AddEmployee(EmployeeRequest request)
        {
            try
            {
                var maxSerialNo = await GenerateEmployeeSerialNo(request.Employee.SalaryType);

                // Adding Employee & EmployeeWorkingDays
                Employee employeeToAdd = _mapper.Map<Employee>(request.Employee);
                employeeToAdd.ImagePath = string.Empty;
                employeeToAdd.EmployeeSerialNo = maxSerialNo;
                employeeToAdd.EmployeeCode = request.Employee.SalaryType == (int)SalaryType.Wages ? $"JSC-{maxSerialNo.ToString("D4")}" : $"JSE-{maxSerialNo.ToString("D4")}";
                _unit.EmployeeRepository.InsertSingle(employeeToAdd);

                // Checking if EmployeeDetail is not null
                if (request.EmployeeDetail != null)
                {
                    // Adding EmployeeDetail
                    var employeeDetail = _mapper.Map<EmployeeDetail>(request.EmployeeDetail);
                    employeeToAdd.EmployeeDetailList = employeeDetail;
                    //                   
                }
                //

                if (await _unit.SaveAsync())
                {
                    var empId = employeeToAdd.EmployeeId;
                    request.Employee.EmployeeId = empId;
                    await CreateEmployeeAccount(request.Employee);
                    // Employee Image Working
                    employeeToAdd.ImagePath = _documentHelperService
                                                        .DeleteAndAddImage(request.Employee.OrganizationId,
                                                                            employeeToAdd.OutletId,
                                                                            employeeToAdd.EmployeeId.ToString(),
                                                                            EntityState.Inserted,
                                                                            PersonType.Employee,
                                                                            FileType.Images,
                                                                            FolderType.Profile,
                                                                            request.Employee.ImagePath,
                                                                            false);

                    _unit.EmployeeRepository.Update(employeeToAdd);
                    //

                    if (await _unit.SaveAsync())
                    {
                        return await GetEmployeeByIdForList(employeeToAdd.EmployeeId);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        private async Task<long> GenerateEmployeeSerialNo(int salaryType)
        {
            var type = salaryType == (int)SalaryType.Wages ? $"{(int)SalaryType.Wages}" : $"{(int)SalaryType.SalaryPerson},{(int)SalaryType.FixSalary}";
            long employeeSerialIndex = 0;

            var query = $@"SELECT ISNULL(MAX(EmployeeSerialNo),0) as EmployeeSerialNo
                                    FROM Employee
                                    WHERE SalaryType IN ({type})";

            employeeSerialIndex = await _unit.DapperRepository.GetSingleQueryAsync<long>(query);
            return employeeSerialIndex + 1;
        }

        public async Task<EmployeeBasicRequest> UpdateEmployee(EmployeeRequest request)
        {
            try
            {
                // Updating Employee
                Employee employeeToUpdate = _mapper.Map<Employee>(request.Employee);
                employeeToUpdate.ImagePath = _documentHelperService
                                                          .DeleteAndAddImage(request.Employee.OrganizationId,
                                                                              employeeToUpdate.OutletId,
                                                                              employeeToUpdate.EmployeeId.ToString(),
                                                                              EntityState.Updated,
                                                                              PersonType.Employee,
                                                                              FileType.Images,
                                                                              FolderType.Profile,
                                                                              request.Employee.ImagePath,
                                                                              request.Employee.IsToDeleteImage);

                _unit.EmployeeRepository.Update(employeeToUpdate);
                //

                // Updating EmployeeDetail
                var employeeDetail = _mapper.Map<EmployeeDetail>(request.EmployeeDetail);
                _unit.EmployeeDetailRepository.Update(employeeDetail);
                //
                await CreateEmployeeAccount(request.Employee);
                if (await _unit.SaveAsync())
                {
                    return await GetEmployeeByIdForList(request.Employee.EmployeeId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        private async Task<bool> CreateEmployeeAccount(EmployeeBasicRequest request)
        {
            try
            {
                var employeeAlreadyRegistered = (await _unit
                                                      .PostingAccountsRepository
                                                      .GetAsync(e => e.EmployeeId == request.EmployeeId))?.FirstOrDefault();

                if (employeeAlreadyRegistered?.EmployeeId > 0)
                {
                    employeeAlreadyRegistered.Name = $"{request.EmployeeName} ({request.DesignationName})";
                    _unit.PostingAccountsRepository.Update(employeeAlreadyRegistered);
                    return true;
                }
                else
                {
                    var configList = await _unit.ConfigurationSettingRepository.GetAsync(u => u.OutletId == request.OutletId);
                    var subAccountId = configList.FirstOrDefault(d => d.AccountName == "Advances to Worker")?.AccountValue ?? 0;
                    if (subAccountId > 0)
                    {
                        var postingAccount = new PostingAccountsRequest()
                        {
                            IsActive = true,
                            PostingAccountsId = 0,
                            EmployeeId = request.EmployeeId,
                            Name = $"{request.EmployeeName} ({request.DesignationName})",
                            OutletId = request.OutletId,
                            SubCategoriesId = subAccountId,
                            OpeningDate = DateHelper.GetCurrentDate().Date,
                            OpeningDebit = decimal.Zero,
                            OpeningCredit = decimal.Zero
                        };

                        await _chartOfAccountsService.AddPostingAccount(postingAccount);
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveEmployee(long employeeId)
        {
            try
            {
                _unit.EmployeeRepository.DeleteById(employeeId);
                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<EmployeeRequest> GetEmployeeById(long employeeId)
        {
            try
            {
                var pEmployeeBasicId = DBHelper.GenerateDapperParameter("EMPLOYEEID", employeeId, DbType.Int64);
                var employeeResponse = await _unit
                                             .DapperRepository
                                             .GetMultiResultsWithStoreProcedureAsync<EmployeeBasicRequest,
                                                                                     EmployeeDetailRequest>("GET_EMPLOYEE_BY_ID",
                                                                                           DBHelper.GetDapperParms
                                                                                           (
                                                                                                pEmployeeBasicId
                                                                                           ));
                EmployeeBasicRequest employeeBasicResponse = employeeResponse.Item1;

                EmployeeDetailRequest employeeDetailResponse = employeeResponse.Item2;

                return new EmployeeRequest
                {
                    Employee = employeeBasicResponse,
                    EmployeeDetail = employeeDetailResponse,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmployeeBasicRequest>> GetEmployeeList(long organizationId, long outletId)
        {
            try
            {
                var query = $@"SELECT                                                               
                                 E.EmployeeId                                                                 
                                 , E.EmployeeName                             
                                 , E.FatherName                                                   
                                 , E.HusbandName                                                   
                                 , E.ContactNumber                                  
                                 , E.Gender                                                                 
                                 , E.CNIC                                     
                                 , E.IsActive                                                  
                                 , E.ImagePath                                      
                                 , E.DepartmentsId                 
                                 , D.Name AS DepartmentsName                
                                 , E.OutletId                                               
                                 , E.EmployeeCode                                               
                                 , E.EmployeeSerialNo                                               
                                 , O.OutletName                                     
                                 , E.OrganizationId                                        
                                 , E.JoiningDate                                   
                                 , CAST(E.SalaryAmount AS DECIMAL(18, 2)) AS SalaryAmount      
                                 , CAST(E.OvertimeHourlyWageAmount AS DECIMAL(18, 2)) AS OvertimeHourlyWageAmount                               
                                 , E.WorkingHours                                   
                                 , E.SalaryType                         
                                 , COUNT(EW.EmployeeWorkingDaysId) AS WorkingDaysCount            
                                 , E.DesignationId             
                                 , E.MaritalStatus       
                                 , DSG.DesignationName       
                                FROM Employee AS E
								INNER JOIN Designation AS DSG
									ON DSG.DesignationId = E.DesignationId
								INNER JOIN Outlet AS O
									ON O.OutletId = E.OutletId
								INNER JOIN Departments AS D
									ON D.DepartmentsId = E.DepartmentsId
                                LEFT JOIN EmployeeWorkingDays AS EW                      
                                 ON EW.EmployeeId = E.EmployeeId                      
                                WHERE E.OutletId = {outletId}                                                       
                                AND E.OrganizationId = {organizationId}                                      
                                GROUP BY                      
                                 E.EmployeeId                                                                 
                                 , E.EmployeeName                               
                                 , E.FatherName                                                   
                                 , E.HusbandName                                                   
                                 , E.ContactNumber                                    
                                 , E.Gender                                                                 
                                 , E.CNIC                                     
                                 , E.IsActive                                                  
                                 , E.ImagePath                                     
                                 , E.DepartmentsId                 
                                 , D.Name                
                                 , E.OutletId                                               
                                 , E.EmployeeCode                                               
                                 , E.EmployeeSerialNo                                                 
                                 , O.OutletName             
                                 , E.OrganizationId                                        
                                 , E.JoiningDate                                   
                                 , E.SalaryAmount                                 
                                 , E.OvertimeHourlyWageAmount      
                                 , E.WorkingHours                                   
                                 , E.SalaryType             
                                 , E.DesignationId                   
                                 , E.MaritalStatus
								 , DSG.DesignationName
                                ORDER BY                                      
                                 E.EmployeeId ASC";

                return await _unit.DapperRepository.GetListQueryAsync<EmployeeBasicRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<EmployeeBasicRequest> GetEmployeeByIdForList(long employeeId)
        {
            try
            {
                var query = $@"SELECT                                                                   
                                 E.EmployeeId                                                                 
                                 , E.EmployeeName                             
                                 , E.FatherName                                                   
                                 , E.HusbandName                                                   
                                 , E.ContactNumber                                    
                                 , E.Gender                                                                 
                                 , E.CNIC                                     
                                 , E.IsActive                                                  
                                 , E.ImagePath                                     
                                 , E.DepartmentsId               
                                 , E.DepartmentsName              
                                 , E.DesignationName              
                                 , E.OutletId                                               
                                 , E.OutletName                                     
                                 , E.OrganizationId                                        
                                 , E.JoiningDate                                  
                                 , E.LeftDate                                  
                                 , CAST(E.SalaryAmount AS DECIMAL(18, 2)) AS SalaryAmount                                   
                                 , CAST(E.OvertimeHourlyWageAmount AS DECIMAL(18, 2)) AS OvertimeHourlyWageAmount                         
                                 , E.WorkingHours                                   
                                 , E.SalaryType                             
                                 , E.MaritalStatus  
                                 , E.EmployeeCode                                               
                                 , E.EmployeeSerialNo   
                                FROM V_EMPLOYEE AS E                    
                                WHERE E.EmployeeId = {employeeId}";

                return await _unit.DapperRepository.GetSingleQueryAsync<EmployeeBasicRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // -------------------------------------------------------------------------
        // ------------------ Employee Document Section ----------------------------
        // -------------------------------------------------------------------------

        public async Task<List<EmployeeDocumentsRequest>> SaveEmployeeDocuments(List<EmployeeDocumentsRequest> request)
        {
            var documentsToAdd = new List<DocumentRequest>();
            var documentsToRemove = new List<DocumentRequest>();
            try
            {
                var employeeDocumentsToAdd = new List<EmployeeDocuments>();
                var employeeDocumentsToRemove = new List<EmployeeDocuments>();
                var defaultDocument = request.FirstOrDefault();

                foreach (var image in request)
                {
                    if (image.IsDeleted)
                    {
                        // Removing new Employee Documents
                        employeeDocumentsToRemove.Add(_mapper.Map<EmployeeDocuments>(image));
                        var fileName = DocumentHelper.GetFileNameFromPath(image.ImagePath);
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            var documentRequest = new DocumentRequest();
                            documentRequest.OrganizationId = defaultDocument.OrganizationId;
                            documentRequest.OutletId = defaultDocument.OutletId;
                            documentRequest.IsSkipPersonId = false;
                            documentRequest.PersonId = image.EmployeeId.ToString();
                            documentRequest.FolderType = FolderType.Documents;
                            documentRequest.FileType = FileType.Images;
                            documentRequest.PersonType = PersonType.Employee.ToString();
                            documentRequest.FileName = fileName;
                            documentsToRemove.Add(documentRequest);
                        }
                        _unit.EmployeeDocumentsRepository.DeleteRangeEntities(employeeDocumentsToRemove);
                        //

                    }
                    else if (DocumentHelper.IsBase64String(image.ImagePath))
                    {
                        // Getting new Employee Documents
                        documentsToAdd.Add(new DocumentRequest()
                        {
                            OrganizationId = defaultDocument.OrganizationId,
                            OutletId = defaultDocument.OutletId,
                            IsSkipPersonId = false,
                            PersonId = image.EmployeeId.ToString(),
                            FolderType = FolderType.Documents,
                            FileType = FileType.Images,
                            PersonType = PersonType.Employee.ToString(),
                            Base64 = image.ImagePath,
                            FileName = $"{Guid.NewGuid()}.jpg"
                        });
                        //
                    }
                }

                // Adding new Employee Documents
                var savedImages = await _documentHelperService.SaveMultipleDoc(documentsToAdd);
                if (savedImages?.Count > 0)
                {
                    foreach (var path in savedImages)
                    {
                        employeeDocumentsToAdd.Add(new EmployeeDocuments()
                        {
                            EmployeeId = defaultDocument.EmployeeId,
                            Imagepath = path,
                            OrganizationId = defaultDocument.OrganizationId,
                            OutletId = defaultDocument.OutletId,
                        });
                    }
                    _unit.EmployeeDocumentsRepository.InsertList(employeeDocumentsToAdd);
                }
                //

                if (await _unit.SaveAsync())
                {
                    // Remove Image files
                    if (documentsToRemove.Count > 0)
                    {
                        await _documentHelperService.DeleteMultipleDoc(documentsToRemove);
                    }
                    //

                    return _mapper.Map<List<EmployeeDocumentsRequest>>(employeeDocumentsToAdd);
                }
            }
            catch (Exception)
            {
                await _documentHelperService.DeleteMultipleDoc(documentsToAdd);
                throw;
            }
            return null;
        }

        public async Task<List<EmployeeDocumentsRequest>> GetEmployeeDocuments(EmployeeDocumentsRequest request)
        {
            try
            {
                var employeeDocuments = await _unit
                                             .EmployeeDocumentsRepository
                                             .GetAsync(x => x.EmployeeId == request.EmployeeId
                                                            && x.OrganizationId == request.OrganizationId
                                                            && x.OutletId == request.OutletId);

                return _mapper.Map<List<EmployeeDocumentsRequest>>(employeeDocuments);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // -------------------------------------------------------------------------
        // ------------------ Employee Qualification Section -----------------------
        // -------------------------------------------------------------------------

        public async Task<EmployeeQualificationsRequest> SaveQualification(EmployeeQualificationsRequest request)
        {
            try
            {
                var qualification = _mapper.Map<EmployeeQualifications>(request);

                request.QualificationSubjectList?.ForEach(subject =>
                {
                    subject.QualificationSubjectId = 0;
                    qualification.QualificationSubjects.Add(_mapper.Map<QualificationSubject>(subject));
                });

                if (request.EmployeeQualificationsId > 0)
                {
                    _unit.EmployeeQualificationsRepository.Update(qualification);

                    var subjectsToDelete = await _unit
                                                 .QualificationSubjectRepository
                                                 .GetAsync(x => x.EmployeeQualificationsId == request.EmployeeQualificationsId);

                    _unit.QualificationSubjectRepository.DeleteRangeEntities(subjectsToDelete.ToList());
                }
                else
                {
                    _unit.EmployeeQualificationsRepository.InsertSingle(qualification);
                }

                if (await _unit.SaveAsync())
                {
                    request.EmployeeQualificationsId = qualification.EmployeeQualificationsId;
                    request.QualificationSubjectList = new List<QualificationSubjectRequest>();
                    qualification?.QualificationSubjects?.DistinctBy(c => c.SubjectsId)?.ToList().ForEach(c =>
                    {
                        request.QualificationSubjectList.Add(_mapper.Map<QualificationSubjectRequest>(c));
                    });

                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<bool> RemoveQualification(long employeeQualificationsId)
        {
            try
            {
                var qualification = await GetQualificationById(employeeQualificationsId);
                if (qualification?.EmployeeQualificationsId > 0 && qualification.QualificationSubjectList?.Count > 0)
                {
                    var subjectsList = _mapper.Map<List<QualificationSubject>>(qualification.QualificationSubjectList.ToList());
                    if (subjectsList.Count > 0)
                    {
                        _unit.QualificationSubjectRepository.DeleteRangeEntities(subjectsList);
                    }
                }

                _unit.EmployeeQualificationsRepository.DeleteById(employeeQualificationsId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmployeeQualificationsRequest>> GetQualificationListByEmployeeId(long employeeId)
        {
            try
            {
                var qualificationList = await _unit
                                              .EmployeeQualificationsRepository
                                              .GetAsync(x => x.EmployeeId == employeeId);

                if (qualificationList?.Count() > 0)
                {
                    return _mapper.Map<List<EmployeeQualificationsRequest>>(qualificationList);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new List<EmployeeQualificationsRequest>();
        }

        public async Task<EmployeeQualificationsRequest> GetQualificationById(long employeeQualificationsId)
        {
            try
            {
                var query = $@"SELECT  
                                 EQ.EmployeeQualificationsId  
                                 , EQ.EmployeeId  
                                 , EQ.CGPA  
                                 , EQ.EducationalOrganizationsId  
                                 , EQ.PassingYear  
                                 , EQ.QualificationId  
                                 , EQ.Percentage  
                                FROM EmployeeQualifications AS EQ  
                                WHERE ISNULL(EQ.IsDeleted, 0) = 0  
                                AND EQ.EmployeeQualificationsId = {employeeQualificationsId}";
                return await _unit.DapperRepository.GetSingleQueryAsync<EmployeeQualificationsRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<bool> GenerateEmployeePostingAccount(long employee)
        //{
        //    try
        //    {
        //        if (employee?.EmployeeId > 0)
        //        {
        //            var postingAccount = await _unit
        //                                       .PostingAccountsRepository
        //                                       .GetSingleAsync(p => p.EmployeeId == employee.EmployeeId);

        //            if (postingAccount?.EmployeeId > 0)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                var configuration = await _unit
        //                                          .ConfigurationSettingRepository
        //                                          .GetSingleAsync(c => c.OutletId == employee.OutletId && c.AccountName == "Debtors");
        //                if (configuration?.AccountValue > 0)
        //                {
        //                    var postingAccountToAdd = new PostingAccounts()
        //                    {
        //                        OutletId = employee.OutletId,
        //                        EmployeeId = employee.EmployeeId,
        //                        IsActive = true,
        //                        Name = employee.EmployeeName,
        //                        OrganizationId = employee.OrganizationId,
        //                        SubCategoriesId = configuration.AccountValue,
        //                        OpeningDate = new DateTime(),
        //                        OpeningCredit = 0,
        //                        OpeningDebit = 0
        //                    };
        //                    _unit.PostingAccountsRepository.InsertSingle(postingAccountToAdd);
        //                    return true;
        //                }
        //            }
        //        }
        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //
    }
}