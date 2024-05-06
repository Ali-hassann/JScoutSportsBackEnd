using AMNSystemsERP.CL.Enums;
using AMNSystemsERP.CL.Models.Commons;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace AMNSystemsERP.CL.Helper
{
    public static class DBHelper
    {
        public static SqlParameter GenerateSqlParameter(string name
        , object value
        , DB_TYPE type
        , int size
        , string customTypeName = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(name)
                    && value != null)
                {
                    var param = new SqlParameter(name, (SqlDbType)type, size);
                    param.Value = value;
                    if (!string.IsNullOrEmpty(customTypeName))
                    {
                        param.TypeName = customTypeName;
                    }
                    return param;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public static List<SqlParameter> GetSqlParms(params SqlParameter[] parameters)
        {
            try
            {
                if (parameters?.Length > 0)
                {
                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    parameters?.ToList()?.ForEach(x => sqlParameters.Add(x));
                    return sqlParameters ?? new List<SqlParameter>();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public static DapperParameter GenerateDapperParameter(string name
        , object value
        , DbType type
        , int? size = null
        , bool isTableType = false
        , string TableTypeName = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    var param = new DapperParameter
                    {
                        Name = name,
                        Value = value ?? DBNull.Value,
                        DbType = type,
                        Size = size,
                        isTableType = isTableType,
                        TableTypeName = TableTypeName
                    };
                    return param;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        public static DynamicParameters GetDapperParms(params DapperParameter[] parameters)
        {
            try
            {
                DynamicParameters dynamicParameters = new DynamicParameters();

                if (parameters != null && parameters.Length > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        dynamicParameters.Add(parameter.Name,
                                              parameter.Value,
                                              parameter.DbType,
                                              ParameterDirection.Input,
                                              size: parameter.Size);
                    }
                }

                return dynamicParameters;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}