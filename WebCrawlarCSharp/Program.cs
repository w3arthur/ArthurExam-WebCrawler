using WebCrawler.Classes;
using WebCrawler.Models;

namespace WebCrawler
{

    class Program
    {
        static public bool isCliProduction = false;     //false for debug mode, true for exe and arguments
        static public int timeout = 5 * 60;   //sec
        static string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @""); //@""
        static string filePath_Debugger = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."); //@""
        static string fileName = @"results.json";
        //variables
        static string url = @"";
        static int depth = 0;

        static async Task Main(string[] args)
        { 
            //arguments
            if (args.Length > 0 || isCliProduction)    //isCliProduction
                try { Arguments.Set(args, out url, out depth); } catch { Console.WriteLine("wrong arguments entered"); return; }
            else
            {
                Console.WriteLine("Debug mode");
                url = @"https://flickr.com/photos/17682242@N06/5440808439/in/photolist-9hMyN4-kBT78S-rV85K9-kBRpS4-WmLNA9-cJDCpj-dSvags-bj6bg-9o1mXS-3ofsFe-dSvbhq-9TeAzw-cJDCyo-qzyEXB-nRZW1T-qihS4a-jzbxoo-bKnEHr-hQSh3P-FepYUZ-nxJtP6-pZSEPG-Fi4F9b-Swi5fn-txniu-2tjLVH-dV26un-dV7EPy-jGng3d-nxJzW6-xvmN3m-d9sv9d-bAg6A2-RFSz1b-oxLukt-N1KUU-YJfWvC-er5CuS-hvv58J-UoWEfA-dvc8TX-raZhk2-23WvnkC-TGJSLW-2dPKDJR-dQs2bU-6bSfhj-g7zra2-vMKuVY-23EkcWw"; //example
                filePath = filePath_Debugger;
            }

            Console.WriteLine("Crawler running, please wait...");
            List<Result> crawlerResults = await new Crawler() { Timeout = timeout }.Run(url, depth) ;
            await WriteToFile.Run(crawlerResults, filePath, fileName);

        }

    }

}

//developer tools
//install newton json
//
//istall jsonize
//https://github.com/JackWFinlay/jsonize
//json pretier
//https://jsonformatter.org/json-pretty-print
//https://jsonformatter.curiousconcept.com/#
//write testing https://stackoverflow.com/questions/372865/path-combine-for-urls
//learn better about debbuger