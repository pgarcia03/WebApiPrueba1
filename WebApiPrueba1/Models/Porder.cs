using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPrueba1.Models
{
    public class Porder
    {
        public int ObjectId{ get; set; }
        public string Corte { get; set; }
        public int Unidades{ get; set; }
        public int IdEstilo { get; set; }
        public string Estilo { get; set; }
    }
}
