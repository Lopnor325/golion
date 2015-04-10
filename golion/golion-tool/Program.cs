using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace golion_tool
{
    class Program
    {
        static void Main(string[] args)
        {
            String operate = args[0];
            if (operate == "v")
            {
                String fileName = args[1];

                Assembly assembly = Assembly.ReflectionOnlyLoadFrom(fileName);
                String assemblyVersion = assembly.GetName().Version.ToString();
                Console.WriteLine(assemblyVersion);
            }
            else
            {
                Console.WriteLine("无效参数");
            }
        }
    }
}
