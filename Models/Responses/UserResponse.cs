using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models.Responses;

public class UserResponse
{
    public int Id { get; set; }
    [Required] public string UserName { get; set; }
}