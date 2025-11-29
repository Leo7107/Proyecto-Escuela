using Proyecto.Datos;
using Proyecto.Entidades;
using System;
using System.Data;
using System.Security.Cryptography;

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
                Contrasena = HashPassword(contrasena),
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
                // Si se proporciona contraseña se guarda hasheada; si es null/empty, se envía null para que la capa de datos/BD decida
                Contrasena = string.IsNullOrWhiteSpace(contrasena) ? null : HashPassword(contrasena),
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

        // Hash de contraseña con PBKDF2; formato devuelto: {iteraciones}.{saltBase64}.{hashBase64}
        private static string HashPassword(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            const int iterations = 10000;
            const int saltSize = 16; // bytes
            const int hashSize = 32; // bytes

            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[saltSize];
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
                {
                    var hash = pbkdf2.GetBytes(hashSize);
                    return $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
                }
            }
        }
    }
}
