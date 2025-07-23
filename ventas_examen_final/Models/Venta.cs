using System.Text.Json.Serialization;

namespace ventas_examen_final.Models
{
    public class Venta
    {
        public int Id { get; set; }

        // Clave foránea a Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }  // Relación muchos a uno con Usuario

        public DateTime FechaVenta { get; set; }
        public decimal Total { get; set; }

        // Relación uno a muchos con DetalleVenta
        public ICollection<DetalleVenta> DetallesVenta { get; set; }
    }
}
