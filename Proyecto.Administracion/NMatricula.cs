using Proyecto.Datos;
using Proyecto.Entidades;
using System.Data;

namespace Sistema.Negocio
{
    public class NMatricula
    {
        public static DataTable Listar()
        {
            DMatriculas datos = new DMatriculas();
            return datos.Listar();
        }

        public static DataTable Buscar(string valor)
        {
            DMatriculas datos = new DMatriculas();
            return datos.Buscar(valor);
        }

        // Buscar matrículas por estudiante (útil para el formulario)
        public static DataTable BuscarPorEstudiante(int idEstudiante)
        {
            if (idEstudiante <= 0) return new DataTable();
            DMatriculas datos = new DMatriculas();
            return datos.BuscarPorEstudiante(idEstudiante);
        }

        public static string Insertar(int idEstudiante, int idAsignatura, int año, int grado)
        {
            if (idEstudiante <= 0) return "Estudiante inválido.";
            if (idAsignatura <= 0) return "Asignatura inválida.";
            if (año <= 0) return "Año inválido.";
            if (grado <= 0) return "Grado inválido.";

            Matricula obj = new Matricula
            {
                ID_Estudiante = idEstudiante,
                ID_Asignatura = idAsignatura,
                Año = año,
                Grado = grado
            };

            DMatriculas datos = new DMatriculas();
            return datos.Insertar(obj);
        }

        public static string Actualizar(int idMatricula, int idEstudiante, int idAsignatura, int año, int grado)
        {
            if (idMatricula <= 0) return "Id de matrícula inválido.";
            if (idEstudiante <= 0) return "Estudiante inválido.";
            if (idAsignatura <= 0) return "Asignatura inválida.";
            if (año <= 0) return "Año inválido.";
            if (grado <= 0) return "Grado inválido.";

            Matricula obj = new Matricula
            {
                ID_Matricula = idMatricula,
                ID_Estudiante = idEstudiante,
                ID_Asignatura = idAsignatura,
                Año = año,
                Grado = grado
            };

            DMatriculas datos = new DMatriculas();
            return datos.Actualizar(obj);
        }

        public static string Eliminar(int idMatricula)
        {
            if (idMatricula <= 0) return "Id de matrícula inválido.";
            DMatriculas datos = new DMatriculas();
            return datos.Eliminar(idMatricula);
        }
    }
}
