using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace MvcMyLibrary.Models
{
    public class BookActions
    {
        public static void BookSave(string title, string authorName, string authorSurname, int genreId, HttpPostedFileBase imageUrlFile)
        {
            string imageUrl = GetImageUrl(imageUrlFile);
            
            using (MyLibraryContext dbLibrary = new MyLibraryContext())
            {
                int authorId = GetAuthorId(authorName, authorSurname);

                //create and save new book
                var newBook = new Book();
                newBook.Title = title;
                newBook.AuthorId = authorId;
                newBook.GenreId = genreId;
                newBook.ImageUrl = imageUrl;
                dbLibrary.Books.Add(newBook);
                dbLibrary.SaveChanges();
            }            
        }

        public static void BookUpdate(int bookId, string title, string authorName, string authorSurname, int genreId, HttpPostedFileBase imageUrlFile)
        {
            string imageUrl = GetImageUrl(imageUrlFile);

            using (MyLibraryContext dbLibrary = new MyLibraryContext())
            {
                int authorId = GetAuthorId(authorName, authorSurname);

                //find book by id
                var book = from b in dbLibrary.Books
                           where b.BookId == bookId
                           select b;

                //update book
                Book bookToUpdate = book.First();
                bookToUpdate.Title = title;
                bookToUpdate.AuthorId = authorId;
                bookToUpdate.GenreId = genreId;
                bookToUpdate.ImageUrl = imageUrl;
                dbLibrary.SaveChanges();
            }
        }

        public static string GetImageUrl(HttpPostedFileBase imageUrlFile)
        {
            string imageUrl;

            if (imageUrlFile != null && imageUrlFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(imageUrlFile.FileName);
                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/"), fileName);
                imageUrlFile.SaveAs(path);
                imageUrl = imageUrlFile.FileName;
            }
            else
                imageUrl = "noimage.jpg";

            return imageUrl;
        }

        public static int GetAuthorId(string authorName, string authorSurname)
        { 
            int authorId;

            using (MyLibraryContext dbLibrary = new MyLibraryContext())
            {
                var paramOut = new SqlParameter();
                paramOut.ParameterName = "@output";
                paramOut.SqlDbType = SqlDbType.Int;
                paramOut.Direction = ParameterDirection.Output;

                SqlParameter paramName = new SqlParameter("@name", authorName);
                SqlParameter paramSurname = new SqlParameter("@surname", authorSurname);
                //stored procedure to check if author exist -- if yes, return author id; if no, create author and return author id
                //it didn't work until I added .FirstOrDefault()
                var data = dbLibrary.Database.SqlQuery<object>("EXEC @output = checkAuthor @name, @surname", paramOut, paramName, paramSurname).FirstOrDefault();
                authorId = (int)paramOut.Value;
            }

            return authorId;
        }
    }
}