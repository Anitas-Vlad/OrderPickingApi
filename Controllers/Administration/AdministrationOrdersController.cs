using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers.Administration;

[ApiController]
[Route("{controller}")]
public class AdministrationOrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public AdministrationOrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }
}