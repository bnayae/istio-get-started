using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

namespace cs_aspcore_echo_v1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureKestrel(options =>
                {
                    options.Listen(IPAddress.Any, 80, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                        //listenOptions.UseHttps("testcert.pfx", "testPassword")
                    });
                })
                .UseStartup<Startup>();
    }
}
