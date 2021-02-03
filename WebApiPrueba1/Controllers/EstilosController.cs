using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using WebApiPrueba1.Models;

namespace WebApiPrueba1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstilosController : ControllerBase
    {
        public IConfiguration Iconfiguration { get; }

        public EstilosController(IConfiguration iconfiguration)
        {
            Iconfiguration = iconfiguration;
        }

        [HttpGet]
        public ActionResult get()
        {
            var connection = Iconfiguration["ConnectionStrings:ConeccionPrueba"];

            using (SqlConnection cn =new SqlConnection(connection))
            {
                cn.Open();

                SqlCommand command = new SqlCommand("getEstilo", cn);
                command.CommandType = CommandType.StoredProcedure;

                var lista = new List<Estilo>();

                var dr = command.ExecuteReader();

                while (dr.Read()) 
                {
                    //  var drr = dr;
                    var obj = new Estilo()
                    {
                        NombreEstilo = dr["Style"].ToString(),
                        ObjectId = Convert.ToInt32(dr["Id_Style"].ToString())
                        // Estado= Convert.ToBoolean(dr["washed"])
                    };


                    lista.Add(obj);
                } 


                return Ok(lista);
                //command.Parameters.Add("@id",);

            }
        }


        [HttpPost]
        public ActionResult post([FromBody] Estilo values)
        {
            var connection = Iconfiguration["ConnectionStrings:ConeccionPrueba"];

            using (SqlConnection cn = new SqlConnection(connection))
            {
                cn.Open();

                var command = new SqlCommand("spdCrearEstilo", cn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@Estilo", SqlDbType.NChar,15).Value = values.NombreEstilo;
                command.Parameters.Add("@wash", SqlDbType.Bit).Value = values.Estado;
                command.Parameters.Add("@precio ", SqlDbType.Money).Value = values.Precio;


                var resp = command.ExecuteNonQuery();

                return Ok(resp);

            }

        }

    }
}
