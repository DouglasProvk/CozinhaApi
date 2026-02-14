using CozinhaApi.Models;
using CozinhaApi.DTOs.Prato;
using CozinhaAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CozinhaApi.Controllers
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

        // POST: api/Pratos (Apenas Admin)
        [HttpPost]
        public async Task<ActionResult<Prato>> PostPrato([FromBody] CreatePratoRequest request)
        {
            // Validar se o CPF fornecido pertence a um admin
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.CPF == request.CPFAdmin && f.IsAdmin);

            if (funcionario == null)
                return Unauthorized(new { message = "Apenas administradores podem criar pratos" });

            var prato = new Prato
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                Categoria = request.Categoria,
                Disponivel = request.Disponivel,
                EhPrincipal = request.EhPrincipal
            };

            _context.Pratos.Add(prato);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrato), new { id = prato.Id }, prato);
        }

        // PUT: api/Pratos/5 (Apenas Admin)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrato(int id, [FromBody] UpdatePratoRequest request)
        {
            // Validar se o CPF fornecido pertence a um admin
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.CPF == request.CPFAdmin && f.IsAdmin);

            if (funcionario == null)
                return Unauthorized(new { message = "Apenas administradores podem editar pratos" });

            var prato = await _context.Pratos.FindAsync(id);
            if (prato == null)
                return NotFound();

            prato.Nome = request.Nome;
            prato.Descricao = request.Descricao;
            prato.Categoria = request.Categoria;
            prato.Disponivel = request.Disponivel;
            prato.EhPrincipal = request.EhPrincipal;

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

        // DELETE: api/Pratos/5 (Apenas Admin)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrato(int id, [FromQuery] string cpfAdmin)
        {
            // Validar se o CPF fornecido pertence a um admin
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.CPF == cpfAdmin && f.IsAdmin);

            if (funcionario == null)
                return Unauthorized(new { message = "Apenas administradores podem deletar pratos" });

            var prato = await _context.Pratos.FindAsync(id);
            if (prato == null)
                return NotFound();

            _context.Pratos.Remove(prato);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}