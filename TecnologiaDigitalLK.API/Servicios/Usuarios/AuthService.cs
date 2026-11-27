using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TecnologiaDigitalLK.API.Configuracion;
using TecnologiaDigitalLK.API.Data;
using TecnologiaDigitalLK.API.DTOs.Usuarios;
using TecnologiaDigitalLK.API.Interfaces.Usuarios;
using TecnologiaDigitalLK.API.Modelos;

namespace TecnologiaDigitalLK.API.Servicios.Usuarios
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        private readonly JwtSettings _jwtSettings; // esto es necesario para leer la clave jwt

        public AuthService(IMapper mapper, AppDbContext appDbContext, IOptions<JwtSettings> jwtOptions)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
            _jwtSettings = jwtOptions.Value;

        }

        public async Task<UsuarioReadDto> RegistrarUsuario(UsuarioCreateDto usuarioCreateDto)
        {
            await ValidarUsuario(usuarioCreateDto);
            await VerificarSucursal(usuarioCreateDto.SucursalId);

            //Mapeo del UsuarioDto --> Usuario
            var usuarioNuevo = _mapper.Map<Usuario>(usuarioCreateDto);


            // Encriptar la contraseña usando BCrypt
            usuarioNuevo.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuarioCreateDto.PasswordHash);

            //guardar en la base de datos 
            _appDbContext.Usuarios.Add(usuarioNuevo);
            await _appDbContext.SaveChangesAsync();

          
            //cargar el usuario para devolver con su sucursal
            await _appDbContext.Entry(usuarioNuevo).Reference(u => u.Sucursal).LoadAsync();


            //Mapeo de la entidad guardada al DTO de lectura 
            return _mapper.Map<UsuarioReadDto>(usuarioNuevo);
        }



        public async Task<string?> Login(string UserName, string password)
        {
            //busca el usuario por el username
            var usuario = await _appDbContext.Usuarios.FirstOrDefaultAsync(u => u.Username == UserName);

            // 2. Si no existe, o la contraseña NO coincide (BCrypt.Verify desencripta y compara)
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
                throw new UnauthorizedAccessException("Usuario o Contraseña incorrecta.");// Credenciales inválidas

            if (!usuario.Activo)
                throw new UnauthorizedAccessException("Este usuario se encuentra inactivo.");

            return GenerarToken(usuario);
        }

        public async Task<UsuarioReadDto> ObtenerUsuarioPorIdAsync(int id)
        {
            var usuario = await _appDbContext.Usuarios
                .AsNoTracking()
                .Include(u => u.Sucursal)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                throw new KeyNotFoundException($"No se encontró el usuario con ID {id}.");

            return _mapper.Map<UsuarioReadDto>(usuario);
        }

        private async Task ValidarUsuario(UsuarioCreateDto usuario)
        {
            //validar si el correo ya existe
            var existeUsuario = await _appDbContext.Usuarios
                .AnyAsync(u => u.Username.ToLower() == usuario.Username.ToLower());

            if (existeUsuario)
                throw new ArgumentException("El usuario ya está registrado.");
        }
        private async Task VerificarSucursal(int? id)
        {
            // Si es null (por ejemplo, un Admin global), no necesita validar sucursal
            if (!id.HasValue) return;

            var sucursal = await _appDbContext.Sucursales
                .AnyAsync(s => s.Id == id);

            if (!sucursal)
                throw new ArgumentException("La sucursal seleccionada no existe.");
        }

        private string GenerarToken(Usuario usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Información que irá dentro del token 
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("SucursalId", usuario.SucursalId?.ToString() ?? string.Empty),
            };

            // Armamos el token
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1), // El token expira en 1 día
                signingCredentials: creds
            );

            // Lo devolvemos como texto (string)
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
