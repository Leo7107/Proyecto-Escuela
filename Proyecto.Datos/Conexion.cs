using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Datos
{
    public class Conexion
    {
        private string Base;
        private string Servidor;
        private static Conexion Con = null;

        private Conexion()
        {
            this.Base = "GestionEscolar";
            this.Servidor = "localhost";
        }
        public SqlConnection CrearConexion()
        {
            SqlConnection Cadena = new SqlConnection();
            try
            {
                Cadena.ConnectionString = $"Server= {this.Servidor}; Database= {this.Base}; Integrated Security =SSPI; ";
            }
            catch (Exception ex)
            {
                Cadena = null;
                throw ex;
            }
            return Cadena;
        }
        public static Conexion GetInstancia()
        {
            if (Con == null)
            {
                Con = new Conexion();
            }
            return Con;
        }
    }
}
