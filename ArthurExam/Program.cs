
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
        static public int timeout = 5*60;   //sec
        static string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."); //@""
        static string fileName = @"results.json";

        static public List<Result>? results;
        static public List<string>? visitedUrl;
        
        static async Task Main(string[] args)
        {
//variables
            string url = @"";
            int depth = 1;

            //arguments
            try
            {
                if (args.Length > 2 || args.Length == 0) throw new Exception();
                if(args.Length == 2)
                {
                    int.TryParse(args[1], out depth);
                    if (depth < 0) depth = 0;
                }
                url = args[0];
                if(String.IsNullOrEmpty(url)) throw new Exception();
            }
            catch
            {
                Console.WriteLine("wrong arguments entered");
                if(isCliProduction) return;
            }

            if (!isCliProduction)  url = @"https://validator.w3.org/";  //example


            results = new List<Result>(); ;
            visitedUrl = new List<string>() { "./", url }; // add url


            var @element = await UrlToJSON(url);

            var task = Task.Run(async() => {
                await Finder(@element, url, depth, 0);
            });

            bool isCompletedSuccessfully = task.Wait(TimeSpan.FromSeconds(timeout));

            if (!isCompletedSuccessfully)
            {
                Console.WriteLine();
                Console.WriteLine("TASK GOT THE TIME LIMMIT AND STOPED!!!");
                Console.WriteLine("time limmit set to (sec): " + timeout);
                Console.WriteLine();
            }
            else Console.WriteLine("task is completed");
            Console.WriteLine();

            string jsonToFile = JsonConvert.SerializeObject(results.ToArray());
            string jsonToFile_results = "{" + "\"results\":" + jsonToFile + "}";

            System.IO.File.WriteAllText(System.IO.Path.Combine(filePath, fileName), jsonToFile_results);

            Console.WriteLine(String.Join(',', results));


        }    // Main End



        static async Task Finder(dynamic @element, string current_url, int depth, int current_depth)
        {
            try
            {
                if (depth != 0 && @element.tag == "a")
                {
                    string new_address = @element.attr.href;
                    if (!visitedUrl!.Contains(new_address))  //!
                    {
                        try
                        {
                            visitedUrl.Add(new_address);
                            var @new_element = await UrlToJSON(new_address);    //!
                            await Finder(@new_element, new_address, depth - 1, current_depth + 1);
                        }
                        catch
                        {   //if cant read relevent path of address
                            new_address = current_url + new_address;
                            visitedUrl.Add(new_address);
                            var @new_element = await UrlToJSON(new_address);
                            await Finder(@new_element, new_address, depth - 1, current_depth + 1);
                        }
                        Console.WriteLine("anker link found");
                    }
                    else Console.WriteLine("site viewed");
                    //do not return, img may include inside as children
                }

                else if (@element.tag == "img")
                {
                    results!.Add(new Result()
                    {
                        imageUrl = @element.attr.src,
                        sourceUrl = current_url,
                        depth = current_depth
                    });
                    Console.WriteLine("image found");
                    return;
                }

                if (@element.children.Count != 0)
                {
                    int count = element.children.Count;
                    Console.WriteLine("children check...");
                    for (int i = 0; i < count; i++) await Finder(@element.children[i], current_url, depth, current_depth);
                }
            }
            catch { Console.WriteLine("~! issue found"); };
        }   // Finder End


        static async Task<dynamic> UrlToJSON(string url)
        {
            using HttpClient client = new HttpClient();
            using var response = await client.GetAsync(url);
            string html = await response.Content.ReadAsStringAsync();
            JsonizeParser parser = new JsonizeParser();
            JsonizeSerializer serializer = new JsonizeSerializer();
            Jsonizer jsonizer = new Jsonizer(parser, serializer);
            var htmlJsonize = await jsonizer.ParseToStringAsync(html);
            // JObject json = JObject.Parse(htmlJsonize);
            dynamic @json = JsonConvert.DeserializeObject(htmlJsonize);
            return @json;
        }   // UrlToJSON End

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