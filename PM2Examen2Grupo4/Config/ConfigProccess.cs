using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2Examen2Grupo4.Config
{
    public static class ConfigProccess
    {

        public static string ipaddress = "18.188.250.246/";
        public static string restapi = "PM02/";
        public static string postRoute = "create.php";

        public static string ApiCreate  =  "http://" + ipaddress + restapi + postRoute;
   //     public static string EndpointPost = "http://192.168.88.227/apirest/create.php";
        //public static string EndpointGet = "http://192.168.0.4/ApiEmple/public/empleados/show";

    }
}
