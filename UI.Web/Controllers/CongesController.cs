using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Data;
using System.Linq;
using System.Reflection;


namespace UI.Web.Controllers
{
    
    public class CongesController : Controller
    {
        IServiceEmployees se;
        IServiceConges sc;
        IServiceTypesConges scc;
        IServiceTypeConfirmation stc;
        IServiceSectioncs ss;

        public CongesController(IServiceSectioncs ss,IServiceEmployees se, IServiceConges sc, IServiceTypesConges scc, IServiceTypeConfirmation stc)
        {
            this.se = se;
            this.sc = sc;
            this.scc = scc;
            this.stc = stc;
            this.ss = ss;
        }

        // GET: CongesController
        [Authorize(Policy = "EmployeeRead")]

        public ActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            var conges = sc.GetMany();

            // Filter by selected type if specified
            if (startDate.HasValue && endDate.HasValue)
            {
                conges = conges.Where(m => m.DateDebut >= startDate.Value && m.DateFin <= endDate.Value);
            }

            return View(conges);
        }

        public ActionResult MesConges(DateTime? startDate, DateTime? endDate)
        {
            
            string mail = User.Identity.Name;
            Employees employee = se.GetMany().FirstOrDefault(e => e.Email == mail);
            var conges = sc.GetMany().Where(c => c.IdEmployees == employee.id).ToList();
            // Filter by selected type if specified
            /*if (startDate.HasValue && endDate.HasValue)
            {
                conges = conges.Where(m => m.DateDebut >= startDate.Value && m.DateFin <= endDate.Value);
            }
            */
            return View(conges);
        }
        public ActionResult PendingConges(DateTime? startDate, DateTime? endDate)
        {
            string mail = User.Identity.Name;
            Employees employee = se.GetMany().FirstOrDefault(e => e.Email == mail);

            if (employee == null)
            {
                return RedirectToAction("NotFoundAction");
            }
            var deps = GetSubDepartments(employee.SectionFk);
            var emps = se.GetMany().Where(e => deps.Contains(e.SectionFk)).ToList();
            var query = sc.GetMany().Where(c => emps.Select(e => e.id).Contains(c.IdEmployees));
            if (startDate != null && endDate != null)
            {
                query = query.Where(c => c.DateDebut >= startDate && c.DateFin <= endDate);
            }

            var conges = query.ToList();
            return View(conges);
        }

        private List<int> GetSubDepartments(int departmentId)
        {
            var subDepartments = ss.GetMany().Where(d => d.ParentSectionId == departmentId).ToList();
            var result = new List<int> { departmentId };
            foreach (var subDept in subDepartments)
            {
                result.AddRange(GetSubDepartments(subDept.SectionId));
            }

            return result;
        





        /*
         string mail = User.Identity.Name;
         Employees employee = se.GetMany().FirstOrDefault(e => e.Email == mail);
         var deps = ss.GetMany().Where(
             d => d==employee.Section &&
             d.ParentSection == employee.Section &&
             d.ParentSection.ParentSection==employee.Section &&
             d.ParentSection.ParentSection.ParentSection== employee.Section &&
             d.ParentSection.ParentSection.ParentSection.ParentSection == employee.Section
         );
         var emps = se.GetMany().Where(e=>deps.Contains(e.Section));
         var conges = sc.GetMany().Where(c => emps.Contains(c.Employees)).ToList();

         return View(conges);*/
    }

        public IActionResult DownloadPdf(int id)
        {
            // Retrieve the Conges based on the provided ID
            var conges = sc.GetById(id);
            if (conges == null || conges.TypeConfirmation.type != "Confirmed")
            {
                return RedirectToAction(nameof(Index));
            }

            // Generate the PDF content
            byte[] pdfBytes = GeneratePdfContent(conges);

            // Generate the filename (customize as needed)
            string filename = $"Conges{id}.pdf";

            // Return the PDF file for download
            return File(pdfBytes, "application/pdf", filename);
        }
        private byte[] GeneratePdfContent(Conges conges)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Arial", 12, XFontStyle.Bold); // Smaller title font
                XFont regularFont = new XFont("Arial", 10); // Smaller regular font

                XRect tableRect = new XRect(10, 30, page.Width - 20, page.Height - 60);

                gfx.DrawString("Conges Details", titleFont, XBrushes.DarkBlue, tableRect.Left, tableRect.Top - 20);

                string[] headers = { "Conges ID", "Start Date", "End Date", "Type", "Raison", "Employee", "Confirmed By" }; // Smaller headers

                double xPosition = tableRect.Left;
                double yPosition = tableRect.Top;
                double cellHeight = 20;
                double tableWidth = tableRect.Width;

                double[] columnWidths = new double[headers.Length];

                for (int i = 0; i < headers.Length; i++)
                {
                    if (conges != null)
                    {
                        // Calculate the maximum width required for the column based on the header and data
                        double headerWidth = gfx.MeasureString(headers[i], regularFont).Width;

                        // Get the data for the current column with a null check and provide a default value if it's null
                        string columnData = GetColumnData(i, conges) ?? "N/A";

                        double dataWidth = gfx.MeasureString(columnData, regularFont).Width;
                        columnWidths[i] = Math.Max(headerWidth, dataWidth) + 10; // Add some padding
                    }
                }

                for (int i = 0; i < headers.Length; i++)
                {
                    gfx.DrawRectangle(XPens.Black, xPosition, yPosition, columnWidths[i], cellHeight);
                    gfx.DrawString(headers[i], regularFont, XBrushes.Black, new XRect(xPosition, yPosition, columnWidths[i], cellHeight), XStringFormats.Center);
                    xPosition += columnWidths[i];
                }

                xPosition = tableRect.Left;
                yPosition += cellHeight;
                for (int i = 0; i < headers.Length; i++)
                {
                    gfx.DrawRectangle(XPens.Black, xPosition, yPosition, columnWidths[i], cellHeight);

                    // Get the data for the current column with a null check and provide a default value if it's null
                    string columnData = GetColumnData(i, conges) ?? "N/A";

                    gfx.DrawString(columnData, regularFont, XBrushes.Black, new XRect(xPosition, yPosition, columnWidths[i], cellHeight), XStringFormats.Center);
                    xPosition += columnWidths[i];
                }

                document.Save(ms);
                document.Close();

                return ms.ToArray();
            }
        }


        // Helper method to get data for a specific column
        private string GetColumnData(int columnIndex, Conges conges)
        {
            switch (columnIndex)
            {
                case 0: return conges.CongesId.ToString();
                case 1: return conges.DateDebut.ToString("MM/dd/yyyy");
                case 2: return conges.DateFin.ToString("MM/dd/yyyy");
                case 3: return conges.TypeConges.Designation;
                case 4: return conges.Raison;
                case 5: return conges.Employees?.Nom ?? string.Empty;
                case 6: return conges.ConfirmeParEmployee?.Nom ?? string.Empty;
                default: return string.Empty;
            }
        }





        // GET: CongesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateConfirmation(int id, bool confirmation)
        {
            var conges = sc.GetById(id);

            if (conges == null)
            {
                return NotFound();
            }

            // Check if confirming the current "Conges" entry would result in overlapping dates for the same employee
            if (confirmation && HasOverlappingDates(conges))
            {
                ModelState.AddModelError(string.Empty, "Impossible de confirmer cette entrée « Conges » en raison de dates qui se chevauchent avec une autre entrée « Conges » confirmée pour le même employé.");
                return BadRequest(ModelState);
            }

           
            sc.Update(conges);
            sc.Commit();

            return Ok();
        }

        private bool HasOverlappingDates(Conges conges)
        {
            var overlappingConges = sc.GetMany(c =>

                c.RemplassePar == conges.RemplassePar &&
                c.CongesId != conges.CongesId &&
                ((conges.DateDebut >= c.DateDebut && conges.DateDebut <= c.DateFin) ||
                (conges.DateFin >= c.DateDebut && conges.DateFin <= c.DateFin) ||
                (conges.DateDebut <= c.DateDebut && conges.DateFin >= c.DateFin)));

            return overlappingConges.Any();
        }

        // GET: CongesController/Create
        [Authorize(Policy = "EmployeeCreate")]
        public ActionResult Create()
        {
            
            ViewBag.IdEmployees = new SelectList(se.GetMany(), "id", "Nom");
            ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
            return View();
        }

        // POST: CongesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Conges collection)
        {
            try
            {
                collection.ConfirmePar = null;
                collection.RemplassePar = null;
               

                // Check if the TypeConfirmation is "Confirmed"
                if (collection.TypeConfirmation != null && collection.TypeConfirmation.type == "Confirmed")
                {
                    int totalDays = (int)(collection.DateFin - collection.DateDebut).TotalDays;

                    // Get the corresponding TypeConges entity
                    var typeConges = scc.GetById(collection.TypeCongesFk);

                    // Check if the TypeConges allows the duration of the leave
                    if (typeConges != null && typeConges.nbMaximumdeJour < totalDays)
                    {
                        ModelState.AddModelError("DateFin", "La durée du congé dépasse le nombre maximum de jours autorisés pour ce TypeConges.");
                        ViewBag.IdEmployees = new SelectList(se.GetMany(), "id", "Nom");
                        ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
                        return View(collection);
                    }

                    // Retrieve the employee associated with the leave request
                    var employee = se.GetById(collection.IdEmployees);

                    // Check if the employee is available for leave during the specified period
                    bool isEmployeeAvailable = IsEmployeeAvailable(collection);

                    if (!isEmployeeAvailable)
                    {
                        ModelState.AddModelError(string.Empty, "L'employé sélectionné n'est pas disponible pour remplacer quelqu'un pendant la période de congé spécifiée.");
                        ViewBag.IdEmployees = new SelectList(se.GetMany(), "id", "Nom");
                        ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
                        return View(collection);
                    }
                    if (employee.CreditConges < totalDays)
                    {
                        ModelState.AddModelError(string.Empty, "Pas assez de crédit.");
                        ViewBag.IdEmployees = new SelectList(se.GetMany(), "id", "Nom");
                        ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
                        return View(collection);
                    }

                    // Subtract the leave duration from the employee's CreditConges
                    //employee.CreditConges -= totalDays;

                    // Update the employee's CreditConges property in the database
                    //se.Update(employee);
                }

                // Check for overlapping Conges
                bool hasOverlappingConges = HasOverlappingConges(collection);

                if (hasOverlappingConges)
                {
                    ModelState.AddModelError(string.Empty, "L'employé a déjà un congé qui chevauche la période spécifiée.");
                    ViewBag.IdEmployees = new SelectList(se.GetMany(), "id", "Nom");
                    ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
                    return View(collection);
                }

                // Set the TypeConfirmation to "Pending" if not already set
                if (collection.TypeConfirmation == null)
                {
                    collection.TypeConfirmation = stc.GetMany().FirstOrDefault(tc => tc.type == "Pending");
                }

                // Save the leave request
                sc.Add(collection);
                sc.Commit();

                return RedirectToAction("MesConges");
            }
            catch
            {
                ViewBag.IdEmployees = new SelectList(se.GetMany(), "id", "Nom");
                ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
                return View(collection);
            }
        }

        /*public ActionResult Create(Conges collection)
        {
            try
            {
                collection.ConfirmePar = null;
                collection.RemplassePar = null;

                // Check if the TypeConfirmation is "Confirmed"
                if (collection.TypeConfirmation != null && collection.TypeConfirmation.type == "Confirmed")
                {
                    int totalDays = (int)(collection.DateFin - collection.DateDebut).TotalDays;

                    // Get the corresponding TypeConges entity
                    var typeConges = scc.GetById(collection.TypeCongesFk);

                    // Check if the TypeConges allows the duration of the leave
                    if (typeConges != null && typeConges.nbMaximumdeJour >= totalDays)
                    {
                        // Retrieve the employee associated with the leave request
                        var employee = se.GetById(collection.IdEmployees);

                        // Check if the employee is available for leave during the specified period
                        bool isEmployeeAvailable = IsEmployeeAvailable(collection);

                        if (!isEmployeeAvailable)
                        {
                            ModelState.AddModelError(string.Empty, "L'employé sélectionné n'est pas disponible pour remplacer quelqu'un pendant la période de congé spécifiée.");
                            ViewBag.IdEmployees = new SelectList(se.GetMany(), "id", "Nom");
                            //ViewBag.RemplassePar = new SelectList(se.GetMany(), "id", "Nom");
                            ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
                            return View(collection);
                        }

                        // Subtract the leave duration from the employee's CreditConges
                        employee.CreditConges -= totalDays;

                        // Update the employee's CreditConges property in the database
                        se.Update(employee);
                    }
                    else
                    {
                        ModelState.AddModelError("DateFin", "La durée du congé dépasse le nombre maximum de jours autorisés pour ce TypeConges.");
                        ViewBag.IdEmployees = new SelectList(se.GetMany(), "id", "Nom");
                        //ViewBag.RemplassePar = new SelectList(se.GetMany(), "id", "Nom");
                        ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
                        return View(collection);
                    }
                }
                

                // Set the TypeConfirmation to "Pending" if not already set
                if (collection.TypeConfirmation == null)
                {
                    collection.TypeConfirmation = stc.GetMany().FirstOrDefault(tc => tc.type == "Pending");
                }

                // Save the leave request
                sc.Add(collection);
                sc.Commit();

                return RedirectToAction("MesConges");
            }
            catch
            {
                ViewBag.IdEmployees = new SelectList(se.GetMany(), "id", "Nom");
                ViewBag.RemplassePar = new SelectList(se.GetMany(), "id", "Nom");
                ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
                return View(collection);
            }
        }*/


        private bool IsEmployeeAvailable(Conges newConges)
        {
            // Find leave requests for the same employee and overlapping date range
            var overlappingConges = sc.GetMany(c =>
                c.IdEmployees == newConges.IdEmployees &&
                c.CongesId != newConges.CongesId &&  // Exclude the current leave request when updating
                ((newConges.DateDebut >= c.DateDebut && newConges.DateDebut <= c.DateFin) ||
                (newConges.DateFin >= c.DateDebut && newConges.DateFin <= c.DateFin) ||
                (newConges.DateDebut <= c.DateDebut && newConges.DateFin >= c.DateFin)));

            // If there are any overlapping leave requests, the employee is not available
            return !overlappingConges.Any();
        }



        // GET: CongesController/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var conges = sc.GetById(id);
            if (conges == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.IdEmployees = new SelectList(se.GetMany(), "id", "Nom");
            ViewBag.ConfirmePar = new SelectList(se.GetMany(), "id", "Nom");
            ViewBag.RemplassePar = new SelectList(se.GetMany(), "id", "Nom");
            ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
            ViewBag.TypeConfirmationFk = new SelectList(stc.GetMany(), "TypeConfirmationId", "type");

            return View(conges);
        }

        // POST: CongesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Conges updatedConges)
        {
            try
            {
                var originalConges = sc.GetById(id);

                if (originalConges == null)
                {
                    return RedirectToAction("Index");
                }

                bool isEmployeeAvailable = IsEmployeeAvailable(updatedConges);

                if (!isEmployeeAvailable)
                {
                    ModelState.AddModelError(string.Empty, "L'employé sélectionné n'est pas disponible pour remplacer quelqu'un pendant la période de congé spécifiée.");
                    ViewBag.IdEmployees = new SelectList(se.GetMany(), "id", "Nom");
                    ViewBag.ConfirmePar = new SelectList(se.GetMany(), "id", "Nom");
                    ViewBag.RemplassePar = new SelectList(se.GetMany(), "id", "Nom");
                    ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
                    ViewBag.TypeConfirmationFk = new SelectList(stc.GetMany(), "TypeConfirmationId", "type");
                    return View(updatedConges);
                }

                // Calculate the difference between DateDebut and DateFin
                int totalDays = (int)(updatedConges.DateFin - updatedConges.DateDebut).TotalDays;

                // Get the corresponding TypeConges entity
                var typeConges = scc.GetById(updatedConges.TypeCongesFk);

                // Check if the TypeConges allows the duration of the leave
                if (typeConges != null && typeConges.nbMaximumdeJour < totalDays)
                {
                    ModelState.AddModelError("DateFin", "La durée du congé dépasse le nombre maximum de jours autorisés pour ce TypeConges.");
                    ViewBag.IdEmployees = new SelectList(se.GetMany(), "id", "Nom");
                    ViewBag.ConfirmePar = new SelectList(se.GetMany(), "id", "Nom");
                    ViewBag.RemplassePar = new SelectList(se.GetMany(), "id", "Nom");
                    ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
                    ViewBag.TypeConfirmationFk = new SelectList(stc.GetMany(), "TypeConfirmationId", "type");

                    return View(updatedConges);
                }

                originalConges.DateDebut = updatedConges.DateDebut;
                originalConges.DateFin = updatedConges.DateFin;
                originalConges.TypeCongesFk = updatedConges.TypeCongesFk;
                originalConges.Raison = updatedConges.Raison;
                originalConges.IdEmployees = updatedConges.IdEmployees;
                originalConges.ConfirmePar = updatedConges.ConfirmePar;
                originalConges.RemplassePar = updatedConges.RemplassePar;
                originalConges.TypeConfirmationFk = updatedConges.TypeConfirmationFk;
                


                sc.Update(originalConges);
                sc.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Policy = "EmployeeUpdate")]
        // GET: CongesController/EditMyConge/5
        public ActionResult EditMyConge(int id)
        {
            var conges = sc.GetById(id);
            if (conges == null)
            {
                return RedirectToAction("MesConges");
            }

            ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");

            return View(conges);
        }

        // POST: CongesController/EditMyConge/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMyConge(int id, Conges updatedConges)
        {
            try
            {
                var originalConges = sc.GetById(id);
                var employee = se.GetById(originalConges.IdEmployees);


                if (originalConges == null)
                {
                    return RedirectToAction("MesConges");
                }
                // Check for overlapping Conges
                bool hasOverlappingConges = HasOverlappingConges(updatedConges);

                if (hasOverlappingConges)
                {
                    ModelState.AddModelError(string.Empty, "L'employé a déjà un congé qui chevauche la période spécifiée.");
                    ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
                    return View(updatedConges);
                }
                int totalDays = (int)(updatedConges.DateFin - updatedConges.DateDebut).TotalDays;

                // Get the corresponding TypeConges entity
                var typeConges = scc.GetById(updatedConges.TypeCongesFk);

                // Check if the TypeConges allows the duration of the leave
                if (typeConges != null && typeConges.nbMaximumdeJour < totalDays)
                {
                    ModelState.AddModelError("DateFin", "La durée du congé dépasse le nombre maximum de jours autorisés pour ce TypeConges.");
                    ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
                
                    return View(updatedConges);
                }
                if (employee.CreditConges < totalDays)
                {
                    ModelState.AddModelError(string.Empty, "Pas assez de crédit.");
                    ViewBag.IdEmployees = new SelectList(se.GetMany(), "id", "Nom");
                    ViewBag.TypeCongesFk = new SelectList(scc.GetMany(), "TypeCongesId", "Designation");
                    return View(updatedConges);
                }

                // Update only the allowed attributes
                originalConges.DateDebut = updatedConges.DateDebut;
                originalConges.DateFin = updatedConges.DateFin;
                originalConges.TypeCongesFk = updatedConges.TypeCongesFk;
                originalConges.Raison = updatedConges.Raison;

                sc.Update(originalConges);
                sc.Commit();

                return RedirectToAction("MesConges");
            }
            catch
            {
                return View();
            }
        }



        //Configure
        // GET: CongesController/Configure/5
        // GET: CongesController/Configure/5
        public ActionResult Configure(int id)
        {
            var conges = sc.GetById(id);
            if (conges == null)
            {
                return RedirectToAction("Index");
            }
            // Retrieve the list of TypeConfirmations for the dropdown
            ViewBag.TypeConfirmationFk = new SelectList(stc.GetMany(), "TypeConfirmationId", "type");
            ViewBag.RemplassePar = new SelectList(se.GetMany(), "id", "Nom");

            return View(conges);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Configure(int id, Conges updatedConges, bool changeDates)
        {
            string mail = User.Identity.Name;
            var employee = se.GetMany().FirstOrDefault(e => e.Email == mail);
            try
            {
                var originalConges = sc.GetById(id);
                if (originalConges == null)
                {
                    return RedirectToAction("Index");
                }

                // Debugging: Print the value of changeDates to the console
                Console.WriteLine($"changeDates: {changeDates}");

                // Check if the TypeConfirmation is changing to "Confirmed"
                if (originalConges.TypeConfirmationFk != updatedConges.TypeConfirmationFk &&
                    updatedConges.TypeConfirmationFk == GetConfirmedTypeConfirmationId())
                {
                    // Calculate the difference between DateDebut and DateFin
                    int totalDays = (int)(originalConges.DateFin - originalConges.DateDebut).TotalDays;

                    // Get the corresponding employee
                    var congesEmployee = se.GetById(originalConges.IdEmployees);

                    // Check if there is enough CreditConges
                    if (congesEmployee.CreditConges < totalDays)
                    {
                        ModelState.AddModelError(string.Empty, "Pas assez de solde pour confirmer.");
                        // Handle the error as needed, such as returning to the view
                        ViewBag.TypeConfirmationFk = new SelectList(stc.GetMany(), "TypeConfirmationId", "type");
                        ViewBag.RemplassePar = new SelectList(se.GetMany(), "id", "Nom");
                        return View(updatedConges);
                    }

                    // Subtract the leave duration from the employee's CreditConges
                    congesEmployee.CreditConges -= totalDays;

                    // Update the employee's CreditConges property in the database
                    se.Update(congesEmployee);
                }

                // Update the TypeConfirmationFk and ConfirmeParEmployee
                originalConges.TypeConfirmationFk = updatedConges.TypeConfirmationFk;

                originalConges.ConfirmeParEmployee = employee;
                originalConges.RemplassePar = updatedConges.RemplassePar;
                if (updatedConges.DateDebut != null && updatedConges.DateFin != null)
                {
                    originalConges.DateDebut = updatedConges.DateDebut;
                    originalConges.DateFin = updatedConges.DateFin;

                }

                // Debugging: Print the updated DateDebut and DateFin values to the console
                Console.WriteLine($"Updated DateDebut: {originalConges.DateDebut}");
                Console.WriteLine($"Updated DateFin: {originalConges.DateFin}");

                sc.Update(originalConges);
                sc.Commit();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Debugging: Print any exception details to the console
                Console.WriteLine($"Exception: {ex.Message}");

                // Handle the error as needed
                ViewBag.TypeConfirmationFk = new SelectList(stc.GetMany(), "TypeConfirmationId", "type");
                ViewBag.RemplassePar = new SelectList(se.GetMany(), "id", "Nom");
                return View();
            }
        }
        /*public ActionResult Configure(int id, Conges updatedConges)
        {
            string mail = User.Identity.Name;
            var employee = se.GetMany().FirstOrDefault(e => e.Email == mail);
            try
            {
                var originalConges = sc.GetById(id);
                if (originalConges == null)
                {return RedirectToAction("Index"); }

                // Check if the TypeConfirmation is changing to "Confirmed"
                if (originalConges.TypeConfirmationFk != updatedConges.TypeConfirmationFk &&
                    updatedConges.TypeConfirmationFk == GetConfirmedTypeConfirmationId())
                {
                    // Calculate the difference between DateDebut and DateFin
                    int totalDays = (int)(originalConges.DateFin - originalConges.DateDebut).TotalDays;

                    // Get the corresponding employee
                    var congesEmployee = se.GetById(originalConges.IdEmployees);

                    // Subtract the leave duration from the employee's CreditConges
                    congesEmployee.CreditConges -= totalDays;

                    // Update the employee's CreditConges property in the database
                    se.Update(congesEmployee);
                }
                // Update the TypeConfirmationFk and ConfirmeParEmployee
                originalConges.TypeConfirmationFk = updatedConges.TypeConfirmationFk;
                
                originalConges.ConfirmeParEmployee = employee;
                originalConges.RemplassePar = updatedConges.RemplassePar;

                sc.Update(originalConges);
                sc.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }*/

        // Helper method to get the TypeConfirmationId for "Confirmed"
        private int GetConfirmedTypeConfirmationId()
        {
            var confirmedTypeConfirmation = stc.GetMany().FirstOrDefault(tc => tc.type == "Confirmed");
            return confirmedTypeConfirmation?.TypeConfirmationId ?? 0;
        }



        // GET: CongesController/Delete/5
        [Authorize(Policy = "EmployeeDelete")]
        public ActionResult Delete(int id)
        {
            var conges = sc.GetById(id);
            if (conges == null)
            {
                return RedirectToAction("Index");
            }

            if (conges.TypeConfirmationFk == GetConfirmedTypeConfirmationId())
            {
                // Calculate the duration of the confirmed leave
                int totalDays = (int)(conges.DateFin - conges.DateDebut).TotalDays;

                // Get the associated employee
                var employee = conges.Employees;

                // Increment the CreditConges of the employee
                employee.CreditConges += totalDays;

                // Update the employee's CreditConges property in the database
                se.Update(employee);
                se.Commit();
            }

            sc.Delete(conges);
            sc.Commit();

            return RedirectToAction(nameof(Index));
        }

        /*public ActionResult Delete(int id)
        {
            var conges = sc.GetById(id);
            if (conges == null)
            {
                return RedirectToAction("Index");
            }

            return View(conges);
        }*/

        // POST: CongesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var conges = sc.GetById(id);
                if (conges == null)
                {
                    return RedirectToAction("Index");
                }

                sc.Delete(conges);
                sc.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        //functions
        private bool HasOverlappingConges(Conges updatedConges)
        {
            // Query the database to check for overlapping Conges
            var overlappingConges = sc.GetMany()
                .Where(c =>
                    c.IdEmployees == updatedConges.IdEmployees && // Same employee
                    c.CongesId != updatedConges.CongesId && // Exclude the current Conges being edited
                    (c.DateDebut <= updatedConges.DateFin && c.DateFin >= updatedConges.DateDebut) // Check for date overlap
                )
                .FirstOrDefault();

            return overlappingConges != null;
        }

    }
}
