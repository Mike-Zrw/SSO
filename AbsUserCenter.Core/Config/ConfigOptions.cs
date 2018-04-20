using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbsUserCenter.Core.Config
{
    public class ConfigOptions
    {
        public static ConnectionOptions ConnectionStrings { get; set; }
        public static AppSettionsOptions AppSettings { get; set; }
        public static IHostingEnvironment HostingEnvironment { get; set; }
    }
    public class ConnectionOptions
    {
        public string RwViewSQLConnString { get; set; }
        public string RoViewSQLConnString { get; set; }
    }

    public class AppSettionsOptions
    {
        public int SqlHelperNonQueryCommandTimeout { get; set; }
        public int SqlHelperQueryCommandTimeout { get; set; }
    }
}
