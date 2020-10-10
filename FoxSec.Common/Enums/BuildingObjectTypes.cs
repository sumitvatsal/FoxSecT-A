using System.ComponentModel.DataAnnotations;

namespace FoxSec.Common.Enums
{
    public enum BuildingObjectTypes
    {
        [Display(Name = "Floor")]
        Floor = 1,
        [Display(Name = "Room")]
        Room  = 2,
        [Display(Name = "Building")]
        Building = 3,
        [Display(Name = "Door")]
        Door  = 8,
        [Display(Name = "Lift")]
        Lift = 9
    }
}