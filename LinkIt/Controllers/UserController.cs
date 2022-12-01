using Microsoft.AspNetCore.Mvc;
using LinkIt.Models.Entities;
using LinkIt.Models.DAO;
using Castle.Core.Resource;
using Newtonsoft.Json;

namespace LinkIt.Controllers
{
    public class UserController : Controller
    {
        private LinkItContext _context;
        private readonly ISession _session;

        public UserController(LinkItContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _session = httpContextAccessor.HttpContext.Session;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Inscription()
        {
            return View();
        }

        public IActionResult InscriptionSubmit(string Username, string Email, string Password)
        {
            if(Username != null || Email != null || Password != null)
            {
                User newUser = new User { Username = Username, Email = Email, Password = Password };
                if (_context.Users?.FirstOrDefault(u => u.Username == newUser.Username || u.Email == newUser.Email) == null)
                {
                    _context.Users?.Add(newUser);
                    _context.SaveChanges();
                    return RedirectToAction("connection");
                }
                else
                {
                    ViewBag.ErrorMessage = "Le nom d'utilisateur ou le email fournit sont déjà utilisés";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Veuillez entrer toutes les informations nécessaires";
            }
            return View("inscription");
        }

        public IActionResult Connection()
        {
            return View();
        }

        public IActionResult ConnectionSubmit(string Email, string Password)
        {
            User? foundUser = _context.Users?.FirstOrDefault(u => u.Email == Email && u.Password == Password);
            if (foundUser != null)
            {
                _session.SetString("userId", foundUser.Id.ToString());
                return RedirectToAction("Index","Link");
            }
            ViewBag.ErrorMessage = "Les infomations d'identification sont invalides!";
            return View("connection");
        }

        public IActionResult Deconnexion()
        {
            _session.Remove("user");
            return RedirectToAction("Connection");
        }
    }
}
