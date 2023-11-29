using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Cryptography;

namespace UI.Web.Controllers
{
    public class TypeCongesController : Controller
    {
        IServiceTypesConges scc;
        IServiceEmployees employeeService;
        IServiceContreVisite contreVisiteService;

        public TypeCongesController(IServiceTypesConges scc, IServiceEmployees employeeService,IServiceContreVisite contreVisiteService)
        {

            this.scc = scc;
            this.employeeService = employeeService;
            this.contreVisiteService = contreVisiteService;



        }
        // GET: TypeCongesController
        [Authorize(Policy = "EmployeeRead")]

        public ActionResult Index()
        {

            return View(scc.GetMany());
        }

        // GET: TypeCongesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TypeCongesController/Create
        [Authorize(Policy = "EmployeeCreate")]
        public ActionResult Create()
        {
             ViewBag.EmployeeList = new SelectList(employeeService.GetMany(), "id", "nom");
            ViewBag.ContreVisiteList = new SelectList(contreVisiteService.GetMany(), "ContreVisiteId", "Date"); // Use appropriate properties for SelectList
            return View();
        }

        // POST: TypeCongesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TypeConges collection)
        {
            try
            {
                scc.Add(collection);
                scc.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        // GET: TypeCongesController/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var typeconges = scc.GetById(id);
            if (typeconges == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ContreVisiteList = new SelectList(contreVisiteService.GetMany(), "ContreVisiteId", "Date"); 

            return View(typeconges);
        }

        // POST: TypeCongesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TypeConges updatedTypeConges)
        {
            try
            {
                var typeconges = scc.GetById(id);

                if (typeconges == null)
                {
                    return RedirectToAction(nameof(Index));
                }


                typeconges.nbMaximumdeJour = updatedTypeConges.nbMaximumdeJour;
                typeconges.Designation = updatedTypeConges.Designation;
                typeconges.NeedsCv = updatedTypeConges.NeedsCv;
               

                scc.Update(typeconges);
                scc.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        // GET: TypeCongesController/Delete/5
        [Authorize(Policy = "EmployeeDelete")]
        public ActionResult Delete(int id)
        {
            var typeconges = scc.GetById(id);
            if (typeconges == null)
            {
                return RedirectToAction("Index");
            }


            return View(typeconges);
        }

        // POST: TypeCongesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var typeconges = scc.GetById(id);
                if (typeconges == null)
                {
                    return RedirectToAction("Index");
                }

                scc.Delete(typeconges);
                scc.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
