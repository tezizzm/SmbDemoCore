using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LighthouseUiCore.Models;

namespace LighthouseUiCore
{
    public class SmbClient : ISmbClient
    {
        public string LocalPath { get; set; }

        private static readonly Regex Regex = new Regex(@"[\S]+", RegexOptions.Compiled & RegexOptions.Multiline);

        public SmbClient(string localPath)
        {
            LocalPath = localPath ?? throw new ArgumentNullException(nameof(localPath));

            Console.WriteLine($"Local path set to {LocalPath}");
        }

        public async Task WriteAsync(string file, string data)
        {
            Console.WriteLine($"Writing data to {file}");
            using (var streamWriter = new StreamWriter($"{LocalPath}\\{file}"))
            {
                await streamWriter.WriteLineAsync(data);
            }
        }

        public async Task<FileDetails> ReadAsync(string file)
        {
            var fileAndPath = $"{LocalPath}\\{file}";
            var fileInfo = new FileInfo(fileAndPath);
            using (var stream = fileInfo.OpenText())
            {
                var task = stream.ReadToEndAsync();
                Console.WriteLine($"Reading data from {fileAndPath}");
                var fileDetail = new FileDetails
                {
                    Directory = fileInfo.DirectoryName,
                    Length = fileInfo.Length,
                    Name = fileInfo.Name,
                    LineCount = File.ReadLines(fileAndPath).Count()
                };

                var content = await task;
                fileDetail.FileContent = content;

                fileDetail.WordCount = Regex.Matches(content).Count;
                return fileDetail;
            }
        }

        public IEnumerable<FileInfo> GetAllFiles()
        {
            Console.WriteLine("Reading all the files in the shared directory");
            return new DirectoryInfo(LocalPath).GetFiles();
        }

        public void DeleteFile(string file)
        {
            File.Delete($"{LocalPath}\\{file}");
        }

        public List<PermissionViewModel> DirectoryInformation()
        {
            var di = new DirectoryInfo(LocalPath);
            
            var acl = di.GetAccessControl();
            var rules = acl.GetAccessRules(true, true, typeof(NTAccount));

            var result = new List<PermissionViewModel>();
            foreach (FileSystemAccessRule authorizationRule in rules)
            {
                result.Add(new PermissionViewModel
                {
                    Access = authorizationRule.AccessControlType == AccessControlType.Deny
                        ? Access.Denied
                        : Access.Allowed,
                    Directory = LocalPath,
                    User = authorizationRule.IdentityReference.Value,
                    Rights = authorizationRule.FileSystemRights.ToString(),
                });
            }

            return result;
        }
    }
}