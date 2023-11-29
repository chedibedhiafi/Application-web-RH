using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace UI.Web.Controllers
{
    public class GestionDocumentController : Controller
    {
        IServiceGestionDocument sgd;
        IServiceAttestation sa;
        IServiceTypeJustificatif stj;

        public GestionDocumentController(IServiceGestionDocument sgd, IServiceAttestation sa, IServiceTypeJustificatif stj)
        {

            this.sgd = sgd;
            this.sa = sa;
            this.stj = stj;
        }
        // GET: GestionDocumentController
        [Authorize(Policy = "EmployeeRead")]

        public ActionResult Index()
        {
            return View(sgd.GetMany());
        }

        // GET: GestionDocumentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GestionDocumentController/Create
        [Authorize(Policy = "EmployeeCreate")]

        public ActionResult Create()
        {
            ViewBag.TypeJustificatifFk = new SelectList(stj.GetMany(), "TypeJustificatifId", "Document");
            ViewBag.AttestationFk = new SelectList(sa.GetMany(), "AttestationId", "DocumentAttestation");
            return View();
        }

        // POST: GestionDocumentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GestionDocument collection)
        {
            try
            {

                sgd.Add(collection);
                sgd.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GestionDocumentController/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var gestiondocument = sgd.GetById(id);
            if (gestiondocument == null)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TypeJustificatifFk = new SelectList(stj.GetMany(), "TypeJustificatifId", "Document", gestiondocument.TypeJustificatifFk);
            ViewBag.AttestationFk = new SelectList(sa.GetMany(), "AttestationId", "DocumentAttestation",gestiondocument.AttestationFk);


            return View(gestiondocument);
        }

        // POST: GestionDocumentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, GestionDocument updatedGestionDocument)
        {
            try
            {
                var gestionDocument = sgd.GetById(id);

                if (gestionDocument == null)
                {
                    return RedirectToAction("Index");
                }

                gestionDocument.AttestationFk = updatedGestionDocument.AttestationFk;
                gestionDocument.TypeJustificatifFk = updatedGestionDocument.TypeJustificatifFk;
              


                sgd.Update(gestionDocument);
                sgd.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: GestionDocumentController/Delete/5
        [Authorize(Policy = "EmployeeDelete")]
        public ActionResult Delete(int id)
        {
            var gestionDocument = sgd.GetById(id);
            if (gestionDocument == null)
            {
                return RedirectToAction("Index");
            }


            return View(gestionDocument);
        }

        // POST: GestionDocumentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var gestiondocument = sgd.GetById(id);
                if (gestiondocument == null)
                {
                    return RedirectToAction("Index");
                }

                sgd.Delete(gestiondocument);
                sgd.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
