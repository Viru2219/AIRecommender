using AIRecommender.Aggregator;
using AIRecommender.DataLoader;
using AIRecommender.Entity;
using AIRecommender.Preference;
using AIRecommender.Recommenders;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRecommendationEngine
{
    public class RecommendationEngine
    {
        IRecommender aiRecommender;
        IDataLoader loadData;
        public RecommendationEngine()
        {
            aiRecommender = new Recommender();
            loadData = new CSVDataLoader();
        }
        public RecommendationEngine(IRecommender recommender, IDataLoader loader)
        {
            aiRecommender = recommender;
            loadData = loader;
        }
        public IList<Book> Recommend(Preferences preference, int limit)
        {
            BookDetails bookDetails = loadData.Load();
            //Console.WriteLine("loading done");

            //list of ratings of book in preference
            List<int> PrefISBNRatings = new List<int>();
            foreach (BookUserRating ratings in bookDetails.Ratings)
            {
                if ((preference.ISBN).Equals(ratings.ISBN))
                {
                    PrefISBNRatings.Add(int.Parse(ratings.Rating));
                }
            }
            //dictionary of all relevant books and their ratings
            AIRecommender.Aggregator.IRatingsAggregator aggregator = new RatingsAggregator();
            Dictionary<string, List<int>> relevantBooks = aggregator.Aggregate(bookDetails, preference);
            Dictionary<string, double> bookCorrelation = new Dictionary<string, double>();

            foreach (var books in relevantBooks)
            {
                bookCorrelation.Add(books.Key, aiRecommender.GetCorrelation(PrefISBNRatings, books.Value));
            }
            List<KeyValuePair<string, double>> list = bookCorrelation.ToList();
            list.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            List<string> finalISBNlist = new List<string>();
            if (list.Count > limit)
            {
                for (int i = 0; i <= limit; i++)
                    finalISBNlist.Add(list[i].Key);
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                    finalISBNlist.Add(list[i].Key);
            }

            Dictionary<string, Book> ISBNToBook = bookDetails.Books.ToDictionary(book => book.ISBN);
            List<Book> ans = new List<Book>();
            foreach (string s in finalISBNlist)
            {
                if (ISBNToBook.ContainsKey(s))
                {
                    ans.Add(ISBNToBook[s]);
                    //Console.WriteLine(bookCorrelation[s]);
                }
            }
            return ans;
        }

    }
}
