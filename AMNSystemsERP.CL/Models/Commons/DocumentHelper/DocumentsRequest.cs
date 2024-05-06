using AMNSystemsERP.CL.Enums;
using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.Commons.DocumentHelper
{
    public class DocumentRequest : CommonBaseModel
    {
        public string PersonId { get; set; }
        public string PersonType { get; set; }
        public FileType FileType { get; set; }
        public FolderType FolderType { get; set; }
        public string Base64 { get; set; }
        public string FileName { get; set; }
        public bool IsSkipPersonId { get; set; }
    }
}
