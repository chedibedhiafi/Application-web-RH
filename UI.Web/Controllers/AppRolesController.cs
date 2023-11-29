using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ApplicationCore.Domain;
using System.Security.Claims;

namespace UI.Web.Controllers
{
    
    public class AppRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AppRolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;


        }
        [HttpGet]
        [Authorize(Policy = "EmployeeRead")]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        [Authorize(Policy = "EmployeeCreate")]
        public async Task<IActionResult> Create(IdentityRole model) 
        {

            if(!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult GrantAuthorization(string roleName)
        {
            // Retrieve the specified role by name
            var role = _roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();

            if (role == null)
            {
                // Handle the case where the role doesn't exist.
                return NotFound();
            }

            // Retrieve a list of all users from the database
            var allUsers = _userManager.Users.ToList();

            // Filter users who are not already in the role in memory
            var usersNotInRole = allUsers
                .Where(user => !_userManager.IsInRoleAsync(user, roleName).GetAwaiter().GetResult())
                .ToList();

            ViewData["RoleName"] = roleName; // Pass roleName to the view
            ViewData["UsersNotInRole"] = usersNotInRole; // Pass usersNotInRole to the view

            return View();
        }




        [HttpPost]
        public async Task<IActionResult> GrantAuthorization(string roleName, List<string> selectedUserIds)
        {
            if (selectedUserIds != null && selectedUserIds.Any())
            {
                // Add selected users to the specified role
                foreach (var userId in selectedUserIds)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        await _userManager.AddToRoleAsync(user, roleName);
                    }
                }

                return RedirectToAction("Index");
            }

            // If no users were selected or there was an issue, redisplay the view with validation errors.
            ViewData["RoleName"] = roleName; // Pass roleName back to the view
            ViewData["UsersNotInRole"] = _userManager.Users.ToList(); // Retrieve users again for the view
            ModelState.AddModelError("", "Please select at least one user.");

            return View();
        }
        [HttpGet]
        public IActionResult RevokeAuthorization(string roleName)
        {
            // Retrieve the specified role by name
            var role = _roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();

            if (role == null)
            {
                // Handle the case where the role doesn't exist.
                return NotFound();
            }

            // Retrieve a list of users who are in the role
            var usersInRole = _userManager.GetUsersInRoleAsync(roleName).GetAwaiter().GetResult();

            ViewData["RoleName"] = roleName; // Pass roleName to the view
            ViewData["UsersInRole"] = usersInRole; // Pass usersInRole to the view

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RevokeAuthorization(string roleName, List<string> selectedUserIds)
        {
            if (selectedUserIds != null && selectedUserIds.Any())
            {
                // Remove selected users from the specified role
                foreach (var userId in selectedUserIds)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        await _userManager.RemoveFromRoleAsync(user, roleName);
                    }
                }

                return RedirectToAction("Index");
            }

            // If no users were selected or there was an issue, redisplay the view with validation errors.
            ViewData["RoleName"] = roleName; // Pass roleName back to the view
            ViewData["UsersInRole"] = _userManager.GetUsersInRoleAsync(roleName).GetAwaiter().GetResult(); // Retrieve users again for the view
            ModelState.AddModelError("", "Please select at least one user.");

            return View();
        }
        [HttpGet]
       
        public IActionResult ManagePermissions(string roleName)
        {
            ViewData["RoleName"] = roleName;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ManagePermissions(string roleName, List<string> selectedPermissions)
        {
            if (selectedPermissions != null)
            {
                // Retrieve the role by name
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role == null)
                {
                    return NotFound(); // Handle the case where the role doesn't exist.
                }

                // Remove all existing permissions for the role (optional, depending on your design)
                var currentRolePermissions = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in currentRolePermissions)
                {
                    var result = await _roleManager.RemoveClaimAsync(role, claim);
                    if (!result.Succeeded)
                    {
                        // Handle the error if needed
                        return RedirectToAction("Error");
                    }
                }

                // Assign the selected permissions to the role
                foreach (var permission in selectedPermissions)
                {
                    var claim = new Claim("Permission", permission);
                    var result = await _roleManager.AddClaimAsync(role, claim);
                    if (!result.Succeeded)
                    {
                        // Handle the error if needed
                        return RedirectToAction("Error");
                    }
                }

                return RedirectToAction("Index"); // Redirect back to the role management page.
            }

            // If no permissions were selected or there was an issue, redisplay the form.
            ViewData["RoleName"] = roleName;
            ModelState.AddModelError("", "Please select at least one permission.");
            return View();
        }


        [HttpGet]
        public IActionResult Delete(string roleName)
        {
            // Retrieve the role by name
            var role = _roleManager.FindByNameAsync(roleName).Result;

            if (role == null)
            {
                // Handle the case where the role doesn't exist.
                return NotFound();
            }

            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string roleName)
        {
            // Retrieve the role by name
            var role = _roleManager.FindByNameAsync(roleName).Result;

            if (role == null)
            {
                // Handle the case where the role doesn't exist.
                return NotFound();
            }

            // Delete the role
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Handle errors, e.g., display an error message or redirect to an error page.
                return RedirectToAction("Error");
            }
        }


    }









}


