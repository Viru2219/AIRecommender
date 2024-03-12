using AIRecommender.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AIRecommender.DataLoader
{
    public interface IDataLoader
    {
        BookDetails Load();
    }

    public class CSVDataLoader : IDataLoader
    {
        public BookDetails bookDetails = new BookDetails();
        public BookDetails Load()
        {
            BookDetails bookDetails = new BookDetails();
            //Loading books
            List<Book> books = LoadBooks();

            //load bookuserratings
            List<BookUserRating> ratings = LoadRatings();

            //load users
            List<User> users = LoadUsers();



            //linking Book and BookUserRating
            // Create a dictionary to store books by their ISBN
            Dictionary<string, Book> bookDictionary = books.ToDictionary(book => book.ISBN);

            // Iterate through ratings and assign them to corresponding books
            foreach (var rating in ratings)
            {
                if (bookDictionary.TryGetValue(rating.ISBN, out Book book))
                {
                    book.Ratings.Add(rating);
                    rating.TheBook = book;
                }
            }

            bookDetails.Books = books;

            //linking User and BookUserRating
            Dictionary<string, User> userDictionary = users.ToDictionary(user => user.UserID);
            foreach (BookUserRating rating1 in ratings)
            {
                if (userDictionary.TryGetValue(rating1.UserID, out User user))
                {
                    user.Ratings.Add(rating1);
                    rating1.TheUser = user;
                }
            }

            bookDetails.Users = users;
            bookDetails.Ratings = ratings;

            return bookDetails;
        }
        List<Book> LoadBooks()
        {
            List<Book> books = new List<Book>();
            using (StreamReader reader = new StreamReader(@"C:\Users\K VIRUPAKSHI\Downloads\BX-CSV-Dump\BX-Books.csv"))
            {
                string headerLine = reader.ReadLine();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] split = line.Split(';');
                    Book book = new Book
                    {
                        ISBN = split[0].Trim('"'),
                        BookTitle = split[1].Trim('"'),
                        BookAuthor = split[2].Trim('"'),
                        YearOfPublication = split[3].Trim('"'),
                        Publisher = split[4].Trim('"'),
                        ImageUrlSmall = split[5].Trim('"'),
                        ImageUrlMedium = split[6].Trim('"'),
                        ImageUrlLarge = split[7].Trim('"')
                    };
                    books.Add(book);
                }
            }
            return books;
        }
        List<BookUserRating> LoadRatings()
        {
            List<BookUserRating> ratings = new List<BookUserRating>();
            using (StreamReader reader = new StreamReader(@"C:\Users\K VIRUPAKSHI\Downloads\BX-CSV-Dump\BX-Book-Ratings.csv"))
            {
                string headerLine = reader.ReadLine();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] split = line.Split(';');
                    BookUserRating rating = new BookUserRating
                    {
                        UserID = split[0].Trim('"'),
                        ISBN = split[1].Trim('"'),
                        Rating = split[2].Trim('"')
                    };
                    ratings.Add(rating);
                }
            }
            return ratings;
        }
        List<User> LoadUsers()
        {
            List<User> users = new List<User>();
            using (StreamReader reader = new StreamReader(@"C:\Users\K VIRUPAKSHI\Downloads\BX-CSV-Dump\BX-Users.csv"))
            {
                string headerLine = reader.ReadLine();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] split = line.Split(';');
                    string[] locationvalues = split[1].Trim('"').Split(',');
                    string city = "";
                    string state = "";
                    string country = "";
                    if (locationvalues.Length >= 3)
                    {
                        city = locationvalues[0].Trim();
                        state = locationvalues[1].Trim();
                        country = locationvalues[2].Trim();
                    }
                    else if (locationvalues.Length >= 2)
                    {
                        city = locationvalues[0].Trim();
                        state = locationvalues[1].Trim();
                    }
                    else
                        city = locationvalues[0].Trim();

                    User user = new User
                    {
                        UserID = split[0].Trim('"'),
                        City = city,
                        State = state,
                        Country = country,
                    };

                    if (split.Length >= 3 && !string.IsNullOrEmpty(split[2]))
                    {
                        int age;
                        if (int.TryParse(split[2].Trim('"'), out age))
                        {
                            user.Age = age;
                        }
                        else
                        {
                            // Handle the case where parsing fails
                            user.Age = 0;
                        }
                    }
                    else user.Age = 0;

                    users.Add(user);
                }
            }
            return users;
        }
    }
}
