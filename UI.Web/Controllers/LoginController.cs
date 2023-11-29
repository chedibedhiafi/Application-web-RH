using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace UI.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        IServiceEmployees _employeeService;
        // GET: LoginController

        /*public LoginController(IServiceEmployees employeeService)
        {
            _employeeService = employeeService;
        }*/
        /*public LoginController(IServiceEmployees employeeService, SignInManager<IdentityUser> signInManager)
        {
            _employeeService = employeeService;
            _signInManager = signInManager;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Sign the user out
            HttpContext.Session.Clear(); // Clear the session
            return RedirectToAction("Index"); // Redirect to the login page or any other page
        }*/
        //end
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string matricule, string nom)
        {
            /*
            var employee = _employeeService.GetByMatriculeAndNom(matricule, nom);

            if (employee != null)
            {
                // Authenticate the user (this is a simple example)
                HttpContext.Session.SetString("UserId", employee.id.ToString());
                return RedirectToAction("Welcome");
            }

            ViewBag.Error = "Invalid matricule or nom.";
            
            */
            return View();
        }
            
        [HttpGet]
        public IActionResult Welcome()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: LoginController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
