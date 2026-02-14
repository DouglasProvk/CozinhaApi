namespace CozinhaApi.DTOs.Reserva
{
    public class UpdateReservaRequest
    {
        public string? Observacoes { get; set; }
        public string Status { get; set; }
        public string CPFRequisitor { get; set; }
    }
}
