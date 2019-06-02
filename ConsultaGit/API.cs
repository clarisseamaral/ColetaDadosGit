using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ConsultaGit
{
    public static class API<T>
    {
        public static async Task<T> Consulta(string requestUri)
        {
           // await Task.Delay(30000);

            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                client.BaseAddress = new Uri("https://api.github.com");

                client.DefaultRequestHeaders.Add("User-Agent", "Anything");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.cloak-preview"));

                var teste = ConfigurationManager.AppSettings["AutenticacaoGit"];
                var byteArray = new UTF8Encoding().GetBytes(teste);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

                var response = await client.SendAsync(request);

                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
