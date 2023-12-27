using System.ComponentModel.DataAnnotations;

namespace PetSearch2.Models
{
    public class Locations
    {
        [Key] public int Location_Id { get; set; }
        public string Location { get; set; }
        

    }
}
