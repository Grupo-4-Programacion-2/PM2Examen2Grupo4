using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PM2Examen2Grupo4.Config;

namespace PM2Examen2Grupo4.Controllers
{
    public static class SitiosController
    {

        public async static Task<Models.Msg> CreateEmple(Models.Sitios sitios)
        {
            var msg = new Models.Msg();

            String jsonObject = JsonConvert.SerializeObject(sitios);
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
                        msg.message = "Creado Correctamente";
                        //msg = JsonConvert.DeserializeObject<Models.Msg>(result);
                    }
                }
            }

            return msg;
        }

        public async static Task<List<Models.Sitios>> GetSitios()
        {
            List<Models.Sitios> emplelist = new List<Models.Sitios>();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseMessage = null;
                    responseMessage = await client.GetAsync(Config.ConfigProccess.ApiGet);

                    if (responseMessage != null)
                    {
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var result = responseMessage.Content.ReadAsStringAsync().Result;
                            emplelist = JsonConvert.DeserializeObject<List<Models.Sitios>>(result);
                        }
                    }
                }

                return emplelist;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message); 
                Models.Msg msg = new Models.Msg();
                msg.message = "Error no se proceso la transaccion";
                return null;
            }
        }

        public async static Task<Models.Msg> DeleteEmple(int empleId)
        {
            var msg = new Models.Msg();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = null;

                string deleteEndpoint = $"{Config.ConfigProccess.ApiDelete}" + $"{empleId}";

                Console.WriteLine(deleteEndpoint);

                responseMessage = await client.DeleteAsync(deleteEndpoint);
                Console.WriteLine(deleteEndpoint);

                if (responseMessage != null)
                {
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var result = responseMessage.Content.ReadAsStringAsync().Result;
                        msg.message = "Eliminado Correctamente";
                    }
                    else
                    {
                        msg.message = $"Error al eliminar: {responseMessage.ReasonPhrase}";
                    }
                }
            }

            return msg;
        }

        public async static Task<Models.Msg> UpdateEmple(Models.Sitios sitios)
        {
            var msg = new Models.Msg();

            string jsonObject = JsonConvert.SerializeObject(sitios);
            Console.WriteLine(jsonObject);

            System.Net.Http.StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = null;

                responseMessage = await client.PutAsync($"{Config.ConfigProccess.ApiUpdate}?id={sitios.id}", content);
                Console.WriteLine(Config.ConfigProccess.ApiUpdate);

                if (responseMessage != null)
                {
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var result = responseMessage.Content.ReadAsStringAsync().Result;
                        msg.message = "Actualizado Correctamente";
                        // msg = JsonConvert.DeserializeObject<Models.Msg>(result);
                    }
                    else
                    {
                        msg.message = $"Error al actualizar: {responseMessage.ReasonPhrase}";
                    }
                }
            }

            return msg;
        }

    }

}
