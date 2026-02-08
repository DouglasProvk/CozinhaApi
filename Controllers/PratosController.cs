using CozinhaApi.Models;
using CozinhaAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CozinhaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PratosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PratosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Pratos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prato>>> GetPratos()
        {
            return await _context.Pratos.ToListAsync();
        }

        // GET: api/Pratos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prato>> GetPrato(int id)
        {
            var prato = await _context.Pratos.FindAsync(id);
            if (prato == null)
                return NotFound();

            return prato;
        }

        // POST: api/Pratos
        [HttpPost]
        public async Task<ActionResult<Prato>> PostPrato(Prato prato)
        {
            _context.Pratos.Add(prato);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrato), new { id = prato.Id }, prato);
        }

        // PUT: api/Pratos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrato(int id, Prato prato)
        {
            if (id != prato.Id)
                return BadRequest();

            _context.Entry(prato).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pratos.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Pratos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrato(int id)
        {
            var prato = await _context.Pratos.FindAsync(id);
            if (prato == null)
                return NotFound();

            _context.Pratos.Remove(prato);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}