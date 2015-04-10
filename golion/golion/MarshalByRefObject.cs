using System;
using System.Collections.Generic;
using System.Text;

namespace golion
{
    public abstract class MarshalByRefObject : System.MarshalByRefObject
    {
        public override object InitializeLifetimeService()
        {
            //经测试，此处初始化默认的生命周期只有5分钟
            //返回null，则生命周期没有倒计时
            return null;
        }
    }
}
