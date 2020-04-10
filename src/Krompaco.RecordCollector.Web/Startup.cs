﻿using System.Globalization;
using Krompaco.RecordCollector.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Krompaco.RecordCollector.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
#pragma warning disable CA1822 // Mark members as static
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<FileSystemWatcherService>();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

                // TODO: This is the place to set the default culture
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(new CustomRequestCultureProvider(this.Configuration));
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
#pragma warning restore CA1822 // Mark members as static
        {
            logger.LogInformation("Inside Configure method.");

            app.UseRequestLocalization();

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "content-table",
                    pattern: "content-table",
                    defaults: new { controller = "Content", action = "Table" });

                endpoints.MapControllerRoute(
                    name: "files",
                    pattern: "{**path}",
                    defaults: new { controller = "Content", action = "Files" });
            });

            if (env != null)
            {
                logger.LogInformation($"In {env.EnvironmentName} using {this.Configuration.GetAppSettingsContentRootPath()}");
            }
        }
    }
}
