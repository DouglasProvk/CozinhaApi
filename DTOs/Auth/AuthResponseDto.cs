namespace CozinhaApi.DTOs.Auth
{
    public class AuthResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Cargo { get; set; }
        public bool IsAdmin { get; set; }
    }
}
