using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace cs_aspcore_echo_v1
{
    public class Startup : IDisposable
    {
        private readonly HttpClient _http;
        private string TARGET_URL = Environment.GetEnvironmentVariable("TARGET_URL") ?? "http://bnaya-pong-service";

        public Startup()
        {

            _http = new HttpClient
            {
                Timeout = new TimeSpan(0, 10, 0)
            };
            _http.DefaultRequestHeaders.Accept.Clear();
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });

            services.AddSingleton(_http);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseCors("SiteCorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                context.Response.ContentType = "application/json";
                string countStr = context.Request.Query["count"].FirstOrDefault();
                if (!int.TryParse(countStr, out int count))
                    count = 0;

                string data = $@"""CS|{new string('-', count)}""";
                if (count <= 1)
                {
                    await context.Response.WriteAsync(data).ConfigureAwait(false);
                    return;
                }

                count--;
                try
                {
                    var res = await _http.GetAsync($"{TARGET_URL}?count={count}").ConfigureAwait(false);
                    if (!res.IsSuccessStatusCode)
                    {
                        await context.Response.WriteAsync(@"[""X""]").ConfigureAwait(false);
                        return;
                    }

                    var arr = await res.Content.ReadAsAsync<JArray>();
                    arr.Add(data);
                    await context.Response.WriteAsync(data).ConfigureAwait(false);
                }
                catch (Exception)
                {

                    await context.Response.WriteAsync($"X:{data}").ConfigureAwait(false);
                }
            });
        }

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}
