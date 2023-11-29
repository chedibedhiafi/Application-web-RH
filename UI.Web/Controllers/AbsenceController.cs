using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace UI.Web.Controllers
{
   

    public class AbsenceController : Controller
    {
        IServiecAbsence sa;
        IServiceTypeJustificatif stj;
        IServiceEmployees se;

        public AbsenceController(IServiecAbsence sa, IServiceTypeJustificatif stj, IServiceEmployees se)
        {

            this.sa = sa;
            this.stj = stj;
            this.se = se;
        }
        [Authorize(Policy = "EmployeeRead")]
        public ActionResult Index(int? nbabsence)
        {
            if(!nbabsence.HasValue)
            return View(sa.GetMany());
            else
                return View(sa.GetMany(c => c.nbAbsence == nbabsence.Value));
        }

        // GET: AbsenceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AbsenceController/Create
        [Authorize(Policy = "EmployeeCreate")]

        public ActionResult Create()
        {
            ViewBag.TypeJustificatifFk = new SelectList(stj.GetMany(), "TypeJustificatifId", "Document");
            ViewBag.EmplyeesFk = new SelectList(se.GetMany(), "id", "Nom");
            return View();
        }

        // POST: AbsenceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Absence collection)
        {
            try
            {

                sa.Add(collection);
                sa.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AbsenceController/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var absence = sa.GetById(id);
            if (absence == null)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TypeJustificatifFk = new SelectList(stj.GetMany(), "TypeJustificatifId", "Document", absence.TypeJustificatifFk);
            ViewBag.EmplyeesFk = new SelectList(se.GetMany(), "id", "Nom",absence.EmplyeesFk);


            return View(absence);
        }

        // POST: AbsenceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Absence updatedabsence)
        {
            try
            {
                var absence = sa.GetById(id);

                if (absence == null)
                {
                    return RedirectToAction("Index");
                }

                absence.nbAbsence = updatedabsence.nbAbsence;
                absence.TypeJustificatifFk = updatedabsence.TypeJustificatifFk;
                absence.EmplyeesFk = updatedabsence.EmplyeesFk;


                sa.Update(absence);
                sa.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Policy = "EmployeeDelete")]


        // GET: AbsenceController/Delete/5
        [Authorize(Roles = "Administrateur")]
        public ActionResult Delete(int id)
        {
            var absence = sa.GetById(id);
            if (absence == null)
            {
                return RedirectToAction("Index");
            }


            return View(absence);
        }

        // POST: AbsenceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var absence = sa.GetById(id);
                if (absence == null)
                {
                    return RedirectToAction("Index");
                }

                sa.Delete(absence);
                sa.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
