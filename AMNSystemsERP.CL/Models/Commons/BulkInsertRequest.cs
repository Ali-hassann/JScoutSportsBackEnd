using System.Collections.Generic;

namespace AMNSystemsERP.CL.Models.Commons
{
    public class BulkInsertRequest<T>
    {
        public List<T> EntityData { get; set; }
        public BulkConfiguration BulkConfiguration { get; set; }
    }

    public class BulkInsertConfiguration
    {
        public string EntityName { get; set; }
        public string DestinationTableName { get; set; }
        public string ParentPropertyName { get; set; }
        public List<string> ChildPropertyName { get; set; }
        public List<BulkInsertConfiguration> ChildConfigurations { get; set; }
        public string IdColumn { get; set; }
        public List<string> ColumnsToNeglact { get; set; }
    }

    public class BulkConfiguration
    {
        public Dictionary<string, string> TableRelatioShips { get; set; }
        public BulkInsertConfiguration BulkInsertConfiguration { get; set; }
    }
}