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
            List<CompleteBook> books = bookList.GetCompleteBooks();

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

            //obrati pažnju da se vraća PartialView
            return PartialView(genres);
        }

        public ActionResult SaveBook()
        {
            //za probu vraćam GenreId = 1
            return RedirectToAction("IndexGenre", new { GenreId = 1 });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}