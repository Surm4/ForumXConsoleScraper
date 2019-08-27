using System;
using System.IO;
namespace ForumXScrap
{
    class Program
    {
        static void Main(string[] args)
        {   
            Console.WriteLine("Podaj uri z forum x do zarchiwizowania (cały)");
            var Thread = Console.ReadLine();
            Console.WriteLine("Archiwizuje...");
            try {
                var Scraper = new Scraper(Thread);
                var SaveFredService = new SaveThread(Scraper.GetThread(), Scraper.GetFilesToDl(), Scraper.GetFilesNames());
                Console.WriteLine("Koniec, pliki są w twoich dokumentach /forumXthreads");
            } catch (Exception) {
                Console.WriteLine("Coś popsułeś, czego ty tam znowu nie umiesz...");
            }
        }
    }
}
