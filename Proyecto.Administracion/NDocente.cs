using Proyecto.Datos;
using Proyecto.Entidades;
using System.Data;

namespace Sistema.Negocio
{
    public class NDocente
    {
        public static DataTable Listar()
        {
            DDocentes datos = new DDocentes();
            return datos.Listar();
        }

        public static DataTable Buscar(string valor)
        {
            DDocentes datos = new DDocentes();
            return datos.Buscar(valor);
        }

        public static string Insertar(string nombre, string apellido, string documento, string especialidad, string telefono, string correo)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return "El nombre es obligatorio.";
            if (string.IsNullOrWhiteSpace(apellido)) return "El apellido es obligatorio.";
            if (string.IsNullOrWhiteSpace(documento)) return "El documento es obligatorio.";

            Docente obj = new Docente
            {
                Nombre = nombre.Trim(),
                Apellido = apellido.Trim(),
                Documento = documento.Trim(),
                Especialidad = especialidad?.Trim(),
                Telefono = telefono?.Trim(),
                Correo = correo?.Trim()
            };

            DDocentes datos = new DDocentes();
            return datos.Insertar(obj);
        }

        public static string Actualizar(int idDocente, string nombre, string apellido, string documento, string especialidad, string telefono, string correo)
        {
            if (idDocente <= 0) return "Id de docente inválido.";
            if (string.IsNullOrWhiteSpace(nombre)) return "El nombre es obligatorio.";
            if (string.IsNullOrWhiteSpace(apellido)) return "El apellido es obligatorio.";
            if (string.IsNullOrWhiteSpace(documento)) return "El documento es obligatorio.";

            Docente obj = new Docente
            {
                ID_Docente = idDocente,
                Nombre = nombre.Trim(),
                Apellido = apellido.Trim(),
                Documento = documento.Trim(),
                Especialidad = especialidad?.Trim(),
                Telefono = telefono?.Trim(),
                Correo = correo?.Trim()
            };

            DDocentes datos = new DDocentes();
            return datos.Actualizar(obj);
        }

        public static string Eliminar(int idDocente)
        {
            if (idDocente <= 0) return "Id de docente inválido.";
            DDocentes datos = new DDocentes();
            return datos.Eliminar(idDocente);
        }
    }
}
