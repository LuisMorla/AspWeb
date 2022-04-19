using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using RegistroClientes.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistroClientes.Controllers
{
    public class RegistrosController : Controller
    {

        private static string conexion = ConfigurationManager.ConnectionStrings["conectar"].ToString();
        // GET: Registro
        private static List<Registros> conexionList = new List<Registros>();

        public ActionResult Inicio()
        {
            using (SqlConnection conectame = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("Select * from [Registro de Clientes]", conectame);
                cmd.CommandType = CommandType.Text;
                conectame.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {

                        Registros contador = new Registros();

                        contador.ID = Convert.ToInt32(dr["ID"]);
                        contador.Cedula = dr["Cedula"].ToString();
                        contador.Nombre = dr["Nombre"].ToString();
                        contador.Apellidos = dr["Apellidos"].ToString();


                        conexionList.Add(contador);
                    }

                }


            }
            return View(conexionList);
        }
        public ActionResult registrarcliente()
        {
            return View();
        }
        [HttpPost]
        public ActionResult registrarcliente(Registros Oregistro)
        {
            using (SqlConnection conectame = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SP_REGISTRAR", conectame);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Cedula", Oregistro.Cedula);
                cmd.Parameters.AddWithValue("Nombre", Oregistro.Nombre);
                cmd.Parameters.AddWithValue("Apellidos", Oregistro.Apellidos);
                conectame.Open();
                cmd.ExecuteNonQuery();
                
            }
            
            return RedirectToAction("Inicio","Registros");
        }
    }
}