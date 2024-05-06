using AMNSystemsERP.CL.Enums;
using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.Commons;
using AMNSystemsERP.CL.Models.Commons.DocumentHelper;
using Core.CL.Enums;

namespace AMNSystemsERP.CL.Services.Documents
{
    public class DocumentHelperService : IDocumentHelperService
    {
        public string SaveDoc(DocumentRequest documentRequest)
        {
            try
            {
                var documentSavedPath = DocumentHelper.GetSavingFolderPath(documentRequest);
                return DocumentHelper.SaveBase64Image(documentRequest.Base64, documentSavedPath, documentRequest.FileName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<string>> SaveMultipleDoc(List<DocumentRequest> documentRequest)
        {
            try
            {
                var defaultRequest = documentRequest.FirstOrDefault();

                var savedFolderPath = DocumentHelper.GetSavingFolderPath(defaultRequest);

                var savedPath = new List<string>();
                foreach (var item in documentRequest)
                {
                    savedPath.Add(DocumentHelper.SaveBase64Image(item.Base64, savedFolderPath, item.FileName));
                    Thread.Sleep(500);
                }
                return Task.FromResult(savedPath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> DeleteMultipleDoc(List<DocumentRequest> documentRequest)
        {
            try
            {
                var defaultRequest = documentRequest.FirstOrDefault();

                var documentSavedPath = DocumentHelper.GetSavingFolderPath(defaultRequest);

                foreach (var item in documentRequest)
                {
                    DocumentHelper.DeleteFileWithPath($"{documentSavedPath}{item.FileName}");
                    Thread.Sleep(500);
                }
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteSingleDoc(DocumentRequest documentRequest)
        {
            try
            {
                var documentSavedPath = DocumentHelper.GetSavingFolderPath(documentRequest);
                DocumentHelper.DeleteFileWithPath($"{documentSavedPath}{documentRequest.FileName}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDocWithPath(string path)
        {
            try
            {
                DocumentHelper.DeleteFileWithPath(path);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void DeleteFolderWithPath(string path)
        {
            try
            {
                DocumentHelper.RemoveFolderWithPath(path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string DeleteAndAddImage(long organizationId
        , long outletId
        , string personId
        , EntityState recordState
        , PersonType personType
        , FileType fileType
        , FolderType folderType
        , string imagePath
        , bool isToDeleteImage)
        {
            var savedImagePath = string.Empty;
            try
            {
                if (DocumentHelper.IsBase64String(imagePath))
                {
                    DocumentRequest documentRequest = GetDocModel(organizationId
                                                                  , outletId
                                                                  , personId
                                                                  , personType.ToString()
                                                                  , fileType
                                                                  , folderType
                                                                  , imagePath);

                    if (EntityState.Updated == recordState)
                    {
                        var directoryToRemove = DocumentHelper.GetSavingFolderPath(documentRequest);
                        DeleteFolderWithPath(directoryToRemove);
                    }

                    savedImagePath = SaveDoc(documentRequest);
                }
                else if (EntityState.Updated == recordState)
                {
                    if (isToDeleteImage)
                    {
                        DeleteDocWithPath(imagePath);
                        return string.Empty;
                    }
                    else
                    {
                        return imagePath ?? string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return savedImagePath;
        }

        public DocumentRequest GetDocModel(long organizationId
        , long outletId
        , string personId
        , string personType
        , FileType fileType
        , FolderType folderType
        , string base64)
        {
            try
            {
                return new DocumentRequest()
                {
                    OrganizationId = organizationId,
                    OutletId = outletId,
                    PersonId = personId,
                    PersonType = personType,
                    FileType = fileType,
                    FolderType = folderType,
                    Base64 = base64,
                    FileName = $"{Guid.NewGuid()}.jpg"
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<Base64Response>> GetBase64StringWithPath(List<Base64Request> request)
        {
            try
            {
                List<Base64Response> responseList = new List<Base64Response>();
                if ((request?.Count ?? 0) > 0)
                {
                    request
                        .Where(req => !string.IsNullOrEmpty(req.FilePath))
                        .ToList()
                        .ForEach(req =>
                        {
                            Base64Response response;
                            var path = $"{DocumentHelper.GetDocumentDirectoryRootPath()}{req.FilePath.Replace("ERPDocuments", "")}";
                            var base64String = DocumentHelper.GetBase64StringFromImage(path);
                            if (!string.IsNullOrEmpty(base64String))
                            {
                                response = new Base64Response();
                                response.FilePath = req.FilePath;
                                response.Base64String = base64String;
                                responseList.Add(response);
                            }
                        });
                }
                return Task.FromResult(responseList);
            }
            catch (Exception)
            {
                throw;
            }
        }        
    }
}
