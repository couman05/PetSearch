using System.ComponentModel.DataAnnotations;

namespace PetSearch2.Models
{
    public class Specialties
    {
        [Key] public int Specialty_Id { get; set; }
        public string Specialty_Name { get; set; }
    }
}
