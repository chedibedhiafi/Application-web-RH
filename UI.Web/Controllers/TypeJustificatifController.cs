using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Runtime.Intrinsics.X86;

namespace UI.Web.Controllers
{
    public class TypeJustificatifController : Controller
    {
        IServiceTypeJustificatif stj;
        public TypeJustificatifController(IServiceTypeJustificatif stj)
        {

            
            this.stj = stj;
          
        }
        // GET: TypeJustificatifController
        [Authorize(Policy = "EmployeeRead")]

        public ActionResult Index()
        {
            return View(stj.GetMany());
        }

        // GET: TypeJustificatifController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TypeJustificatifController/Create
        [Authorize(Policy = "EmployeeCreate")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeJustificatifController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TypeJustificatif collection, IFormFile DocumentUpload)
        {
            try
            {
                if (DocumentUpload != null && DocumentUpload.Length > 0)
                {
                    var fileName = Path.GetFileName(DocumentUpload.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadsTypeJustificatif", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        DocumentUpload.CopyTo(stream);
                    }

                    collection.Document = fileName;
                }




                stj.Add(collection);
                stj.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TypeJustificatifController/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var typeJustificatif = stj.GetById(id);

            if (typeJustificatif == null)
            {
                return RedirectToAction("Index");
            }
         

            return View(typeJustificatif);
        }

        // POST: TypeJustificatifController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TypeJustificatif updatedTypeJustificatif, IFormFile DocumentUpload)
        {
            try
            {
                var typeJustificatif = stj.GetById(id);

                if (typeJustificatif == null)
                {
                    return RedirectToAction("Index");
                }

                // Update the properties of the existing employee with the edited values
                typeJustificatif.type = updatedTypeJustificatif.type;

             
               

                // Handle the photo update only if a new photo is provided
                if (DocumentUpload != null && DocumentUpload.Length > 0)
                {
                    var fileName = Path.GetFileName(DocumentUpload.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadsTypeJustificatif", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        DocumentUpload.CopyTo(stream);
                    }

                    // Assign the new file name to the Photo property
                    typeJustificatif.Document = fileName;
                }

                stj.Update(typeJustificatif);
                stj.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TypeJustificatifController/Delete/5
        [Authorize(Policy = "EmployeeDelete")]
        public ActionResult Delete(int id)
        {
            var typeJustificatif = stj.GetById(id);

            if (typeJustificatif == null)
            {
                return RedirectToAction("Index");
            }

            return View(typeJustificatif);
        }

        // POST: TypeJustificatifController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var typejustificatif = stj.GetById(id);
                if (typejustificatif == null)
                {
                    return RedirectToAction("Index");
                }

                stj.Delete(typejustificatif);
                stj.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
