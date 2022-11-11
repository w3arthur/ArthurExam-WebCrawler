
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
                    Crawler.ArgumentsSet(args, ref url, ref depth);
                }
                catch
                {
                    Console.WriteLine("wrong arguments entered");
                    return;
                }
            }
            else url = @"https://validator.w3.org/";  //example

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