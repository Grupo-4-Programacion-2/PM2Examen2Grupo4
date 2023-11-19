using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PM2Examen2Grupo4.Controllers
{
    public static class SitiosController
    {

        public async static Task<Models.Msg> CreateEmple(Models.Sitios sitios){
            var msg = new Models.Msg();

            String jsonObject = JsonConvert.SerializeObject(sitios);
            System.Net.Http.StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = null;

                responseMessage = await client.PostAsync(Config.ConfigProccess.EndpointPost, content);

                if (responseMessage != null)
                {
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var result = responseMessage.Content.ReadAsStringAsync().Result;
                        msg = JsonConvert.DeserializeObject<Models.Msg>(result);
                    }
                }
            }

            return msg;
        }

    }
}
