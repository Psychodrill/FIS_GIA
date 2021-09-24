using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EgePlatform.PackageManager.Core;

namespace Fbs.Updater.Tasks
{
    public class DeployWebappTask //: Task
    {
        public string Title { get { return "Обновление веб-приложения"; } }

        public bool FailOnError { get { return true; } }

        public bool Execute(PackageConfig config)
        {
            throw new NotImplementedException();
        }
    }
}
