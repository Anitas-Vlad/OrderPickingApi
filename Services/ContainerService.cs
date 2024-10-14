using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services;

public class ContainerService
{
    private readonly OrderPickingContext _context;

    public ContainerService(OrderPickingContext context)
    {
        _context = context;
    }

    public async Task<Container> QueryContainerById(int containerId)
    {
        var container = await _context.Containers.SingleOrDefaultAsync(container => container.Id == containerId);
        if (container == null)
            throw new ArgumentException("Container not found.");
        return container;
    }

    public async Task AddItemToContainer(int containerId, Item item) //TODO Integrate all Item updates. Location/Stock
    {
        var container = await QueryContainerById(containerId);

        
        
    }
}