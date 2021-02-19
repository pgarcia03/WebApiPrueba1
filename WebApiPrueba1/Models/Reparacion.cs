using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPrueba1.Models
{
    public class Reparacion
    {
        public int id { get; set; }
        public int idModulo { get; set; }
        public int idPosicion { get; set; }
        public int idDefecto { get; set; }
        public string posicion { get; set; }
        public string defecto{ get; set; }
        public string inspector { get; set; }

        public string corte { get; set; }
        public string color { get; set; }

        public int cantidad { get; set; }
    }

    public class posicionDefecto
    {
        public int id { get; set; }
        public int idDefecto { get; set; }
        public string defecto { get; set; }
    }

    public class ReparacionMasterJson
    {     
        public int idModulo { get; set; }
        public int idPosicion { get; set; }  
        public string posicion { get; set; }
        public List<posicionDefecto> defecto { get; set; }
    }
}
