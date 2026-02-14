using CozinhaApi.Models;
using CozinhaApi.DTOs.Auth;
using CozinhaAPI.Data;
using CozinhaApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CozinhaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Backend está funcionando!", timestamp = DateTime.UtcNow });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Cpf))
                    return BadRequest(new { message = "CPF é obrigatório" });

                if (string.IsNullOrWhiteSpace(request.Senha))
                    return BadRequest(new { message = "Senha é obrigatória" });

                var cpfLimpo = request.Cpf.Replace(".", "").Replace("-", "").Trim();

                var funcionario = await _context.Funcionarios
                    .FirstOrDefaultAsync(f => f.CPF == cpfLimpo && f.Ativo);

                if (funcionario == null)
                    return Unauthorized(new { message = "CPF não encontrado ou funcionário inativo" });

                // Verificar senha
                if (!PasswordHasher.VerifyPassword(request.Senha, funcionario.SenhaHash))
                    return Unauthorized(new { message = "Senha incorreta" });

                var response = new AuthResponseDto
                {
                    Id = funcionario.Id,
                    Nome = funcionario.Nome,
                    Email = funcionario.Email,
                    CPF = funcionario.CPF,
                    Cargo = funcionario.Cargo,
                    IsAdmin = funcionario.IsAdmin
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro no servidor: {ex.Message}" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Nome))
                    return BadRequest(new { message = "Nome é obrigatório" });

                if (string.IsNullOrWhiteSpace(request.Email))
                    return BadRequest(new { message = "Email é obrigatório" });

                if (string.IsNullOrWhiteSpace(request.Cpf))
                    return BadRequest(new { message = "CPF é obrigatório" });

                if (string.IsNullOrWhiteSpace(request.Senha) || request.Senha.Length < 6)
                    return BadRequest(new { message = "Senha deve ter no mínimo 6 caracteres" });

                var cpfLimpo = request.Cpf.Replace(".", "").Replace("-", "").Trim();

                if (cpfLimpo.Length != 11)
                    return BadRequest(new { message = "CPF deve ter 11 dígitos" });

                var cpfJaExiste = await _context.Funcionarios
                    .AnyAsync(f => f.CPF == cpfLimpo);

                if (cpfJaExiste)
                    return BadRequest(new { message = "Este CPF já está registrado" });

                // Hash da senha
                var senhaHash = PasswordHasher.HashPassword(request.Senha);

                var funcionario = new Funcionario
                {
                    Nome = request.Nome.Trim(),
                    Email = request.Email.Trim(),
                    CPF = cpfLimpo,
                    SenhaHash = senhaHash,
                    Telefone = request.Telefone?.Trim() ?? "",
                    Cargo = request.Cargo?.Trim() ?? "Funcionário",
                    Departamento = request.Departamento?.Trim() ?? "Salão",
                    DataAdmissao = DateTime.UtcNow,
                    Ativo = true,
                    IsAdmin = false
                };

                _context.Funcionarios.Add(funcionario);
                await _context.SaveChangesAsync();

                var response = new AuthResponseDto
                {
                    Id = funcionario.Id,
                    Nome = funcionario.Nome,
                    Email = funcionario.Email,
                    CPF = funcionario.CPF,
                    Cargo = funcionario.Cargo,
                    IsAdmin = funcionario.IsAdmin
                };

                return Ok(response);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = $"Erro ao salvar no banco: {dbEx.InnerException?.Message ?? dbEx.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro no servidor: {ex.Message}" });
            }
        }
    }
}

