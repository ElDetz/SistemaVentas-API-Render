using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ventas_examen_final.Data;
using ventas_examen_final.Models;
using ventas_examen_final.DTOs;

namespace ventas_examen_final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(UsuarioCreateDto usuarioDto)
        {
            var usuario = new Usuario
            {
                Nombre = usuarioDto.Nombre,
                Correo = usuarioDto.Correo,
                Contrasena = usuarioDto.Contrasena,  // Recuerda cifrar la contraseña antes de guardarla en un entorno real.
                FechaRegistro = DateTime.Now,
                Rol = usuarioDto.Rol
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // POST: api/Usuarios/login
        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == loginDto.Correo && u.Contrasena == loginDto.Contrasena);
            if (usuario == null)
            {
                return Unauthorized();
            }
            usuario.Contrasena = null;
            return usuario;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioCreateDto usuarioDto)
        {
            // El parámetro id ya está en la URL, por lo que no necesitas acceder a `usuarioDto.Id`
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();  // Si no se encuentra el usuario con ese id, se devuelve 404
            }

            // Se actualizan las propiedades del usuario
            usuario.Nombre = usuarioDto.Nombre;
            usuario.Correo = usuarioDto.Correo;
            usuario.Contrasena = usuarioDto.Contrasena;
            usuario.FechaRegistro = DateTime.Now;
            usuario.Rol = usuarioDto.Rol;

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();  // Retorna un 204 No Content si la actualización es exitosa
        }


        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
