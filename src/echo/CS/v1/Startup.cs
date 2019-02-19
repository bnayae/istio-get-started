using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace cs_aspcore_echo_v1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync($@"
  <dir>
  <h1>Echo</h1>
  <p>Params: {string.Join(',', context.Request.Query)}</p>
  <p>Query:  {context.Request.Path}</p>
  <h3>C# ASP.NET Core</h3>
  <p>Pod Language: C# (ASP.NET)</p>
  <p>Pod Version: 1</p>
  <div>");
            });
        }
    }
}
