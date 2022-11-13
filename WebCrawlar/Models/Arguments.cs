using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class Arguments
    {
        public static void Set(string[] args, out string url, out int depth)
        {
            depth = 0;
            url = @"";
            if (args.Length > 2 || args.Length == 0) throw new Exception();
            if (args.Length == 2)
            {
                int.TryParse(args[1], out depth);
                if (depth < 0) depth = 0;
            }
            url = args[0];
            if (String.IsNullOrEmpty(url)) throw new Exception();
        }   // ArgumentsSet End
    }
}
