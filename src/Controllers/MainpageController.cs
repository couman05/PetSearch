using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PetSearch2.Data;
using PetSearch2.Models;
using PetSearch2.ViewModels;
using System;
using System.Linq;

namespace PetSearch2.Controllers
{
    public class MainpageController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public MainpageController(ApplicationDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            webHostEnvironment = hostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {

            var animals = _dbContext.Animals.Include(a => a.Location).Include(a => a.Specialties).ToList(); // aici am incercat prima data sa afisez toate animalele        


            foreach (var animal in animals)
            {
                if (animal.Person_Id != null)
                {
                    var person = _dbContext.Persons.Find(animal.Person_Id);
                    animal.Person = person;
                }
            }


            return View(animals);
        }

        [HttpGet]
        public IActionResult Lost()
        {
            //pentru drop-down menu de la locatii 
            List<Locations> locations = _dbContext.Locations.ToList();

            // Create a SelectList containing the locations
            SelectList locationSelectList = new SelectList(locations, "Location_Id", "Location");

            // Add the SelectList to the ViewBag
            ViewBag.Locations = locationSelectList;

            return View(new AnimalsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lost(AnimalsViewModel model)
        {
            // drop down list pentru locatii
            var locations = _dbContext.Locations.ToList();
            ViewBag.LocationList = new SelectList(locations, "Location_Id", "Location", model.Location_Id);

            if (ModelState.IsValid)
            {
                // Check the database for existing entries that match the new animal object
                var existingAnimals = _dbContext.Animals
                    .Where(a => a.Race == model.Race &&
                                a.Color == model.Color &&
                                a.Gender == model.Gender &&
                                a.Height == model.Height &&
                                a.Species == model.Species &&
                                a.Location_Id == model.Location_Id)
                    .ToList();



                // aici cautam in tabela persoane entry-ul corespunzator numelui
                var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Person_Name == model.Person_Name);

                if (existingAnimals.Any())
                {
                    // If there are existing animals, return a view with the matching animals
                    return View("MatchingAnimals",existingAnimals);
                }
                else
                {
                    // else it means there are no animals found with these criteria, so we create another entry with the specified 
                    //details
                    string uniqueFileName = UploadedFile(model);
                    Animals animal = new Animals
                    {
                        Animal_Name = model.Animal_Name,
                        Race = model.Race,
                        Color = model.Color,
                        Gender = model.Gender,
                        Height = model.Height,
                        Species = model.Species,
                        Location_Id = model.Location_Id,
                        Person_Id = person.Person_Id, //chiar daca in baza de date, in tabela animale nu avem un camp de tip person_name
                                                      // ne folosim de viewmodel, preluand numele introdus de user,apoi cautandu-l in tabela persons, si atribuindu-i 
                                                      // animalului id-ul corespunzator persoanei respective
                        Animal_Image = uniqueFileName,
                        Current_Holder_PhoneNumber = person.Person_Phone_Number,
                        Specialty_Id = 6,
                    };
                    _dbContext.Add(animal);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Index", "Mainpage"); // TO DO: !!! aici ar trebui adaugat ori o pagina noua, in care se se confirme
                    // introducerea, ori un mesaj sau cv de genu
                }
            }
            else
            {
                return View(model);
            }
        }

        private string UploadedFile(AnimalsViewModel model)
		{

            // functia prin care uploadam imaginea animalului 
			string uniqueFileName = null;

			if (model.Image != null)
			{
				string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
				uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					model.Image.CopyTo(fileStream);
				}
			}
			return uniqueFileName;
		}





        public IActionResult Found()
        {
            List<Locations> locations = _dbContext.Locations.ToList();

            // Create a SelectList containing the locations
            SelectList locationSelectList = new SelectList(locations, "Location_Id", "Location");

            // Add the SelectList to the ViewBag
            ViewBag.Locations = locationSelectList;



            List<Specialties> treatments = _dbContext.Specialties.ToList();

            // Create a SelectList containing the locations
            SelectList treatmentsSelectList = new SelectList(treatments, "Specialty_Id", "Specialty_Name");

            // Add the SelectList to the ViewBag
            ViewBag.Specialties = treatmentsSelectList;
            return View(new AnimalsViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Found(AnimalsViewModel model)
        {
            // drop down list pentru locatii
            var locations = _dbContext.Locations.ToList();
            ViewBag.LocationList = new SelectList(locations, "Location_Id", "Location", model.Location_Id);

            var treatments = _dbContext.Specialties.ToList();
            ViewBag.TreatmentsList = new SelectList(treatments, "Specialty_Id", "Specialty_Name", model.Treatment_Needed);

            if (ModelState.IsValid)
            {
                // Check the database for existing entries that match the new animal object
                var existingAnimals = _dbContext.Animals
                    .Where(a => a.Race == model.Race &&
                                a.Color == model.Color &&
                                a.Gender == model.Gender &&
                                a.Height == model.Height &&
                                a.Species == model.Species &&
                                a.Location_Id == model.Location_Id)
                    .ToList();
               
                var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Person_Name == model.Person_Name);

                if (existingAnimals.Any())
                {
                    // If there are existing animals, return a view with the matching animals
                  
                    return View("MatchingAnimalsFound", existingAnimals);
                }
                else
                {
                    // else it means there are no animals found with these criteria, so we create another entry with the specified 
                    //details
                    string uniqueFileName = UploadedFile(model);
                    Animals animal = new Animals
                    {
                        Animal_Name = "Unknown",
                        Race = model.Race,
                        Color = model.Color,
                        Gender = model.Gender,
                        Height = model.Height,
                        Species = model.Species,
                        Location_Id = model.Location_Id,
                        Person_Id = person.Person_Id, //chiar daca in baza de date, in tabela animale nu avem un camp de tip person_name
                     // ne folosim de viewmodel, preluand numele introdus de user,apoi cautandu-l in tabela persons, si atribuindu-i 
                     // animalului id-ul corespunzator persoanei respective
                        Animal_Image = uniqueFileName,
                        Current_Holder_PhoneNumber = person.Person_Phone_Number,
                         Specialty_Id=model.Treatment_Needed
                    };
                    _dbContext.Add(animal);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Index", "Mainpage"); // TO DO: !!! aici ar trebui adaugat ori o pagina noua, in care se se confirme
                    // introducerea, ori un mesaj sau cv de genu
                }
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult AssignTask()
        {
            List<Specialties> treatments = _dbContext.Specialties.ToList();

            // Create a SelectList containing the locations
            SelectList treatmentsSelectList = new SelectList(treatments, "Specialty_Id", "Specialty_Name");

            // Add the SelectList to the ViewBag
            ViewBag.Specialties = treatmentsSelectList;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AssignTask(Tasks model)
        {
            var treatments = _dbContext.Specialties.ToList();
            ViewBag.TreatmentsList = new SelectList(treatments, "Specialty_Id", "Specialty_Name", model.Specialty_Id);
            if (ModelState.IsValid)
            {
                Tasks task = new Tasks
                {
                    Specialty_Id = model.Specialty_Id,
                    Animal_Name = model.Animal_Name,
                    Person_Name = model.Person_Name,
                    Person_PhoneNumber = model.Person_PhoneNumber
                };
                _dbContext.Add(task);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }



        [HttpGet]
        public IActionResult MatchingAnimals(IEnumerable<Animals> existingAnimals)
        {
            // ne folosim de metoda asta ca sa afisam animalele corespunzatoare detaliilor introduse de user, vezi linia 83
            return View(existingAnimals);
        }
    }
}
