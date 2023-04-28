using AngleSharp.Dom;
using System.Net;
using TZ2V.Parser;

namespace TZ2V
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Uri uri = new Uri("https://www.ilcats.ru/toyota/?function=getModels&market=EU");
            string dir = @"C:\Users\Стас\OneDrive\Документы\Shemes\";

            HtmlCarsParser htmlParser = new HtmlCarsParser(ConfigurationType.Loader, uri, dir);

            await htmlParser.ParseAsync(1);
            
                

        }

    }
}