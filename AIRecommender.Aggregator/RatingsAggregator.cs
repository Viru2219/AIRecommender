using AIRecommender.Entity;
using AIRecommender.Preference;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRecommender.Aggregator
{
    public interface IRatingsAggregator
    {
        Dictionary<string, List<int>> Aggregate(BookDetails bookDetails, Preferences preference);
    }

    public class RatingsAggregator : IRatingsAggregator
    {
        public Dictionary<string, List<int>> Aggregate(BookDetails bookDetails, Preferences preference)
        {
            Dictionary<string, List<int>> dict = new Dictionary<string, List<int>>();
            int minAge;
            int maxAge;
            if (preference.Age >= 1 && preference.Age <= 16)
            {
                minAge = 1;
                maxAge = 16;
            }
            else if (preference.Age >= 17 && preference.Age <= 30)
            {
                minAge = 17;
                maxAge = 30;
            }
            else if (preference.Age >= 31 && preference.Age <= 50)
            {
                minAge = 31;
                maxAge = 50;
            }
            else if (preference.Age >= 51 && preference.Age <= 50)
            {
                minAge = 51;
                maxAge = 60;
            }
            else
            {
                minAge = 61;
                maxAge = 100;
            }
            //List<string> userIds = new List<string>();
            //Console.WriteLine(preference.Age);
            //Console.WriteLine(bookDetails.Users.Count);
            foreach (User u in bookDetails.Users)
            {
                //Console.WriteLine(preference.Age >= minAge);
                //Console.WriteLine(u.Ratings.Count);
                if ((preference.Age>=minAge) && (preference.Age<=maxAge) && (u.State).Equals(preference.State))
                {
                    foreach (BookUserRating ratings in u.Ratings)
                    {
                        //Console.WriteLine((preference.Age >= minAge) && (preference.Age <= maxAge) && (u.State).Equals(preference.State));
                        if (!dict.ContainsKey(ratings.ISBN))
                        {
                            dict[ratings.ISBN] = new List<int>();
                        }
                        dict[ratings.ISBN].Add(int.Parse(ratings.Rating));
                    }
                }
            }
            return dict;
        }

    }
}
