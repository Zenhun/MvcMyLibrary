using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMyLibrary.Models;

namespace MvcMyLibrary.Controllers
{
    public class BookActionsController : Controller
    {
        //MyLibraryContext dbLibrary = new MyLibraryContext();

        public ActionResult SaveBook(string Title, string AuthorName, string AuthorSurname, int GenreId, string ImageUrl)
        {
            //Debug Output just to test user input
            System.Diagnostics.Debug.WriteLine("Title is " + Title);
            System.Diagnostics.Debug.WriteLine("Author is " + AuthorName + " " + AuthorSurname);

            BookActions.BookSave(Title, AuthorName, AuthorSurname, GenreId, ImageUrl);

            return RedirectToAction("IndexGenre", "Home", new { GenreId });
        }

        // GET: BookActions/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BookActions/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookActions/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //// GET: BookActions/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    //using (var library = new MyLibraryContext())
        //    //{
        //    //    var book = from b in library.Books
        //    //               where b.BookId == id
        //    //               select b;

        //    //    library.Books.Remove(book.First());
        //    //    library.SaveChanges();
        //    //}

        //    return RedirectToAction("Index", "Home");
        //}

        // POST: BookActions/Delete/5
        //[HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (var library = new MyLibraryContext())
                {
                    var book = from b in library.Books
                               where b.BookId == id
                               select b;

                    library.Books.Remove(book.First());
                    library.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
