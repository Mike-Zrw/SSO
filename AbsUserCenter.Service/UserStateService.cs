using AbsUserCenter.Core.Config;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;

namespace AbsUserCenter.Service
{
    public class UserStateService
    {
        public void GetConfig()
        {
            var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            ApplicationEnvironment env = PlatformServices.Default.Application;
            IConfiguration Configuration = builder.Build();
            var a = ConfigOptions.AppSettings.SqlHelperNonQueryCommandTimeout;

        }
    }
}
