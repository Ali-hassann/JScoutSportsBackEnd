using AutoMapper;
using AMNSystemsERP.DL.DB.DBSets.EmployeePayroll;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Attendance;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Overtime;
using System.Data;

namespace AMNSystemsERP.BL.Repositories.EmployeePayroll.AttendanceRepo
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public AttendanceService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        // ------------------ Employee Section Start --------------------

        public async Task<List<AttendanceSummaryResponse>> GetAttendanceSummaryAndHistory(EmployeeFilterRequest request)
        {
            try
            {
                var query = $@"                                            
                                SELECT                                             
                                 EA.AttendanceId                                                
                                 , EAS.StatusName                                                      
                                 , EAS.ShortDescription                                                         
                                 , FORMAT(EAD.CheckIn, 'hh:mm tt') AS CheckIn                                          
                                 , FORMAT(EAD.CheckOut, 'hh:mm tt') AS CheckOut                                          
                                 , FORMAT(EA.AttendanceDate, 'MMM dd yyyy') AS AttendanceDate    
                                 , ISNULL(FORMAT(((SUM(DATEDIFF(MINUTE, EAD.CheckIn, EAD.CheckOut)) / 60) * 100) + (SUM(DATEDIFF(MINUTE, EAD.CheckIn, EAD.CheckOut)) % 60), '0#:0#'), '00:00') AS WorkingDetail                  
                                  , E.EmployeeId                        
                                  , EA.MarkType              
                                FROM V_EMPLOYEE AS E                                            
                                INNER JOIN Attendance AS EA                                                                      
                                 ON EA.EmployeeId = E.EmployeeId                                                        
                                LEFT JOIN                                          
                                (                                          
                                 SELECT                                          
                                  EAD.AttendanceId                                          
                                  , MIN(EAD.CheckTime) AS CheckIn                                                                  
                                  , MAX(EAD.CheckTime) AS CheckOut      
                                 FROM AttendanceDetail AS EAD                                          
                                 GROUP BY                                          
                                  EAD.AttendanceId                            
                                ) AS EAD                                          
                                 ON EAD.AttendanceId = EA.AttendanceId                        
                                INNER JOIN AttendanceStatus AS EAS                        
                                 ON EA.AttendanceStatusId = EAS.AttendanceStatusId                        
                                WHERE E.OrganizationId = {request.OrganizationId}       
                                AND E.OutletId = {request.OutletId}                                                                     
                                AND E.EmployeeId = {request.EmployeeIds}
                                {(request.StatusIds.Length > 0 ?
                                $"AND EA.AttendanceStatusId IN ({request.StatusIds})" : "")}
                                AND CAST(EA.AttendanceDate AS DATE)                                                                    
                                 BETWEEN {request.FromDate}                                  
                                 AND {request.ToDate}  
                                 GROUP BY  
                                 EA.AttendanceId                                                
                                 , EAS.StatusName                                                      
                                 , EAS.ShortDescription                                                         
                                 , EAD.CheckIn                                        
                                 , EAD.CheckOut                                          
                                 , EA.AttendanceDate    
                                 , E.EmployeeId                        
                                 , EA.MarkType   
                                ORDER BY                                                                        
                                 EA.AttendanceDate DESC";

                return await _unit.DapperRepository.GetListQueryAsync<AttendanceSummaryResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AttendanceResponse>> GetAttendanceList(EmployeeFilterRequest request)
        {
            try
            {
                var query = $@"
                                 SELECT                                    
                                    ISNULL(EA.AttendanceId , 0) AS AttendanceId                                    
                                    , E.EmployeeId                                          
                                    , E.EmployeeName                                                              
                                    , E.FatherName                                 
                                    , E.HusbandName                                 
                                    , E.OutletName                                                               
                                    , E.DepartmentsName                                            
                                    , ISNULL(UPPER(EAS.StatusName),'N/A') AS StatusName                                                                 
                                    , ISNULL(EAS.ShortDescription,'NA') AS ShortDescription                                                                                              
                                    , ISNULL(FORMAT(CAST(EAD.CheckIn as datetime), 'hh:mm tt'),'NILL') AS CheckIn                                              
                                    , ISNULL(FORMAT(CAST(EAD.CheckOut as datetime), 'hh:mm tt'),'NILL') AS CheckOut                                                        
                                    , FORMAT(ISNULL(EA.AttendanceDate, '{request.FromDate}'), 'MMM dd yyyy') AS AttendanceDate                                      
                                    , ISNULL(FORMAT(((SUM(DATEDIFF(MINUTE, EAD.CheckIn, EAD.CheckOut)) / 60) * 100) + (SUM(DATEDIFF(MINUTE, EAD.CheckIn, EAD.CheckOut)) % 60), '0#:0#'), '00:00') AS WorkingDetail                                                            
                                    , E.EmployeeId                        
                                    , ISNULL(EA.MarkType, '') AS MarkType
                                    , E.DesignationName
                                FROM V_EMPLOYEE AS E                                            
                                LEFT JOIN Attendance AS EA                                            
                                    ON EA.EmployeeId = E.EmployeeId                                
                                    AND E.OrganizationId = {request.OrganizationId}                                              
                                    AND CAST(EA.AttendanceDate AS DATE) = '{request.FromDate}'                              
                                LEFT JOIN AttendanceStatus AS EAS                                    
                                ON EAS.AttendanceStatusId = EA.AttendanceStatusId                                   
                                LEFT JOIN                                              
                                (                                              
                                    SELECT                             
                                    EAD.AttendanceId                                              
                                    , MIN(EAD.CheckIn) AS CheckIn                                                                      
                                    , MAX(EAD.CheckOut) AS CheckOut                                      
                                    FROM AttendanceDetail AS EAD                                      
                                    WHERE AttendanceId IN                            
                                    (                                            
                                    SELECT                                             
                                    AttendanceId                                            
                                    FROM Attendance                                             
                                    WHERE ISNULL(IsDeleted, 0) = 0                                          
                                    AND CAST(AttendanceDate AS DATE) = '{request.FromDate}'                                            
                                    )                                            
                                    GROUP BY AttendanceId                                              
                                ) AS EAD                                              
                                    ON EAD.AttendanceId = EA.AttendanceId                                             
                                WHERE E.OrganizationId = {request.OrganizationId}                                                             
                                AND E.OutletId = {request.OutletId}   
                                AND E.JoiningDate <= '{request.FromDate}'
                                AND E.DepartmentTypeId <> 3   
                                AND E.SalaryType <> 2   
                                {(request.StatusIds.Length > 0 ?
                                $"AND EA.AttendanceStatusId IN ({request.StatusIds})" : "")}
                                GROUP BY    
                                    EA.AttendanceId                                   
                                    , E.EmployeeId                                          
                                    , E.EmployeeName                                                              
                                    , E.FatherName                                 
                                    , E.HusbandName                                 
                                    , E.OutletName                                                               
                                    , E.DepartmentsName                                            
                                    , EAS.StatusName                                                               
                                    , EAS.ShortDescription                                                                                              
                                    , EAD.CheckIn                                             
                                    , EAD.CheckOut                                                           
                                    , EA.AttendanceDate                              
                                    , E.EmployeeId                        
                                    , EA.MarkType
                                    , E.DesignationName
                                ORDER BY                                                         
                                    EA.AttendanceDate DESC                                            
                                    , E.EmployeeId ASC";

                return await _unit.DapperRepository.GetListQueryAsync<AttendanceResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AttendanceRequest>> SaveAttendance(List<AttendanceRequest> request)
        {
            try
            {
                var attendanceList = _mapper.Map<List<Attendance>>(request);

                var attendanceToAdd = attendanceList.FindAll(x => x.AttendanceId == 0);
                var attendanceToUpdate = attendanceList.FindAll(x => x.AttendanceId > 0);

                // add attendance
                if (attendanceToAdd?.Count > 0)
                {
                    _unit.AttendanceRepository.InsertList(attendanceToAdd);
                }
                //

                // update attendance
                if (attendanceToUpdate?.Count > 0)
                {
                    _unit.AttendanceRepository.UpdateList(attendanceToUpdate);
                }
                //
                if (attendanceToUpdate?.Count > 0 || attendanceToAdd?.Count > 0)
                {
                    await _unit.SaveAsync();

                    return _mapper.Map<List<AttendanceRequest>>(attendanceList);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<List<AttendanceDetailResponse>> GetAttendanceDetailList(long attendanceId)
        {
            try
            {
                var query = $@"
                                SELECT                                        
                                 EA.AttendanceId                                
                                 , EAD.AttendanceDetailId                                
                                 , EAS.StatusName                                                            
                                 , EAS.ShortDescription                                                               
                                 , CASE                                                 
                                  WHEN ISNULL(EAD.CheckIn, '') = ''                                                 
                                  THEN 'NILL' ELSE EAD.CheckIn                                                              
                                  END AS CheckIn 
								 , EAD.CheckOut
                                 , FORMAT(EA.AttendanceDate, 'MMM dd yyyy') AS AttendanceDate            
                                 , E.EmployeeId                   
                                 , EAD.DetailMarkType          
                                FROM V_EMPLOYEE AS E                                                  
                                INNER JOIN Attendance AS EA                                                                            
                                 ON EA.EmployeeId = E.EmployeeId                                                              
                                LEFT JOIN                                                
                                (                                                                      
                                 SELECT                                            
                                  EAD.AttendanceId                                
                                  , EAD.AttendanceDetailId                   
                                  , FORMAT(CAST(EAD.CheckIn as datetime), 'hh:mm tt') AS CheckIn     
                                  , FORMAT(CAST(EAD.CheckOut as datetime), 'hh:mm tt') AS CheckOut                                     
                                  , EAD.DetailMarkType                                                      
                                 FROM AttendanceDetail AS EAD                                          
                                 WHERE EAD.AttendanceId = {attendanceId}                                        
                                ) AS EAD                                                
                                 ON EAD.AttendanceId = EA.AttendanceId                            
                                INNER JOIN AttendanceStatus AS EAS                            
                                 ON EAS.AttendanceStatusId = EA.AttendanceStatusId                            
                                WHERE EAD.AttendanceId = {attendanceId}                            
                                ORDER BY                                
                                 EAD.AttendanceDetailId ASC
                                ";

                return await _unit.DapperRepository.GetListQueryAsync<AttendanceDetailResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SaveAttendanceDetailList(List<AttendanceDetailRequest> request)
        {
            try
            {
                request.ForEach(att =>
                {
                    // Adding AttendanceDetail
                    var attendanceDetail = _mapper.Map<AttendanceDetail>(att);
                    if (att.AttendanceDetailId > 0)
                    {
                        _unit.AttendanceDetailRepository.Update(attendanceDetail);
                    }
                    else
                    {
                        _unit.AttendanceDetailRepository.InsertSingle(attendanceDetail);
                    }
                    //
                });

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveAttendanceDetail(long attendanceDetailId)
        {
            try
            {
                var query = @$"DELETE FROM AttendanceDetail WHERE AttendanceDetailId = {attendanceDetailId}";
                return await _unit.DapperRepository.ExecuteNonQuery(query, null, CommandType.Text) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AttendanceDashboardSummaryResponse> GetAttendanceDashboardSummary(AttendanceDashboardSummaryRequest request)
        {
            try
            {
                var query = $@"
                                ;WITH EMP_ATD_CTE                             
                                (                            
                                    EmployeeId                            
                                    , AttendanceId                            
                                    , AttendanceStatusId                            
                                    , AttendanceDate                            
                                )                                          
                                AS                                          
                                (                                          
                                    SELECT                                          
                                    EA.EmployeeId          
                                    , EA.AttendanceId                              
                                    , EA.AttendanceStatusId                              
                                    , EA.AttendanceDate                                
                                    FROM Employee AS EMP                                         
                                    INNER JOIN Attendance AS EA                                
                                    ON EMP.EmployeeId = EA.EmployeeId          
                                    WHERE ISNULL(EMP.IsDeleted, 0) = 0                                           
                                    AND ISNULL(EA.IsDeleted, 0) = 0          
                                    AND EMP.OutletId = {request.OutletId}                               
                                    AND EMP.OrganizationId = {request.OrganizationId}                            
                                    AND CAST(EA.AttendanceDate AS DATE) >= {request.FromDate}                                        
                                    AND CAST(EA.AttendanceDate AS DATE) <= {request.ToDate}                                
                                )                                          
                              
                                SELECT                                           
                                    COUNT(PRESENT.EmployeeId) AS PresentCount                            
                                    , COUNT(ABSENT.EmployeeId) AS AbsentCount                            
                                    , COUNT(LEAVEPAID.EmployeeId) AS LeavePaidCount          
                                    , COUNT(LEAVEUNPAID.EmployeeId) AS LeaveUnPaidCount          
                                    , COUNT(HALFLEAVEPAID.EmployeeId) AS HalfLeavePaidCount                                           
                                      
                                FROM EMP_ATD_CTE AS EA                                  
                                LEFT JOIN Attendance AS PRESENT                                           
                                    ON PRESENT.EmployeeId = EA.EmployeeId                                          
                                    AND PRESENT.AttendanceId = EA.AttendanceId                                        
                                    AND EA.AttendanceStatusId = 1                              
                                LEFT JOIN Attendance AS ABSENT                                           
                                    ON ABSENT.EmployeeId = EA.EmployeeId                                          
                                    AND ABSENT.AttendanceId = EA.AttendanceId                                        
                                    AND EA.AttendanceStatusId = 4                             
                                LEFT JOIN Attendance AS LEAVEPAID                                           
                                    ON LEAVEPAID.EmployeeId = EA.EmployeeId                                          
                                    AND LEAVEPAID.AttendanceId = EA.AttendanceId                                        
                                    AND EA.AttendanceStatusId = 2          
                                LEFT JOIN Attendance AS LEAVEUNPAID                                           
                                    ON LEAVEUNPAID.EmployeeId = EA.EmployeeId                              
                                    AND LEAVEUNPAID.AttendanceId = EA.AttendanceId            
                                    AND EA.AttendanceStatusId = 3          
                                LEFT JOIN Attendance AS HALFLEAVEPAID                                           
                                    ON HALFLEAVEPAID.EmployeeId = EA.EmployeeId                                          
                                    AND HALFLEAVEPAID.AttendanceId = EA.AttendanceId                                        
                                    AND EA.AttendanceStatusId = 5";

                return await _unit.DapperRepository.GetSingleQueryAsync<AttendanceDashboardSummaryResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OvertimeResponse>> GetOvertimeList(EmployeeFilterRequest request)
        {
            try
            {
                var query = $@"
                                 SELECT                                    
                                    ISNULL(EA.OvertimeId , 0) AS OvertimeId                                    
                                    , E.EmployeeId                                          
                                    , E.EmployeeName                                                              
                                    , E.FatherName                                 
                                    , E.HusbandName                                 
                                    , E.OutletName                                                               
                                    , E.DepartmentsName                                                                                                                                  
                                    , ISNULL(FORMAT(CAST(EAD.CheckIn as datetime), 'hh:mm tt'),'NILL') AS CheckIn                                              
                                    , ISNULL(FORMAT(CAST(EAD.CheckOut as datetime), 'hh:mm tt'),'NILL') AS CheckOut                                                        
                                    , FORMAT(ISNULL(EA.OvertimeDate, '{request.FromDate}'), 'MMM dd yyyy') AS OvertimeDate                                      
                                    , ISNULL(FORMAT(((SUM(DATEDIFF(MINUTE, EAD.CheckIn, EAD.CheckOut)) / 60) * 100) + (SUM(DATEDIFF(MINUTE, EAD.CheckIn, EAD.CheckOut)) % 60), '0#:0#'), '00:00') AS WorkingDetail                                                            
                                    , E.EmployeeId                        
                                    , ISNULL(EA.MarkType, '') AS MarkType            
                                FROM V_EMPLOYEE AS E                                            
                                LEFT JOIN Overtime AS EA                                            
                                    ON EA.EmployeeId = E.EmployeeId                                
                                    AND E.OrganizationId = {request.OrganizationId}                                              
                                    AND CAST(EA.OvertimeDate AS DATE) = '{request.FromDate}'     
                                LEFT JOIN                                              
                                (                                              
                                    SELECT                             
                                    EAD.OvertimeId                                              
                                    , MIN(EAD.CheckIn) AS CheckIn                                                                      
                                    , MAX(EAD.CheckOut) AS CheckOut                                      
                                    FROM OvertimeDetail AS EAD                                      
                                    WHERE OvertimeId IN                            
                                    (                                            
                                    SELECT                                             
                                    OvertimeId                                            
                                    FROM Overtime                                             
                                    WHERE CAST(OvertimeDate AS DATE) = '{request.FromDate}'                                            
                                    )                                            
                                    GROUP BY OvertimeId                                              
                                ) AS EAD                                              
                                    ON EAD.OvertimeId = EA.OvertimeId                                             
                                WHERE E.OrganizationId = {request.OrganizationId}                                                             
                                AND E.OutletId = {request.OutletId}
                                AND E.DepartmentTypeId <> 3
                                AND E.JoiningDate <= '{request.FromDate}'
                                AND E.SalaryType <> 2
                                GROUP BY    
                                    EA.OvertimeId                                   
                                    , E.EmployeeId                                          
                                    , E.EmployeeName                                                              
                                    , E.FatherName                                 
                                    , E.HusbandName                                 
                                    , E.OutletName                                                               
                                    , E.DepartmentsName                                                                                                 
                                    , EAD.CheckIn                                             
                                    , EAD.CheckOut                                                           
                                    , EA.OvertimeDate                                
                                    , E.EmployeeId                        
                                    , EA.MarkType     
                                ORDER BY                                                         
                                    EA.OvertimeDate DESC                                            
                                    , E.EmployeeId ASC";

                return await _unit.DapperRepository.GetListQueryAsync<OvertimeResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<bool> SaveOvertime(List<OvertimeRequest> request)
        {
            try
            {
                var OvertimeList = _mapper.Map<List<Overtime>>(request);

                var OvertimeToAdd = OvertimeList.FindAll(x => x.OvertimeId == 0);
                var OvertimeToUpdate = OvertimeList.FindAll(x => x.OvertimeId > 0);

                // add Overtime
                if (OvertimeToAdd?.Count > 0)
                {
                    _unit.OvertimeRepository.InsertList(OvertimeToAdd);
                }
                //

                // update Overtime
                if (OvertimeToUpdate?.Count > 0)
                {
                    _unit.OvertimeRepository.UpdateList(OvertimeToUpdate);
                }
                //
                if (OvertimeToUpdate?.Count > 0 || OvertimeToAdd?.Count > 0)
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

        public async Task<List<OvertimeDetailResponse>> GetOvertimeDetailList(long OvertimeId)
        {
            try
            {
                var query = $@"
                                SELECT                                        
                                 EA.OvertimeId                                
                                 , EAD.OvertimeDetailId                                                               
                                 , CASE                                                 
                                  WHEN ISNULL(EAD.CheckIn, '') = ''                                                 
                                  THEN 'NILL' ELSE EAD.CheckIn                                                              
                                  END AS CheckIn 
								 , EAD.CheckOut
                                 , FORMAT(EA.OvertimeDate, 'MMM dd yyyy') AS OvertimeDate            
                                 , E.EmployeeId                   
                                 , EAD.DetailMarkType          
                                FROM V_EMPLOYEE AS E                                                  
                                INNER JOIN Overtime AS EA                                                                            
                                 ON EA.EmployeeId = E.EmployeeId                                                              
                                LEFT JOIN                                                
                                (                                                                      
                                 SELECT                                            
                                  EAD.OvertimeId                                
                                  , EAD.OvertimeDetailId                   
                                  , FORMAT(CAST(EAD.CheckIn as datetime), 'hh:mm tt') AS CheckIn     
                                  , FORMAT(CAST(EAD.CheckOut as datetime), 'hh:mm tt') AS CheckOut                                     
                                  , EAD.DetailMarkType                                                      
                                 FROM OvertimeDetail AS EAD                                          
                                 WHERE EAD.OvertimeId = {OvertimeId}                                        
                                ) AS EAD                                                
                                 ON EAD.OvertimeId = EA.OvertimeId                                       
                                WHERE EAD.OvertimeId = {OvertimeId}                            
                                ORDER BY                                
                                 EAD.OvertimeDetailId ASC
                                ";

                return await _unit.DapperRepository.GetListQueryAsync<OvertimeDetailResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SaveOvertimeDetailList(List<OvertimeDetailRequest> request)
        {
            try
            {
                request.ForEach(att =>
                {
                    // Adding OvertimeDetail
                    var OvertimeDetail = _mapper.Map<OvertimeDetail>(att);
                    if (att.OvertimeDetailId > 0)
                    {
                        _unit.OvertimeDetailRepository.Update(OvertimeDetail);
                    }
                    else
                    {
                        _unit.OvertimeDetailRepository.InsertSingle(OvertimeDetail);
                    }
                    //
                });

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveOvertimeDetail(long overtimeDetailId)
        {
            try
            {
                var query = @$"DELETE FROM OvertimeDetail WHERE OvertimeDetailId = {overtimeDetailId}";
                return await _unit.DapperRepository.ExecuteNonQuery(query, null, CommandType.Text) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}