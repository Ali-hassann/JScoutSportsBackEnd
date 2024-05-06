using FastMember;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;

namespace AMNSystemsERP.CL.Helper
{
    public static class CommonHelper
    {
        public static readonly string SystemDefaultUser = "SMS_SYSTEM_USER"; // HardCoded Change in Future

        public static void WriteToFile(string textToSave, string serviceHolderName)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + $"\\Logs\\{serviceHolderName}";
                // Create Directory if not exist
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //

                string filepath = AppDomain.CurrentDomain.BaseDirectory + $"\\Logs\\{serviceHolderName}\\{serviceHolderName}Log_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
                if (!File.Exists(filepath))
                {
                    // Create a file to write to.   
                    using (StreamWriter sw = File.CreateText(filepath))
                    {
                        sw.WriteLine(textToSave);
                    }
                    //
                }
                else
                {
                    // if file exist appends coming text into existing file
                    using (StreamWriter sw = File.AppendText(filepath))
                    {
                        sw.WriteLine(textToSave);
                    }
                    //
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetEnumValue(Type enumType, string value)
        {
            try
            {
                // Converting To Pascal Case 
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                var pascalCaseValue = myTI.ToTitleCase(value.ToLower());
                //
                if (Enum.IsDefined(enumType, pascalCaseValue))
                {
                    var enumValue = Enum.Parse(enumType, pascalCaseValue);
                    return enumValue == null ? "" : Convert.ToString(enumValue);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return "";
        }

        public static object GetEnumObject(Type enumType, string value)
        {
            try
            {
                // Converting To Pascal Case 
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo; // Change culture info based on user prefrences
                var pascalCaseValue = myTI.ToTitleCase(value.ToLower());
                //
                if (Enum.IsDefined(enumType, pascalCaseValue))
                {
                    return Enum.Parse(enumType, pascalCaseValue);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public static string GetEnumIdsFromEnumString(Type enumType, string stringToConvert)
        {
            try
            {
                var enumIds = "";

                stringToConvert
                        ?.Split(',')
                        ?.ToList()
                        ?.ForEach((enumString) =>
                        {
                            if (Enum.IsDefined(enumType, enumString))
                            {
                                var enumValue = Enum.Parse(enumType, enumString);
                                enumIds = $"{enumIds}, {(enumValue == null ? "" : ((int)enumValue).ToString())}";
                            }
                        });

                return enumIds?.TrimStart(new char[] { ',', ' ' });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable ConvertListToDataTable<T>(IEnumerable<T> listToConvert)
        {
            try
            {
                DataTable convertedTable = new DataTable();
                using (var reader = ObjectReader.Create(listToConvert))
                {
                    convertedTable.Load(reader);
                }
                return convertedTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable ConvertListToDataTable<T>(IEnumerable<T> listToConvert, List<string> columnsToRemove)
        {
            try
            {
                DataTable convertedTable = new DataTable();
                using (var reader = ObjectReader.Create(listToConvert))
                {
                    convertedTable.Load(reader);
                }
                if (columnsToRemove?.Count > 0)
                {
                    foreach (var column in columnsToRemove)
                    {
                        convertedTable.Columns.Remove(column);
                    }
                }
                return convertedTable;
            }
            catch (Exception)
            {
                throw;
            }
        }
                
        public static long ConvertStringToLong(string value)
        {
            try
            {
                long.TryParse(value, out long longValue);
                return longValue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<long> ConvertDelimitedStringToList(string stringToConvert)
        {
            try
            {
                if (!string.IsNullOrEmpty(stringToConvert))
                {
                    return stringToConvert
                                .Split(",")
                                ?.Select(long.Parse)
                                ?.ToList() ?? new List<long>();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new List<long>();
        }

        public static string ConvertListToDelimitedString(List<long> listToConvert)
        {
            try
            {
                if (listToConvert?.Count > 0)
                {
                    return string.Join(",", listToConvert);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return string.Empty;
        }

        public static SqlBulkCopy SetColumnMappingsForBulkInsert(SqlBulkCopy bulkCopy
        , List<string> sourceTableColumns
        , List<string> destinationTableColumns)
        {
            try
            {
                for (int i = 0; i < sourceTableColumns.Count && i < destinationTableColumns.Count; i++)
                {
                    bulkCopy.ColumnMappings.Add(sourceTableColumns[i], destinationTableColumns[i]);
                }
                return bulkCopy;
            }
            catch (Exception)
            {
                throw;
            }
        }        

        public static Task<bool> AssignIdentityValueToObject<T>(T obj, object valueToAssign, string propertyName)
        {
            try
            {
                if (obj != null && !string.IsNullOrEmpty(propertyName))
                {
                    PropertyInfo prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(obj, valueToAssign, null);
                    }
                }
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Task<bool> AutoMapColumns(SqlBulkCopy sbc, DataTable dataTableToMap)
        {
            try
            {
                foreach (DataColumn column in dataTableToMap.Columns)
                {
                    sbc.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Task<DataTable> ConvertListToDataTable<T>(List<T> listToConvert, List<string> columnsToRemove)
        {
            try
            {
                DataTable convertedTable = new DataTable();
                using (var reader = new ObjectReader(listToConvert[0].GetType(), listToConvert, null))
                {
                    convertedTable.Load(reader);
                }

                if (columnsToRemove?.Count > 0)
                {
                    foreach (var columnName in columnsToRemove)
                    {
                        if (convertedTable.Columns.Contains(columnName))
                        {
                            convertedTable.Columns.Remove(columnName);
                        }
                    }
                }
                return Task.FromResult(convertedTable);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<object> FillChildObjectWithParentIdentity(object parentObject, string parentIdColumnName, string childProperty)
        {
            List<object> response = new List<object>();
            var type = parentObject.GetType();
            var idColumnValue = parentObject.GetType().GetProperty(parentIdColumnName).GetValue(parentObject);
            var propertyInfo = type.GetProperties().FirstOrDefault(c => c.Name == childProperty);
            if (propertyInfo != null)
            {
                object value = propertyInfo.GetValue(parentObject, null);
                if (propertyInfo.PropertyType.IsGenericType)
                {
                    foreach (object childObject in (IEnumerable)value)
                    {
                        AssignIdentityValueToObject(childObject, idColumnValue, parentIdColumnName);
                        response.Add(childObject);
                    }
                }
            }
            return response;
        }

        public static DataTable ListToDataTable(List<long> idList)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(long));

            if (idList?.Count > 0)
            {
                idList.ForEach(c =>
                {
                    DataRow row = table.NewRow();
                    row["Id"] = c;
                    table.Rows.Add(row);
                });
            }

            return table;
        }
    }
}