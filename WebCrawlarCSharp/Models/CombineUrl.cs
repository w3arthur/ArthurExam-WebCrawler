using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class CombineUrl
    {
        public static string Run(string currentUri, string additionUri)
        {
            Uri baseUri = new Uri(currentUri);
            string new_address =
                additionUri.Contains("://")   //additional url starts with protocol like https://
                ? additionUri
                : new Uri(baseUri, additionUri).ToString();
            return new_address;
        }
    }
}
