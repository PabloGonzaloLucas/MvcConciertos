using MvcConciertos.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcConciertos.Services
{
    public class ServiceConciertos
    {
        private string ApiUrl;
        private MediaTypeWithQualityHeaderValue header;

        public ServiceConciertos()
        {
            this.ApiUrl = "https://0lt7jnj9b3.execute-api.us-east-1.amazonaws.com/Prod/";
            this.header = new MediaTypeWithQualityHeaderValue("application/json");

        }

        public async Task<T> CallApiAsync<T>(string request)
        {
            return await this.CallApiAsync<T>(request, HttpMethod.Get);
        }

        public async Task<T> CallApiAsync<T>(string request, HttpMethod method, object? body = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.ApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);

                using HttpRequestMessage message = new HttpRequestMessage(method, request);
                if (body != null)
                {
                    string jsonBody = JsonConvert.SerializeObject(body);
                    message.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = await client.SendAsync(message);
                if (response.IsSuccessStatusCode == true)
                {
                    if (response.Content == null)
                    {
                        return default(T);
                    }

                    string json = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        return default(T);
                    }

                    T data = JsonConvert.DeserializeObject<T>(json);
                    return data;
                }

                return default(T);
            }
        }


        public async Task<List<Evento>> GetEventosAsync()
        {
            string request = "api/Conciertos";
            List<Evento> eventos = await this.CallApiAsync<List<Evento>>(request);
            return eventos;
        }
        public async Task<List<CategoriaEvento>> GetCategoriasAsync()
        {
            string request = "api/Conciertos/GetCategorias";
            List<CategoriaEvento> categoriaEventos = await this.CallApiAsync<List<CategoriaEvento>>(request);
            return categoriaEventos;
        }
        public async Task<List<Evento>> GetEventosCategoriaAsync(int categoriaId)
        {
            string request = $"api/Conciertos/GetEventosCategoria/{categoriaId}";
            List<Evento> eventos = await this.CallApiAsync<List<Evento>>(request);
            return eventos;
        }



    }
}
