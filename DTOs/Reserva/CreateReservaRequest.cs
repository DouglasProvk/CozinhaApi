namespace CozinhaApi.DTOs.Reserva
{
    public class CreateReservaRequest
    {
        public string NomePessoa { get; set; }
        public string CPFFuncionario { get; set; }
        public int PratoId { get; set; }
        public string Periodo { get; set; }
        public string? Observacoes { get; set; }
    }
}
