namespace CozinhaApi.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public string NomePessoa { get; set; }
        public int PratoId { get; set; }
        public string? PratoNome { get; set; }
        public DateTime DataRefeicao { get; set; }
        public string Periodo { get; set; } // almoco, jantar
        public string? Observacoes { get; set; }
        public string Status { get; set; } = "confirmada"; // confirmada, cancelada, servida
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation
        public virtual Prato? Prato { get; set; }
    }
}
