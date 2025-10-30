using System;

namespace Proyecto_Escuela.Models
{
    public class Asignatura
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Creditos { get; set; }
        public int? DocenteId { get; set; } // null = sin asignar
    }
}