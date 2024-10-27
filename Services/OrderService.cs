using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class OrderService : IOrderService
{
    private readonly OrderPickingContext _context;
    private readonly ILocationService _locationService;

    public OrderService(OrderPickingContext context, ILocationService locationService)
    {
        _context = context;
        _locationService = locationService;
    }

    //TODO AdminApi
    public async Task<List<Order>> QueryAllOrders()
        => await _context.Orders.Order().ToListAsync();

    public async Task<Order> QueryOrderById(int orderId)
    {
        var order = await _context.Orders.SingleOrDefaultAsync(order => order.Id == orderId);
        if (order == null)
            throw new ArgumentException("Order not found.");
        return order;
    }

    public async Task<Location> QueryNextLocation(Order order)
    // Break into smaller methods
    {
        var pickingLocationsQueue = await _locationService.QueryPickingLocationsQueue();

        var itemFound = false;

        do
        {
            var nextLocation = pickingLocationsQueue.Dequeue();
            if (nextLocation == null)
            {
                if (order.ReplenishItems.Count != 0)
                {
                    throw new ArgumentException(
                        "There are still items to be picked. Please leave the Palette in the Replenish Area.");
                }

                FinishOrder(order); //TODO Not implemented yet
            }

            var nextLocationItem = nextLocation.Item;

            var wantedItem = order.Items.FirstOrDefault(item => item.Id == nextLocationItem.Id);

            if (wantedItem == null)
                continue;

            if (!nextLocationItem.CheckIfQuantityIsEnoughToPick(wantedItem.Quantity))
            {
                order.ReplenishItems.Enqueue(wantedItem);
                _context.Orders.Update(order);
            }
            else
            {
                itemFound = true;
            }
        } while (itemFound == false);
        
        
    }

    private void FinishOrder(Order order)
    {
        if (order.ReplenishItems.Count > 0)
        {
            // Idk yet. :)))
        }

        throw new NotImplementedException();
    }
}