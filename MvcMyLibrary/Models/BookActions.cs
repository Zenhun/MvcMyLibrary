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
                //if imageUrlFile == null we are keeping current cover, so don't include in update
                if(imageUrlFile != null)
                    bookToUpdate.ImageUrl = GetImageUrl(imageUrlFile);
                dbLibrary.SaveChanges();
            }
        }

        public static void BookDelete(int bookId, string path)
        {
            using (var library = new MyLibraryContext())
            {
                var book = from b in library.Books
                           where b.BookId == bookId
                           select b;

                library.Books.Remove(book.First());
                library.SaveChanges();
            }

            //prevent deleting noimage.jpg file
            if (File.Exists(path) && path != "")
            {
                File.Delete(path);
            }
        }

        public static int GenreSave(string genre)
        {
            int genreId;
            using (MyLibraryContext dbLibrary = new MyLibraryContext())
            {
                var paramOut = new SqlParameter("@output", SqlDbType.Int);
                paramOut.Direction = ParameterDirection.Output;

                var paramGenre = new SqlParameter("@genreName", genre);
                var data = dbLibrary.Database.SqlQuery<object>("EXEC @output = checkAndInsertGenre @genreName", paramOut, paramGenre).FirstOrDefault();

                genreId = (int)paramOut.Value;
            }

            return genreId;
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

        internal static List<CompleteBook> Search(string search, bool chkSwitch)
        {
            List<CompleteBook> books = new List<CompleteBook>();

            //spliting the search string, approximating everything after first space as surname
            //in order to search for eg. "Anne Rice"
            string[] names = search.ToString().Trim().Split(new char[] { ' ' }, 2);
            string name = names[0];
            string surname;
            //check if there are at least two words in search string
            if (names.Length > 1)
                surname = names[1];
            else
                surname = name;
            
            using (var dbLibrary = new MyLibraryContext())
            {
                var booksLib = (from b in dbLibrary.Books
                              join a in dbLibrary.Authors on b.AuthorId equals a.AuthorId
                              join g in dbLibrary.Genres on b.GenreId equals g.GenreId
                              where (chkSwitch ? (a.Name.Contains(name) || a.Surname.Contains(surname)) : b.Title.Contains(search) )
                              orderby b.Title
                              select new { b.BookId, b.Title, a.Name, a.Surname, g.GenreId, g.GenreName, b.ImageUrl }).ToList();

                foreach (var book in booksLib)
                {
                    CompleteBook newBook = new CompleteBook();
                    newBook.BookId = book.BookId;
                    newBook.Title = book.Title;
                    newBook.AuthorName = book.Name;
                    newBook.AuthorSurname = book.Surname;
                    newBook.GenreId = book.GenreId;
                    newBook.Genre = book.GenreName;
                    newBook.CoverImageUrl = book.ImageUrl;
                    books.Add(newBook);
                }
            }

            return books;
        }
    }
}