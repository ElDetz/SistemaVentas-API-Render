namespace ventas_examen_final.DTOs
{
    public class VentaCreateDto
    {
        public int UsuarioId { get; set; }  // Solo el ID del usuario
        public DateTime FechaVenta { get; set; }
        public decimal Total { get; set; }
    }
}
