using Proyecto.Datos;
using Proyecto.Entidades;
using System.Data;

namespace Sistema.Negocio
{
    public class NRol
    {
        public static DataTable Listar()
        {
            DRoles datos = new DRoles();
            return datos.Listar();
        }

        public static DataTable Buscar(string valor)
        {
            DRoles datos = new DRoles();
            return datos.Buscar(valor);
        }

        public static string Insertar(string nombreRol)
        {
            if (string.IsNullOrWhiteSpace(nombreRol))
                return "El nombre del rol es obligatorio.";

            Rol obj = new Rol
            {
                NombreRol = nombreRol.Trim()
            };

            DRoles datos = new DRoles();
            return datos.Insertar(obj);
        }

        public static string Actualizar(int idRol, string nombreRol)
        {
            if (idRol <= 0) return "Id de rol inválido.";
            if (string.IsNullOrWhiteSpace(nombreRol)) return "El nombre del rol es obligatorio.";

            Rol obj = new Rol
            {
                ID_Rol = idRol,
                NombreRol = nombreRol.Trim()
            };

            DRoles datos = new DRoles();
            return datos.Actualizar(obj);
        }

        public static string Eliminar(int id_Rol)
        {
            if (id_Rol <= 0) return "Id de rol inválido.";

            DRoles datos = new DRoles();
            return datos.Eliminar(id_Rol);
        }
    }
}
