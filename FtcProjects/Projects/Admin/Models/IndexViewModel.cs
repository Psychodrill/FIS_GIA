using System.Collections.Generic;
 
namespace Admin.Models
{
    public class IndexViewModel
    {
        public IEnumerable<EntrantViewModel> EntrantViewModel { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}