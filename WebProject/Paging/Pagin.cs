using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProject.Paging
{
    public class Pagin
    {

        public string UrlArea { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string UrlAction { get; set; }
        public bool IsPage { get; set; } = false;
    }
}
