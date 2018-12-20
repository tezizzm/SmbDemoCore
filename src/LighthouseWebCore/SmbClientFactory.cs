using Microsoft.Extensions.Configuration;

namespace LighthouseUiCore
{
    public class SmbClientFactory : ISmbClientFactory
    {
        private readonly IConfiguration _configuration;

        public SmbClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ISmbClient GetInstance()
        {
            var containerDirectory = _configuration["vcap:services:smbvolume:0:volume_mounts:0:container_dir"];
            return new SmbClient(containerDirectory);
        }
    }
}