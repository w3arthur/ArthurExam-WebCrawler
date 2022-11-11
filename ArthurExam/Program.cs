﻿
namespace ArthurExam
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
            {
                try { Crawler.ArgumentsSet(args, out url, out depth); }
                catch { Console.WriteLine("wrong arguments entered"); return; }
            }
            else
            {
                Console.WriteLine("Debug mode");
                url = @"https://validator.w3.org/"; //example
                //depth = depth; //example
                filePath = filePath_Debugger;
            }

            Console.WriteLine("Crawler running, please wait...");

            Crawler crawler = await new Crawler() { Timeout = timeout }.Run(url, depth) ;

            await crawler.WriteToFile(filePath, fileName);

        }    // Main End

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