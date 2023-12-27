using PetSearch2.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSearch2.Models
{
    public class Vets
    {
        
        [Key] public int Vet_Id { get; set; }

        public string Vet_Name { get; set; }
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format")]
        public string Vet_Email { get; set; }

        public string Vet_Username { get; set; }

        public string Vet_Password { get; set; }


        public int Clinic_Id { get; set; }

        public string Vet_Phone_Number { get; set; }

        [ForeignKey("Specialties")]

        public int? Specialty_Id { get; set; }

        public Specialties? Specialties { get; set; }





    }
}
