using Jsonize.Parser;
using Jsonize.Serializer;
using Jsonize;
using Newtonsoft.Json;
using System.Linq;

namespace WebCrawler
{

    public class Crawler : Result
    {
        public List<Result>? Results { get; private set; }
        private List<string>? VisitedUrl { get; set; }
        public int Timeout { private get; set; } = 5*60; // sec     // = default settings

        public Crawler() 
        { 
            Results = new List<Result>();
            VisitedUrl = new List<string>() { "./" };
        }

        public async Task WriteToFile(string filePath, string fileName)
        {
            string jsonToFile = await Task.Run(() => JsonConvert.SerializeObject(Results));
            string jsonToFile_results = "{" + "\"results\":" + jsonToFile + "}";
            await System.IO.File.WriteAllTextAsync(System.IO.Path.Combine(filePath, fileName), jsonToFile_results);

            Console.WriteLine(String.Join(',', Results!));
        }   // WriteToFile End

        public async Task<Crawler> Run(string url, int depth) 
        {
            var @element = await UrlToJSON(url);
            VisitedUrl!.Add(url);
            var task = Task.Run(async () =>
            { 
                await Finder(@element, url, depth, 0);
            });

            bool isCompletedSuccessfully = task.Wait(TimeSpan.FromSeconds(value: Timeout));
            if (isCompletedSuccessfully)
            {
                Console.WriteLine("task is completed");
                Console.WriteLine();
            }
            else 
            {
                Console.WriteLine("TASK GOT THE TIME LIMMIT AND STOPED!!!");
                Console.WriteLine("time limmit set to (sec): " + Timeout);
                Console.WriteLine();
            }
            return this;
        }   // Run End

        private async Task Finder(dynamic @element, string current_url, int depth, int current_depth)
        {
            try
            {
                if (depth != 0 && @element.tag == "a") // <a href="...">...</a>
                {
                    string new_address = @element.attr.href;
                    if (!VisitedUrl!.Contains(new_address))  //!
                    {
                        try
                        {
                            VisitedUrl.Add(new_address);
                            var @new_element = await UrlToJSON(new_address);    //!
                            await Finder(@new_element, new_address, depth - 1, current_depth + 1);
                        }
                        catch
                        {   //if cant read relevent path of address
                            new_address = current_url + new_address;
                            VisitedUrl.Add(new_address);
                            var  @new_element = await UrlToJSON(new_address);
                            await Finder(@new_element, new_address, depth - 1, current_depth + 1);
                        }
                        Console.WriteLine("anker link found");
                    }
                    else Console.WriteLine("site viewed");
                    //do not return, img may included inside <a>...</a> as children
                }
                else if (@element.tag == "img") // <img src="..." />
                {
                    string imageUrl = @element.attr.src;
                    if (! Results!.Any( x => x.imageUrl == imageUrl) )
                    {
                        Results!.Add(new Result()
                        {
                            imageUrl = imageUrl,
                            sourceUrl = current_url,
                            depth = current_depth
                        });
                        Console.WriteLine("image found");
                    }
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


       public static async Task<dynamic> UrlToJSON(string url)
        {
            using HttpClient client = new HttpClient();
            using var response = await client.GetAsync(url);
            string html = await response.Content.ReadAsStringAsync();
            JsonizeParser parser = new JsonizeParser();
            JsonizeSerializer serializer = new JsonizeSerializer();
            Jsonizer jsonizer = new Jsonizer(parser, serializer);
            var htmlJsonize = await jsonizer.ParseToStringAsync(html);
            // JObject json = JObject.Parse(htmlJsonize);
            dynamic? @json = JsonConvert.DeserializeObject(htmlJsonize);
            if (@json is null) throw new Exception();
            return @json;
        }   // UrlToJSON End

        public static void ArgumentsSet(string[] args, out string url, out int depth)
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



// make the functions shorter
// fix another issues
// learn about debugger