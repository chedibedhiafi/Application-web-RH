using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace UI.Web.Controllers
{
    public class SituationController : Controller

    {
        IServiceSituation ss;

        public SituationController(IServiceSituation ss)
        {
           
            this.ss = ss;
        }
        // GET: SituationController
        [Authorize(Policy = "EmployeeRead")]

        public ActionResult Index()
        {
            return View(ss.GetMany());
        }

        // GET: SituationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SituationController/Create
        [Authorize(Policy = "EmployeeCreate")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: SituationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Situation collection)
        {
            try
            {
                ss.Add(collection);
                ss.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SituationController/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var situation = ss.GetById(id);
            if (situation == null)
            {
                return RedirectToAction(nameof(Index));
            }

          

            return View(situation);
        }

        // POST: SituationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Situation updatedSituation)
        {
            try
            {
                var situation = ss.GetById(id);

                if (situation == null)
                {
                    return RedirectToAction("Index");
                }

                situation.nomSituation = updatedSituation.nomSituation;
              

                ss.Update(situation);
                ss.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }







        // GET: SituationController/Delete/5
        [Authorize(Policy = "EmployeeDelete")]
        public ActionResult Delete(int id)
        {
            var situation = ss.GetById(id);
            if (situation == null)
            {
                return RedirectToAction("Index");
            }

            return View(situation);
        }

        // POST: SituationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var situation = ss.GetById(id);
                if (situation == null)
                {
                    return RedirectToAction("Index");
                }

                ss.Delete(situation);
                ss.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
