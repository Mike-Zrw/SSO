using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Text;

namespace AbsUserCenter.Tool
{
    /// <summary>
    /// 配置文件获取类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 读取appsettings.json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetConfig<T>(string key)
        {
            IConfiguration Configuration = GetConfiguration();
            var sec = Configuration.GetSection(key);
            return Configuration.GetValue<T>(key);
        }

        public static IHostingEnvironment GetEnv()
        {
            IHostingEnvironment env = IoC.Resolve<IHostingEnvironment>();
            return env;
        }
        public static IConfiguration GetConfiguration()
        {
            IConfiguration config = IoC.Resolve<IConfiguration>(); 
            return config;
        }
    }
}
