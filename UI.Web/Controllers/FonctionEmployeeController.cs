using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace UI.Web.Controllers
{
    public class FonctionEmployeeController : Controller
    {
        IServiceFonctionEmployee sfe;
        public FonctionEmployeeController(IServiceFonctionEmployee sfe)
        {

            this.sfe = sfe;
           
        }
        // GET: FonctionEmployeeController
        [Authorize(Policy = "EmployeeRead")]

        public ActionResult Index()
        {
            return View(sfe.GetMany());
        }

        // GET: FonctionEmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FonctionEmployeeController/Create
        [Authorize(Policy = "EmployeeCreate")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: FonctionEmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FonctionEmployee collection)
        {
            try
            {
                sfe.Add(collection);
                sfe.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FonctionEmployeeController/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var fonctionEmployee = sfe.GetById(id);
            if (fonctionEmployee == null)
            {
                return RedirectToAction(nameof(Index));
            }



            return View(fonctionEmployee);
        }

        // POST: FonctionEmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FonctionEmployee updatedFonctionEmployee)
        {
            try
            {
                var fonctionEmployee = sfe.GetById(id);

                if (fonctionEmployee == null)
                {
                    return RedirectToAction("Index");
                }

                fonctionEmployee.Fonction = updatedFonctionEmployee.Fonction;


                sfe.Update(fonctionEmployee);
                sfe.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FonctionEmployeeController/Delete/5
        [Authorize(Policy = "EmployeeDelete")]
        public ActionResult Delete(int id)
        {
            var fonctionEmployee = sfe.GetById(id);
            if (fonctionEmployee == null)
            {
                return RedirectToAction("Index");
            }

            return View(fonctionEmployee);
        }

        // POST: FonctionEmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var fonctionEmployee = sfe.GetById(id);
                if (fonctionEmployee == null)
                {
                    return RedirectToAction("Index");
                }

                sfe.Delete(fonctionEmployee);
                sfe.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
