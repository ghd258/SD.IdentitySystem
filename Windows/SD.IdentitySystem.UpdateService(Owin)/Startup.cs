﻿using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.StaticFiles.ContentTypes;
using Owin;
using SD.Toolkits.AspNet;
using System;
using System.IO;

namespace SD.IdentitySystem.UpdateService
{
    /// <summary>
    /// OWIN启动器
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 配置应用程序
        /// </summary>
        /// <param name="appBuilder">应用程序建造者</param>
        public void Configuration(IAppBuilder appBuilder)
        {
            //配置文件服务器
            string staticFilesPath = Path.IsPathRooted(AspNetSetting.StaticFilesPath)
                ? AspNetSetting.StaticFilesPath
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AspNetSetting.StaticFilesPath);
            string fileServerPath = Path.IsPathRooted(AspNetSetting.FileServerPath)
                ? AspNetSetting.FileServerPath
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AspNetSetting.FileServerPath);
            Directory.CreateDirectory(staticFilesPath);
            Directory.CreateDirectory(fileServerPath);
            StaticFileOptions staticFileOptions = new StaticFileOptions
            {
                FileSystem = new PhysicalFileSystem(staticFilesPath)
            };
            FileServerOptions fileServerOptions = new FileServerOptions
            {
                FileSystem = new PhysicalFileSystem(fileServerPath),
                EnableDirectoryBrowsing = true
            };
            FileExtensionContentTypeProvider contentTypeProvider = (FileExtensionContentTypeProvider)fileServerOptions.StaticFileOptions.ContentTypeProvider;
            contentTypeProvider.Mappings.Add(".apk", "application/vnd.android.package-archive");

            appBuilder.UseStaticFiles(staticFileOptions);
            appBuilder.UseFileServer(fileServerOptions);
        }
    }
}
