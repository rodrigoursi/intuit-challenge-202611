using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConsultasPlan.Data;
using ConsultasPlan.Models;

namespace ConsultasPlan.Controllers;

[ApiController]
[Route("[controller]")]
public class AfiliadosController : ControllerBase
{
    private readonly AppDbContext _context;

    public AfiliadosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var afiliados = await _context.Afiliados.ToListAsync();
        return Ok(afiliados);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var afiliado = await _context.Afiliados.FindAsync(id);
        if (afiliado == null) return NotFound();
        return Ok(afiliado);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Afiliado afiliado)
    {
        afiliado.createdAt = DateTime.UtcNow;
        afiliado.isActive = true;
        _context.Afiliados.Add(afiliado);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = afiliado.Id }, afiliado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Afiliado afiliado)
    {
        var existing = await _context.Afiliados.FindAsync(id);
        if (existing == null) return NotFound();

        existing.NombreCompleto = afiliado.Nombre;
        existing.DNI = afiliado.DNI;
        existing.Email = afiliado.Email;
        existing.Telefono = afiliado.Telefono;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var afiliado = await _context.Afiliados.FindAsync(id);
        if (afiliado == null) return NotFound();

        _context.Afiliados.Remove(afiliado);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
