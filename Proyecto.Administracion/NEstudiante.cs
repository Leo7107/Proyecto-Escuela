using Proyecto.Datos;
using Proyecto.Entidades;
using System;
using System.Data;

namespace Sistema.Negocio
{
    public class NEstudiante
    {
        public static DataTable Listar()
        {
            DEstudiantes datos = new DEstudiantes();
            return datos.Listar();
        }

        public static DataTable Buscar(string valor)
        {
            DEstudiantes datos = new DEstudiantes();
            return datos.Buscar(valor);
        }

        public static string Insertar(string nombre, string apellido, string documento, DateTime fechaNacimiento, string direccion, string telefono, string correo, string grado)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return "El nombre es obligatorio.";
            if (string.IsNullOrWhiteSpace(apellido)) return "El apellido es obligatorio.";
            if (string.IsNullOrWhiteSpace(documento)) return "El documento es obligatorio.";
            if (fechaNacimiento == DateTime.MinValue) return "Fecha de nacimiento inválida.";
            if (string.IsNullOrWhiteSpace(grado)) return "El grado es obligatorio.";

            Estudiante obj = new Estudiante
            {
                Nombre = nombre.Trim(),
                Apellido = apellido.Trim(),
                Documento = documento.Trim(),
                FechaNacimiento = fechaNacimiento,
                Direccion = direccion?.Trim(),
                Telefono = telefono?.Trim(),
                Correo = correo?.Trim(),
                Grado = grado.Trim()
            };

            DEstudiantes datos = new DEstudiantes();
            return datos.Insertar(obj);
        }

        public static string Actualizar(int idEstudiante, string nombre, string apellido, string documento, DateTime fechaNacimiento, string direccion, string telefono, string correo, string grado)
        {
            if (idEstudiante <= 0) return "Id de estudiante inválido.";
            if (string.IsNullOrWhiteSpace(nombre)) return "El nombre es obligatorio.";
            if (string.IsNullOrWhiteSpace(apellido)) return "El apellido es obligatorio.";
            if (string.IsNullOrWhiteSpace(documento)) return "El documento es obligatorio.";
            if (fechaNacimiento == DateTime.MinValue) return "Fecha de nacimiento inválida.";
            if (string.IsNullOrWhiteSpace(grado)) return "El grado es obligatorio.";

            Estudiante obj = new Estudiante
            {
                ID_Estudiante = idEstudiante,
                Nombre = nombre.Trim(),
                Apellido = apellido.Trim(),
                Documento = documento.Trim(),
                FechaNacimiento = fechaNacimiento,
                Direccion = direccion?.Trim(),
                Telefono = telefono?.Trim(),
                Correo = correo?.Trim(),
                Grado = grado.Trim()
            };

            DEstudiantes datos = new DEstudiantes();
            return datos.Actualizar(obj);
        }

        public static string Eliminar(int idEstudiante)
        {
            if (idEstudiante <= 0) return "Id de estudiante inválido.";
            DEstudiantes datos = new DEstudiantes();
            return datos.Eliminar(idEstudiante);
        }
    }
}
    