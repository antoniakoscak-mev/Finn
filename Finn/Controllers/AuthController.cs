using Finn.Data;
using Finn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Finn.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser>
            _userManager;

        private readonly SignInManager<IdentityUser>
            _signInManager;

        private readonly ApplicationDbContext
             _context;

        public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ApplicationDbContext context)
        {
            _userManager = userManager;

            _signInManager = signInManager;

            _context = context;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(
            LoginViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user =
                await _userManager
                .FindByEmailAsync(model.Email);

            if (user == null)
            {
                ViewBag.Greska =
                    "Korisnik ne postoji";

                return View(model);
            }

            var result =
                await _signInManager
                .PasswordSignInAsync(
                    user.UserName,
                    model.Password,
                    false,
                    false);

            if (result.Succeeded)
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            ViewBag.Greska =
                "Pogrešan email ili lozinka";

            return View(model);
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            IdentityUser user =
                new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

            var result =
                await _userManager
                .CreateAsync(
                    user,
                    model.Password);

            if (result.Succeeded)
            {
                _context.UserSettings.Add(
                    new UserSettings
                    {
                        UserId = user.Id,
                        TipRacuna = model.TipRacuna
                    });

                _context.SaveChanges();

                await _signInManager
                    .SignInAsync(user, false);

                return RedirectToAction(
                    "Index",
                    "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    "",
                    error.Description);
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager
                .SignOutAsync();

            return RedirectToAction(
                "Login");
        }
    }
}