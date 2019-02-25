using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Bnaya.Samples
{
    public class Startup : IDisposable
    {
        private readonly HttpClient _http;
        private string TARGET_URL = Environment.GetEnvironmentVariable("TARGET_URL") ?? "http://localhost/api/v1/pong";

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

                string newElement = $@"CS|{new string('-', count)}";
                var data = new JArray(newElement);
                if (count <= 1)
                {
                    string payload = JsonConvert.SerializeObject(data);
                    await context.Response.WriteAsync(payload).ConfigureAwait(false);
                    return;
                }

                count--;
                try
                {
                    var res = await _http.GetAsync($"{TARGET_URL}?count={count}").ConfigureAwait(false);
                    if (!res.IsSuccessStatusCode)
                    {
                        data.Add("X");
                        string payload = JsonConvert.SerializeObject(data);
                        await context.Response.WriteAsync(payload).ConfigureAwait(false);
                    }
                    else
                    {
                        var result = await res.Content.ReadAsAsync<JArray>();
                        result.Merge(data);
                        string payload = JsonConvert.SerializeObject(result);
                        await context.Response.WriteAsync(payload).ConfigureAwait(false);
                    }
                }
                catch (Exception)
                {

                    data.Add("X!");
                    string payload = JsonConvert.SerializeObject(data);
                    await context.Response.WriteAsync(payload).ConfigureAwait(false);
                }
            });
        }

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}
