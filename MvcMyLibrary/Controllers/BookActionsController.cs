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
        [HttpPost]
        public ActionResult SaveBook(int hiddenId, string Title, string AuthorName, string AuthorSurname, int GenreId, HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {
                //New Book modal has data-id=0
                //Update Book modal has data-id=BookId
                if (hiddenId == 0)
                {
                    BookActions.BookSave(Title, AuthorName, AuthorSurname, GenreId, ImageUrl);
                }
                else
                {
                    int BookId = hiddenId;
                    BookActions.BookUpdate(BookId, Title, AuthorName, AuthorSurname, GenreId, ImageUrl);
                }
                
            }
            
            return RedirectToAction("Index", "Home");
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
