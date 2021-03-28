using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SD.IdentitySystem.WebApiCore.Authentication.Filters;
using SD.Infrastructure.AspNetCore.Server.Middlewares;
using SD.Toolkits.OwinCore.Middlewares;
using System;
using System.IO;
using System.Reflection;

namespace SD.IdentitySystem.AppService.Host
{
    public class Startup
    {
        /// <summary>
        /// ���÷���
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            //���ÿ���
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(_ => true)
                        .AllowCredentials();
                }));

            //���Swagger
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "�����֤ϵͳ WebApi �ӿ��ĵ�"
                });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });

            //��������֤������
            services.AddControllers(options =>
            {
                options.Filters.Add<WebApiAuthenticationFilter>();
            });
        }

        /// <summary>
        /// ����Ӧ�ó���
        /// </summary>
        public void Configure(IApplicationBuilder appBuilder)
        {
            //����ȫ���м��
            appBuilder.UseMiddleware<GlobalMiddleware>();

            //���û���OwinContext�м��
            appBuilder.UseMiddleware<CacheOwinContextMiddleware>();

            //���ÿ���
            appBuilder.UseCors("CorsPolicy");

            //����Swagger�м��
            appBuilder.UseSwagger();
            appBuilder.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1.0/swagger.json", "�����֤ϵͳ WebApi �ӿ��ĵ� v1.0");
            });

            appBuilder.UseRouting();
            appBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
