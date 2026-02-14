using CozinhaApi.Models;
using CozinhaApi.DTOs.Reserva;
using CozinhaAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CozinhaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReservasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Reservas (Apenas Admin pode listar todas)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas([FromQuery] string cpfAdmin) 
        {
            // Validar se é admin
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.CPF == cpfAdmin && f.IsAdmin);

            if (funcionario == null)
                return Unauthorized(new { message = "Apenas administradores podem ver todas as reservas" });

            return await _context.Reservas
                .Include(r => r.Prato)
                .OrderByDescending(r => r.DataRefeicao)
                .ToListAsync();
        }

        // GET: api/Reservas/porData?data=YYYY-MM-DD (Apenas Admin)
        [HttpGet("porData")]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservasPorData([FromQuery] string data, [FromQuery] string cpfAdmin)
        {
            // Validar se é admin
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.CPF == cpfAdmin && f.IsAdmin);

            if (funcionario == null)
                return Unauthorized(new { message = "Apenas administradores podem ver reservas por data" });

            if (!DateTime.TryParse(data, out var dataRefeicao))
                return BadRequest(new { message = "Formato de data inválido" });

            var dataFim = dataRefeicao.AddDays(1);

            return await _context.Reservas
                .Where(r => r.DataRefeicao >= dataRefeicao && r.DataRefeicao < dataFim)
                .Include(r => r.Prato)
                .OrderBy(r => r.Periodo)
                .ToListAsync();
        }

        // GET: api/Reservas/verificar?nome=...&cpf=...
        // Qualquer pessoa pode verificar sua própria reserva (D-1)
        [HttpGet("verificar")]
        public async Task<ActionResult<IEnumerable<Reserva>>> VerificarReserva([FromQuery] string nome, [FromQuery] string cpf)
        {
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(cpf))
                return BadRequest(new { message = "Nome e CPF são obrigatórios" });

            // Reservas apenas para amanhã (D-1)
            var amanha = DateTime.UtcNow.AddDays(1).Date;
            var amanhaFim = amanha.AddDays(1);

            var reservas = await _context.Reservas
                .Where(r => r.NomePessoa.ToLower() == nome.ToLower() 
                    && r.CPFFuncionario == cpf
                    && r.DataRefeicao >= amanha 
                    && r.DataRefeicao < amanhaFim)
                .Include(r => r.Prato)
                .ToListAsync();

            return reservas;
        }

        // GET: api/Reservas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id) 
        {
            var reserva = await _context.Reservas
                .Include(r => r.Prato)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            return reserva;
        }

        // POST: api/Reservas (Sistema D-1: Funcionário reserva para amanhã)
        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva([FromBody] CreateReservaRequest request)
        {
            // Validar se o prato existe
            var prato = await _context.Pratos.FindAsync(request.PratoId);
            if (prato == null)
                return NotFound(new { message = "Prato não encontrado" });

            // Validar se o prato está disponível
            if (!prato.Disponivel)
                return BadRequest(new { message = "Prato não está disponível" });

            // Validar se o funcionário existe
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.CPF == request.CPFFuncionario);

            if (funcionario == null)
                return NotFound(new { message = "Funcionário não encontrado" });

            // A reserva é SEMPRE para amanhã (D+1)
            var amanha = DateTime.UtcNow.AddDays(1).Date;
            var dataRefeicao = amanha.AddHours(12); // Meio-dia como padrão

            // Validar se já não existe uma reserva para este funcionário no mesmo dia/período
            var reservaExistente = await _context.Reservas
                .FirstOrDefaultAsync(r => 
                    r.CPFFuncionario == request.CPFFuncionario 
                    && r.DataRefeicao.Date == amanha
                    && r.Periodo == request.Periodo);

            if (reservaExistente != null)
                return BadRequest(new { message = "Você já tem uma reserva para este período amanhã" });

            var reserva = new Reserva
            {
                NomePessoa = request.NomePessoa,
                CPFFuncionario = request.CPFFuncionario,
                PratoId = request.PratoId,
                PratoNome = prato.Nome,
                DataRefeicao = dataRefeicao,
                Periodo = request.Periodo,
                Observacoes = request.Observacoes,
                Status = "confirmada"
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReserva), new { id = reserva.Id }, reserva);
        }

        // PUT: api/Reservas/{id} (Apenas Admin pode atualizar, ou funcionário sua própria reserva)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, [FromBody] UpdateReservaRequest request)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return NotFound();

            // Validar permissão: apenas admin ou o próprio funcionário podem editar
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.CPF == request.CPFRequisitor);

            if (funcionario == null)
                return Unauthorized(new { message = "Usuário não encontrado" });

            bool isAdmin = funcionario.IsAdmin;
            bool isOwner = reserva.CPFFuncionario == request.CPFRequisitor;

            if (!isAdmin && !isOwner)
                return Unauthorized(new { message = "Você não tem permissão para editar esta reserva" });

            // Se não é admin, validar se está tentando editar uma reserva de D-1
            if (!isAdmin)
            {
                var amanha = DateTime.UtcNow.AddDays(1).Date;
                if (reserva.DataRefeicao.Date != amanha)
                    return BadRequest(new { message = "Você só pode editar reservas para amanhã" });
            }

            reserva.Observacoes = request.Observacoes;
            reserva.Status = request.Status;

            _context.Entry(reserva).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Reservas/{id} (Apenas Admin ou funcionário da reserva)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id, [FromQuery] string cpfRequisitor)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return NotFound();

            // Validar permissão
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.CPF == cpfRequisitor);

            if (funcionario == null)
                return Unauthorized(new { message = "Usuário não encontrado" });

            bool isAdmin = funcionario.IsAdmin;
            bool isOwner = reserva.CPFFuncionario == cpfRequisitor;

            if (!isAdmin && !isOwner)
                return Unauthorized(new { message = "Você não tem permissão para deletar esta reserva" });

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}