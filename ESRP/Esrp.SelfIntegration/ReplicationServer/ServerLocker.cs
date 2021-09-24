using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esrp.SelfIntegration.ReplicationServer
{
    internal static class ServerLocker
    {
        public static readonly object Locker = new object();
    }
}
