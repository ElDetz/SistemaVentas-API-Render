using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ventas_examen_final.Data;
using ventas_examen_final.Models;
using ventas_examen_final.DTOs;

namespace ventas_examen_final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleVentasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DetalleVentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/DetalleVentas
        [HttpPost]
        public async Task<ActionResult<DetalleVenta>> PostDetalleVenta(DetalleVentaCreateDto detalleVentaDto)
        {
            // Buscar el producto
            var producto = await _context.Productos.FindAsync(detalleVentaDto.ProductoId);
            if (producto == null)
            {
                return BadRequest("Producto no encontrado");
            }
            if (producto.CantidadStock < detalleVentaDto.Cantidad)
            {
                return BadRequest($"Stock insuficiente para el producto '{producto.Nombre}'. Stock actual: {producto.CantidadStock}");
            }

            // Descontar stock
            producto.CantidadStock -= detalleVentaDto.Cantidad;
            _context.Entry(producto).State = EntityState.Modified;

            var detalleVenta = new DetalleVenta
            {
                VentaId = detalleVentaDto.VentaId,
                ProductoId = detalleVentaDto.ProductoId,
                Cantidad = detalleVentaDto.Cantidad,
                Precio = detalleVentaDto.Precio
            };

            _context.DetalleVentas.Add(detalleVenta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDetalleVenta), new { id = detalleVenta.Id }, detalleVenta);
        }

        // GET: api/DetalleVentas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleVenta>>> GetDetalleVentas()
        {
            return await _context.DetalleVentas
                .Include(dv => dv.Producto)
                .Include(dv => dv.Venta)
                .ToListAsync();
        }

        // GET: api/DetalleVentas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleVenta>> GetDetalleVenta(int id)
        {
            var detalleVenta = await _context.DetalleVentas
                .Include(dv => dv.Producto)
                .Include(dv => dv.Venta)
                .FirstOrDefaultAsync(dv => dv.Id == id);

            if (detalleVenta == null)
            {
                return NotFound();
            }

            return detalleVenta;
        }

        // PUT: api/DetalleVentas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleVenta(int id, DetalleVentaCreateDto detalleVentaDto)
        {
            // Ahora solo se compara el ID de la URL
            var detalleVenta = await _context.DetalleVentas.FindAsync(id);
            if (detalleVenta == null)
            {
                return NotFound();
            }

            detalleVenta.VentaId = detalleVentaDto.VentaId;
            detalleVenta.ProductoId = detalleVentaDto.ProductoId;
            detalleVenta.Cantidad = detalleVentaDto.Cantidad;
            detalleVenta.Precio = detalleVentaDto.Precio;

            _context.Entry(detalleVenta).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/DetalleVentas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleVenta(int id)
        {
            var detalleVenta = await _context.DetalleVentas.FindAsync(id);
            if (detalleVenta == null)
            {
                return NotFound();
            }

            // Sumar stock al producto
            var producto = await _context.Productos.FindAsync(detalleVenta.ProductoId);
            if (producto != null)
            {
                producto.CantidadStock += detalleVenta.Cantidad;
                _context.Entry(producto).State = EntityState.Modified;
            }

            _context.DetalleVentas.Remove(detalleVenta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
