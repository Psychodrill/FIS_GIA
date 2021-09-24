using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace FBSLoaderConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessManager manager = new ProcessManager(DateTime.Now.Year);
            manager.Do();
        }
    }
}
