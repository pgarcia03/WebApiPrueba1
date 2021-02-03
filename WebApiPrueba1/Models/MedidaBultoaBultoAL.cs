using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPrueba1.Models
{
    public class MedidaBultoaBultoAL
    {
        public int IdPorder { get; set; }
        public int idEstilo { get; set; }
        public string Size { get; set; }
        public int Cantidad { get; set; }
        public bool estado { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
