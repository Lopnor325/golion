using System;
using System.Collections.Generic;
using System.Text;

namespace golion.calc.aaasoft
{
    class CalcServiceImpl : golion.MarshalByRefObject, CalcService
    {
        public int Add(int x, int y)
        {
            return x + y;
        }

        public long Mul(int x, int y)
        {
            return x * y;
        }
    }
}
