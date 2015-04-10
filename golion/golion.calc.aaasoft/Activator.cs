using System;
using System.Collections.Generic;
using System.Text;
using org.osgi.framework;

namespace golion.calc.aaasoft
{
    public class Activator : golion.MarshalByRefObject, BundleActivator
    {

        private static BundleContext context;
        private ServiceRegistration calcServiceRegistration;

        public void start(BundleContext context)
        {
            Activator.context = context;
            CalcService calcService = new CalcServiceImpl();
            calcServiceRegistration = context.registerService(typeof(CalcService).FullName, calcService, null);
        }

        public void stop(BundleContext context)
        {
            calcServiceRegistration.unregister();
            Activator.context = null;
        }
    }
}
