using System;

namespace Proyecto.Entidades
{
    public class Estudiante
    {
        public int ID_Estudiante { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Documento { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Grado { get; set; }
    }
}
