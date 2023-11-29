using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace UI.Web.Controllers
{
    public class SectionController : Controller
    {
        IServiceSectioncs ss;
        public SectionController(IServiceSectioncs ss)
        {

            this.ss = ss;
        }
        // GET: SectionController
        [Authorize(Policy = "EmployeeRead")]

        public ActionResult Index()
        {
            return View(ss.GetMany());
        }

        // GET: SectionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SectionController/Create
        [Authorize(Policy = "EmployeeCreate")]
        public ActionResult Create()
        {
            ViewBag.Departments = new SelectList(ss.GetMany(), "SectionId", "NomSection");
            return View();
        }

        // POST: SectionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Section collection)
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

        // GET: SectionController/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var section = ss.GetById(id);
            if (section == null)
            {
                return RedirectToAction(nameof(Index));
            }


            ViewBag.Departments = new SelectList(ss.GetMany(), "SectionId", "NomSection");
            return View(section);
        }

        // POST: SectionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Section updatedSection)
        {
            try
            {
                var section = ss.GetById(id);

                if (section == null)
                {
                    return RedirectToAction("Index");
                }

                section.NomSection = updatedSection.NomSection;
                section.ParentSectionId = updatedSection.ParentSectionId; // Update the ParentSectionId

                ss.Update(section);
                ss.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: SectionController/Delete/5
        [Authorize(Policy = "EmployeeDelete")]
        public ActionResult Delete(int id)
        {
            var section = ss.GetById(id);
            if (section == null)
            {
                return RedirectToAction("Index");
            }

            return View(section);
        }

        // POST: SectionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var section = ss.GetById(id);
                if (section == null)
                {
                    return RedirectToAction("Index");
                }

                ss.Delete(section);
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
