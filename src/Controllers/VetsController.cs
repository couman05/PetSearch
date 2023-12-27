using Microsoft.AspNetCore.Mvc;
using PetSearch2.Data;
using PetSearch2.Models;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PetSearch2.Controllers
{
    public class VetsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public VetsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            List<Specialties> specialties = _dbContext.Specialties.ToList();

            // Create a SelectList containing the locations
            SelectList specialtySelectList = new SelectList(specialties, "Specialty_Id", "Specialty_Name");

            // Add the SelectList to the ViewBag
            ViewBag.Specialties = specialtySelectList;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVet(Vets vet)
        {
            if (_dbContext.Vets.Any(p => p.Vet_Email == vet.Vet_Email))
            {
                // in caz ca exista deja un user cu emailu respectiv
                TempData["Error"] = "This email is already in use.";
                return RedirectToAction("Register");
            }
            else
            if (_dbContext.Vets.Any(p => p.Vet_Phone_Number == vet.Vet_Phone_Number))
            {
                // in caz ca exista deja un user cu nr. de telefon respectiv
                TempData["Error"] = "This phone number is already in use.";
                return RedirectToAction("Register");
            }
            else
            {
                //adaugarea persoanei in tabela Persons
                _dbContext.Vets.Add(vet);
                _dbContext.SaveChanges();
                // meniu de drop-down pentru locatii
                var specialties = _dbContext.Specialties.ToList();
                ViewBag.SpecialtyList = new SelectList(specialties, "Specialty_Id", "Specialty_Name", vet.Specialty_Id);
                return RedirectToAction("Index","MainpageVets");
            }



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Vets vet)
        {
            var result = _dbContext.Vets.FirstOrDefault(p => p.Vet_Username == vet.Vet_Username && p.Vet_Password == vet.Vet_Password);
            if (result != null)
            {
                // if the email and password combination is valid, redirect to the home page
                return RedirectToAction("Index", "MainpageVets");// aici ar trebui sa facem un index page ca la persoane, ca sa fie redirectionat unde trebe
            }
            else
            {
                // if the email and password combination is invalid, display an error message
                TempData["Error"] = "Invalid email or password";
                return View();
            }
        }
    }
}
