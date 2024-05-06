using System.Data;
namespace AMNSystemsERP.CL.Models.Commons
{
    public class DapperParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public int? Size { get; set; }
        public DbType DbType { get; set; }
        public bool isTableType { get; set; }
        public string TableTypeName { get; set; }

    }
}
