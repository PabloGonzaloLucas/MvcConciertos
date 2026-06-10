using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcConciertos.Services
{
    public class ServiceLambdaIA
    {
        private string ApiUrl;
        private MediaTypeWithQualityHeaderValue header;

        public ServiceLambdaIA()
        {
            this.ApiUrl = "https://xy9gacylvf.execute-api.us-east-1.amazonaws.com/default/my-function-examen-pgl";
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

                    return (T)(object)json;
                }

                return default(T);
            }
        }




        public async Task<string> GetRespuestaIAAsync(string pregunta)
        {

            // Crear un objeto JSON (payload) con la propiedad "pregunta" y enviarlo al API
            var payload = new { pregunta = pregunta };
            string respuesta = await this.CallApiAsync<string>(this.ApiUrl, HttpMethod.Get, payload);
            return respuesta;
        }
    }
}
