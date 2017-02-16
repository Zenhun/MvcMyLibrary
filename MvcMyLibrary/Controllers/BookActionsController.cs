using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MvcMyLibrary.Models;

namespace MvcMyLibrary.Controllers
{
    public class BookActionsController : Controller
    {
        [HttpPost]
        public ActionResult SaveUpdateBook(int hiddenId, string Title, string AuthorName, string AuthorSurname, int GenreId, HttpPostedFileBase ImageUrl)
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

        [HttpPost]
        public ActionResult CreateGenre(string genre)
        {
            int genreId = BookActions.GenreSave(genre);

            return Json(new { genreId = genreId });
        }

        // POST: BookActions/Delete/5
        //[HttpPost]
        public ActionResult Delete(int id, string image)
        {
            try
            {
                string path;
                // delete book from db and image file from server
                if(image != "noimage.jpg")
                    path = Request.MapPath("~/Content/Images/" + image);
                else
                    path = "";
                BookActions.BookDelete(id, path);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Search(string search, FormCollection formCollection)
        {
            bool chkSwitch = false;
            if(formCollection["chkSwitch"] == "on")
                chkSwitch = true;
            List<CompleteBook> books = BookActions.Search(search, chkSwitch);
            TempData["books"] = books;
            

            //i'm passing model to a different controller so I have to use TempData
            //TempData = short lived session used for redirects
            return RedirectToAction("Index", "Home");
        }
    }
}
