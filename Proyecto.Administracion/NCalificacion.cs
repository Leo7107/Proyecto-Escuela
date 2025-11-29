using Proyecto.Datos;
using Proyecto.Entidades;
using System.Data;

namespace Sistema.Negocio
{
    public class NCalificacion
    {
        public static DataTable Listar()
        {
            DCalificaciones datos = new DCalificaciones();
            return datos.Listar();
        }

        public static DataTable Buscar(string valor)
        {
            DCalificaciones datos = new DCalificaciones();
            return datos.Buscar(valor);
        }

        // Buscar por estudiante (útil para formularios)
        public static DataTable BuscarPorEstudiante(int idEstudiante)
        {
            if (idEstudiante <= 0) return new DataTable();
            DCalificaciones datos = new DCalificaciones();
            return datos.BuscarPorEstudiante(idEstudiante);
        }

        
        public static string Insertar(int idEstudiante, int idAsignatura, decimal nota1, decimal nota2, decimal nota3, bool estado)
        {
            if (idEstudiante <= 0) return "Estudiante inválido.";
            if (idAsignatura <= 0) return "Asignatura inválida.";

            if (nota1 < 0m || nota1 > 10m) return "Nota 1 inválida (0-10).";
            if (nota2 < 0m || nota2 > 10m) return "Nota 2 inválida (0-10).";
            if (nota3 < 0m || nota3 > 10m) return "Nota 3 inválida (0-10).";

            decimal promedio = (nota1 + nota2 + nota3) / 3m;

            Calificacion obj = new Calificacion
            {
                ID_Estudiante = idEstudiante,
                ID_Asignatura = idAsignatura,
                Nota1 = nota1,
                Nota2 = nota2,
                Nota3 = nota3,
                Promedio = promedio,
                Estado = estado
            };

            DCalificaciones datos = new DCalificaciones();
            return datos.Insertar(obj);
        }
        public static string Actualizar(int idCalificacion, int idEstudiante, int idAsignatura, decimal nota1, decimal nota2, decimal nota3,bool estado)
        {
            if (idCalificacion <= 0) return "Id de calificación inválido.";
            if (idEstudiante <= 0) return "Estudiante inválido.";
            if (idAsignatura <= 0) return "Asignatura inválida.";

            if (nota1 < 0m || nota1 > 10m) return "Nota 1 inválida (0-10).";
            if (nota2 < 0m || nota2 > 10m) return "Nota 2 inválida (0-10).";
            if (nota3 < 0m || nota3 > 10m) return "Nota 3 inválida (0-10).";

            decimal promedio = (nota1 + nota2 + nota3) / 3m;

            Calificacion obj = new Calificacion
            {
                ID_Calificacion = idCalificacion,
                ID_Estudiante = idEstudiante,
                ID_Asignatura = idAsignatura,
                Nota1 = nota1,
                Nota2 = nota2,
                Nota3 = nota3,
                Promedio = promedio,
                Estado = estado
            };

            DCalificaciones datos = new DCalificaciones();
            return datos.Actualizar(obj);
        }
        public static string Eliminar(int idCalificacion)
        {
            if (idCalificacion <= 0) return "Id de calificación inválido.";
            DCalificaciones datos = new DCalificaciones();
            return datos.Eliminar(idCalificacion);
        }
    }
}
