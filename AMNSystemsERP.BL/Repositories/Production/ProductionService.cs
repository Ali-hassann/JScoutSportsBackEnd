using AMNSystemsERP.CL.Enums;
using AMNSystemsERP.CL.Models.InventoryModels;
using AMNSystemsERP.CL.Models.ProductionModels;
using AMNSystemsERP.DL.DB.DBSets.Production;
using AutoMapper;

namespace AMNSystemsERP.BL.Repositories.Production
{
    public class ProductionService : IProductionService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public ProductionService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        #region Categories
        public async Task<ProductCategoryRequest> AddProductCategory(ProductCategoryRequest request)
        {
            try
            {
                var category = _mapper.Map<ProductCategory>(request);

                _unit.ProductCategoryRepository.InsertSingle(category);
                if (await _unit.SaveAsync())
                {
                    request.ProductCategoryId = category.ProductCategoryId;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ProductCategoryRequest();
        }

        public async Task<ProductCategoryRequest> UpdateProductCategory(ProductCategoryRequest request)
        {
            try
            {
                var category = _mapper.Map<ProductCategory>(request);

                _unit.ProductCategoryRepository.Update(category);

                if (await _unit.SaveAsync())
                {
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ProductCategoryRequest();
        }

        public async Task<bool> RemoveProductCategory(long categoryId)
        {
            try
            {
                _unit.ProductCategoryRepository.DeleteById(categoryId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductCategoryRequest>> GetProductCategoryList(long outletId)
        {
            try
            {
                var query = $@"SELECT 
	                                IC.ProductCategoryId
	                                , IC.ProductCategoryName
	                                , IC.OutletId
                                FROM ProductCategory AS IC
                                WHERE IC.OutletId = {outletId}";

                return await _unit.DapperRepository.GetListQueryAsync<ProductCategoryRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region ProcessType
        public async Task<ProcessTypeRequest> AddProcessType(ProcessTypeRequest request)
        {
            try
            {
                var processType = _mapper.Map<ProcessType>(request);

                _unit.ProcessTypeRepository.InsertSingle(processType);
                if (await _unit.SaveAsync())
                {
                    request.ProcessTypeId = processType.ProcessTypeId;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ProcessTypeRequest();
        }

        public async Task<ProcessTypeRequest> UpdateProcessType(ProcessTypeRequest request)
        {
            try
            {
                var processType = _mapper.Map<ProcessType>(request);

                _unit.ProcessTypeRepository.Update(processType);

                if (await _unit.SaveAsync())
                {
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ProcessTypeRequest();
        }

        public async Task<bool> RemoveProcessType(int processTypeId)
        {
            try
            {
                _unit.ProcessTypeRepository.DeleteById(processTypeId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProcessTypeRequest>> GetProcessTypeList()
        {
            try
            {
                var query = $@"SELECT 
	                                ProcessTypeId
	                                , SortOrder
	                                , MainProcessTypeId
	                                , ProcessTypeName
                                FROM ProcessType
                                ORDER BY SortOrder ASC";

                return await _unit.DapperRepository.GetListQueryAsync<ProcessTypeRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ProductSizes
        public async Task<ProductSizeRequest> AddProductSize(ProductSizeRequest request)
        {
            try
            {
                var size = _mapper.Map<ProductSize>(request);

                _unit.ProductSizeRepository.InsertSingle(size);

                if (await _unit.SaveAsync())
                {
                    request.ProductSizeId = size.ProductSizeId;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ProductSizeRequest();
        }

        public async Task<ProductSizeRequest> UpdateProductSize(ProductSizeRequest request)
        {
            try
            {
                var size = _mapper.Map<ProductSize>(request);

                _unit.ProductSizeRepository.Update(size);

                if (await _unit.SaveAsync())
                {
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ProductSizeRequest();
        }

        public async Task<bool> RemoveProductSize(int sizeId)
        {
            try
            {
                _unit.ProductSizeRepository.DeleteById(sizeId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductSizeRequest>> GetProductSizeList()
        {
            try
            {
                var unitList = (await _unit
                                      .ProductSizeRepository
                                      .GetAsync()
                               )?.ToList();

                if (unitList?.Count > 0)
                {
                    return _mapper.Map<List<ProductSizeRequest>>(unitList);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region Process
        public async Task<bool> SaveProcess(List<ProcessRequest> request)
        {
            try
            {
                var result = await DeleteAlreadyExistProcess(request);
                var listToInsert = new List<Process>();
                var listToUpdate = new List<Process>();

                request.ForEach(process =>
                {
                    if (process.Selected)
                    {
                        if (process.EntityState == EntityState.Inserted && process.ProcessId == 0)
                            listToInsert.Add(_mapper.Map<Process>(process));
                        else if (process.EntityState == EntityState.Updated && process.ProcessId > 0)
                            listToUpdate.Add(_mapper.Map<Process>(process));
                    }
                });

                if (listToInsert?.Count > 0 || listToUpdate.Count > 0)
                {
                    if (listToInsert?.Count > 0)
                        _unit.ProcessRepository.InsertList(listToInsert);

                    if (listToUpdate.Count > 0)
                        _unit.ProcessRepository.UpdateList(listToUpdate);

                    result = await _unit.SaveAsync();
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }

            return false;
        }

        public async Task<bool> SaveBulkProcess(List<ProcessRequest> request)
        {
            try
            {
                var listToInsert = new List<Process>();

                request.ForEach(process =>
                {
                    if (process.Selected)
                    {
                        if (process.ProcessId == 0 && process.ProcessRate > 0)
                            listToInsert.Add(_mapper.Map<Process>(process));
                    }
                });

                if (listToInsert?.Count > 0)
                {
                    if (listToInsert?.Count > 0)
                        _unit.ProcessRepository.InsertList(listToInsert);

                    return await _unit.SaveAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return false;
        }

        private async Task<List<ProcessRequest>> GetProcessList(List<ProcessRequest> request)
        {
            try
            {
                var orderMasterId = request?.FirstOrDefault()?.OrderMasterId;
                var productIds = request?.Select(r => r.ProductId)?.Distinct()?.ToList();
                var productSizeIds = request?.Select(r => r.ProductSizeId)?.Distinct()?.ToList();

                if (productIds?.Count > 0)
                {
                    var query = @$"
                                    SELECT
                                    ProcessId
	                                , 2 AS EntityState
	                                , ProductId
	                                , ProcessTypeId
	                                , ProcessRate
	                                , PRS.OtherRate
	                                , PRS.Description
                                    FROM Process
                                    WHERE OrderMasterId = {orderMasterId}
                                    AND ProductId IN ({string.Join(",", productIds)})
                                    AND ProductSizeId IN ({string.Join(",", productSizeIds)})";

                    return await _unit.DapperRepository.GetListQueryAsync<ProcessRequest>(query);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        private async Task<bool> DeleteAlreadyExistProcess(List<ProcessRequest> request)
        {
            try
            {
                var deletedProcess = request.FindAll(e => e.EntityState == EntityState.Deleted && e.ProcessId > 0);

                if (deletedProcess?.Count > 0)
                {
                    var processIds = deletedProcess?.Select(r => r.ProcessId)?.Distinct()?.ToList();

                    if (processIds?.Count > 0)
                    {
                        var queryToDeleteProcess = @$"
                                                    DELETE FROM ProductionProcess 
                                                      WHERE ProcessId IN ({string.Join(",", processIds)})
                                                    DELETE FROM Process 
                                                      WHERE ProcessId IN ({string.Join(",", processIds)})";

                        await _unit.DapperRepository.ExecuteNonQuery(queryToDeleteProcess, null, System.Data.CommandType.Text);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return false;
        }

        public async Task<bool> TransferProcess(long fromProductId, List<long> toProductIds)
        {
            try
            {
                var processList = (await _unit.ProcessRepository.GetAsync(r => r.ProductId == fromProductId))?.ToList();
                var processListToDelete = (await _unit.ProcessRepository.GetAsync(r => toProductIds.Contains(r.ProductId)))?.ToList();
                if (processListToDelete?.Count > 0)
                {
                    _unit.ProcessRepository.DeleteRangeEntities(processListToDelete);
                }

                if (processList?.Count > 0)
                {
                    toProductIds.ForEach(toProductId =>
                    {
                        processList.ForEach(process =>
                        {
                            var processToInsert = new Process();
                            processToInsert.ProcessRate = process.ProcessRate;
                            processToInsert.OtherRate = process.OtherRate;
                            processToInsert.ProcessTypeId = process.ProcessTypeId;
                            processToInsert.Description = process.Description;
                            processToInsert.ProductId = toProductId;
                            processToInsert.ProcessId = 0;

                            _unit.ProcessRepository.InsertSingle(processToInsert);
                        });
                    });
                }

                if (processList?.Count > 0)
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

        public async Task<bool> RemoveProcess(int processId)
        {
            try
            {
                _unit.ProcessRepository.DeleteById(processId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProcessRequest>> GetProcessListForStore()
        {
            try
            {
                var query = $@"SELECT DISTINCT
	                                ProcessId
									, O.OrderMasterId
									, O.OrderName
									, PR.ProcessTypeId
									, ProcessRate
									, OtherRate
									, Description
									, P.ProductId									
                                    , PR.ProductSizeId
									, P.ProductName
									, PT.ProcessTypeName
									, PT.MainProcessTypeId
                                FROM OrderMaster AS O
								INNER JOIN OrderDetail AS OD
									ON O.OrderMasterId = OD.OrderMasterId
								INNER JOIN Product AS P
									ON OD.ProductId = P.ProductId
								INNER JOIN Process AS PR
									ON PR.ProductId = OD.ProductId
									AND O.OrderMasterId = PR.OrderMasterId
								INNER JOIN ProcessType AS PT
									ON PT.ProcessTypeId = PR.ProcessTypeId";

                return await _unit.DapperRepository.GetListQueryAsync<ProcessRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProcessRequest>> GetProcessList()
        {
            try
            {
                var query = $@"SELECT 
								    O.OrderMasterId
									, O.OrderName
									, P.ProductId
									, P.ProductName
                                FROM OrderMaster AS O
								INNER JOIN OrderDetail AS OD
									ON O.OrderMasterId = OD.OrderMasterId
								INNER JOIN Product AS P
									ON OD.ProductId = P.ProductId";

                return await _unit.DapperRepository.GetListQueryAsync<ProcessRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProcessRequest>> GetProcessListByProduct(ProductionFilterRequest request)
        {
            try
            {
                var query = $@"SELECT 
	                                ISNULL(PRS.ProcessId,0) AS ProcessId
	                                , CASE WHEN PRS.ProcessId>0 THEN 1 ELSE 0 END AS Selected
	                                , CASE WHEN PRS.ProcessId>0 THEN 4 ELSE 1 END AS EntityState
	                                , PRO.ProductId
	                                , PRO.ProductName
	                                , PT.ProcessTypeId
	                                , PT.ProcessTypeName
	                                , ISNULL(PRS.ProcessRate,0) AS ProcessRate
	                                , ISNULL(PRS.OtherRate,0) AS OtherRate
	                                , ISNULL(PRS.Description,'') AS Description
                                FROM ProcessType AS PT
                                CROSS JOIN Product AS PRO
                                LEFT JOIN Process AS PRS
	                                ON PRS.ProcessTypeId = PT.ProcessTypeId
	                                AND PRO.ProductId = PRS.ProductId
                                    {(request.ProductSizeId > 0 ? $"AND PRS.ProductSizeId = {request.ProductSizeId}" : "")}
                                    {(request.ProductId > 0 ? $"AND PRS.ProductId = {request.ProductId}" : "")}
                                    {(request.OrderMasterId > 0 ? $"AND PRS.OrderMasterId = {request.OrderMasterId}" : "")}
                                WHERE PRO.ProductId = {request.ProductId}
                                ORDER BY 
                                    ISNULL(PRS.ProcessId,0) DESC
                                    , PT.SortOrder ASC";

                return await _unit.DapperRepository.GetListQueryAsync<ProcessRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region PlaningMaster
        public async Task<bool> SavePlaningMaster(List<PlaningMasterRequest> requestList)
        {
            try
            {
                // delete Existing Planning 
                await DeleteAlreadyExistPlaning(requestList);

                requestList.ForEach(request =>
                {
                    request.PlaningMasterId = 0;
                    var planingMaster = _mapper.Map<PlaningMaster>(request);
                    planingMaster.PlaningDetails = new List<PlaningDetail>();

                    request.Items.ForEach(x =>
                    {
                        var planingDetail = new PlaningDetail();
                        planingDetail.PlaningDetailId = 0;
                        planingDetail.PlaningMasterId = 0;
                        planingDetail.ItemId = x.ItemId;
                        planingDetail.UnitId = x.UnitId;
                        planingDetail.Price = x.Price;
                        planingDetail.Quantity = x.Quantity;
                        planingDetail.IsManualPrice = x.IsManualPrice;
                        planingMaster.PlaningDetails.Add(planingDetail);
                    });
                    _unit.PlaningMasterRepository.InsertSingle(planingMaster);
                });
                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        private async Task DeleteAlreadyExistPlaning(List<PlaningMasterRequest> request)
        {
            var productIds = request?.Select(r => r.ProductId)?.Distinct()?.ToList();
            var sizeIds = request?.Select(r => r.ProductSizeId)?.Distinct()?.ToList();

            if (productIds?.Count > 0 && sizeIds?.Count > 0)
            {
                var queryToDeletePlaning = @$"
                                              BEGIN TRANSACTION;
                                                BEGIN TRY
                                                    DELETE PD FROM PlaningDetail AS PD
	                                                INNER JOIN PlaningMaster AS PM
		                                                ON PM.PlaningMasterId = PD.PlaningMasterId
		                                                AND PM.ProductSizeId IN({string.Join(",", sizeIds)})	
		                                                AND PM.ProductId IN({string.Join(",", productIds)})
		                                                AND PM.OrderMasterId = {request?.FirstOrDefault()?.OrderMasterId}
	                                                DELETE FROM PlaningMaster 
	                                                WHERE ProductSizeId IN({string.Join(",", sizeIds)})	
		                                                AND ProductId IN({string.Join(",", productIds)})
		                                                AND OrderMasterId = {request?.FirstOrDefault()?.OrderMasterId}
                                                    COMMIT TRANSACTION; -- Only reached if there are no errors
                                                END TRY
                                                BEGIN CATCH
                                                    -- Handle the error
                                                    ROLLBACK TRANSACTION; -- Rollback changes due to the error
                                                END CATCH;";

                await _unit.DapperRepository.ExecuteNonQuery(queryToDeletePlaning, null, System.Data.CommandType.Text);
            }
        }

        public async Task<bool> RemovePlaningMaster(long planingMasterId)
        {
            try
            {
                var detailsToDelete = await _unit
                                                   .PlaningDetailRepository
                                                   .GetAsync(x => x.PlaningMasterId == planingMasterId);

                _unit.PlaningDetailRepository.DeleteRangeEntities(detailsToDelete.ToList());
                _unit.PlaningMasterRepository.DeleteById(planingMasterId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ItemRequest>> GetPlaningDetailById(ProductionFilterRequest request)
        {
            try
            {
                var query = $@"SELECT 
	                                B.PlaningMasterId 
	                                , BD.ItemId
                                    , BD.Quantity
                                    , BD.IsManualPrice
									, MAX(BD.Price) AS Price
	                                , I.ItemName
	                                , I.IsActive
	                                , I.PartNo
	                                , I.PurchasePrice
	                                , I.ReorderLevel
	                                , BD.UnitId
	                                , I.SalePrice
	                                , I.OutletId
	                                , U.UnitName
	                                , IC.ItemCategoryName
									, B.ProductId
									, B.ProductSizeId
									, PRS.ProductSizeName
									, P.ProductName
                                FROM PlaningMaster AS B
                                INNER JOIN PlaningDetail AS BD
	                                ON BD.PlaningMasterId = B.PlaningMasterId
								INNER JOIN Product AS P 
									ON P.ProductId = B.ProductId
								INNER JOIN ProductSize AS PRS
									ON PRS.ProductSizeId = B.ProductSizeId
                                INNER JOIN Item AS I
	                                ON I.ItemId = BD.ItemId
                                INNER JOIN ItemCategory AS IC
	                                ON IC.ItemCategoryId = I.ItemCategoryId
                                INNER JOIN Unit AS U
	                                ON U.UnitId = BD.UnitId
								WHERE B.OrderMasterId = {request.OrderMasterId}
								AND B.ProductId = {request.ProductId}
								AND B.ProductSizeId = {request.ProductSizeId}
								GROUP BY
	                                B.PlaningMasterId 
	                                , BD.ItemId
                                    , BD.Quantity
                                    , BD.IsManualPrice
	                                , I.ItemName
	                                , I.IsActive
	                                , I.PartNo
	                                , I.PurchasePrice
	                                , I.ReorderLevel
	                                , BD.UnitId
	                                , I.SalePrice
	                                , I.OutletId
	                                , U.UnitName
	                                , IC.ItemCategoryName
									, B.ProductId
									, B.ProductSizeId
									, PRS.ProductSizeName
									, P.ProductName
                               ";

                return await _unit.DapperRepository.GetListQueryAsync<ItemRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PlaningMasterRequest>> GetPlaningMasterList(long outletId)
        {
            try
            {
                var query = $@"SELECT 
	                                ISNULL(M.PlaningMasterId, 0) AS PlaningMasterId
	                                , P.OutletId
	                                , P.ProductId
	                                , ISNULL(M.Amount, 0) AS Amount
	                                , P.ProductName
									, PC.ProductCategoryName
								FROM Product AS P
                                LEFT JOIN PlaningMaster AS M
									ON P.ProductId = M.ProductId
								INNER JOIN ProductCategory AS PC
									ON PC.ProductCategoryId = P.ProductCategoryId
                                WHERE P.OutletId = {outletId}";

                return await _unit.DapperRepository.GetListQueryAsync<PlaningMasterRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ProductionProcess
        public async Task<bool> SaveProductionProcess(List<ProductionProcessRequest> request)
        {
            try
            {
                var listToInsert = new List<ProductionProcess>();
                var listToUpdate = new List<ProductionProcess>();
                var maxIssueNo = request?.FirstOrDefault()?.IssuanceNo ?? 0;
                if (maxIssueNo == 0)
                {
                    maxIssueNo = await GetMaxProductionIssuanceNo();
                }

                if (maxIssueNo > 0)
                {
                    request.ForEach(singleData =>
                    {
                        if (singleData.ProductionProcessId > 0 && singleData.EntityState == EntityState.Updated)
                        {
                            listToUpdate.Add(_mapper.Map<ProductionProcess>(singleData));
                        }
                        else if (singleData.ProductionProcessId == 0 && singleData.EntityState == EntityState.Inserted)
                        {
                            singleData.IssuanceNo = maxIssueNo;
                            listToInsert.Add(_mapper.Map<ProductionProcess>(singleData));
                        }
                    });
                }
                if (listToInsert?.Count > 0)
                {
                    _unit.ProductionProcessRepository.InsertList(listToInsert);
                }
                if (listToUpdate?.Count > 0)
                {
                    _unit.ProductionProcessRepository.UpdateList(listToUpdate);
                }

                if (listToInsert?.Count > 0 || listToUpdate?.Count > 0)
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

        private async Task<int> GetMaxProductionIssuanceNo()
        {
            try
            {
                var query = $@"Select ISNULL(Max(DISTINCT(IssuanceNo)),0) AS IssuanceNo FROM ProductionProcess";
                return (await _unit.DapperRepository.GetSingleQueryAsync<int>(query)) + 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SaveReceiveProcessList(List<ProductionProcessRequest> request)
        {
            try
            {
                var isToUpdate = request.FirstOrDefault()?.ProductionProcessId > 0;
                var list = _mapper.Map<List<ProductionProcess>>(request);
                if (isToUpdate)
                {
                    _unit.ProductionProcessRepository.UpdateList(list);
                }
                else
                {
                    _unit.ProductionProcessRepository.InsertList(list);
                }

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductionProcessRequest>> GetReceiveListByOrder(ProductionParameterRequest request)
        {
            try
            {
                var query = $@"SELECT 
	                                PP.ProductionProcessId
	                                , PP.IssuanceNo
									, PP.EmployeeId
									, PP.OrderMasterId
									, PP.ProcessId
									, PP.Status
									, PP.ProductionDate
	                                , PP.ProductId
	                                , PP.IssueQuantity
	                                , PP.ReceiveQuantity
	                                , PP.ProductSizeId
	                                , PS.ProductSizeName
	                                , PRO.ProcessRate
									, ORD.OrderName
	                                , P.ProductName
									, pt.ProcessTypeName
                              FROM  ProductionProcess PP 
				INNER JOIN V_EMPLOYEE E ON E.EmployeeId = PP.EmployeeId 
				INNER JOIN OrderMaster ORD ON ORD.OrderMasterId = PP.OrderMasterId 
				INNER JOIN Process PRO ON PRO.ProcessId= PP.ProcessId 
				  INNER JOIN ProductSize PS ON PP.ProductSizeId = PS.ProductSizeId 
				  INNER JOIN Product P ON PP.ProductId = P.ProductId
				  inner join ProcessType pt on PRO.ProcessTypeId=pt.ProcessTypeId
                                WHERE PP.OrderMasterId = {request.OrderMasterId}
                                AND PP.EmployeeId = {request.EmployeeId}
                                AND PP.Status = 2
                                AND Cast(PP.ProductionDate AS DATE) = CAST('{request.ToDate}' AS DATE)";

                return await _unit.DapperRepository.GetListQueryAsync<ProductionProcessRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductionProcessRequest>> GetIssuanceListByOrder(ProductionParameterRequest request)
        {
            try
            {
                var query = $@"SELECT 
									OD.OrderMasterId
									, P.ProductId				
									, P.ProductName
									, PS.ProductSizeId
									, PS.ProductSizeName
									, OD.Quantity AS OrderQuantity
								FROM OrderDetail AS OD
								INNER JOIN Product AS P
									ON P.ProductId = OD.ProductId
								INNER JOIN ProductSize AS PS
									ON PS.ProductSizeId = OD.ProductSizeId
								WHERE OD.OrderMasterId = {request.OrderMasterId}
";

                return await _unit.DapperRepository.GetListQueryAsync<ProductionProcessRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveProductionProcess(long productionProcessId)
        {
            try
            {
                _unit.ProductionProcessRepository.DeleteById(productionProcessId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveProductionProcessByIssueNo(int issueNo)
        {
            try
            {
                var processIdListToDelete = (await GetProcessListByIssueNo(issueNo))
                                                ?.Select(e => e.ProductionProcessId)?.ToList();

                if (processIdListToDelete?.Count > 0)
                {
                    _unit.ProductionProcessRepository.DeleteRangeByIds(processIdListToDelete);
                    return await _unit.SaveAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public async Task<List<ProductionProcessRequest>> GetProductionProcessList(ProductionParameterRequest request)
        {
            try
            {
                var query = $@"SELECT DISTINCT
	                                PP.IssuanceNo 
									, PP.EmployeeId
									, PP.OrderMasterId
									, PP.Status
									, PP.ProductionDate
	                            --    , PP.OutletId
									, ORD.OrderName
									, E.EmployeeName
                                FROM ProductionProcess AS PP
								INNER JOIN V_EMPLOYEE AS E
									ON E.EmployeeId = PP.EmployeeId
								INNER JOIN OrderMaster AS ORD
									ON ORD.OrderMasterId = PP.OrderMasterId
                                WHERE PP.OrderMasterId = {request.OrderMasterId}
                                AND PP.EmployeeId = {request.EmployeeId}
                                AND PP.ProductionDate BETWEEN CAST('{request.FromDate}' AS Date) AND CAST('{request.ToDate}' AS Date)
								AND Status = 1";

                return await _unit.DapperRepository.GetListQueryAsync<ProductionProcessRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductionProcessRequest>> GetProcessListByIssueNo(int issueNo)
        {
            try
            {
                var query = $@"SELECT 
	                                PP.ProductionProcessId
	                                , PP.IssuanceNo
									, PP.EmployeeId
									, PP.OrderMasterId
									, PP.Status
									, PP.ProductionDate
	                                , PP.ProductId
	                                , PP.IssueQuantity
									, ORD.OrderName
	                                , P.ProductName
									, pt.ProcessTypeName
									, PP.ProductSizeId
									, PS.ProductSizeName
									FROM     ProductionProcess PP INNER JOIN
                  V_EMPLOYEE E ON E.EmployeeId = PP.EmployeeId INNER JOIN
                  OrderMaster ORD ON ORD.OrderMasterId = PP.OrderMasterId INNER JOIN
                  Process PRO 
				  ON PRO.ProcessId= PP.ProcessId 
				  INNER JOIN ProductSize PS 
				  ON PP.ProductSizeId = PS.ProductSizeId 
				  INNER JOIN Product P 
				  ON PP.ProductId = P.ProductId
				  inner join ProcessType pt
				  on PRO.ProcessTypeId=pt.ProcessTypeId
                  WHERE PP.IssuanceNo = {issueNo}";

                return await _unit.DapperRepository.GetListQueryAsync<ProductionProcessRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductionProcessRequest>> GetReceivingProcessList(ProductionParameterRequest request)
        {
            try
            {
                var query = $@"SELECT 
		PP.EmployeeId
		, PP.OrderMasterId
	    , PP.ProductId
	    , PP.ProductSizeId
	    , SUM(ISNULL(PP.IssueQuantity,0)) AS IssueQuantity
	    , SUM(ISNULL(PP.ReceiveQuantity,0)) AS AlreadyReceiveQuantity
		, ORD.OrderName
		, E.EmployeeName
	    , P.ProductName
	    , PRO.ProcessRate
	    , PRO.ProcessId
		, PS.ProductSizeName
		, pt.ProcessTypeName
		, Cast(GetDAte() AS Date) AS ProductionDate
    FROM  ProductionProcess PP 
				INNER JOIN V_EMPLOYEE E ON E.EmployeeId = PP.EmployeeId 
				INNER JOIN OrderMaster ORD ON ORD.OrderMasterId = PP.OrderMasterId 
				INNER JOIN Process PRO ON PRO.ProcessId= PP.ProcessId 
				  INNER JOIN ProductSize PS ON PP.ProductSizeId = PS.ProductSizeId 
				  INNER JOIN Product P ON PP.ProductId = P.ProductId
				  inner join ProcessType pt on PRO.ProcessTypeId=pt.ProcessTypeId
                   WHERE PP.OrderMasterId = {request.OrderMasterId}
                                AND PP.EmployeeId = {request.EmployeeId}
								GROUP BY
									PP.EmployeeId
									, PP.OrderMasterId
	                                , PP.ProductId
	                                , PP.ProductSizeId
									, ORD.OrderName
									, E.EmployeeName
	                                , P.ProductName
	                                , PRO.ProcessRate
                                    , PRO.ProcessId
									, PS.ProductSizeName
									, pt.ProcessTypeName
                                Having SUM(ISNULL(PP.IssueQuantity,0)) > SUM(ISNULL(PP.ReceiveQuantity,0))";

                return await _unit.DapperRepository.GetListQueryAsync<ProductionProcessRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Products
        public async Task<ProductRequest> AddProduct(ProductRequest request)
        {
            try
            {
                var Product = _mapper.Map<Product>(request);

                _unit.ProductRepository.InsertSingle(Product);

                if (await _unit.SaveAsync())
                {
                    return await GetProductById(Product.ProductId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ProductRequest();
        }

        public async Task<ProductRequest> UpdateProduct(ProductRequest request)
        {
            try
            {
                var Product = _mapper.Map<Product>(request);
                _unit.ProductRepository.Update(Product);
                var isUpdated = await _unit.SaveAsync();

                if (isUpdated)
                {
                    return await GetProductById(Product.ProductId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ProductRequest();
        }

        public async Task<bool> RemoveProduct(long productId)
        {
            try
            {
                _unit.ProductRepository.DeleteById(productId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductRequest>> GetProductList(long outletId)
        {
            try
            {
                var query = $@"SELECT 
	                                I.ProductId
	                                , I.ProductName
	                                , I.OutletId
	                                , I.ProductCategoryId
	                                , C.ProductCategoryName
	                                , I.UnitId
	                                , U.UnitName
	                                , I.PartNo
	                                , I.IsActive
	                                , I.SalePrice
                                    , I.Color
                                FROM Product AS I
                                INNER JOIN Unit AS U
	                                ON I.UnitId = U.UnitId
                                INNER JOIN ProductCategory AS C
	                                ON I.ProductCategoryId = C.ProductCategoryId
                                WHERE I.OutletId = {outletId}";

                return await _unit.DapperRepository.GetListQueryAsync<ProductRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductRequest> GetProductById(long productId)
        {
            try
            {
                var query = $@"SELECT 
	                                I.ProductId
	                                , I.ProductName
	                                , I.OutletId
	                                , I.ProductCategoryId
	                                , C.ProductCategoryName
	                                , I.ProductSizeId
	                                , U.ProductSizeName
	                                , I.PartNo
	                                , I.IsActive
	                                , I.SalePrice
	                                , I.Color
                                FROM Product AS I
                                INNER JOIN ProductSize AS U
	                                ON I.ProductSizeId = U.ProductSizeId
                                INNER JOIN ProductCategory AS C
	                                ON I.ProductCategoryId = C.ProductCategoryId
                                WHERE I.ProductId = {productId}";

                var Product = await _unit.DapperRepository.GetSingleQueryAsync<ProductRequest>(query);
                if (Product?.ProductId > 0)
                {
                    return Product;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ProductRequest();
        }
        #endregion

        #region Production
        // ------------------ Order Services--------------------------------------        

        public async Task<OrderMasterRequest> AddOrder(OrderMasterRequest request)
        {
            try
            {
                var orderMaster = _mapper.Map<OrderMaster>(request);

                if (orderMaster?.OutletId > 0)
                {
                    request
                        .OrderDetailsRequest
                        .ForEach(orderDetail =>
                        {
                            var detail = _mapper.Map<OrderDetail>(orderDetail);
                            if (detail != null)
                            {
                                orderMaster.OrderDetails.Add(detail);
                            }
                        });

                    _unit.OrderMasterRepository.InsertSingle(orderMaster);

                    if (await _unit.SaveAsync())
                    {
                        return await GetOrderById(orderMaster.OrderMasterId);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new OrderMasterRequest();
        }

        public async Task<OrderMasterRequest> UpdateOrder(OrderMasterRequest request)
        {
            try
            {
                var dBOrder = await GetOrderDetailById(request.OrderMasterId);

                if (dBOrder?.Count > 0)
                {
                    var orderMaster = _mapper.Map<OrderMaster>(request);

                    /*Deleting existing line items against this order.No*/
                    dBOrder?.ForEach(orderDetail =>
                    {
                        _unit.OrderDetailRepository.DeleteById(orderDetail.OrderDetailId);
                    });

                    /*Adding new line items ...*/

                    request.OrderDetailsRequest.ForEach(orderDetail =>
                    {
                        var detail = _mapper.Map<OrderDetail>(orderDetail);
                        if (detail?.OrderMasterId > 0)
                        {
                            detail.OrderDetailId = 0;
                            orderMaster.OrderDetails.Add(detail);
                        }
                    });

                    _unit.OrderMasterRepository.Update(orderMaster);

                    if (await _unit.SaveAsync())
                    {
                        return await GetOrderById(orderMaster.OrderMasterId);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new OrderMasterRequest();
        }

        public async Task<OrderMasterRequest> GetOrderById(long orderMasterId)
        {
            try
            {
                var query = $@"SELECT   
	                                OrderMasterId        
	                                , OrderDate     	                                
                                    , OrderName           
	                                , DeliveryDate        
	                                , IM.ParticularId        
	                                , ParticularName      
	                                , Remarks        
	                                , OrderStatus        
	                                , TotalAmount            
	                                , OutletId
									, IM.OrderStatus
									, IM.OrderName
									, IM.OtherCost
                                FROM OrderMaster AS IM
                                INNER JOIN Particular AS P
	                                ON P.ParticularId = IM.ParticularId
                                WHERE OrderMasterId = {orderMasterId}";

                return await _unit.DapperRepository.GetSingleQueryAsync<OrderMasterRequest>(query) ?? new OrderMasterRequest();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OrderDetailRequest>> GetOrderDetailById(long orderMasterId)
        {
            try
            {
                var query = @$"
                                SELECT   
                                 PD.OrderDetailId  
                                 , PD.OrderMasterId  
                                 , PD.Quantity  
                                 , PD.ProductId  
                                 , PD.Price  
                                 , PD.ProductSizeId  
                                 , PS.ProductSizeName  
                                 , PD.Amount  
                                 , I.ProductName  
                                 , U.UnitName  
                                 , PD.Quantity AS OrderQuantity
                                FROM OrderMaster AS PO  
                                INNER JOIN OrderDetail AS PD  
                                 ON PD.OrderMasterId = PO.OrderMasterId  
                                 AND PD.OrderMasterId = {orderMasterId}  
                                INNER JOIN Product AS I  
                                 ON I.ProductId = PD.ProductId  
                                INNER JOIN Unit AS U  
                                 ON U.UnitId = I.UnitId
                                INNER JOIN ProductSize AS PS  
                                 ON PS.ProductSizeId = PD.ProductSizeId
                                INNER JOIN ProductCategory AS PC  
                                 ON PC.ProductCategoryId = I.ProductCategoryId  
                                WHERE PO.OrderMasterId = {orderMasterId}";

                return await _unit
                             .DapperRepository
                             .GetListQueryAsync<OrderDetailRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OrderMasterRequest>> GetOrderList(long outletId)
        {
            try
            {
                var query = @$"
                                SELECT                                        
                                     OM.OrderMasterId  
                                     , OM.OrderDate                                                   
                                     , OM.OrderName                                               
                                     , OM.DeliveryDate                                                   
                                     , OM.ParticularId                                        
                                     , OM.Remarks                                        
                                     , OM.TotalAmount                                        
                                     , OM.OrderStatus                
                                     , OM.OtherCost                
                                     , OM.OutletId      
                                     , P.ParticularName                              
                                    FROM OrderMaster AS OM                      
                                    INNER JOIN Particular AS P    
                                     ON P.ParticularId = OM.ParticularId    
                                    WHERE OM.OutletId = {outletId}";

                return await _unit
                             .DapperRepository
                             .GetListQueryAsync<OrderMasterRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveOrder(long orderMasterId)
        {
            try
            {
                var dBOrderMaster = await GetOrderDetailById(orderMasterId);

                if (dBOrderMaster?.Count > 0)
                {
                    dBOrderMaster
                        .ForEach(data =>
                        {
                            _unit.OrderDetailRepository.DeleteById(data.OrderDetailId);
                        });
                    _unit.OrderMasterRepository.DeleteById(orderMasterId);
                    return await _unit.SaveAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public async Task<bool> PostMultipleOrders(List<long> orderMasterIds, short orderStatus)
        {
            try
            {
                if (orderMasterIds?.Count > 0)
                {
                    var orders = (await _unit
                                         .OrderMasterRepository
                                         .GetAsync(x => orderMasterIds.Contains(x.OrderMasterId)))
                                         ?.ToList();

                    if (orders?.Count > 0)
                    {
                        orders.ForEach(x => x.OrderStatus = orderStatus);

                        _unit.OrderMasterRepository.UpdateList(orders);

                        return await _unit.SaveAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion
    }
}
