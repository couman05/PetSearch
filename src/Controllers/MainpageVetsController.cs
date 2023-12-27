using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PetSearch2.Data;
using PetSearch2.Models;
using PetSearch2.ViewModels;

namespace PetSearch2.Controllers
{
    public class MainpageVetsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public MainpageVetsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //pentru drop-down menu de la locatii 
            List<Specialties> specialties = _dbContext.Specialties.ToList();

            // Create a SelectList containing the locations
            SelectList specialtiesSelectList = new SelectList(specialties, "Specialty_Id", "Specialty_Name");

            // Add the SelectList to the ViewBag
            ViewBag.Specialties = specialtiesSelectList;

            return View(new Tasks());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(TasksViewModel task)
        {
            if (ModelState.IsValid)
            {
                var existingTasks = _dbContext.Tasks
                    .Where(a => a.Specialty_Id == task.Specialty_Id )
                    .ToList();
                if (existingTasks.Any())
                {
                    // If there are existing animals, return a view with the matching animals
                    return View("MatchingTasks", existingTasks);
                }
                else
                {
                    return RedirectToAction("Index", "Mainpagevets");
                }
               
            }
            else
            {
                return View();
            }
        }


    }
}
