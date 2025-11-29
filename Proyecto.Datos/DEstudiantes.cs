using System;
using System.Data;
using Proyecto.Entidades;
using System.Data.SqlClient;

namespace Proyecto.Datos
{
    public class DEstudiantes
    {
        // Listar
        public DataTable Listar()
        {
            SqlDataReader Resultado;
            DataTable Tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.GetInstancia().CrearConexion();
                SqlCommand Comando = new SqlCommand("estudiante_listar", SqlCon);
                Comando.CommandType = CommandType.StoredProcedure;

                SqlCon.Open();
                Resultado = Comando.ExecuteReader();
                Tabla.Load(Resultado);

                return Tabla;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
        }

        // Buscar
        public DataTable Buscar(string Valor)
        {
            SqlDataReader Resultado;
            DataTable Tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.GetInstancia().CrearConexion();
                SqlCommand Comando = new SqlCommand("estudiante_buscar", SqlCon);
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.Parameters.Add("@valor", SqlDbType.VarChar).Value = Valor;

                SqlCon.Open();
                Resultado = Comando.ExecuteReader();
                Tabla.Load(Resultado);

                return Tabla;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
        }

        // Insertar
        public string Insertar(Estudiante Obj)
        {
            string Rpta = "";
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.GetInstancia().CrearConexion();
                SqlCommand Comando = new SqlCommand("usuario_insertar", SqlCon);
                Comando.CommandType = CommandType.StoredProcedure;

                Comando.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = Obj.Nombre;
                Comando.Parameters.Add("@Apellido", SqlDbType.VarChar).Value = Obj.Apellido;
                Comando.Parameters.Add("@Documento", SqlDbType.VarChar).Value = Obj.Documento;
                Comando.Parameters.Add("@FechaNacimiento", SqlDbType.Date).Value = Obj.FechaNacimiento;
                Comando.Parameters.Add("@Direccion", SqlDbType.VarChar).Value = Obj.Direccion;
                Comando.Parameters.Add("@Telefono", SqlDbType.VarChar).Value = Obj.Telefono;
                Comando.Parameters.Add("@Correo", SqlDbType.VarChar).Value = Obj.Correo;
                Comando.Parameters.Add("@Grado", SqlDbType.VarChar).Value = Obj.Grado;

                SqlCon.Open();
                Rpta = Comando.ExecuteNonQuery() == 1 ? "OK" : "No se pudo insertar el registro";
            }
            catch (Exception ex)
            {
                Rpta = ex.Message;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }

            return Rpta;
        }

        // Actualizar
        public string Actualizar(Estudiante Obj)
        {
            string Rpta = "";
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.GetInstancia().CrearConexion();
                SqlCommand Comando = new SqlCommand("usuario_actualizar", SqlCon);
                Comando.CommandType = CommandType.StoredProcedure;

                Comando.Parameters.Add("@ID_Estudiante", SqlDbType.Int).Value = Obj.ID_Estudiante;
                Comando.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = Obj.Nombre;
                Comando.Parameters.Add("@Apellido", SqlDbType.VarChar).Value = Obj.Apellido;
                Comando.Parameters.Add("@Documento", SqlDbType.VarChar).Value = Obj.Documento;
                Comando.Parameters.Add("@FechaNacimiento", SqlDbType.Date).Value = Obj.FechaNacimiento;
                Comando.Parameters.Add("@Direccion", SqlDbType.VarChar).Value = Obj.Direccion;
                Comando.Parameters.Add("@Telefono", SqlDbType.VarChar).Value = Obj.Telefono;
                Comando.Parameters.Add("@Correo", SqlDbType.VarChar).Value = Obj.Correo;
                Comando.Parameters.Add("@Grado", SqlDbType.VarChar).Value = Obj.Grado;

                SqlCon.Open();
                Rpta = Comando.ExecuteNonQuery() == 1 ? "OK" : "No se pudo actualizar el registro";
            }
            catch (Exception ex)
            {
                Rpta = ex.Message;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }

            return Rpta;
        }

        // Eliminar
        public string Eliminar(int ID)
        {
            string Rpta = "";
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.GetInstancia().CrearConexion();
                SqlCommand Comando = new SqlCommand("estudiante_eliminar", SqlCon);
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.Parameters.Add("@ID_Estudiante", SqlDbType.Int).Value = ID;

                SqlCon.Open();
                Rpta = Comando.ExecuteNonQuery() == 1 ? "OK" : "No se pudo eliminar el registro";
            }
            catch (Exception ex)
            {
                Rpta = ex.Message;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }

            return Rpta;
        }
    }
}
