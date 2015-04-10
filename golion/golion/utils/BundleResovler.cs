using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using org.osgi.framework;

namespace golion.utils
{
    public class BundleResovler : golion.MarshalByRefObject
    {
        public static BundleResovler instance;

        private BundleImpl bundle;
        private Assembly bundleAssembly;

        public BundleImpl GetBundle()
        {
            return bundle;
        }

        public BundleResovler()
        {
            instance = this;
        }

        public void Init(BundleImpl bundle)
        {
            this.bundle = bundle;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            bundleAssembly = Assembly.Load(File.ReadAllBytes(bundle.GetBundleAssemblyFileName()));
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            String assemblyFullName = args.Name;

            //Console.WriteLine(String.Format("插件[{0}]正试图加载程序集[{1}].", bundle.getSymbolicName(), assemblyFullName));
            //如果是加载Bundle的主程序集
            if (bundleAssembly.FullName.Equals(assemblyFullName)) return bundleAssembly;
            //如果是依赖的Bundle的主程序集
            foreach (BundleImpl requredBundle in bundle.GetRequiredBundles())
            {
                String requredBundleAssemblyFullName = requredBundle.GetBundleAssemblyFullName();
                if (assemblyFullName.Equals(requredBundleAssemblyFullName))
                {
                    return Assembly.Load(File.ReadAllBytes(requredBundle.GetBundleAssemblyFileName()));
                }
            }

            //尝试从插件引用程序集目录下加载
            String[] assemblyNameParts = assemblyFullName.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            String assemblyName = assemblyNameParts[0];
            String assemblyFileName = assemblyNameParts[0] + ".dll";

            String bundleLibsDirectoryName = bundle.getBundleLibsDirectoryName();
            String assemblyFullFileName = Path.Combine(bundleLibsDirectoryName, assemblyFileName);
            if (File.Exists(assemblyFullFileName))
            {
                Assembly assembly = Assembly.Load(File.ReadAllBytes(assemblyFullFileName));
                return assembly;
            }
            //Console.WriteLine(String.Format("解析插件[{0}]时未能解析程序集[{1}]", bundle.getSymbolicName(), assemblyFullName));
            return null;
        }

        /// <summary>
        /// 搜索Activator类名
        /// </summary>
        /// <returns></returns>
        public String FindBundleActivatorClassName()
        {
            Type[] bundleAssemblyTypes = bundleAssembly.GetTypes();

            //搜索BundleActivator
            foreach (Type type in bundleAssemblyTypes)
            {
                if (type.GetInterface(typeof(BundleActivator).FullName) != null)
                {
                    return type.FullName;
                }
            }
            return null;
        }

        /// <summary>
        /// 得到清单中的资源名称
        /// </summary>
        /// <returns></returns>
        public String[] GetManifestResourceNames()
        {
            return bundleAssembly.GetManifestResourceNames();
        }

        public Stream GetManifestResourceStream(String name)
        {
            return bundleAssembly.GetManifestResourceStream(name);
        }

        /// <summary>
        /// 获取当前应用程序域已加载的程序集信息
        /// </summary>
        /// <returns></returns>
        public String GetAssemblies()
        {
            StringBuilder sb = new StringBuilder();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                AssemblyName assemblyName = assembly.GetName();
                sb.Append(String.Format("[{0}]: {1}, Version:{2}", i, assemblyName.Name, assemblyName.Version));
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
