namespace ventas_examen_final.Models
{
    public class DetalleVenta
    {
        public int Id { get; set; }

        // Clave foránea a Venta
        public int VentaId { get; set; }
        public Venta Venta { get; set; }  // Relación muchos a uno con Venta

        // Clave foránea a Producto
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }  // Relación muchos a uno con Producto

        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
