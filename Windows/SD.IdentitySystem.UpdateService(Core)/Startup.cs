using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using SD.Toolkits.AspNet;
using System;
using System.IO;

namespace SD.IdentitySystem.UpdateService
{
    /// <summary>
    /// Ӧ�ó���������
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ����Ӧ�ó���
        /// </summary>
        public void Configure(IApplicationBuilder appBuilder)
        {
            //���÷�����
            string staticFilesRoot = Path.Combine(AppContext.BaseDirectory, AspNetSetting.StaticFilesPath);
            string fileServerRoot = Path.Combine(AppContext.BaseDirectory, AspNetSetting.FileServerPath);
            Directory.CreateDirectory(staticFilesRoot);
            Directory.CreateDirectory(fileServerRoot);
            StaticFileOptions staticFileOptions = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilesRoot)
            };
            FileServerOptions fileServerOptions = new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(fileServerRoot),
                EnableDirectoryBrowsing = true
            };
            appBuilder.UseStaticFiles(staticFileOptions);
            appBuilder.UseFileServer(fileServerOptions);
        }
    }
}
