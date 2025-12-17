using Microsoft.AspNetCore.Mvc;
using StudentPortal.Models.ViewModels;
using StudentPortal.Models.Entities;

namespace StudentPortal.Controllers
{
    public class AccountController : Controller
    {
       private static readonly Admin admin = new Admin
       {
           Id = 1,
           Fullname = "System Admin",
           Email = "admin@studentportal.com",
           Password = "Admin@123"
       };

       public IActionResult Login()
       {
           return View();
       }

       [HttpPost]
       public IActionResult Login(LoginViewModel model)
       {
           if (ModelState.IsValid)
           {
               if (model.Email == admin.Email && model.Password == admin.Password)
               {
                    HttpContext.Session.SetString("AdminLoggedIn", "true");
                    HttpContext.Session.SetString("AdminEmail", admin.Email);
                    HttpContext.Session.SetString("AdminFullname", admin.Fullname);
                    
                   // In a real application, you would set up authentication here
                   return RedirectToAction("Index", "Home");
               }
               else
               {
                   ModelState.AddModelError(string.Empty, "Invalid login attempt.");
               }
           }
           return View(model);
       }

       public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}