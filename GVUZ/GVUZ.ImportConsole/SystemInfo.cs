using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportConsole
{
    public class SystemInfo
    {
        private uint processors;
        private uint cores;

        public uint Processors
        {
            get { return this.processors; }
        }

        public uint Cores
        {
            get { return this.cores; }
        }
        public SystemInfo()
        {
            this.processors = 0;
            this.cores = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                this.processors = (uint)item.GetPropertyValue("NumberOfProcessors");
            }

             foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                this.cores += (uint)item.GetPropertyValue("NumberOfCores");
                //this.cores += uint.Parse(item["NumberOfCores"].ToString());
            }
        }
    }
}
