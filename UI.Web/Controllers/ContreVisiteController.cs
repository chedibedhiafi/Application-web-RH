
using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace UI.Web.Controllers
{
    public class ContreVisiteController : Controller
    {
        IServiceContreVisite cv;
        IServiceEmployees se;
        IServiceConges sc;
        public ContreVisiteController(IServiceConges sc, IServiceContreVisite cv, IServiceEmployees se)
        {
            this.sc = sc;
            this.cv = cv;
            this.se = se;
        }

        // GET: ContreVisite
        [Authorize(Policy = "EmployeeRead")]

        public ActionResult Index()
        {
            var contreVisites = cv.GetMany().ToList();

            // Fetch the Conge information for each ContreVisite and format it
            var formattedConges = contreVisites.Select(cv =>
            {
                var conge = sc.GetById(cv.CongesFk);
                return $"{conge.Employees.Nom} {conge.Employees.Prenom} - {conge.DateDebut:dd/MM/yyyy} - {conge.DateFin:dd/MM/yyyy}";
            }).ToList();

            ViewBag.FormattedConges = formattedConges;

            return View(contreVisites);
        }

        public ActionResult MesContreVisites(DateTime? startDate, DateTime? endDate)
        {

            string mail = User.Identity.Name;
            Employees employee = se.GetMany().FirstOrDefault(e => e.Email == mail);
            var contreVisites = cv.GetMany().Where(cv => cv.EffectuerPar == employee.id).ToList();
            // Filter by selected type if specified
            /*if (startDate.HasValue && endDate.HasValue)
            {
                conges = conges.Where(m => m.DateDebut >= startDate.Value && m.DateFin <= endDate.Value);
            }
            */
            return View(contreVisites);
        }


        // GET: ContreVisite/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContreVisite/Create
       
        [Authorize(Policy = "EmployeeCreate")]
        public ActionResult Create()
        {
            var LesConges = sc.GetMany()
       .Where(c => c.TypeConges.NeedsCv == true && c.TypeConges.Designation=="maladie")
       .Select(c => new
       {
           CongesId = c.CongesId,
           DisplayText = $"{c.Employees.Nom} {c.Employees.Prenom} - {c.DateDebut:dd/MM/yyyy} - {c.DateFin:dd/MM/yyyy}"
       });
            ViewBag.EffectuerPar = new SelectList(se.GetMany(), "id", "Nom");
            ViewBag.Conge = new SelectList(LesConges, "CongesId", "DisplayText");
            return View();
        }


        // POST: ContreVisite/Create
        // POST: ContreVisite/Create
        // POST: ContreVisite/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContreVisite collection)
        {
            try
            {
                collection.Etat = true;
                collection.DoneOrNot = false;
                cv.Add(collection);
                cv.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }




        // GET: ContreVisite/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var contreVisite = cv.GetById(id);
            if (contreVisite == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Fetch available Conges and build SelectList
            var availableConges = sc.GetMany().Where(c => c.TypeConges.NeedsCv == true);
            var congesSelectList = new SelectList(availableConges, "CongesId", "DisplayAttributes");

            // Load the selected Conges for the current ContreVisite
            var selectedConges = sc.GetById(contreVisite.CongesFk);

            // Pass data to the view
            ViewBag.Conges = congesSelectList;
            ViewBag.SelectedConges = selectedConges;

            ViewBag.EffectuerPar = new SelectList(se.GetMany(), "id", "Nom", contreVisite.EffectuerPar);

            return View(contreVisite);
        }



        // POST: ContreVisite/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ContreVisite updatedContreVisite)
        {
            try
            {
                var contreVisite = cv.GetById(id);

                if (contreVisite == null)
                {
                    return RedirectToAction("Index");
                }

                // Update ContreVisite properties
                contreVisite.Date = updatedContreVisite.Date;
                contreVisite.EffectuerPar = updatedContreVisite.EffectuerPar;
                contreVisite.Avis = updatedContreVisite.Avis;
                contreVisite.Details= updatedContreVisite.Details;
                contreVisite.NomDoc = updatedContreVisite.NomDoc;
                contreVisite.DoneOrNot = updatedContreVisite.DoneOrNot;
                // Update CongesFk with the selected value from the form
                contreVisite.CongesFk = updatedContreVisite.CongesFk;

                cv.Update(contreVisite);
                cv.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: ContreVisite/Delete/5
        [Authorize(Policy = "EmployeeDelete")]
        public ActionResult Delete(int id)
        {
            var contreVisite = cv.GetById(id);
            if (contreVisite == null)
            {
                return RedirectToAction("Index");
            }


            return View(contreVisite);
        }

        // POST: ContreVisite/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var contreVisite = cv.GetById(id);
                if (contreVisite == null)
                {
                    return RedirectToAction("Index");
                }

                cv.Delete(contreVisite);
                cv.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
