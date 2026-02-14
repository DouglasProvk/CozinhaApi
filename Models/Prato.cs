namespace CozinhaApi.Models
{
    public class Prato
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public string Categoria { get; set; } // carne, frango, peixe, vegetariano, vegano
        public bool Disponivel { get; set; } = true;
        public bool EhPrincipal { get; set; } = false; // Define se é o prato principal
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
