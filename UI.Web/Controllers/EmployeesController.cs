using ApplicationCore.Domain;
using ApplicationCore.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Web.Controllers
{
    
    public class EmployeesController : Controller
    {
        IServiceEmployees se;
        IServiceSectioncs sc;
        IServiceSituation ss;
    
        IServiecAbsence sa;
       
        IServiceGenres sg;
        IServiceFonctionEmployee sfe;
     
        public EmployeesController(IServiceEmployees se,IServiceSectioncs sc, IServiceSituation ss,  IServiecAbsence sa, IServiceGenres sg, IServiceFonctionEmployee sfe)
        {
            this.se = se;
            this.sc = sc;
            this.ss = ss;
           
            this.sa = sa;
           
            this.sg = sg;
            this.sfe = sfe;
          
        }

        // GET: EmployeesController
        [Authorize(Policy = "EmployeeRead")]
      

        public ActionResult Index()
        {
            
                return View(se.GetMany());
              
           
        }

        // GET: EmployeesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeesController/Create
        [Authorize(Policy = "EmployeeCreate")]
       


        public ActionResult Create()
        {
            
            ViewBag.SectionFk = new SelectList(sc.GetMany(), "SectionId", "NomSection");
            ViewBag.SituationFk = new SelectList(ss.GetMany(), "SituationId", "nomSituation");     
            ViewBag.GenreFk = new SelectList(sg.GetMany(), "GenreId", "NomGenre");
            ViewBag.FonctionEmployeeFk = new SelectList(sfe.GetMany(), "FonctionEmployeeId", "Fonction");
          
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(Employees collection, IFormFile PhotoImg)
        {
            try
            {
                if (PhotoImg != null && PhotoImg.Length > 0)
                {
                    var fileName = Path.GetFileName(PhotoImg.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        PhotoImg.CopyTo(stream);
                    }

                    collection.Photo = fileName;
                }
                var employeeFunction = sfe.GetById(collection.FonctionEmployeeFk);

                // Check if the employee's function is "Responsable"
                if (employeeFunction.Fonction == "Responsable")
                {
                    // Check if there is already a responsible employee in the same department
                    var existingResponsibleEmployee = se.GetMany()
                        .FirstOrDefault(e => e.SectionFk == collection.SectionFk && e.FonctionEmployee.Fonction == "Responsable");

                    if (existingResponsibleEmployee != null)
                    {
                        ModelState.AddModelError("FonctionEmployeeFk", "Il y a déjà un employé responsable dans ce département.");
                   
                        ViewBag.SectionFk = new SelectList(sc.GetMany(), "SectionId", "NomSection");
                        ViewBag.SituationFk = new SelectList(ss.GetMany(), "SituationId", "nomSituation");
                        ViewBag.GenreFk = new SelectList(sg.GetMany(), "GenreId", "NomGenre");
                        ViewBag.FonctionEmployeeFk = new SelectList(sfe.GetMany(), "FonctionEmployeeId", "Fonction");
                        // Repopulate dropdowns and return the view with an error message
                        // ...
                        return View();
                    }
                }

                // Check if an employee with the same Cin already exists
                var existingEmployeeWithCin = se.GetMany().FirstOrDefault(e => e.Cin == collection.Cin);
                if (existingEmployeeWithCin != null)
                {
                    ModelState.AddModelError("Cin", "Un utilisateur avec ce Cin existe déjà");
                    ViewBag.SectionFk = new SelectList(sc.GetMany(), "SectionId", "NomSection");
                    ViewBag.SituationFk = new SelectList(ss.GetMany(), "SituationId", "nomSituation");
                    ViewBag.GenreFk = new SelectList(sg.GetMany(), "GenreId", "NomGenre");
                    ViewBag.FonctionEmployeeFk = new SelectList(sfe.GetMany(), "FonctionEmployeeId", "Fonction");
                    return View();
                }

                // Check if an employee with the same Matricule already exists
                var existingEmployeeWithMatricule = se.GetMany().FirstOrDefault(e => e.Matricule == collection.Matricule);
                if (existingEmployeeWithMatricule != null)
                {
                    ModelState.AddModelError("Matricule", "Un utilisateur avec ce Matricule existe déjà.");
                    ViewBag.SectionFk = new SelectList(sc.GetMany(), "SectionId", "NomSection");
                    ViewBag.SituationFk = new SelectList(ss.GetMany(), "SituationId", "nomSituation");
                    ViewBag.GenreFk = new SelectList(sg.GetMany(), "GenreId", "NomGenre");
                    ViewBag.FonctionEmployeeFk = new SelectList(sfe.GetMany(), "FonctionEmployeeId", "Fonction");
                    return View();
                }

                se.Add(collection);
                se.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        // GET: EmployeesController/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var employee = se.GetById(id);

            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.SectionFk = new SelectList(sc.GetMany(), "SectionId", "NomSection", employee.SectionFk);
            ViewBag.SituationFk = new SelectList(ss.GetMany(), "SituationId", "nomSituation", employee.SituationFk);
          
            ViewBag.GenreFk = new SelectList(sg.GetMany(), "GenreId", "NomGenre",employee.GenreFk);
            ViewBag.FonctionEmployeeFk = new SelectList(sfe.GetMany(), "FonctionEmployeeId", "Fonction",employee.FonctionEmployeeFk);
          

            return View(employee);
        }

        // POST: EmployeesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Employees updatedEmployee, IFormFile PhotoImg)
        {
            try
            {
                var employee = se.GetById(id);

                if (employee == null)
                {
                    return RedirectToAction("Index");
                }

                var existingEmployeeWithSameMatricule = se.GetMany()
                 .FirstOrDefault(e => e.Matricule == updatedEmployee.Matricule && e.id != id);

                // Check if another employee has the same Cin
                var existingEmployeeWithSameCin = se.GetMany()
                    .FirstOrDefault(e => e.Cin == updatedEmployee.Cin && e.id != id);

                if (existingEmployeeWithSameMatricule != null)
                {
                    ModelState.AddModelError("Matricule", "Un autre employé a déjà le même matricule.");
                }

                if (existingEmployeeWithSameCin != null)
                {
                    ModelState.AddModelError("Cin", "Un autre employé a déjà le même numéro de CIN.");
                }

                if (existingEmployeeWithSameMatricule != null || existingEmployeeWithSameCin != null)
                {
                    ViewBag.SectionFk = new SelectList(sc.GetMany(), "SectionId", "NomSection", employee.SectionFk);
                    ViewBag.SituationFk = new SelectList(ss.GetMany(), "SituationId", "nomSituation", employee.SituationFk);
                    ViewBag.GenreFk = new SelectList(sg.GetMany(), "GenreId", "NomGenre", employee.GenreFk);
                    ViewBag.FonctionEmployeeFk = new SelectList(sfe.GetMany(), "FonctionEmployeeId", "Fonction", employee.FonctionEmployeeFk);
                  
                    return View(employee);
                }

                // Update the properties of the existing employee with the edited values


                employee.Nom = updatedEmployee.Nom;
                employee.Prenom = updatedEmployee.Prenom;
                employee.DateDeNaissance = updatedEmployee.DateDeNaissance;
                employee.Cin = updatedEmployee.Cin;
                employee.SituationFk = updatedEmployee.SituationFk;
              
                employee.GenreFk = updatedEmployee.GenreFk;
                employee.FonctionEmployeeFk = updatedEmployee.FonctionEmployeeFk;
            
                employee.NomArabe = updatedEmployee.NomArabe;
                employee.PrenomArabe = updatedEmployee.PrenomArabe;
                employee.CreditConges = updatedEmployee.CreditConges;
                employee.Matricule = updatedEmployee.Matricule;
                employee.SectionFk = updatedEmployee.SectionFk;
                employee.StartedAt = updatedEmployee.StartedAt;
                employee.Email = updatedEmployee.Email;

                // Handle the photo update only if a new photo is provided
                if (PhotoImg != null && PhotoImg.Length > 0)
                {
                    var fileName = Path.GetFileName(PhotoImg.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        PhotoImg.CopyTo(stream);
                    }

                    // Assign the new file name to the Photo property
                    employee.Photo = fileName;
                }
                var employeeFunction = sfe.GetById(employee.FonctionEmployeeFk);

                // Check if the employee's function is "Responsable"
                if (employeeFunction.Fonction == "Responsable")
                {
                    // Check if there is already a responsible employee in the same department
                    var existingResponsibleEmployee = se.GetMany()
                        .FirstOrDefault(e => e.SectionFk == employee.SectionFk && e.FonctionEmployee.Fonction == "Responsable");

                    if (existingResponsibleEmployee != null)
                    {
                        ModelState.AddModelError("FonctionEmployeeFk", "Il y a déjà un employé responsable dans ce département.");

                        ViewBag.SectionFk = new SelectList(sc.GetMany(), "SectionId", "NomSection", employee.SectionFk);
                        ViewBag.SituationFk = new SelectList(ss.GetMany(), "SituationId", "nomSituation", employee.SituationFk);
                        ViewBag.GenreFk = new SelectList(sg.GetMany(), "GenreId", "NomGenre", employee.GenreFk);
                        ViewBag.FonctionEmployeeFk = new SelectList(sfe.GetMany(), "FonctionEmployeeId", "Fonction", employee.FonctionEmployeeFk);

                        // Return the view with the model so that user input is preserved
                        return View(employee);
                    }

                }


                se.Update(employee);
                se.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeesController/Delete/5
        [Authorize(Policy = "EmployeeDelete")]
      
        public ActionResult Delete(int id)
        {
            var employee = se.GetById(id);

            if (employee == null)
            {
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // POST: EmployeesController/Delete/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var employee = se.GetById(id);
                if (employee == null)
                {
                    return RedirectToAction("Index");
                }

                se.Delete(employee);
                se.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
