using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize(Policy = "Troubleshooting")]
    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetAllOrders()
        => await _orderService.QueryAllOrders();

    [Authorize(Policy = "Troubleshooting")]
    [HttpGet]
    [Route("/{orderId}")]
    public async Task<ActionResult<Order>> GetOrderById(int orderId)
        => await _orderService.QueryOrderById(orderId);
}