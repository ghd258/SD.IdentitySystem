using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SD.IdentitySystem.WebApiCore.Authentication.Filters;
using SD.Infrastructure.AspNetCore.Server.Middlewares;
using SD.Toolkits.OwinCore.Middlewares;
using SD.Toolkits.WebApiCore.Filters;
using System;
using System.IO;
using System.Reflection;

namespace SD.IdentitySystem.AppService.Host
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
            //��ӿ������
            services.AddCors(options => options.AddPolicy(typeof(Startup).FullName,
                policyBuilder =>
                {
                    policyBuilder.AllowAnyMethod();
                    policyBuilder.AllowAnyHeader();
                    policyBuilder.AllowCredentials();
                    policyBuilder.SetIsOriginAllowed(_ => true);
                }));

            //���Swagger
            services.AddSwaggerGen(options =>
            {
                OpenApiInfo apiInfo = new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "�����֤ϵͳ WebApi �ӿ��ĵ�"
                };

                string xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);

                options.SwaggerDoc("v1.0", apiInfo);
                options.IncludeXmlComments(xmlFilePath);
            });

            //��ӹ�������Camel��������
            services.AddControllers(options =>
            {
                options.Filters.Add<WebApiAuthenticationFilter>();
                options.Filters.Add<WebApiExceptionFilter>();
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });
        }

        /// <summary>
        /// ����Ӧ�ó���
        /// </summary>
        public void Configure(IApplicationBuilder appBuilder)
        {
            //�����м��
            appBuilder.UseMiddleware<GlobalMiddleware>();
            appBuilder.UseMiddleware<CacheOwinContextMiddleware>();

            //���ÿ���
            appBuilder.UseCors(typeof(Startup).FullName);

            //����Swagger
            appBuilder.UseSwagger();
            appBuilder.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1.0/swagger.json", "�����֤ϵͳ WebApi �ӿ��ĵ� v1.0"));

            //����·��
            appBuilder.UseRouting();
            appBuilder.UseEndpoints(routeBuilder => routeBuilder.MapControllers());
        }
    }
}
