using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVUZ.CompositionExportModel
{
    public class PagesCountFile
    {
        public PagesCountFile(string[] content)
        {
            Content = content;
        }
        public string[] Content { get; private set; }
    }
}
