using Core.CL.Enums;
using System.Reflection;
using AMNSystemsERP.CL.Models.Commons.DocumentHelper;
using System.Drawing;

namespace AMNSystemsERP.CL.Helper
{
    public static class DocumentHelper
    {
        private static string DocumentsFolderName { get; } = "ERPDocuments";
        public static string DocumentsFolderPath { get; } = $"C:/inetpub/wwwroot/{DocumentsFolderName}";
        public static string CreateFolder(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return string.Empty;
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    return path;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return path;
        }

        public static string GetDocumentDirectoryRootPath()
        {
            try
            {
                var contentRootPath = $"{DocumentsFolderPath}/{EnvironmentHelper.EnvironmentName}";
                return CreateFolder(contentRootPath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool DeleteFileWithPath(string filePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        System.Threading.Thread.Sleep(100);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool RemoveFolderWithPath(string directoryPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(directoryPath))
                {
                    if (Directory.Exists(directoryPath))
                    {
                        Directory.Delete(directoryPath, true);
                        System.Threading.Thread.Sleep(100);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsBase64String(string base64)
        {
            try
            {
                if (!string.IsNullOrEmpty(base64))
                {
                    Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
                    return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string SaveBase64Image(string base64
        , string path
        , string fileName)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);

                MemoryStream ms = new MemoryStream(bytes);
                //Image streamImage = Image.FromStream(ms);
                //streamImage.Save($"{path}{fileName}");

                using (var imageFile = new FileStream($"{path}{fileName}", FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
                return GetSavedFileRelativePath($"{path}{fileName}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetSavedFileRelativePath(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    //var pathToReturn = $"{path.Split(DocumentsFolderName)?.LastOrDefault()}";
                    var pathToReturn = $"{path.Split("C:/inetpub/wwwroot/")?.LastOrDefault()}";
                    pathToReturn = pathToReturn
                                        ?.Replace(@"\", "/")
                                        ?.TrimStart('/', '/') ?? "";

                    return pathToReturn;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return string.Empty;
        }

        public static string GetSavingFolderPath(DocumentRequest documentRequest)
        {
            try
            {
                if (documentRequest != null)
                {
                    var basePath = GetDocumentDirectoryRootPath();
                    var directoryPath = string.Empty;

                    if (documentRequest.PersonType == PersonType.Organization.ToString())
                    {
                        directoryPath = @$"{basePath}\{documentRequest.FileType}\{documentRequest.FolderType}\";
                    }
                    else if (documentRequest.PersonType == PersonType.Outlet.ToString())
                    {
                        directoryPath = @$"{basePath}\{documentRequest.FileType}\{documentRequest.OutletId}\{documentRequest.FolderType}\";
                    }
                    else if (documentRequest.PersonType == PersonType.Voucher.ToString())
                    {
                        directoryPath = @$"{basePath}\{documentRequest.FileType}\{documentRequest.OutletId}\{documentRequest.FolderType}\";
                    }
                    else
                    {
                        directoryPath = @$"{basePath}\{documentRequest.FileType}\{documentRequest.OutletId}\{documentRequest.PersonType}\{documentRequest.PersonId}\{documentRequest.FolderType}\";
                    }
                    return CreateFolder(directoryPath);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return string.Empty;
        }

        public static string GetBase64StringFromImage(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    using Image image = Image.FromFile(path);

                    using MemoryStream m = new MemoryStream();

                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Converting byte[] to Base64 String
                    return Convert.ToBase64String(imageBytes);
                    //
                }
            }
            catch (Exception)
            {
                throw;
            }
            return string.Empty;
        }

        public static string GetReportPath(string personType)
        {
            try
            {
                if (!string.IsNullOrEmpty(personType))
                {
                    var assemblyPath = Assembly.GetExecutingAssembly().Location;
                    var index = assemblyPath.LastIndexOf("\\");
                    var reportsBasePath = @$"{assemblyPath.Substring(0, index)}\{GetReportsBasePath()}";
                    return $"{reportsBasePath}{personType}";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return string.Empty;
        }

        public static string GetReportsBasePath()
        {
            try
            {
                return @"Repositories\Reports\";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetFileNameFromPath(string path)
        {
            try
            {
                return path.Split('/').LastOrDefault() ?? "";
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}