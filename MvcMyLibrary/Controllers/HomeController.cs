using System.Collections.Generic;
using System.Linq;
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
            //notice that a PartialView is being returned
            return PartialView(BookActions.GetGenres());
        }
    }
}