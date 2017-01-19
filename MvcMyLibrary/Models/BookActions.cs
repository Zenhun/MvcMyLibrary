using System;
using System.Collections.Generic;
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

            MyLibraryContext dbLibrary = new MyLibraryContext();

            SqlParameter paramName = new SqlParameter("@Name", authorName);
            SqlParameter paramSurname = new SqlParameter("@Surname", authorSurname);
            authorId = dbLibrary.Database.ExecuteSqlCommand("spCheckAuthor @Name, @Surname, @AuthorId", paramName, paramSurname, new SqlParameter("@AuthorId", DBNull.Value));
            System.Diagnostics.Debug.WriteLine("Returned Author Id: " + authorId);
        }
    }
}