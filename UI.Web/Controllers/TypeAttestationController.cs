using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Web.Controllers
{
    public class TypeAttestationController : Controller
       
    {
        IServiceTypeAttestation sta;
        public TypeAttestationController(IServiceTypeAttestation sta)
        {
            this.sta = sta;

        }
        // GET: TypeAttestationController

        [Authorize(Policy = "EmployeeRead")]
        public ActionResult Index()
        {
            return View(sta.GetMany());
        }

        // GET: TypeAttestationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TypeAttestationController/Create
        [Authorize(Policy = "EmployeeCreate")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: TypeAttestationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TypeAttestation collection)
        {
            try
            {
                sta.Add(collection);
                sta.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TypeAttestationController/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var typeAttestation = sta.GetById(id);

            if (typeAttestation == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(typeAttestation);
        }


        // POST: TypeAttestationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TypeAttestation updatedCollection)
        {
            try
            {
                var typeconfirmation = sta.GetById(id);

                if (typeconfirmation == null)
                {
                    return RedirectToAction("Index");
                }

                typeconfirmation.Type = updatedCollection.Type;
                typeconfirmation.Contenu=updatedCollection.Contenu;


                sta.Update(typeconfirmation);
                sta.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Policy = "EmployeeDelete")]
        // GET: TypeAttestationController/Delete/5
        public ActionResult Delete(int id)
        {
            var typeconfirmation = sta.GetById(id);
            if (typeconfirmation == null)
            {
                return RedirectToAction("Index");
            }

            return View(typeconfirmation);
        }

        // POST: TypeAttestationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var typeconfirmation = sta.GetById(id);
                if (typeconfirmation == null)
                {
                    return RedirectToAction("Index");
                }

                sta.Delete(typeconfirmation);
                sta.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
