using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SD.Infrastructure.Constants;
using SD.Toolkits.OwinCore.Middlewares;

namespace SD.IdentitySystem.AspNetCore.Tests
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
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                //Camel��������
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                //����ʱ���ʽ����
                IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter()
                {
                    DateTimeFormat = CommonConstants.TimeFormat
                };
                options.SerializerSettings.Converters.Add(dateTimeConverter);
            });
        }

        /// <summary>
        /// ����Ӧ�ó���
        /// </summary>
        public void Configure(IApplicationBuilder appBuilder)
        {
            //�����м��
            appBuilder.UseMiddleware<CacheOwinContextMiddleware>();

            //����·��
            appBuilder.UseRouting();
            appBuilder.UseEndpoints(routeBuilder => routeBuilder.MapControllers());
        }
    }
}
