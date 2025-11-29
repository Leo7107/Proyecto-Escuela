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

        public static string Insertar(int idEstudiante, int idAsignatura, decimal nota, int promedio, decimal estado)
        {
            if (idEstudiante <= 0) return "Estudiante inválido.";
            if (idAsignatura <= 0) return "Asignatura inválida.";
            if (nota < 0m || nota > 10) return "Nota inválida. Ajusta el rango según tu escala (ej. 0-100 o 0-5).";
            if (promedio < 0 || promedio > 10) return "Promedio inválido.";
            if (estado < 0m) return "Estado inválido.";

            Calificacion obj = new Calificacion
            {
                ID_Estudiante = idEstudiante,
                ID_Asignatura = idAsignatura,
                Nota = nota,
                Promedio = promedio,
                Estado = estado
            };

            DCalificaciones datos = new DCalificaciones();
            return datos.Insertar(obj);
        }

        public static string Actualizar(int idCalificacion, int idEstudiante, int idAsignatura, decimal nota, int promedio, decimal estado)
        {
            if (idCalificacion <= 0) return "Id de calificación inválido.";
            if (idEstudiante <= 0) return "Estudiante inválido.";
            if (idAsignatura <= 0) return "Asignatura inválida.";
            if (nota < 0m || nota > 10) return "Nota inválida. Ajusta el rango según tu escala.";
            if (promedio < 0 || promedio > 10) return "Promedio inválido.";
            if (estado < 0m) return "Estado inválido.";

            Calificacion obj = new Calificacion
            {
                ID_Calificacion = idCalificacion,
                ID_Estudiante = idEstudiante,
                ID_Asignatura = idAsignatura,
                Nota = nota,
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
