namespace TecnologiaDigitalLK.API.Interfaces.Imagenes
{
    public interface IImagenService
    {
        Task<(string Url, string PublicId)> SubirImagenAsync(IFormFile archivo, string carpeta = "productos");
        Task EliminarImagenAsync(string publicId);
    }
}
