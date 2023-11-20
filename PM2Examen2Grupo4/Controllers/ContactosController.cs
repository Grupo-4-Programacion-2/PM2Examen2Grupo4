using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PM2Examen2Grupo4.Controllers
{
    public static class ContactosController
    {

        public async static Task<Models.Msg> CreateEmple(Models.Contactos contactos){
            var msg = new Models.Msg();

            String jsonObject = JsonConvert.SerializeObject(contactos);

            Console.WriteLine(jsonObject);
            System.Net.Http.StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = null;

                responseMessage = await client.PostAsync(Config.ConfigProccess.ApiCreate, content);

                Console.WriteLine(Config.ConfigProccess.ApiCreate);

                if (responseMessage != null)
                {
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var result = responseMessage.Content.ReadAsStringAsync().Result;
                        //return await Task.FromResult(true);
                        //var resultado = "Creado Correctamente";
                        msg = JsonConvert.DeserializeObject<Models.Msg>(result);
                    }
                }
            }

            return msg;
        }

    }
}
