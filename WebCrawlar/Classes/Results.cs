using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Classes
{
    public class Results
    {
        public List<string> results { get; set; } = new List<string>();




        public string Json()
        {
            return "";
        }

        public override string ToString()
        {
            return "Results: " + String.Join(',', results);
        }
    }
}
