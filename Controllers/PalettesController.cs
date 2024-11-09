using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class PalettesController : ControllerBase
{
    private readonly IPaletteService _paletteService;

    public PalettesController(IPaletteService paletteService)
    {
        _paletteService = paletteService;
    }

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
    public async Task<ActionResult<Palette>> SetContainer(string containerId)
        => await _paletteService.SetContainer(containerId);
}