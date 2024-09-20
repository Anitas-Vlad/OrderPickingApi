using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class Location
{
    public int Id { get; set; }
    [Required] public string Address { get; set; }

    [Required(ErrorMessage = "Mobile no. is required")]
    [Phone(ErrorMessage = "Please enter a valid number")]
    public string ContactNumber { get; set; }
}