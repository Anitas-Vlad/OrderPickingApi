using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class Palette
{
    [Required] public string Id { get; set; }
    public int OrderId { get; set; }
    public List<Container> Containers { get; set; } = new();
    public Container? OngoingContainer { get; set; }
    public string? OngoingContainerId { get; set; }

    // public Container? GetOngoingContainer()
    // {
    //     if (OngoingContainer == null)
    //         throw new ArgumentException("There is no container in progress.");
    //     return OngoingContainer;
    // }
    public void SetContainer(Container container)
    {
        if (!Containers.Contains(container))
        {
            OngoingContainer = container;
            Containers.Add(OngoingContainer);
        }
        else OngoingContainer = container;
    }
}