using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace UI.Web.Controllers
{
   
    public class GenreController : Controller
    {
        IServiceGenres sg;
        // GET: GenreController


        public GenreController(IServiceGenres sg)
        {

            this.sg = sg;
            
        }
        [Authorize(Policy = "EmployeeRead")]

        public ActionResult Index()
        {
            return View(sg.GetMany());
        }

        // GET: GenreController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GenreController/Create
        [Authorize(Policy = "EmployeeCreate")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: GenreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Create(Genre collection)
        {
            try
            {
                sg.Add(collection);
                sg.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GenreController/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var genre = sg.GetById(id);
            if (genre == null)
            {
                return RedirectToAction(nameof(Index));
            }



            return View(genre);
        }

        // POST: GenreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
    
        public ActionResult Edit(int id, Genre updateGenre)
        {
            try
            {
                var genre = sg.GetById(id);

                if (genre == null)
                {
                    return RedirectToAction("Index");
                }

                genre.NomGenre = updateGenre.NomGenre;


                sg.Update(genre);
                sg.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Policy = "EmployeeDelete")]
        public ActionResult Delete(int id)
        {
            var genre = sg.GetById(id);
            if (genre == null)
            {
                return RedirectToAction("Index");
            }

            return View(genre);
        }

        // POST: GenreController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var genre = sg.GetById(id);
                if (genre == null)
                {
                    return RedirectToAction("Index");
                }

                sg.Delete(genre);
                sg.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
