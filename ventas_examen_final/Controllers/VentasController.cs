using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ventas_examen_final.Data;
using ventas_examen_final.Models;
using ventas_examen_final.DTOs;

namespace ventas_examen_final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Ventas
        [HttpPost]
        public async Task<ActionResult<Venta>> PostVenta(VentaCreateDto ventaDto)
        {
            var venta = new Venta
            {
                UsuarioId = ventaDto.UsuarioId,
                FechaVenta = ventaDto.FechaVenta,
                Total = ventaDto.Total
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVenta), new { id = venta.Id }, venta);
        }

        // GET: api/Ventas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentas()
        {
            return await _context.Ventas.Include(v => v.DetallesVenta)  // Incluir detalles de venta
        .ThenInclude(dv => dv.Producto) // Incluir el producto en cada detalle de venta
        .ThenInclude(p => p.Categoria)  // Incluir la categoría asociada al producto
        .Include(v => v.Usuario)       // Incluir el usuario relacionado
        .ToListAsync();

        }

        // GET: api/Ventas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVenta(int id)
        {
            var venta = await _context.Ventas.Include(v => v.DetallesVenta)  // Incluir detalles de venta
        .ThenInclude(dv => dv.Producto) // Incluir el producto en cada detalle de venta
        .ThenInclude(p => p.Categoria)  // Incluir la categoría asociada al producto
        .Include(v => v.Usuario)       // Incluir el usuario relacionado
        .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
            {
                return NotFound();
            }

            return venta;
        }

        // PUT: api/Ventas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenta(int id, VentaCreateDto ventaDto)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }

            venta.FechaVenta = ventaDto.FechaVenta;
            venta.Total = ventaDto.Total;

            _context.Entry(venta).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Ventas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenta(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.DetallesVenta)
                .FirstOrDefaultAsync(v => v.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            // Sumar stock de todos los productos de los detalles
            foreach (var detalle in venta.DetallesVenta)
            {
                var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                if (producto != null)
                {
                    producto.CantidadStock += detalle.Cantidad;
                    _context.Entry(producto).State = EntityState.Modified;
                }
            }

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
