using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSearch2.Models
{
    public class Tasks
    {
        [Key]
        public int Task_Id { get; set; }
        [ForeignKey("Specialty_Id")]
        public Specialties? Specialty { get; set; }
        public int Specialty_Id { get; set; }


        public string Animal_Name { get; set;}

        public string Person_Name { get; set;}  

        public string Person_PhoneNumber { get; set;}




    }
}
