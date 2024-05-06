using AMNSystemsERP.CL.Enums;
using AMNSystemsERP.CL.Models.Commons;
using AMNSystemsERP.CL.Models.Commons.DocumentHelper;
using Core.CL.Enums;

namespace AMNSystemsERP.CL.Services.Documents
{
    public interface IDocumentHelperService
    {
        string SaveDoc(DocumentRequest documentRequest);
        Task<List<string>> SaveMultipleDoc(List<DocumentRequest> documentRequest);
        Task<bool> DeleteMultipleDoc(List<DocumentRequest> documentRequest);
        void DeleteSingleDoc(DocumentRequest documentRequest);
        Task<List<Base64Response>> GetBase64StringWithPath(List<Base64Request> request);
        void DeleteDocWithPath(string path);
        string DeleteAndAddImage(long organizationId
        , long outletId
        , string personId
        , EntityState recordState
        , PersonType personType
        , FileType documentType
        , FolderType directoryType
        , string imagePath
        , bool IsToDeleteImage);
        DocumentRequest GetDocModel(long organizationId
        , long outletId
        , string personId
        , string personType
        , FileType documentType
        , FolderType directoryType
        , string base64);
    }
}