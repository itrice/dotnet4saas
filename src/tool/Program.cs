using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace tool
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] rawAssembly = loadFile("iTrice.SAAS.WinHost.dll");            
            var h = AppDomain.CurrentDomain.Load(rawAssembly);
            var inst = h.CreateInstance("iTrice.SAAS.WinHost.Proxy");

        }
        static byte[] loadFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            byte[] buffer = new byte[(int)fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            return buffer;
        }
    }
}
