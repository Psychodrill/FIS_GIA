using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class CompositionFileViewModel
    {
        public string Name { set; get; }
        public string Year { set; get; }
        public string Code { set; get; }
    }
}
