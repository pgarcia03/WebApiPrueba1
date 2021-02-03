using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPrueba1.Models
{
    public class Estilo
    {
        public int ObjectId { get; set; }
        public string NombreEstilo { get; set; }
        public bool Estado{ get; set; }
        public float Precio { get; set; }
    }
}
