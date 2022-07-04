using Microsoft.AspNetCore.Http;

namespace _0_Framework.Application

{
    public interface IFileUploader
    {
        string Uploader(IFormFile file, string folder, string fileName);

        OperationResult DeleteFile(string folder, string fileName);
    }
}