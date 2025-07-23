using System.Text.Json.Serialization;

namespace ventas_examen_final.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Rol { get; set; } // Ejemplo: Admin, Vendedor, Usuario

        // Relación uno a muchos con Venta
        [JsonIgnore]
        public ICollection<Venta> Ventas { get; set; }
    }
}
