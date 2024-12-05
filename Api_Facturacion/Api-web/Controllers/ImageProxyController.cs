using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Api_web.Controllers
{
    public class ImageProxyController : ApiController
    {
        [HttpGet]
        [Route("api/imageproxy")]
        public async Task<HttpResponseMessage> GetImage(string url)
        {
            try
            {
                // Asegurarnos de que la URL esté correctamente decodificada
                string decodedUrl = HttpUtility.UrlDecode(url);

                using (var client = new HttpClient())
                {
                    // Configurar un timeout razonable
                    client.Timeout = TimeSpan.FromSeconds(10);

                    // Agregar headers necesarios para Google Drive
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                    client.DefaultRequestHeaders.Add("Accept", "image/webp,image/apng,image/*,*/*;q=0.8");

                    var response = await client.GetAsync(decodedUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsByteArrayAsync();
                        var result = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ByteArrayContent(content)
                        };

                        // Copiar el Content-Type de la respuesta original
                        result.Content.Headers.ContentType = response.Content.Headers.ContentType;

                        // Agregar headers para permitir el cacheo
                        result.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
                        {
                            Public = true,
                            MaxAge = TimeSpan.FromDays(1)
                        };

                        return result;
                    }

                    // Log del error para debugging
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Error fetching image. Status: {response.StatusCode}, Content: {errorContent}");

                    return new HttpResponseMessage(response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                // Log del error para debugging
                System.Diagnostics.Debug.WriteLine($"Exception in GetImage: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Error processing image: {ex.Message}")
                };
            }
        }
    }
}