using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TecnologiaDigitalLK.API.Data;
using TecnologiaDigitalLK.API.DTOs.Usuarios;
using TecnologiaDigitalLK.API.Interfaces.Usuarios;
using TecnologiaDigitalLK.API.Modelos;

namespace TecnologiaDigitalLK.API.Servicios.Usuarios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;

        public UsuarioService(IMapper mapper, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<UsuarioReadDto> ActualizarUsuario(int usuarioId, UsuarioUpdateDto usuarioDto)
        {
            var usuario = await VerificarUsuario(usuarioId);

            //verifico si al actualizar, el username esta repetido
            var usernameEnUso = await _appDbContext.Usuarios
                .AnyAsync(u => u.Username.ToLower() == usuarioDto.Username.ToLower() && u.Id != usuarioId);

            if (usernameEnUso)
                throw new ArgumentException("El nombre de usuario ya está en uso por otro empleado.");

            await VerificarSucursal(usuarioDto.SucursalId);

            _mapper.Map(usuarioDto, usuario);
            await _appDbContext.SaveChangesAsync();

            // Recargar la sucursal 
            await _appDbContext.Entry(usuario).Reference(u => u.Sucursal).LoadAsync();

            return _mapper.Map<UsuarioReadDto>(usuario);
        }
        public async Task CambiarEstadoUsuario(int usuarioId, bool nuevoEstado)
        {
            var usuario = await VerificarUsuario(usuarioId);

            usuario.Activo = nuevoEstado;
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UsuarioReadDto>> ListarUsuarios()
        {
            var usuarios = await _appDbContext.Usuarios
                .Include(u => u.Sucursal)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UsuarioReadDto>>(usuarios);
        }

        private async Task<Usuario> VerificarUsuario(int id)
        {
            var usuario = await _appDbContext.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            return usuario;
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
    }
}
