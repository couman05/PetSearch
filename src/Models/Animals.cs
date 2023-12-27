using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSearch2.Models
{
    public class Animals
    {
        [Key]
        public int Animal_Id { get; set; }

        [Display(Name = "Name")]
        public string Animal_Name { get; set; }
        public string Race { get; set; }
        public string Color { get; set; }
        public string Gender { get; set; }
        public int Height { get; set; }
        public string Species { get; set; }
        
        [ForeignKey("Location_Id")]
       public Locations? Location { get; set; } 
        public int? Location_Id { get; set; }

       
        public int? Person_Id { get; set; }
        [ForeignKey("Person_Id")]
        [InverseProperty("Animals")]
        public Persons? Person { get; set; }

        public string Animal_Image { get; set; }

        public string Current_Holder_PhoneNumber { get; set; }
        [ForeignKey("Specialty_Id")]
        public Specialties? Specialties { get; set; }
        public int Specialty_Id { get; set; }
    }
}
