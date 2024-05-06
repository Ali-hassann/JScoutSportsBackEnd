using AMNSystemsERP.CL.Enums;

namespace AMNSystemsERP.CL.Models.ProductionModels
{
    public class ProcessTypeRequest
    {
        public int ProcessTypeId { get; set; }
        public MainProcessType MainProcessTypeId { get; set; }
        public string ProcessTypeName { get; set; }
        public string MainProcessTypeName
        {
            get { return MainProcessTypeId.ToString(); }
        }
        public int SortOrder { get; set; }
    }
}