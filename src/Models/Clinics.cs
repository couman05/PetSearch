using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSearch2.Models
{
    public class Clinics
    {
        [Key] public int Clinic_Id { get; set; }
        public string Clinic_Name { get; set; }

        [ForeignKey("Locations")]
        public int? Location_Id { get; set; }
        public Locations? Locations { get; set; }

    }
}
