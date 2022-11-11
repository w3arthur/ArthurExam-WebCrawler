
using Jsonize;
using Jsonize.Parser;
using Jsonize.Serializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
//sing System.Xml;


namespace ArthurExam
{
    class Program
    {
        static public bool isCliProduction = false;     //false for debug mode, true for exe and arguments
        static public int timeout = 5 * 60;   //sec
        static string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."); //@""
        static string fileName = @"results.json";

        static async Task Main(string[] args)
        {
            //variables
            string url = @"";
            int depth = 1;


            //arguments
            if (isCliProduction)
            { 
                try
                {
                    if (args.Length > 2 || args.Length == 0) throw new Exception();
                    if (args.Length == 2)
                    {
                        int.TryParse(args[1], out depth);
                        if (depth < 0) depth = 0;
                    }
                    url = args[0];
                    if (String.IsNullOrEmpty(url)) throw new Exception();
                }
                catch
                {
                    Console.WriteLine("wrong arguments entered");
                     return;
                }
            }
            else url = @"https://validator.w3.org/";  //example

            Crawler models = new Crawler() { Timeout = timeout };

            await models.Run(url, depth);
            await models.WriteToFile(filePath, fileName);

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