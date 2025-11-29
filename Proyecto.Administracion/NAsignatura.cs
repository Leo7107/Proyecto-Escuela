using Proyecto.Datos;
using Proyecto.Entidades;
using System.Data;

namespace Sistema.Negocio
{
    public class NAsignatura
    {
        public static DataTable Listar()
        {
            DAsignaturas datos = new DAsignaturas();
            return datos.Listar();
        }

        public static DataTable Buscar(string valor)
        {
            DAsignaturas datos = new DAsignaturas();
            return datos.Buscar(valor);
        }

        // Buscar por docente (útil en formularios)
        public static DataTable BuscarPorDocente(int idDocente)
        {
            if (idDocente <= 0) return new DataTable();
            DAsignaturas datos = new DAsignaturas();
            return datos.BuscarPorDocente(idDocente);
        }

        public static string Insertar(string nombre, string descripcion, int creditos, int idDocente)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return "El nombre de la asignatura es obligatorio.";
            if (creditos <= 0) return "Créditos inválidos.";
            if (idDocente <= 0) return "Seleccione un docente válido.";

            Asignatura obj = new Asignatura
            {
                Nombre = nombre.Trim(),
                Descripcion = descripcion?.Trim(),
                Creditos = creditos,
                ID_Docente = idDocente
            };

            DAsignaturas datos = new DAsignaturas();
            return datos.Insertar(obj);
        }

        public static string Actualizar(int idAsignatura, string nombre, string descripcion, int creditos, int idDocente)
        {
            if (idAsignatura <= 0) return "Id de asignatura inválido.";
            if (string.IsNullOrWhiteSpace(nombre)) return "El nombre de la asignatura es obligatorio.";
            if (creditos <= 0) return "Créditos inválidos.";
            if (idDocente <= 0) return "Seleccione un docente válido.";

            Asignatura obj = new Asignatura
            {
                ID_Asignatura = idAsignatura,
                Nombre = nombre.Trim(),
                Descripcion = descripcion?.Trim(),
                Creditos = creditos,
                ID_Docente = idDocente
            };

            DAsignaturas datos = new DAsignaturas();
            return datos.Actualizar(obj);
        }

        public static string Eliminar(int idAsignatura)
        {
            if (idAsignatura <= 0) return "Id de asignatura inválido.";
            DAsignaturas datos = new DAsignaturas();
            return datos.Eliminar(idAsignatura);
        }
    }
}
