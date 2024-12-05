using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemsController(IItemService itemService)
    {
        _itemService = itemService;
    }
    
    [Authorize(Policy = "Picker")]
    [HttpPost]
    [Route("/VerifyItem")]
    public ActionResult VerifyItem(int itemId)
    {
        var expectedItemId = HttpContext.Session.GetInt32("ExpectedItemId");
        
        _itemService.ScanItem(expectedItemId, itemId);
        return Ok("Item verified successfully.");
    }
}