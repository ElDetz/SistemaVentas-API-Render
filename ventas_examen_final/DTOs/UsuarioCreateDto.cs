namespace ventas_examen_final.DTOs
{
    public class UsuarioCreateDto
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Rol { get; set; } // Ejemplo: Admin, Vendedor, Usuario
    }
}
