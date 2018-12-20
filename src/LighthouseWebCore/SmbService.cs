using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LighthouseUiCore.Models;
using Microsoft.AspNetCore.Http;

namespace LighthouseUiCore
{
    public class SmbService : ISmbService
    {
        private readonly ISmbClient _smbClient;

        public SmbService(ISmbClientFactory smbClientFactory)
        {
            _smbClient = smbClientFactory.GetInstance();
        }

        public IEnumerable<FileViewModel> GetFiles()
        {
            var files = _smbClient.GetAllFiles();

            return files.Select(fileInfo =>
                new FileViewModel
                {
                    Directory = fileInfo.DirectoryName,
                    Length = fileInfo.Length,
                    Name = fileInfo.Name
                }).ToList();
        }

        public async Task<FileDetailViewModel> GetFileAsync(string file)
        {
            var fileDetail = await _smbClient.ReadAsync(file);
            return new FileDetailViewModel
            {
                LineCount = fileDetail.LineCount,
                Directory = fileDetail.Directory,
                Length = fileDetail.Length,
                Name = fileDetail.Name,
                FileContent = fileDetail.FileContent,
                WordCount = fileDetail.WordCount
            };
        }

        public async Task CreateFileAsync(IFormFile file)
        {
            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                var content = await streamReader.ReadToEndAsync();
                await _smbClient.WriteAsync(file.FileName, content);
            }
        }

        public void DeleteFile(string path)
        {
            _smbClient.DeleteFile(path);
        }

        public async Task<DeleteFileViewModel> GetDeleteModel(string name)
        {
            var file = await GetFileAsync(name);
            return new DeleteFileViewModel {Name = file.Name};
        }

        public List<PermissionViewModel> GetFolderPermissions()
        {
            return _smbClient.DirectoryInformation();
        }
    }
}
