using org.osgi.framework;
using System;
using System.Collections.Generic;
using System.Text;
using golion.calc;
using log4net;

namespace golion.console
{
    public class Class1 : golion.MarshalByRefObject, BundleActivator
    {
        private static ILog log = LogManager.GetLogger(typeof(Class1));
        private static BundleContext bundleContext;
        public static BundleContext getBundleContext()
        {
            return bundleContext;
        }

        Ionic.Zip.ZipFile a = null;
        public void start(BundleContext context)
        {
            bundleContext = context;
            a = new Ionic.Zip.ZipFile();

            ServiceReference reference = bundleContext.getServiceReference(typeof(CalcService).FullName);
            if (reference == null)
            {
                log.Warn("未找到服务引用！！！");
                return;
            }
            int x = 4324;
            int y = 7833;
            CalcService calcService = bundleContext.getService(reference) as CalcService;
            log.Info(String.Format("调用服务测试：{0} + {1} = {2}", x, y, calcService.Add(x, y)));
        }

        public void stop(BundleContext context)
        {

        }
    }
}
