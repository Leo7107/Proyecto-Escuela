using System;

namespace Proyecto_Escuela.Models
{
    public class Docente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Especialidad { get; set; }
        public string Documento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
    }
}