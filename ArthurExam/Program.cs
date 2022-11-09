
using Jsonize;
using Jsonize.Parser;
using Jsonize.Serializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
//sing System.Xml;


namespace ArthurExam
{
    class Program
    {
        static string filePath = System.IO.Path.Combine( AppDomain.CurrentDomain.BaseDirectory, @"..\..\..");
        static string fileName = @"results.json";
        static string url = @"https://validator.w3.org/";
        //List<string> programmerLinks = new List<string>();
        static public List<Result> results;
        static public List<string> visitedUrl;

        static async Task Finder(dynamic @element, string current_url, int depth, int current_depth) 
        {
            try
            {
                if (depth != 0 && @element.tag == "a")
                {
                    string new_address = @element.attr.href;
                    if (!visitedUrl.Contains(new_address)) 
                    {

                        try
                        {
                            visitedUrl.Add(new_address);
                            var @new_element = await AddressToJSON(new_address);
                            await Finder(@new_element, new_address, depth - 1, current_depth + 1);
                        }
                        catch 
                        {   //if cant read relevent path of address
                            new_address = current_url + new_address;
                            visitedUrl.Add(new_address);
                            var @new_element = await AddressToJSON(new_address);
                            await Finder(@new_element, new_address, depth - 1, current_depth + 1);
                        }

                        Console.WriteLine("anker link found");
                    }
                    else Console.WriteLine("site viewed");

                }
            }
            catch { };


            try
            {
                if (@element.tag == "img") 
                {
                    results!.Add(new Result()
                        {
                            imageUrl = @element.attr.src,
                            sourceUrl = current_url,
                            depth = current_depth
                    }
                    );
                    
                    Console.WriteLine("image found"); 
                    return; 
                }
            }
            catch { };

            try
            {
                if (@element.children.Count != 0)
                {
                    int count = element.children.Count;
                    Console.WriteLine("children check...");
                    for (int i = 0; i < count; i++)
                        await Finder(@element.children[i], current_url, depth, current_depth);
                }
            }
            catch { };
        }



        static async Task<dynamic> AddressToJSON(string url) 
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            string html = await response.Content.ReadAsStringAsync();
            JsonizeParser parser = new JsonizeParser();
            JsonizeSerializer serializer = new JsonizeSerializer();
            Jsonizer jsonizer = new Jsonizer(parser, serializer);
            var htmlJsonize = await jsonizer.ParseToStringAsync(html);
            // JObject json = JObject.Parse(htmlJsonize);
            dynamic @json = JsonConvert.DeserializeObject(htmlJsonize);
            return @json;

        }

        static async Task Main(string[] args)
        {
            results = new List<Result>(); ;
            visitedUrl = new List<string>() { "./" };


            visitedUrl.Add(url);

            var @element = await AddressToJSON(url);

            await Finder(@element, url, 1, 0);

            
            string jsonToFile = JsonConvert.SerializeObject(results.ToArray());
            string jsonToFile_results = "{" + "\"results\":" + jsonToFile + "}";

            System.IO.File.WriteAllText(System.IO.Path.Combine(filePath, fileName), jsonToFile_results);


            Console.WriteLine(results);
            Console.WriteLine("-- done");
            // Console.ReadLine();

        }
    }
}


//developer tools
//install newton json
//
//istall jsonize
//https://github.com/JackWFinlay/jsonize
//json pretier
//https://jsonformatter.curiousconcept.com/#