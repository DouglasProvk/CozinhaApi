namespace CozinhaApi.DTOs.Prato
{
    public class CreatePratoRequest
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Categoria { get; set; }
        public bool Disponivel { get; set; }
        public bool EhPrincipal { get; set; } = false;
        public string CPFAdmin { get; set; }
    }
}
