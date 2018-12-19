﻿using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace LighthouseUiCore
{
    public class SmbClientFactory : ISmbClientFactory
    {
        private readonly CloudFoundryServicesOptions _services;

        public SmbClientFactory(IOptions<CloudFoundryServicesOptions> services)
        {
            _services = services.Value;
        }

        public ISmbClient GetInstance()
        {
            foreach (var service in _services.ServicesList)
            {
                if (service.Label == "user-provided" && service.Name == "win-fs-ups")
                {
                    var url = service.Credentials["winfs-share"].Value;
                    //var domain = service.Credentials["domain"]?.Value;
                    return new SmbClient(url);
                }
            }

            return null;
        }
    }
}