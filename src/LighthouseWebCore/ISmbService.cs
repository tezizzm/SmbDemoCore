using LighthouseUiCore.Models;

using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LighthouseUiCore
{
    public interface ISmbService
    {
        IEnumerable<FileViewModel> GetFiles();
        Task<FileDetailViewModel> GetFileAsync(string file);
        Task CreateFileAsync(IFormFile file);
        void DeleteFile(string path);
        Task<DeleteFileViewModel> GetDeleteModel(string name);
        List<PermissionViewModel> GetFolderPermissions();
    }
}