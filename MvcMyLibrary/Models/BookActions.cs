using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MvcMyLibrary.Models
{
    public class BookActions
    {
        public static void BookSave(string title, string authorName, string authorSurname, int genreId, string imageUrl)
        {
            if (imageUrl == "")
                imageUrl = "noimage.jpg";

            int authorId;

            using (MyLibraryContext dbLibrary = new MyLibraryContext())
            {
                var paramOut = new SqlParameter();
                paramOut.ParameterName = "@output";
                paramOut.SqlDbType = SqlDbType.Int;
                paramOut.Direction = ParameterDirection.Output;

                SqlParameter paramName = new SqlParameter("@name", authorName);
                SqlParameter paramSurname = new SqlParameter("@surname", authorSurname);
                //it didn't work until I added .FirstOrDefault()
                var data = dbLibrary.Database.SqlQuery<object>("EXEC @output = checkAuthor @name, @surname", paramOut, paramName, paramSurname).FirstOrDefault();
                authorId = (int)paramOut.Value;

                var newBook = new Book();
                newBook.Title = title;
                newBook.AuthorId = authorId;
                newBook.GenreId = genreId;
                newBook.ImageUrl = imageUrl;
                dbLibrary.Books.Add(newBook);
                dbLibrary.SaveChanges();
            }            
        }
    }
}