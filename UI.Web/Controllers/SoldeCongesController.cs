using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UI.Web.Controllers
{
    public class SoldeCongesController : Controller
    {
        IServiceSoldeConges ssc;
        public SoldeCongesController(IServiceSoldeConges ssc)
        {

            this.ssc = ssc;
        }

        // GET: SoldeCongesController
        public ActionResult Index()
        {
            return View(ssc.GetMany());
        }

        // GET: SoldeCongesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SoldeCongesController/Create
        public ActionResult Create()
        {
            var existingActivatedSoldeConges = ssc.GetMany().FirstOrDefault(s => s.IsActivated);

            if (existingActivatedSoldeConges != null)
            {
                // An active SoldeConges already exists, so you can't create a new one with IsActive=true
                ModelState.AddModelError("IsActivated", "An active SoldeConges already exists.");
                return View();
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SoldeConges collection)
        {
            try
            {
                // Check if there's already an active SoldeConges
                var existingActivatedSoldeConges = ssc.GetMany().FirstOrDefault(s => s.IsActivated);

                if (existingActivatedSoldeConges != null && collection.IsActivated)
                {
                    // An active SoldeConges already exists, so you can't create a new one with IsActive=true
                    ModelState.AddModelError("IsActivated", "An active SoldeConges already exists.");
                    return View(collection); // Return the view with validation errors
                }

                ssc.Add(collection);
                ssc.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }





        // GET: SoldeCongesController/Edit/5
        public ActionResult Edit(int id)
        {
            var situation = ssc.GetById(id);
            if (situation == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(situation);
        }

        // POST: SoldeCongesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SoldeConges updatedSituation)
        {
            try
            {
                var situation = ssc.GetById(id);

                if (situation == null)
                {
                    return RedirectToAction("Index");
                }

                // Check if there's already an active SoldeConges
                var existingActivatedSoldeConges = ssc.GetMany().FirstOrDefault(s => s.IsActivated);

                if (existingActivatedSoldeConges != null && updatedSituation.IsActivated)
                {
                    // An active SoldeConges already exists, so you can't edit to set IsActive=true
                    ModelState.AddModelError("IsActivated", "An active SoldeConges already exists.");
                    return View(updatedSituation);
                }

                // Check if the situation being edited was previously active
                var wasActive = situation.IsActivated;

                situation.Nom = updatedSituation.Nom;
                situation.Nombre = updatedSituation.Nombre;
                situation.IsActivated = updatedSituation.IsActivated;

                // If the situation was previously active and you're deactivating it, check for another active SoldeConges
                if (wasActive && !situation.IsActivated)
                {
                    var remainingActivatedSoldeConges = ssc.GetMany().FirstOrDefault(s => s.IsActivated);
                    if (remainingActivatedSoldeConges != null)
                    {
                        // There's still an active SoldeConges, so you can't deactivate this one
                        ModelState.AddModelError("IsActivated", "An active SoldeConges already exists.");
                        return View(updatedSituation);
                    }
                }

                ssc.Update(situation);
                ssc.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        // GET: SoldeCongesController/Delete/5
        public ActionResult Delete(int id)
        {
            var situation = ssc.GetById(id);
            if (situation == null)
            {
                return RedirectToAction("Index");
            }

            return View(situation);
        }

        // POST: SoldeCongesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var situation = ssc.GetById(id);
                if (situation == null)
                {
                    return RedirectToAction("Index");
                }

                ssc.Delete(situation);
                ssc.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
