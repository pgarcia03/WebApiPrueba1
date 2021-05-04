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
    public class PordersController : ControllerBase
    {
        public IConfiguration Configuration { get; }

        public PordersController(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        [HttpGet("{id}")]
        public IEnumerable<Porder> get(int id)
        {


            //string connectionString = Configuration["ConnectionStrings:ConeccionPrueba"];
            var lista = new List<Porder>();
            //using (SqlConnection con = new SqlConnection(connectionString))
            //{
            //    con.Open();
            //    SqlCommand command = new SqlCommand("getporderXid", con);

            //    command.CommandType = CommandType.StoredProcedure;

            //    command.Parameters.AddWithValue("@id", id);

            //    SqlDataReader dtr = command.ExecuteReader();

            //SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            //DataTable dt = new DataTable();
            //dataAdapter.Fill(dt);

            // var r = dtr.Read();

            //  while (dtr.Read()) // (dtr.Read())
            // {
            var obj = new Porder()
            {
                ObjectId = 1,// Convert.ToInt32(dtr["Id_Order"].ToString()),
                Corte = "corte",//dtr["Porder"].ToString(),
                IdEstilo = 10,//Convert.ToInt32(dtr["Id_Style"].ToString()),
                Unidades = 1152//Convert.ToInt32(dtr["Quantity"].ToString())

            };

            lista.Add(obj);

            //  }
            //;

            return lista.AsEnumerable();

            // }
        }

        [HttpGet("{corte},{test}")]
        public ActionResult get(string corte, string test)
        {
            string connectionString = Configuration["ConnectionStrings:ConeccionPrueba"];
            var lista = new List<Porder>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("spdConstainPorder", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@corte", corte);

                SqlDataReader dtr = command.ExecuteReader();

                while (dtr.Read())
                {
                    var obj = new Porder()
                    {
                        //ObjectId = Convert.ToInt32(dtr["Id_Order"].ToString()),
                        Corte = dtr["porder"].ToString(),
                        Estilo = dtr["style"].ToString(),
                        Unidades = Convert.ToInt32(dtr["quantity"].ToString()),
                        descr = dtr["descr"].ToString()

                    };

                    lista.Add(obj);

                }


                return Ok(lista);

            }
        }

        [HttpGet]
        public IEnumerable<Porder> get()
        {
            try
            {
                string connectionString = Configuration["ConnectionStrings:ConeccionPrueba"];
                var lista = new List<Porder>();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("getporderAll", con);

                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dtr = command.ExecuteReader();

                    ///var r = dtr.Read();
                    //var h = 10 / Convert;

                    while (dtr.Read())
                    {
                        var obj = new Porder()
                        {
                            ObjectId = Convert.ToInt32(dtr["Id_Order"].ToString()),
                            Corte = dtr["Porder"].ToString(),
                            IdEstilo = Convert.ToInt32(dtr["Id_Style"].ToString()),
                            Unidades = Convert.ToInt32(dtr["Quantity"].ToString())
                        };

                        lista.Add(obj);

                    }
                    //;

                    return lista.AsEnumerable();

                }
            }
            catch (Exception ex)
            {
                var obj = new Porder()
                {
                    ObjectId =1,// Convert.ToInt32(dtr["Id_Order"].ToString()),
                    Corte =ex.Message,// dtr["Porder"].ToString(),
                    IdEstilo =1,// Convert.ToInt32(dtr["Id_Style"].ToString()),
                    Unidades = 1152//Convert.ToInt32(dtr["Quantity"].ToString())
                };

                var lista = new List<Porder>();

                lista.Add(obj);

                return lista.AsEnumerable();

                //return //Ok(ex.Message);
                   //throw;
               }

        }




        // POST api/<ConexionController>
        [HttpPost]
        public int Post([FromBody] Porder data)
        {
            var coneccion = Configuration["ConnectionStrings:ConeccionPrueba"];

            using (SqlConnection cn = new SqlConnection(coneccion))
            {
                cn.Open();
                var cmm = new SqlCommand("spdCrearPorder", cn);
                cmm.CommandType = CommandType.StoredProcedure;
                cmm.Parameters.Add("@po", SqlDbType.NChar, 15).Value = data.Corte;
                cmm.Parameters.Add("@unidades", SqlDbType.Int).Value = data.Unidades;
                cmm.Parameters.Add("@idestilo", SqlDbType.Int).Value = data.IdEstilo;

                var resp = cmm.ExecuteNonQuery();

                return resp;
            }

        }

        // PUT api/<ConexionController>/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] Porder data)
        {
            var conecction = Configuration["ConnectionStrings:ConeccionPrueba"];

            using (SqlConnection cn = new SqlConnection(conecction))
            {
                cn.Open();

                var command = new SqlCommand("spdActualizarPorder", cn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@id", SqlDbType.Int).Value = data.ObjectId;
                command.Parameters.Add("@po", SqlDbType.NChar, 15).Value = data.Corte;
                command.Parameters.Add("@unidades", SqlDbType.Int).Value = data.Unidades;
                command.Parameters.Add("@idestilo", SqlDbType.Int).Value = data.IdEstilo;

                var resp = command.ExecuteNonQuery();

                return resp;

            }
        }

        // DELETE api/<ConexionController>/5
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            var conecction = Configuration["ConnectionStrings:ConeccionPrueba"];

            using (SqlConnection cn = new SqlConnection(conecction))
            {
                cn.Open();

                SqlCommand commnad = new SqlCommand("spdEliminarPorder", cn);
                commnad.CommandType = CommandType.StoredProcedure;

                commnad.Parameters.Add("@id", SqlDbType.Int).Value = id;

                var resp = commnad.ExecuteNonQuery();

                return resp;
            }


        }


    }
}
