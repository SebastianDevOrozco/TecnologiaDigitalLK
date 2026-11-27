using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TecnologiaDigitalLK.API.Data;
using TecnologiaDigitalLK.API.DTOs.Categorias;
using TecnologiaDigitalLK.API.Interfaces.Categorias;
using TecnologiaDigitalLK.API.Modelos;

namespace TecnologiaDigitalLK.API.Servicios.Categorias
{
    public class CategoriaService : ICategoriaService 
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;

        public CategoriaService(IMapper mapper, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        public async Task<CategoriaReadDto> CrearCategoria(CategoriaCreateDto categoriaCreateDto)
        {
            await ValidarNombreCategoria(categoriaCreateDto.Nombre);

            var nuevaCategoria = _mapper.Map<Categoria>(categoriaCreateDto);

            _appDbContext.Categorias.Add(nuevaCategoria);
            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<CategoriaReadDto>(nuevaCategoria);
        }

        public async Task<IEnumerable<CategoriaReadDto>> ListarCategorias()
        {
            var categorias = await _appDbContext.Categorias
                .ToListAsync();

            return _mapper.Map<IEnumerable<CategoriaReadDto>>(categorias);
        }

        public async Task<CategoriaReadDto> ObtenerCategoriaPorIdAsync(int id)
        {
            var categoria = await _appDbContext.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
                throw new KeyNotFoundException($"No se encontró la categoría con ID {id}.");

            return _mapper.Map<CategoriaReadDto>(categoria);
        }

        public async Task<CategoriaReadDto> ActualizarCategoria(int categoriaId, CategoriaUpdateDto categoriaUpdateDto)
        {
            var categoria = await ValidarExistenciaCategoria(categoriaId);

            await ValidarNombreCategoria(categoriaUpdateDto.Nombre);

            _mapper.Map(categoriaUpdateDto, categoria);

            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<CategoriaReadDto>(categoria);
        }

        public async Task EditarEstadoCategoria(int categoriaId, bool nuevoEstado)
        {
            var categoria = await ValidarExistenciaCategoria(categoriaId);

            categoria.Activo = nuevoEstado;

            if (!nuevoEstado)
                await DesactivarProductosCascada(categoria);

            await _appDbContext.SaveChangesAsync();
        }


        //METODOS SECUNDARIOS

        private async Task ValidarNombreCategoria(string nombre)
        {
            var nombreExiste = await _appDbContext.Categorias
                 .AnyAsync(c => c.Nombre.ToLower() == nombre.Trim().ToLower());

             if (nombreExiste)
                 throw new ArgumentException($"Ya existe una categoría con el nombre '{nombre}'.");
        }

        private async Task<Categoria> ValidarExistenciaCategoria(int idCategoria)
        {
            var categoria = await _appDbContext.Categorias
                .FirstOrDefaultAsync(c => c.Id == idCategoria);

            if (categoria == null)
                throw new KeyNotFoundException($"La Categoria con el ID {idCategoria} No Existe");

            return categoria;
        }

        private async Task DesactivarProductosCascada(Categoria categoria)
        {
            foreach (var producto in categoria.Productos)
            {
                producto.Activo = false;
            }
        }

    }
}
