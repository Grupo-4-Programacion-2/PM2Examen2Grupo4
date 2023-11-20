using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2Examen2Grupo4.Models
{
    public class Contactos
    {
        public int id { get; set; }
        public string nombre { get; set; }

        public String telefono { get; set; }

        public String latitud { get; set; }

        public String longitud { get; set; }

        public String imagen { get; set; }       

    }
}
