namespace ventas_examen_final.DTOs
{
    public class DetalleVentaCreateDto
    {
        public int VentaId { get; set; }  // Solo el ID de la venta
        public int ProductoId { get; set; }  // Solo el ID del producto
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
