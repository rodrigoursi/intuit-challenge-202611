using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConsultasPlan.Data;

namespace ConsultasPlan.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfesionalesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProfesionalesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var profesionales = await _context.Profesionales.Include(p => p.Sede).ToListAsync();
        return Ok(profesionales);
    }
}
