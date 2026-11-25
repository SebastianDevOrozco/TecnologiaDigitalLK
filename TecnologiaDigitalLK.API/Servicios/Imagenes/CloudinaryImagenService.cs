using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using TecnologiaDigitalLK.API.Configuracion;
using TecnologiaDigitalLK.API.Interfaces.Imagenes;

namespace TecnologiaDigitalLK.API.Servicios.Imagenes
{
    public class CloudinaryImagenService : IImagenService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryImagenService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<(string Url, string PublicId)> SubirImagenAsync(IFormFile archivo, string carpeta = "productos")
        {
            if (archivo == null || archivo.Length == 0)
                throw new ArgumentException("El archivo de imagen es inválido o está vacío.");

            var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            if (!extensionesPermitidas.Contains(extension))
                throw new ArgumentException("Formato no permitido. Solo se aceptan imágenes JPG, JPEG, PNG o WEBP.");

            await using var stream = archivo.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(archivo.FileName, stream),
                Folder = $"tecnologia-digital-lk/{carpeta}",
                Transformation = new Transformation().Width(800).Height(800).Crop("limit").Quality("auto")
            };

            var resultado = await _cloudinary.UploadAsync(uploadParams);

            if (resultado.Error != null)
                throw new Exception($"Error al subir imagen a Cloudinary: {resultado.Error.Message}");

            return (resultado.SecureUrl.ToString(), resultado.PublicId);
        }

        public async Task EliminarImagenAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId)) return;

            var deleteParams = new DeletionParams(publicId);
            await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
