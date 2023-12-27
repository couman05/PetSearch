using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PetSearch2.Data;
using PetSearch2.Models;
using PetSearch2.ViewModels;
using System.Web;
using System.IO;
using System.Threading.Tasks;


namespace PetSearch2.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
		private readonly IWebHostEnvironment webHostEnvironment;

		public AnimalsController(ApplicationDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
			webHostEnvironment = hostEnvironment;
		}
        [HttpGet]
        public async Task<IActionResult> Index()
        {
			var animal = await _dbContext.Animals.ToListAsync();
            return View(animal);
        }
		public IActionResult NewAnimal()
		{
			return View();
		}

		// AICI AM TESTAT PRIMA OARA INSERAREA DE IMAGINE, nu mai e actuala, vezi mainpageController 
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> NewAnimal(AnimalsViewModel model)
		{
			if (ModelState.IsValid)
			{
				string uniqueFileName = UploadedFile(model);

				Animals animal = new Animals
				{
					Animal_Name = model.Animal_Name,
					Race = model.Race,
					Color = model.Animal_Name,
					Gender = model.Gender,
					Height = model.Height,
					Species = model.Species,
					Location_Id = model.Location_Id,
      
					Animal_Image = uniqueFileName,
				};
				_dbContext.Add(animal);
				await _dbContext.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View();
		}

		private string UploadedFile(AnimalsViewModel model)
		{
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



		[HttpPost]
        public ActionResult UpdateAnimal(int Animal_Id, string Race, string Color, string Gender, int Height, string Species, int Location_Id,int Clinic_Id,int Person_Id)
        {
            // update the person's information in the database
            var animal = _dbContext.Animals.Find(Animal_Id);
            if (animal != null)
            {
               animal.Animal_Id = Animal_Id;
                animal.Race =Race ;
                animal.Color = Color;
                animal.Gender = Gender;
                animal.Height = Height;
                animal.Species = Species;
                animal.Location_Id = Location_Id;
              
                animal.Person_Id = Person_Id;
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index","Home");
        }
        // delete animal

        [HttpPost]
        public ActionResult DeleteAnimal(int Animal_Id)
        {
            var animal = _dbContext.Animals.Find(Animal_Id);
            if (animal != null)
            {
                _dbContext.Animals.Remove(animal);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        

        
        




    }
}
