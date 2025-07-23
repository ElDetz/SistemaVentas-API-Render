namespace ventas_examen_final.DTOs
{
    public class ProductoCreateDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int CantidadStock { get; set; }

        // Solo el ID de la categoría
        public int CategoriaId { get; set; }
    }
}
