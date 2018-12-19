using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LighthouseUiCore.Models;
using Microsoft.AspNetCore.Http;

namespace LighthouseUiCore
{
    public interface ISmbService
    {
        IEnumerable<FileViewModel> GetFiles();
        Task<FileDetailViewModel> GetFile(string file);
        Task CreateFile(IFormFile file);
        void DeleteFile(string path);
        Task<DeleteFileViewModel> GetDeleteModel(string name);
        List<PermissionViewModel> GetFolderPermissions();
    }
}