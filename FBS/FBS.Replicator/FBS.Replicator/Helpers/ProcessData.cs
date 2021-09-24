using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FBS.Replicator.Helpers
{
    public static class ProcessData
    {

        public static void LogMemoryStatus(string message = "")
        {
            if(message.Length > 0)
                Common.Logger.WriteLine(message);
            Process proc = Process.GetCurrentProcess();
            if (!proc.HasExited && proc.Responding)
            {
                // Refresh the current process property values.
                proc.Refresh();
                Common.Logger.WriteLine("+---------------------------------------------------------------------------------------+");
                Common.Logger.WriteLine(string.Format("| Процессорное время | U: {0,12:g}  | P: {1,12:g} | T: {2,12:g} |", proc.UserProcessorTime, proc.PrivilegedProcessorTime, proc.TotalProcessorTime));
                Common.Logger.WriteLine(string.Format("| Память страничная  | PS: {0} | P: {1} | PPM: {2} | PVM: {3} |", proc.PagedSystemMemorySize64, proc.PagedMemorySize64, proc.PeakPagedMemorySize64, proc.PeakVirtualMemorySize64));
                Common.Logger.WriteLine(string.Format("| Память             | PM: {0,12:N3} Mb | VM: {1,12:N3} Mb |", proc.PrivateMemorySize64%1000000, proc.VirtualMemorySize64%1000000));
                Common.Logger.WriteLine("+---------------------------------------------------------------------------------------+");

                //Console.WriteLine($"{myProcess} -");
                //Console.WriteLine("-------------------------------------");

                //Console.WriteLine($"  Physical memory usage     : {myProcess.WorkingSet64}");
                //Console.WriteLine($"  Base priority             : {myProcess.BasePriority}");
                //Console.WriteLine($"  Priority class            : {myProcess.PriorityClass}");
                //Console.WriteLine($"  User processor time       : {myProcess.UserProcessorTime}");
                //Console.WriteLine($"  Privileged processor time : {myProcess.PrivilegedProcessorTime}");
                //Console.WriteLine($"  Total processor time      : {myProcess.TotalProcessorTime}");
                //Console.WriteLine($"  Paged system memory size  : {myProcess.PagedSystemMemorySize64}");
                //Console.WriteLine($"  Paged memory size         : {myProcess.PagedMemorySize64}");

                //// Update the values for the overall peak memory statistics.
                //peakPagedMem = myProcess.PeakPagedMemorySize64;
                //peakVirtualMem = myProcess.PeakVirtualMemorySize64;
                //peakWorkingSet = myProcess.PeakWorkingSet64;

                //if (myProcess.Responding)
                //{
                //    Console.WriteLine("Status = Running");
                //}
                //else
                //{
                //    Console.WriteLine("Status = Not Responding");
                //}
            }
            else
            {
                if(proc.HasExited)
                    Common.Logger.WriteLine("Процесс более не запущен...");
                if (!proc.Responding)
                    Common.Logger.WriteLine("Процесс не отвечает...");

            }
            return;
        }
    }
}
