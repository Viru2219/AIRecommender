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
            preference.ISBN = "3442446937";
            preference.State = "dc";
            preference.Age = 40;
            int limit = 10;
            IList<Book> books = engine.Recommend(preference, limit);
            if (books.Count == 0)
            {
                Console.WriteLine("No books to be recommended");
                return;
            }

            Console.WriteLine("Recommended Books are: \n");
            foreach (Book book in books)
            {
                Console.WriteLine($"{book.BookTitle} - {book.ISBN}");
            }
        }
    }
}
