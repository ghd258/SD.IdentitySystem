using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SD.Common;
using SD.IdentitySystem.AppService.Implements;
using SD.IdentitySystem.WCF.Authentication;
using SD.Infrastructure.WCF.Server;
using SD.IOC.Integration.WCF.Behaviors;
using System.Configuration;

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
            //���WCF����
            services.AddServiceModelServices();

            //���WCF����
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            services.AddServiceModelConfigurationManagerFile(configuration.FilePath);
        }

        /// <summary>
        /// ����Ӧ�ó���
        /// </summary>
        public void Configure(IApplicationBuilder appBuilder)
        {
            //����WCF����
            DependencyInjectionBehavior dependencyInjectionBehavior = new DependencyInjectionBehavior();
            InitializationBehavior initializationBehavior = new InitializationBehavior();
            AuthenticationBehavior authenticationBehavior = new AuthenticationBehavior();
            IServiceBehavior[] serviceBehaviors =
            {
                dependencyInjectionBehavior, initializationBehavior, authenticationBehavior
            };
            appBuilder.UseServiceModel(builder =>
            {
                builder.ConfigureServiceHostBase<AuthenticationContract>(host => host.Description.Behaviors.AddRange(serviceBehaviors));
                builder.ConfigureServiceHostBase<AuthorizationContract>(host => host.Description.Behaviors.AddRange(serviceBehaviors));
                builder.ConfigureServiceHostBase<UserContract>(host => host.Description.Behaviors.AddRange(serviceBehaviors));
            });
        }
    }
}
