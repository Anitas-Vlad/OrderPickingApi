using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class PalettesController : ControllerBase
{
    private readonly IPaletteService _paletteService;
    private readonly IOrderService _orderService;

    public PalettesController(IPaletteService paletteService, IOrderService orderService)
    {
        _paletteService = paletteService;
        _orderService = orderService;
    }

    [Authorize(Policy = "Troubleshooting")]
    [HttpGet]
    [Route("/{paletteId}")]
    public async Task<ActionResult<Palette>> GetPaletteById(string paletteId)
    {
        var palette = await _paletteService.QueryPaletteById(paletteId);
        if (palette == null)
            throw new AggregateException("Palette not found.");

        return palette;
    }
    
    [HttpPatch]
    public async Task<ActionResult<Palette>> SetPalette(string paletteId)
        => await _paletteService.SetPalette(paletteId);
}