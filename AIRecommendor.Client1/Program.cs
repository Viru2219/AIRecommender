using AIRecommendationEngine;
using AIRecommender.Preference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRecommender.Entity;

namespace AIRecommendor.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RecommendationEngine engine = new RecommendationEngine();
            Preferences preference = new Preferences();
            preference.ISBN = "0195153448";
            preference.State = "california";
            preference.Age = 18;
            int limit = 10;
            List<Book> books = engine.Recommend(preference, limit);
            foreach (Book book in books)
            {
                Console.WriteLine(book.BookTitle);
            }
        }
    }
}
