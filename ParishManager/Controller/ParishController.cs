using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParishManager.Model;

namespace ParishManager.Controller;

[ApiController]
[Route("/api/[controller]")]
public class ParishController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await db.Parishes.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var parish = await db.Parishes.FindAsync(id);
        if (parish == null) return NotFound();
        return Ok(parish);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Parish parish)
    {
        db.Parishes.Add(parish);
        await db.SaveChangesAsync();
        return Ok(parish);
    }
}