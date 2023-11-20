using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2Examen2Grupo4.Models
{
    public class Sitios
    {
        public int id { get; set; }
        public string descripcion { get; set; }

        public Double latitud { get; set; }

        public Double longitud { get; set; }

        public byte[] firmaDigital { get; set; }

        public string audioFile { get; set; }

    }
}
