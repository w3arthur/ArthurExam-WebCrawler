using Jsonize.Parser;
using Jsonize.Serializer;
using Jsonize;
using Newtonsoft.Json;
using System.Linq;
using WebCrawler.Classes;
using WebCrawler.Models;


namespace WebCrawler
{

    public class Crawler : Result
    {
        public List<Result>? Results { get; private set; } = new List<Result>();
        private List<string>? VisitedUrl { get; set; } = new List<string>() { "./" };
        public int Timeout { private get; set; } = 5*60; // sec     // = default settings

        //public Crawler() { }

        public async Task<List<Result>> Run(string url, int depth) 
        {
            Console.WriteLine("Async Run Thread " + Thread.CurrentThread.ManagedThreadId);
            var @element = await UrlToJson.Run(url);
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
            return Results;
        }   // Run End

        private async Task Finder(dynamic @element, string current_url, int depth, int current_depth)
        {
            try
            {
                Console.WriteLine("Async Finder Thread " + Thread.CurrentThread.ManagedThreadId);
                if (depth != 0 && @element.tag == "a") // <a href="...">...</a>
                {
                    string new_address = @element.attr.href;
                    if (!VisitedUrl!.Contains(new_address))  //!
                    {
                        try
                        {
                            VisitedUrl.Add(new_address);
                            var @new_element = await UrlToJson.Run(new_address);    //!
                            await Finder(@new_element, new_address, depth - 1, current_depth + 1);
                        }
                        catch
                        {   //if cant read relevent path of address
                            new_address = current_url + new_address;
                            VisitedUrl.Add(new_address);
                            var  @new_element = await UrlToJson.Run(new_address);
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
                    if (! Results!.Any( result => result.imageUrl == imageUrl) )
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




    }
}


// make the functions shorter
// fix another issues
// learn about debugger