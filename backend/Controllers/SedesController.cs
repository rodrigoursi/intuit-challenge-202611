using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConsultasPlan.Data;

namespace ConsultasPlan.Controllers;

[ApiController]
[Route("[controller]")]
public class SedesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SedesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sedes = await _context.Sedes.ToListAsync();
        return Ok(sedes);
    }
}
