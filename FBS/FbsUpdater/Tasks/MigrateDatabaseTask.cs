using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EgePlatform.PackageManager.Core;

namespace Fbs.Updater.Tasks
{
    public class MigrateDatabaseTask //: Task
    {
        public string Title { get { return "Обновление базы данных"; } }

        public bool FailOnError { get { return true; } }

        public bool Execute(PackageConfig config)
        {
            throw new NotImplementedException();
        }
    }
}
