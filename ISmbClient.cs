using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LighthouseUiCore.Models;

namespace LighthouseUiCore
{
    public interface ISmbClient
    {
        Task<FileDetails> ReadAsync(string file);
        Task WriteAsync(string file, string data);
        IEnumerable<FileInfo> GetAllFiles();
        void DeleteFile(string file);
        List<PermissionViewModel> DirectoryInformation();
    }
}