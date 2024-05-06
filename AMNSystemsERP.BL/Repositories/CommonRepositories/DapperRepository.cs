using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.Commons.Pagination;
using AMNSystemsERP.CL.Models.Commons;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace AMNSystemsERP.BL.Repositories.CommonRepositories
{
    public class DapperRepository
    {
        private string _connectionString { get; set; }
        public DapperRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual async Task<T> GetSingleWithStoreProcedureAsync<T>(string spName
        , DynamicParameters parms
        , CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using IDbConnection db = new SqlConnection(_connectionString);
                return (await db.QueryAsync<T>(spName, parms, commandType: commandType)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<List<T>> GetManyWithStoreProcedureAsync<T>(string spName
        , DynamicParameters parms
        , CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using IDbConnection db = new SqlConnection(_connectionString);
                return (await db.QueryAsync<T>(spName, parms, commandType: commandType)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<int> ExecuteNonQuery(string spName
        , DynamicParameters parms
        , CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using IDbConnection db = new SqlConnection(_connectionString);
                return await db.ExecuteAsync(spName, parms, commandType: commandType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<List<T>> GetListQueryAsync<T>(string query)
        {
            try
            {
                using IDbConnection db = new SqlConnection(_connectionString);
                return (await db.QueryAsync<T>(query, commandType: CommandType.Text)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<T> GetSingleQueryAsync<T>(string query)
        {
            try
            {
                using IDbConnection db = new SqlConnection(_connectionString);
                return (await db.QueryAsync<T>(query, commandType: CommandType.Text)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public virtual async Task<int> ExecuteNonQueryWithTransaction(string spName
        , DynamicParameters parms
        , CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var affectedRows = await connection.ExecuteAsync(spName, parms, commandType: commandType, transaction: transaction);
                        transaction.Commit();
                        return affectedRows;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> BulkInsertWithIdentityAsync<T>(List<T> entityData
        , string destinationTableName
        , string idColumn
        , List<string> columnsToNeglact)
        {
            try
            {
                if ((entityData?.Count() ?? 0) > 0)
                {
                    DataTable dataToInsert = await CommonHelper.ConvertListToDataTable(entityData, columnsToNeglact);
                    if (dataToInsert?.Rows?.Count > 0)
                    {
                        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                        {
                            sqlConnection.Open();
                            using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction(IsolationLevel.Serializable))
                            {
                                try
                                {
                                    var options = SqlBulkCopyOptions.KeepIdentity;
                                    var bulkCopy = new SqlBulkCopy(sqlConnection, options, sqlTransaction);

                                    var command = new SqlCommand($"SELECT Max({idColumn}) from {destinationTableName};", sqlConnection, sqlTransaction);
                                    var id = command.ExecuteScalar();

                                    int maxId = 0;

                                    if (id != null && id != DBNull.Value)
                                    {
                                        maxId = Convert.ToInt32(id);
                                    }

                                    for (int i = 0; i < dataToInsert.Rows.Count; i++)
                                    {
                                        DataRow row = dataToInsert.Rows[i];
                                        maxId = maxId + 1;
                                        row[idColumn] = maxId;
                                        await CommonHelper.AssignIdentityValueToObject(entityData[i], maxId, idColumn);
                                    }
                                    bulkCopy.DestinationTableName = destinationTableName;
                                    await CommonHelper.AutoMapColumns(bulkCopy, dataToInsert);
                                    await bulkCopy.WriteToServerAsync(dataToInsert);
                                    sqlTransaction.Commit();
                                    return entityData;
                                }
                                catch (Exception)
                                {
                                    sqlTransaction.Rollback();
                                }
                                finally
                                {
                                    sqlConnection.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public virtual async Task<IEnumerable<T>> BulkInsertWithParentChildAsync<T>(BulkInsertRequest<T> bulkInsertRequest)
        {
            try
            {
                if (bulkInsertRequest?.EntityData?.Count > 0
                    && bulkInsertRequest?.BulkConfiguration?.BulkInsertConfiguration != null)
                {
                    using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                    {
                        sqlConnection.Open();
                        using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction(IsolationLevel.Serializable))
                        {
                            try
                            {
                                var parentConfigurations = bulkInsertRequest?.BulkConfiguration.BulkInsertConfiguration;
                                var parentEntities = await BukInsertion(sqlConnection,
                                                                        sqlTransaction,
                                                                        bulkInsertRequest.EntityData,
                                                                        parentConfigurations.ColumnsToNeglact,
                                                                        parentConfigurations.IdColumn,
                                                                        parentConfigurations.DestinationTableName);

                                var dictionary = new Dictionary<string, List<object>>();

                                if (parentEntities?.Count > 0)
                                {
                                    foreach (var parent in parentEntities)
                                    {
                                        foreach (var childProperty in parentConfigurations.ChildPropertyName)
                                        {
                                            var childs = CommonHelper.FillChildObjectWithParentIdentity(parent, parentConfigurations.IdColumn, childProperty);

                                            if (dictionary.ContainsKey(childProperty))
                                            {
                                                dictionary.TryGetValue(childProperty, out List<object> values);
                                                if (values?.Count > 0)
                                                {
                                                    values.AddRange(childs);
                                                }
                                            }
                                            else
                                            {
                                                dictionary.Add(childProperty, childs);
                                            }
                                        }
                                    }

                                    foreach (var childConfigurations in parentConfigurations.ChildConfigurations)
                                    {
                                        dictionary.TryGetValue(childConfigurations.EntityName, out List<object> values);
                                        var childsElements = await BukInsertion(sqlConnection,
                                                                                sqlTransaction,
                                                                                values,
                                                                                childConfigurations.ColumnsToNeglact,
                                                                                childConfigurations.IdColumn,
                                                                                childConfigurations.DestinationTableName);
                                    }
                                }

                                sqlTransaction.Commit();
                                return parentEntities;
                            }
                            catch (Exception ex)
                            {
                                sqlTransaction.Rollback();
                            }
                            finally
                            {
                                sqlConnection.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        private async Task<List<T>> BukInsertion<T>(SqlConnection sqlConnection
        , SqlTransaction sqlTransaction
        , List<T> entityData
        , List<string> columnsToNeglact
        , string idColumn
        , string destinationTableName)
        {
            if (entityData?.Count > 0
                && sqlConnection != null
                && sqlTransaction != null
                && idColumn != null
                && destinationTableName != null)
            {
                DataTable dataToInsert = await CommonHelper.ConvertListToDataTable(entityData, columnsToNeglact);
                var options = SqlBulkCopyOptions.KeepIdentity;
                var bulkCopy = new SqlBulkCopy(sqlConnection, options, sqlTransaction);
                bulkCopy.BatchSize = 500;
                bulkCopy.BulkCopyTimeout = 60;

                var command = new SqlCommand($"SELECT Max({idColumn}) from {destinationTableName};", sqlConnection, sqlTransaction);
                var id = command.ExecuteScalar();

                int maxId = 0;

                if (id != null && id != DBNull.Value)
                {
                    maxId = Convert.ToInt32(id);
                }

                for (int i = 0; i < dataToInsert.Rows.Count; i++)
                {
                    DataRow row = dataToInsert.Rows[i];
                    maxId = maxId + 1;
                    row[idColumn] = maxId;
                    await CommonHelper.AssignIdentityValueToObject(entityData[i], maxId, idColumn);
                }

                bulkCopy.DestinationTableName = destinationTableName;
                await CommonHelper.AutoMapColumns(bulkCopy, dataToInsert);
                await bulkCopy.WriteToServerAsync(dataToInsert);
                return entityData;
            }
            return null;
        }

        #region multi result store procedure executation

        public virtual async Task<Tuple<A, B>> GetMultiResultsWithStoreProcedureAsync<A, B>(string spName, DynamicParameters parms)
        {
            try
            {
                A item1;
                B item2;

                using IDbConnection db = new SqlConnection(_connectionString);
                var reader = await db.QueryMultipleAsync(spName, parms, commandType: CommandType.StoredProcedure);

                try
                {
                    item1 = reader.ReadSingle<A>();
                }
                catch (Exception)
                {
                    item1 = default;
                }

                try
                {
                    item2 = reader.ReadSingle<B>();
                }
                catch (Exception)
                {
                    item2 = default;
                }

                var tuple = Tuple.Create(item1, item2);
                return tuple;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>>> GetMultiResultsWithStoreProcedureAsync<A, B, C, D, E, F, G>(string spName, DynamicParameters parms)
        {
            List<A> item1;
            List<B> item2;
            List<C> item3;
            List<D> item4;
            List<E> item5;
            List<F> item6;
            List<G> item7;

            using IDbConnection db = new SqlConnection(_connectionString);
            var reader = await db.QueryMultipleAsync(spName, parms, commandType: CommandType.StoredProcedure);

            try
            {
                item1 = reader.Read<A>().ToList();
            }
            catch (Exception)
            {
                item1 = default;
            }

            try
            {
                item2 = reader.Read<B>().ToList();
            }
            catch (Exception)
            {
                item2 = default;
            }

            try
            {
                item3 = reader.Read<C>().ToList();
            }
            catch (Exception)
            {
                item3 = new List<C>();
            }

            try
            {
                item4 = reader.Read<D>().ToList();
            }
            catch (Exception)
            {
                item4 = new List<D>();
            }

            try
            {
                item5 = reader.Read<E>().ToList();
            }
            catch (Exception)
            {
                item5 = new List<E>();
            }

            try
            {
                item6 = reader.Read<F>().ToList();
            }
            catch (Exception)
            {
                item6 = new List<F>();
            }

            try
            {
                item7 = reader.Read<G>().ToList();
            }
            catch (Exception)
            {
                item7 = new List<G>();
            }

            var tuple = Tuple.Create(item1, item2, item3, item4, item5, item6, item7);
            return tuple;
        }

        public async Task<Tuple<List<A>, List<B>>> GetMultipleResultsWithStoreProcedureAsync<A, B>(string spName, DynamicParameters parms)
        {
            List<A> item1;
            List<B> item2;

            using IDbConnection db = new SqlConnection(_connectionString);
            var reader = await db.QueryMultipleAsync(spName, parms, commandType: CommandType.StoredProcedure);

            try
            {
                item1 = reader.Read<A>().ToList();
            }
            catch (Exception)
            {
                item1 = default;
            }

            try
            {
                item2 = reader.Read<B>().ToList();
            }
            catch (Exception)
            {
                item2 = default;
            }

            var tuple = Tuple.Create(item1, item2);
            return tuple;
        }

        public virtual async Task<Tuple<A, List<B>>> GetMultiResultsWithStoreProcedureAsync<A, B>(string spName
        , DynamicParameters parms
        , CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                A item1;
                List<B> item2;

                using IDbConnection db = new SqlConnection(_connectionString);
                var reader = await db.QueryMultipleAsync(spName, parms, commandType: CommandType.StoredProcedure);

                try
                {
                    item1 = reader.ReadSingle<A>();
                }
                catch (Exception)
                {
                    item1 = default;
                }

                try
                {
                    item2 = reader.Read<B>().ToList();
                }
                catch (Exception)
                {
                    item2 = new List<B>();
                }

                var tuple = Tuple.Create(item1, item2);
                return tuple;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<Tuple<A, B, List<C>>> GetMultiResultsWithStoreProcedureAsync<A, B, C>(string spName, DynamicParameters parms)
        {
            try
            {
                A item1;
                B item2;
                List<C> item3;

                using IDbConnection db = new SqlConnection(_connectionString);
                var reader = await db.QueryMultipleAsync(spName, parms, commandType: CommandType.StoredProcedure);

                try
                {
                    item1 = reader.ReadSingle<A>();
                }
                catch (Exception)
                {
                    item1 = default;
                }

                try
                {
                    item2 = reader.ReadSingle<B>();
                }
                catch (Exception)
                {
                    item2 = default;
                }

                try
                {
                    item3 = reader.Read<C>().ToList();
                }
                catch (Exception)
                {
                    item3 = new List<C>();
                }

                var tuple = Tuple.Create(item1, item2, item3);
                return tuple;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<Tuple<A, B, List<C>, List<D>, List<E>>> GetMultiResultsWithStoreProcedureAsync<A, B, C, D, E>(string spName, DynamicParameters parms)
        {
            try
            {
                A item1;
                B item2;
                List<C> item3;
                List<D> item4;
                List<E> item5;

                using IDbConnection db = new SqlConnection(_connectionString);
                var reader = await db.QueryMultipleAsync(spName, parms, commandType: CommandType.StoredProcedure);

                try
                {
                    item1 = reader.ReadSingle<A>();
                }
                catch (Exception)
                {
                    item1 = default;
                }

                try
                {
                    item2 = reader.ReadSingle<B>();
                }
                catch (Exception)
                {
                    item2 = default;
                }

                try
                {
                    item3 = reader.Read<C>().ToList();
                }
                catch (Exception)
                {
                    item3 = new List<C>();
                }

                try
                {
                    item4 = reader.Read<D>().ToList();
                }
                catch (Exception)
                {
                    item4 = new List<D>();
                }

                try
                {
                    item5 = reader.Read<E>().ToList();
                }
                catch (Exception)
                {
                    item5 = new List<E>();
                }

                var tuple = Tuple.Create(item1, item2, item3, item4, item5);
                return tuple;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<Tuple<A, List<C>, List<D>, List<E>>> GetMultiResultsWithParentChildAndDetailAsync<A, C, D, E>(string spName, DynamicParameters parms)
        {
            try
            {
                A item1;
                List<C> item3;
                List<D> item4;
                List<E> item5;

                using IDbConnection db = new SqlConnection(_connectionString);
                var reader = await db.QueryMultipleAsync(spName, parms, commandType: CommandType.StoredProcedure);

                try
                {
                    item1 = reader.ReadSingle<A>();
                }
                catch (Exception)
                {
                    item1 = default;
                }

                try
                {
                    item3 = reader.Read<C>().ToList();
                }
                catch (Exception)
                {
                    item3 = new List<C>();
                }

                try
                {
                    item4 = reader.Read<D>().ToList();
                }
                catch (Exception)
                {
                    item4 = new List<D>();
                }

                try
                {
                    item5 = reader.Read<E>().ToList();
                }
                catch (Exception)
                {
                    item5 = new List<E>();
                }

                var tuple = Tuple.Create(item1, item3, item4, item5);
                return tuple;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<Tuple<A, List<B>, List<C>>> GetMultiResultsWithParentChildAndDetailAsync<A, B, C>(string spName
        , DynamicParameters parms
        , CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                A item1;
                List<B> item2;
                List<C> item3;

                using IDbConnection db = new SqlConnection(_connectionString);
                var reader = await db.QueryMultipleAsync(spName, parms, commandType: CommandType.StoredProcedure);

                try
                {
                    item1 = reader.ReadSingle<A>();
                }
                catch (Exception)
                {
                    item1 = default;
                }

                try
                {
                    item2 = reader.Read<B>().ToList();
                }
                catch (Exception)
                {
                    item2 = new List<B>();
                }

                try
                {
                    item3 = reader.Read<C>().ToList();
                }
                catch (Exception)
                {
                    item3 = new List<C>();
                }

                var tuple = Tuple.Create(item1, item2, item3);
                return tuple;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<PaginationResponse<A>> GetPaginationResultsWithStoreProcedureAsync<A>(string spName, DynamicParameters parms)
        {
            try
            {
                List<A> data = default;
                PaginationResponse<A> pagination = default;

                using IDbConnection db = new SqlConnection(_connectionString);
                var reader = await db.QueryMultipleAsync(spName, parms, commandType: CommandType.StoredProcedure);

                try
                {
                    var result = await reader.ReadAsync<A>();
                    if (result?.Count() > 0)
                    {
                        data = result.ToList();
                    }
                }
                catch (Exception)
                {
                    data = default;
                }

                try
                {
                    pagination = reader.ReadSingle<PaginationResponse<A>>();
                }
                catch (Exception)
                {
                    pagination = default;
                }

                if (pagination?.PageNumber > 0)
                {
                    pagination.Data = data;
                }
                return pagination;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}