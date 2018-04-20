using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsUserCenter.UI.Middleware
{
    public class ConfigMapMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _env;



        public ConfigMapMiddleware(RequestDelegate next, IApplicationBuilder app, IHostingEnvironment env)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            _next = next;
            _env = env;
        }   

        public async Task Invoke(HttpContext context)
        {
            var name = _env.EnvironmentName;
            await _next(context);
            return;
        }


      
    }
}
