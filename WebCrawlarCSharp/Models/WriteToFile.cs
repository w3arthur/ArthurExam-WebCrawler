using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Classes;

namespace WebCrawler.Models
{
    public class WriteToFile
    {
        public static async Task Run(List<Result> results, string filePath, string fileName)
        {
            Console.WriteLine("Async WriteToFile Thread " + Thread.CurrentThread.ManagedThreadId);
            string jsonToFile = await Task.Run(() => JsonConvert.SerializeObject(results));
            string jsonToFile_results = "{" + "\"results\":" + jsonToFile + "}";
            await System.IO.File.WriteAllTextAsync(System.IO.Path.Combine(filePath, fileName), jsonToFile_results);

            Console.WriteLine(String.Join(',', results));
        }

    }
}
