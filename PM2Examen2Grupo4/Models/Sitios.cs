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
        public int Id { get; set; }
        public string descripcionAudio { get; set; }

        public Double lat { get; set; }

        public Double lgn { get; set; }

        public string audio { get; set; }

        public byte[] imageSignature { get; set; }       

    }
}
