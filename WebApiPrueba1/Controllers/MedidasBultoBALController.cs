using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPrueba1.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WebApiPrueba1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedidasBultoBALController : ControllerBase
    {
        IConfiguration Iconfiguration { get; }

        public MedidasBultoBALController(IConfiguration iconfiguration)
        {
            Iconfiguration = iconfiguration;
        }

        [HttpPost]
        public ActionResult post([FromBody] MedidaBultoaBultoAL values)
        {
            var connection = Iconfiguration["ConnectionStrings:ConeccionPrueba"];

            using (SqlConnection cn=new SqlConnection(connection))
            {
                cn.Open();

                var command = new SqlCommand("spdCrearMedidasBultoaBulto", cn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@idporder",SqlDbType.Int).Value=values.IdPorder;
                command.Parameters.Add("@idEstilo", SqlDbType.Int).Value = values.idEstilo;
                command.Parameters.Add("@size", SqlDbType.NChar,8).Value = values.Size;
                command.Parameters.Add("@cantidad", SqlDbType.Int).Value = values.Cantidad;
                command.Parameters.Add("@estado", SqlDbType.Bit).Value = values.estado;
                command.Parameters.Add("@fecha", SqlDbType.DateTime).Value = values.fechaRegistro;

                var resp = command.ExecuteNonQuery();

                return Ok(resp);

            }

        }
    }
}
