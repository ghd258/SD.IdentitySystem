using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SD.IdentitySystem.AspNetCore.Authentication.Filters;
using SD.Infrastructure.Constants;
using SD.Toolkits.AspNet;
using SD.Toolkits.AspNetCore.Filters;
using SD.Toolkits.OwinCore.Middlewares;
using SD.Toolkits.Redis;
using System;
using System.IO;

namespace SD.IdentitySystem.Client
{
    /// <summary>
    /// Ӧ�ó���������
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ���÷���
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                //��ӹ�����
                options.Filters.Add(new MvcExceptionFilter());
                options.Filters.Add(new MvcAuthenticationFilter());
            }).AddNewtonsoftJson(options =>
            {
                //��������
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                //����ʱ���ʽ����
                IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter()
                {
                    DateTimeFormat = CommonConstants.DateTimeFormat
                };
                options.SerializerSettings.Converters.Add(dateTimeConverter);
            });

            //���Session
            services.AddSession();
            services.AddStackExchangeRedisCache(options => options.ConfigurationOptions = RedisManager.RedisConfigurationOptions);

            //���Session����
            services.AddDataProtection(options =>
            {
                options.ApplicationDiscriminator = AspNetSection.Setting.ApplicationName.Value;
            });
        }

        /// <summary>
        /// ����Ӧ�ó���
        /// </summary>
        public void Configure(IApplicationBuilder appBuilder)
        {
            appBuilder.UseMiddleware<CacheOwinContextMiddleware>();
            appBuilder.UseStaticFiles();
            appBuilder.UseRouting();
            appBuilder.UseSession();
            appBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //���þ�̬�ļ�
            string staticFilesRoot = Path.Combine(AppContext.BaseDirectory, AspNetSetting.StaticFilesPath);
            Directory.CreateDirectory(staticFilesRoot);
            StaticFileOptions staticFileOptions = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilesRoot)
            };
            appBuilder.UseStaticFiles(staticFileOptions);
        }
    }
}
