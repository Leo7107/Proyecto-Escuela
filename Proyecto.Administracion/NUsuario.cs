using Proyecto.Datos;
using Proyecto.Entidades;
using System.Data;

namespace Sistema.Negocio
{
    public class NUsuario
    {
        public static DataTable Listar()
        {
            DUsuarios datos = new DUsuarios();
            return datos.Listar();
        }

        public static DataTable Buscar(string valor)
        {
            DUsuarios datos = new DUsuarios();
            return datos.Buscar(valor);
        }

        public static string Insertar(string nombre, string usuarioLogin, string contrasena, int idRol)
        {
            // Validación básica
            if (string.IsNullOrWhiteSpace(nombre)) return "El nombre es obligatorio.";
            if (string.IsNullOrWhiteSpace(usuarioLogin)) return "El usuario es obligatorio.";
            if (string.IsNullOrWhiteSpace(contrasena)) return "La contraseña es obligatoria.";

            Usuario obj = new Usuario
            {
                Nombre = nombre,
                UsuarioLogin = usuarioLogin,
                Contrasena = contrasena,
                ID_Rol = idRol
            };

            DUsuarios datos = new DUsuarios();
            return datos.Insertar(obj);
        }

        public static string Actualizar(int idUsuario, string nombre, string usuarioLogin, string contrasena, int idRol)
        {
            if (idUsuario <= 0) return "Id de usuario inválido.";
            if (string.IsNullOrWhiteSpace(nombre)) return "El nombre es obligatorio.";
            if (string.IsNullOrWhiteSpace(usuarioLogin)) return "El usuario es obligatorio.";

            Usuario obj = new Usuario
            {
                ID_Usuario = idUsuario,
                Nombre = nombre,
                UsuarioLogin = usuarioLogin,
                Contrasena = contrasena,
                ID_Rol = idRol
            };

            DUsuarios datos = new DUsuarios();
            return datos.Actualizar(obj);
        }

        public static string Eliminar(int idUsuario)
        {
            if (idUsuario <= 0) return "Id de usuario inválido.";
            DUsuarios datos = new DUsuarios();
            return datos.Eliminar(idUsuario);
        }
    }
}
