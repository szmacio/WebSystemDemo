using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Optimization;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Infrastructure.Utilities;
using JuCheap.Web.Models;

namespace JuCheap.Web
{
    /// <summary>
    /// BundleConfig
    /// </summary>
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //启用压缩配置
            BundleTable.EnableOptimizations = ConfigurationManager.AppSettings["EnableOptimizations"] == "true";
            //重新处理bundle忽略资源的规则
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            //配置文件列表
            var fileList = new List<StaticFileModel>();
            //获取js配置的文件
            var jsFilePath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "/Configs/Scripts.config");
            var cssFilePath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "/Configs/Styles.config");
            var jsFileList = SerializeUtil.DeserializeFromXml<List<StaticFileModel>>(jsFilePath);
            var cssFileList = SerializeUtil.DeserializeFromXml<List<StaticFileModel>>(cssFilePath);

            if (jsFileList.AnyOne())
                fileList.AddRange(jsFileList);
            if (cssFileList.AnyOne())
                fileList.AddRange(cssFileList);

            fileList.ForEach(f =>
            {
                string type = f.Type.Trim().ToLower(), virtualUrl = f.VirtualUrl.Trim().ToLower();
                if (type.Equals("js"))
                {
                    bundles.Add(new ScriptBundle(virtualUrl).Include(f.Srcs.ToArray()));
                }
                else if (type.Equals("css"))
                {
                    bundles.Add(new StyleBundle(virtualUrl).Include(f.Srcs.ToArray()));
                }
            });
        }

        /// <summary>
        /// 压缩静态文件，需要忽略的文件命名规则
        /// </summary>
        /// <param name="ignoreList"></param>
        private static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");

            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }
    }
}