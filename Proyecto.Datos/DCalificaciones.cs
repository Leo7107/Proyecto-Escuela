using System;
using System.Data;
using Proyecto.Entidades;
using System.Data.SqlClient;

namespace Proyecto.Datos
{
    public class DCalificaciones
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
                SqlCommand Comando = new SqlCommand("calificacion_listar", SqlCon);
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
                SqlCommand Comando = new SqlCommand("calificacion_buscar", SqlCon);
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
        public string Insertar(Calificacion Obj)
        {
            string Rpta = "";
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.GetInstancia().CrearConexion();
                SqlCommand Comando = new SqlCommand("calificacion_insertar", SqlCon);
                Comando.CommandType = CommandType.StoredProcedure;

                Comando.Parameters.Add("@ID_Estudiante", SqlDbType.Int).Value = Obj.ID_Estudiante;
                Comando.Parameters.Add("@ID_Asignatura", SqlDbType.Int).Value = Obj.ID_Asignatura;
                Comando.Parameters.Add("@Nota1", SqlDbType.Decimal).Value = Obj.Nota1;
                Comando.Parameters.Add("@Nota2", SqlDbType.Decimal).Value = Obj.Nota2;
                Comando.Parameters.Add("@Nota3", SqlDbType.Decimal).Value = Obj.Nota3;
                Comando.Parameters.Add("@Promedio", SqlDbType.Decimal).Value = Obj.Promedio;
                // Estado como bit: 1 = activo, 0 = inactivo
                Comando.Parameters.Add("@Estado", SqlDbType.Bit).Value = Obj.Estado;

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
        public string Actualizar(Calificacion Obj)
        {
            string Rpta = "";
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.GetInstancia().CrearConexion();
                SqlCommand Comando = new SqlCommand("calificacion_actualizar", SqlCon);
                Comando.CommandType = CommandType.StoredProcedure;

                Comando.Parameters.Add("@ID_Calificacion", SqlDbType.Int).Value = Obj.ID_Calificacion;
                Comando.Parameters.Add("@ID_Estudiante", SqlDbType.Int).Value = Obj.ID_Estudiante;
                Comando.Parameters.Add("@ID_Asignatura", SqlDbType.Int).Value = Obj.ID_Asignatura;
                Comando.Parameters.Add("@Nota1", SqlDbType.Decimal).Value = Obj.Nota1;
                Comando.Parameters.Add("@Nota2", SqlDbType.Decimal).Value = Obj.Nota2;
                Comando.Parameters.Add("@Nota3", SqlDbType.Decimal).Value = Obj.Nota3;
                Comando.Parameters.Add("@Promedio", SqlDbType.Decimal).Value = Obj.Promedio;
                Comando.Parameters.Add("@Estado", SqlDbType.Bit).Value = Obj.Estado;

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
                SqlCommand Comando = new SqlCommand("calificacion_eliminar", SqlCon);
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.Parameters.Add("@ID_Calificacion", SqlDbType.Int).Value = ID;

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

        // Buscar por Estudiante
        public DataTable BuscarPorEstudiante(int idEstudiante)
        {
            SqlDataReader Resultado;
            DataTable Tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.GetInstancia().CrearConexion();
                SqlCommand Comando = new SqlCommand("calificacion_buscar_por_estudiante", SqlCon); // ajusta el SP según BD
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.Parameters.Add("@ID_Estudiante", SqlDbType.Int).Value = idEstudiante;

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
    }
}
