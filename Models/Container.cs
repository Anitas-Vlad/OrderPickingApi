namespace OrderPickingSystem.Models;

public class Container
{
    public int Id { get; set; }
    public int PaletteId { get; set; }
    public List<Item> Items { get; set; }

    public void AddItem(Item itemToAdd)
    {
        var optionalExistingItem = Items.FirstOrDefault(item => item.Id == itemToAdd.Id);
        if (optionalExistingItem == null)
        {
            Items.Add(itemToAdd);
        }
        else
        {
            optionalExistingItem.Quantity += itemToAdd.Quantity;
        }
    }

    public bool CheckIfAlreadyContainsItem(int itemId)
        => Items.Any(item => item.Id == itemId);
}