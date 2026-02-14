namespace CozinhaApi.Models
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; } // CPF único para identificar o funcionário
        public string SenhaHash { get; set; } // Senha com hash bcrypt
        public string Cargo { get; set; } // chef, sous-chef, cozinheiro, ajudante, etc
        public string Departamento { get; set; } // cozinha, salão, gerencia, etc
        public bool Ativo { get; set; } = true;
        public bool IsAdmin { get; set; } = false; // Define se é admin (pode editar/deletar pratos)
        public DateTime DataAdmissao { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
    }
}
