using Jsonize.Parser;
using Jsonize.Serializer;
using Jsonize;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class UrlToJson
    {

        public static async Task<dynamic> Run(string url)
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
        }

    }
}
