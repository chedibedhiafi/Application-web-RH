using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace UI.Web.Controllers
{
    public class TypeConfirmationController : Controller
    {
        IServiceTypeConfirmation st;
        public TypeConfirmationController(IServiceTypeConfirmation st)
        {
            this.st = st;
            
        }
        // GET: TypeConfirmationController
        [Authorize(Policy = "EmployeeRead")]

        public ActionResult Index()
        {
            return View(st.GetMany());
        }

        // GET: TypeConfirmationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TypeConfirmationController/Create
        [Authorize(Policy = "EmployeeCreate")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeConfirmationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TypeConfirmation collection)
        {
            try
            {
                st.Add(collection);
                st.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TypeConfirmationController/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var typeconfirmation = st.GetById(id);
            if (typeconfirmation == null)
            {
                return RedirectToAction(nameof(Index));
            }



            return View(typeconfirmation);
        }

        // POST: TypeConfirmationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TypeConfirmation updatedTypeConfirmation)
        {
            try
            {
                var typeconfirmation = st.GetById(id);

                if (typeconfirmation == null)
                {
                    return RedirectToAction("Index");
                }

                typeconfirmation.type = updatedTypeConfirmation.type;


                st.Update(typeconfirmation);
                st.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TypeConfirmationController/Delete/5
        [Authorize(Policy = "EmployeeDelete")]
        public ActionResult Delete(int id)
        {
            var typeconfirmation = st.GetById(id);
            if (typeconfirmation == null)
            {
                return RedirectToAction("Index");
            }

            return View(typeconfirmation);
        }

        // POST: TypeConfirmationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var typeconfirmation = st.GetById(id);
                if (typeconfirmation == null)
                {
                    return RedirectToAction("Index");
                }

                st.Delete(typeconfirmation);
                st.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
