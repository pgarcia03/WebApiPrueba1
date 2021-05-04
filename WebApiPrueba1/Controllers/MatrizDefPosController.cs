using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPrueba1.Models;

namespace WebApiPrueba1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatrizDefPosController : ControllerBase
    {

        IConfiguration Configuration { get; }

        public MatrizDefPosController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /*
        [HttpGet("{id}")]
        public ActionResult get(int id)
        {
            string connectionString = Configuration["ConnectionStrings:ConeccionPrueba"];
            var lista = new List<Reparacion>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand(" select mpd.*, m.Modulo, p.Posicion, d.Defecto from matrizPosicionDefectos mpd"
                                                   + " join Modulo m on m.Id_Modulo = mpd.idmodulo"
                                                   + " join tbPosicion p on mpd.idPosicion = p.idPosicion"
                                                   + " join tbDefectos d on mpd.idDefecto = d.idDefecto"
                                                   + " where m.Id_Modulo =" + id + " and mpd.descProducto = 'P'", con);

                command.CommandType = CommandType.Text;

                SqlDataReader dtr = command.ExecuteReader();

                //SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                //DataTable dt = new DataTable();
                //dataAdapter.Fill(dt);

                // var r = dtr.Read();
        */
     //*********************
        /*
                while (dtr.Read()) // (dtr.Read())
                {
                    var obj = new Reparacion()
                    {
                        // ObjectId = Convert.ToInt32(dtr["Id_Order"].ToString()),
                        id = Convert.ToInt32(dtr["id"].ToString()),
                        idModulo = Convert.ToInt32(dtr["idModulo"].ToString()),
                        idPosicion = Convert.ToInt32(dtr["idPosicion"].ToString()),
                        idDefecto = Convert.ToInt32(dtr["idDefecto"].ToString()),
                        posicion = dtr["Posicion"].ToString(),
                        defecto = dtr["Defecto"].ToString(),

                    };

                    lista.Add(obj);

                }


                //var lista1 = lista.GroupBy(x=> new {x.idModulo,x.idPosicion,x.posicion }).Select(x=>new ReparacionMasterJson { idModulo=x.Key.idModulo,
                //                                                        idPosicion=x.Key.idPosicion,
                //                                                        posicion=x.Key.posicion,
                //                                                        defecto=lista.Where(y=>y.idPosicion==x.Key.idPosicion)
                //                                                                     .Select(y=>new posicionDefecto { id=y.id,defecto=y.defecto,idDefecto=y.idDefecto })
                //                                                                     .ToList()
                //                                                         }).ToList();

                return Ok(lista);

            }
        }
        */


        [HttpGet("{corte}")]
        public async Task<ActionResult> total(string corte)
        {
            string connection = Configuration["ConnectionStrings:ConeccionPrueba"];

            using (SqlConnection con =new SqlConnection(connection))
            {
                con.Open();

                SqlCommand command = new SqlCommand("select isnull(sum(r.cantidad),0) as total from tbRegistroReparacion r where rtrim(ltrim(r.Corte))="+ corte ,con);
                command.CommandType = CommandType.Text;

                var rd =await command.ExecuteReaderAsync();

                var total = 0;// rd.ReadAsync;
                while( await rd.ReadAsync())
                {
                    total =int.Parse(rd["total"].ToString());
                }

                return Ok(total);

            }

        }

        [HttpGet("{id}&{desc}&{corte}")]
        public ActionResult get(int id, string desc,string corte)
        {
            string connectionString = Configuration["ConnectionStrings:ConeccionPrueba"];
            var lista = new List<Reparacion>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                /*SqlCommand command = new SqlCommand(" select mpd.*, m.Modulo, p.Posicion, d.Defecto from matrizPosicionDefectos mpd"
                                                   + " join Modulo m on m.Id_Modulo = mpd.idmodulo"
                                                   + " join tbPosicion p on mpd.idPosicion = p.idPosicion"
                                                   + " join tbDefectos d on mpd.idDefecto = d.idDefecto"
                                                   + " where m.Id_Modulo =" + id + " and mpd.descProducto = '"+desc+"'", con);
                */
                SqlCommand command = new SqlCommand("getListaReparacionXCorte", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@modulo", SqlDbType.Int).Value = id;
                command.Parameters.Add("@des", SqlDbType.NChar).Value = desc;
                command.Parameters.Add("@corte", SqlDbType.NChar).Value = corte;


                SqlDataReader dtr = command.ExecuteReader();

                while (dtr.Read())
                {
                    var obj = new Reparacion()
                    {
                        id = Convert.ToInt32(dtr["id"].ToString()),
                        idModulo = Convert.ToInt32(dtr["idModulo"].ToString()),
                        idPosicion = Convert.ToInt32(dtr["idPosicion"].ToString()),
                        idDefecto = Convert.ToInt32(dtr["idDefecto"].ToString()),
                        posicion = dtr["Posicion"].ToString(),
                        defecto = dtr["Defecto"].ToString(),
                        cantidad =Convert.ToInt32(dtr["cantidad"].ToString())
                    };

                    lista.Add(obj);

                }

                return Ok(lista);

            }
        }

        [HttpPost]
        public ActionResult post([FromBody] Reparacion data)
        {
            var config = Configuration["ConnectionStrings:ConeccionPrueba"];

            using (SqlConnection cn = new SqlConnection(config))
            {
                cn.Open();

                var command = new SqlCommand("spdCrearReparacion", cn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@id", SqlDbType.Int).Value = data.id;
                command.Parameters.Add("@iddefecto", SqlDbType.Int).Value = data.idDefecto;
                command.Parameters.Add("@idposicion", SqlDbType.Int).Value = data.idPosicion;
                command.Parameters.Add("@inspector", SqlDbType.NChar).Value = data.inspector;
                command.Parameters.Add("@corte", SqlDbType.NChar).Value = data.corte;
                command.Parameters.Add("@color", SqlDbType.NChar).Value = data.color;
                command.Parameters.Add("@unidades", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@unidadesTotal", SqlDbType.Int).Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();

                var unidades = int.Parse(command.Parameters["@unidades"].Value.ToString());
                var unidaestotal = int.Parse(command.Parameters["@unidadesTotal"].Value.ToString());

                var objresp = new resultado { unidades = unidades, unidadestotal = unidaestotal };

                return Ok(objresp);
            }

        }

        [HttpDelete("{id}&{corte}&{inspector}")]
        public ActionResult delete(int id, string corte, string inspector)
        {
            var config = Configuration["ConnectionStrings:ConeccionPrueba"];

            using (SqlConnection cn = new SqlConnection(config))
            {
                cn.Open();

                var command = new SqlCommand("spdEliminarReparacion", cn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                command.Parameters.Add("@inspector", SqlDbType.NChar).Value = inspector;
                command.Parameters.Add("@corte", SqlDbType.NChar).Value = corte;
                command.Parameters.Add("@unidades", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@unidadesTotal", SqlDbType.Int).Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();

                var unidades = int.Parse(command.Parameters["@unidades"].Value.ToString());
                var unidaestotal = int.Parse(command.Parameters["@unidadesTotal"].Value.ToString());

                var objresp = new resultado { unidades = unidades, unidadestotal = unidaestotal };

                return Ok(objresp);

            }

        }


    }
}
