using Microsoft.AspNetCore.Mvc;
using PetSearch2.Data;
using PetSearch2.Models;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetSearch2.ViewModels;

namespace PETSearch.Controllers
{
    public class PersonsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public PersonsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       

        public IActionResult Index()
        {
            Persons person = new Persons();

            return View(person); 
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            List<Locations> locations = _dbContext.Locations.ToList();

            // Create a SelectList containing the locations
            SelectList locationSelectList = new SelectList(locations, "Location_Id", "Location");

            // Add the SelectList to the ViewBag
            ViewBag.Locations = locationSelectList;
            return View();
        }
       


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePerson(Persons person)
        {
            if (_dbContext.Persons.Any(p => p.Person_Email == person.Person_Email))
            {
                // in caz ca exista deja un user cu emailu respectiv
                TempData["Error"] = "This email is already in use.";
                return RedirectToAction("Register");
            }
            else
            if (_dbContext.Persons.Any(p => p.Person_Phone_Number == person.Person_Phone_Number))
            {
                // in caz ca exista deja un user cu nr. de telefon respectiv
                TempData["Error"] = "This phone number is already in use.";
                return RedirectToAction("Register");
            }
            else
            {
                //adaugarea persoanei in tabela Persons
                _dbContext.Persons.Add(person);
                _dbContext.SaveChanges();
                // meniu de drop-down pentru locatii
                var locations = _dbContext.Locations.ToList();
                ViewBag.LocationList = new SelectList(locations, "Location_Id", "Location", person.Location_Id);
                return RedirectToAction("Index", "Mainpage");
            }

            
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(PersonsViewModel person)
        {
            var result = _dbContext.Persons.FirstOrDefault(p => p.Person_Username == person.Person_Username && p.Person_Password == person.Person_Password);
            if (result != null)
            {
                // if the email and password combination is valid, redirect to the home page
                return RedirectToAction("Index","Mainpage");
            }
            else
            {
                // if the email and password combination is invalid, display an error message
                TempData["Error"] = "Invalid username or password";
                return View();
            }
        }









    }

}
