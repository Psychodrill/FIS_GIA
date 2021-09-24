using System;
using System.Linq;
using FogSoft.Helpers;

namespace FogSoft.Patcher
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length == 0 || args[0].In("-?", "/?") || args.Length < 2)
            {
                Console.Write(@"Usage:
FogSoft.Patcher.exe <patch file> <xml file1>...<xml fileN>
Patch file format:
<patches>
To delete element: <delete xpath=""...""/>
To remove attribute: <remove xpath=""..."" name=""...""/>
To append attribute: <append xpath=""..."" name=""..."" value=""...""/>
</patches>");
                return 1;
            }
            try
            {
                string[] targets = args.Skip(1).ToArray();
                XmlPatcher.Patch(args[0], targets);
                return 0;
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (StackOverflowException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return 3;
            }
        }
    }
}