using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MvcMyLibrary.Models;

namespace MvcMyLibrary.Controllers
{
    public class GenresController : Controller
    {
        private MyLibraryContext db = new MyLibraryContext();

        // GET: Genres
        public ActionResult Index()
        {
            return View(db.Genres.OrderBy(g => g.GenreName).ToList());
        }

        // GET: Genres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GenreId,GenreName")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                //db.Genres.Add(genre);
                //db.SaveChanges();

                int result = BookActions.GenreSave(genre.GenreName);

                return RedirectToAction("Index");
            }

            return View(genre);
        }

        // GET: Genres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GenreId,GenreName")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genre);
        }

        // GET: Genres/Delete/5
        public ActionResult Delete(int id)
        {
            if (BookActions.BookGenreCheck(id))
            {
                Genre genre = db.Genres.Find(id);
                db.Genres.Remove(genre);
                db.SaveChanges();
            }
            else
                TempData["bookExists"] = "true";
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
