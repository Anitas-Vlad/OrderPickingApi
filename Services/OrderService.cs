using Microsoft.AspNetCore.Mvc;
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
    {
        var pickingLocationsQueue = await _locationService.QueryPickingLocationsQueue();
        var itemFound = false;

        var nextLocation = pickingLocationsQueue.Dequeue();

        while (itemFound == false)
        {
            if (nextLocation == null)
            {
                if (order.ReplenishItems.Count > 0)
                    throw new ArgumentException(
                        "There are still items to be picked. Leave the Palette in the Replenish Area.");

                throw new ArgumentException("The order is completed. You must now print the label.");
            }

            var nextLocationItem = nextLocation.Item;
            var wantedItem = order.GetItemById(nextLocationItem.LocationId);

            if (wantedItem == null)
                continue;

            if (!nextLocationItem.CheckIfQuantityIsEnoughToPick(wantedItem.Quantity))
            {
                order.EnqueueReplenishItem(wantedItem);
                _context.Orders.Update(order);
            }
            else
                itemFound = true;
        }

        return nextLocation;
    }
}