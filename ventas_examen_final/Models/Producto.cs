using System.Text.Json.Serialization;

namespace ventas_examen_final.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int CantidadStock { get; set; }

        // Clave foránea a Categoria
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }  // Relación muchos a uno con Categoria

        // Relación uno a muchos con DetalleVenta
        [JsonIgnore]
        public ICollection<DetalleVenta> DetallesVenta { get; set; }
    }
}
