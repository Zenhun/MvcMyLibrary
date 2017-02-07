using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMyLibrary.Models;

namespace MvcMyLibrary.Controllers
{
    public class HomeController : Controller
    {
        MyLibraryContext dbLibrary = new MyLibraryContext();
        CompleteBook bookList = new CompleteBook();

        public ActionResult Index()
        {
            List<CompleteBook> books = new List<CompleteBook>();
            if (TempData["books"] != null)
                books = (List<CompleteBook>)TempData["books"];
            else
                books = bookList.GetCompleteBooks();
            
            return View(books);
        }

        public ActionResult IndexGenre(int GenreId)
        {
            List<CompleteBook> books = bookList.GetCompleteBooks(GenreId);

            return View("Index", books);
        }

        [ChildActionOnly]
        public ActionResult SideBar()
        {
            List<Genre> genres = (from g in dbLibrary.Genres
                                  orderby g.GenreName
                                  select g).ToList();

            //notice that a PartialView is being returned
            return PartialView(genres);
        }
    }
}