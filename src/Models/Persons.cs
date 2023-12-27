using PetSearch2.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSearch2.Models
{
    public class Persons
    {
        [Key]
        public int Person_Id { get; set; }

        public string Person_Name { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",ErrorMessage = "Invalid email format")]
        public string Person_Email { get; set; }
       
        public string Person_Username { get; set; }
        public string Person_Password { get; set; }
        public string Person_Phone_Number { get; set; }
        public int Location_Id { get; set; }

        [InverseProperty("Person")]
        public ICollection<Animals> Animals { get; set; }
    }
}
