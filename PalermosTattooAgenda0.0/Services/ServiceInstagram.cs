using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using PalermosTattooAgenda0.Web.Models;
using System.Text.Json;

namespace PalermosTattooAgenda0.Web.Services
{
    public class ServiceInstagram : IServiceInstagram
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ServiceInstagram> _logger;

        public ServiceInstagram(HttpClient http,
            IConfiguration configuration,
            IMemoryCache cache,
            ILogger<ServiceInstagram> logger)
        {
            _http = http;
            _configuration = configuration;
            _cache = cache;
            _logger = logger;
        }
        public async Task<List<PublicacionInstagram>>
           ListAsync(int cantidad)
        {
            var token =
                _configuration["Instagram:AccessToken"];
            if (string.IsNullOrEmpty(token))
            {
                return new List<PublicacionInstagram>();
            }

            //Si ya esta en cache, no consultar Instagram
            if (_cache.TryGetValue("instagram",
                out List<PublicacionInstagram>? guardadas))
            {
                return guardadas!;
            }

            try
            {
                var url =
                    "https://graph.instagram.com/me/media"
                    + "?fields=media_type,media_url,"
                    + "thumbnail_url,permalink"
                    + "&limit=" + cantidad
                    + "&access_token=" + token;
                var json = await _http.GetStringAsync(url);
                using var doc = JsonDocument.Parse(json);

                var lista = new List<PublicacionInstagram>();
                var datos = doc.RootElement
                    .GetProperty("data").EnumerateArray();
                foreach (var item in datos)
                {
                    //Los videos usan su miniatura
                    var tipo = item
                        .GetProperty("media_type").GetString();
                    var campo = tipo == "VIDEO"
                        ? "thumbnail_url" : "media_url";
                    if (!item.TryGetProperty(campo,
                        out var imagen))
                    {
                        continue;
                    }

                    //Si es video, guardar tambien el archivo
                    string? video = null;
                    if (tipo == "VIDEO"
                        && item.TryGetProperty("media_url",
                            out var mp4))
                    {
                        video = mp4.GetString();
                    }

                    lista.Add(new PublicacionInstagram
                    {
                        Imagen = imagen.GetString() ?? "",
                        Enlace = item.GetProperty("permalink")
                            .GetString() ?? "",
                        Video = video
                    });
                }
            

                //Guardar 1 hora para no consultar siempre
                _cache.Set("instagram", lista,
                    TimeSpan.FromHours(1));
                return lista;
            }
            catch (Exception ex)
            {
                //Si Instagram falla, se usan las fotos fijas
                _logger.LogWarning(ex,
                    "No se pudo consultar Instagram");
                return new List<PublicacionInstagram>();
            }
        }
    }
}