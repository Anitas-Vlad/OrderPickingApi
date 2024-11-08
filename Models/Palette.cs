using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class Palette
{
    [Required] public string Id { get; set; }
    public int OrderId { get; set; }
    public List<Container> Containers { get; set; }
    public Container? OngoingContainer { get; set; }

    public void AddContainer(Container container)
    {
        if (!Containers.Contains(container))
        {
            OngoingContainer = container;
            Containers.Add(OngoingContainer);
        }
        else OngoingContainer = container;
    }
}