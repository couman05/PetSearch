using Microsoft.AspNetCore.Http;
using PetSearch2.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSearch2.ViewModels
{
    public class AnimalsFoundViewModel
    {
        [Required(ErrorMessage = "Please enter the race")]
        public string Race { get; set; }
        [Required(ErrorMessage = "Please enter the color")]
        public string Color { get; set; }
        [Required(ErrorMessage = "Please enter the gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please enter the height")]
        public int Height { get; set; }
        [Required(ErrorMessage = "Please enter the species")]
        public string Species { get; set; }

        [Required(ErrorMessage = "Please enter the location")]
        public int Location_Id { get; set; }

        [Required(ErrorMessage = "Please enter the owner")]

        public string Person_Name { get; set; }
        [Display(Name = "Phone number of current holder ")]
        public string? Current_Holder_PhoneNumber { get; set; }

        public int Treatment_Needed { get; set; }

        [Required(ErrorMessage = "Please enter the image")]
        [Display(Name = "Picture")]
        public IFormFile Image { get; set; }

    }
}
