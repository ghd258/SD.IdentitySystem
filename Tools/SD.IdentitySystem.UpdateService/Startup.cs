using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
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
            string staticFilesPath = Path.IsPathRooted(AspNetSetting.StaticFilesPath)
                ? AspNetSetting.StaticFilesPath
                : Path.Combine(AppContext.BaseDirectory, AspNetSetting.StaticFilesPath);
            string fileServerPath = Path.IsPathRooted(AspNetSetting.FileServerPath)
                ? AspNetSetting.FileServerPath
                : Path.Combine(AppContext.BaseDirectory, AspNetSetting.FileServerPath);
            Directory.CreateDirectory(staticFilesPath);
            Directory.CreateDirectory(fileServerPath);
            StaticFileOptions staticFileOptions = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilesPath)
            };
            FileServerOptions fileServerOptions = new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(fileServerPath),
                EnableDirectoryBrowsing = true
            };

            FileExtensionContentTypeProvider contentTypeProvider = new FileExtensionContentTypeProvider();
            contentTypeProvider.Mappings.Add(".apk", "application/vnd.android.package-archive");
            fileServerOptions.StaticFileOptions.ContentTypeProvider = contentTypeProvider;

            appBuilder.UseStaticFiles(staticFileOptions);
            appBuilder.UseFileServer(fileServerOptions);
        }
    }
}
