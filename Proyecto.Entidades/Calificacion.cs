namespace Proyecto.Entidades
{
    public class Calificacion
    {
        public int ID_Calificacion { get; set; }
        public int ID_Estudiante { get; set; }
        public int ID_Asignatura { get; set; }
        public decimal Nota { get; set; }
        public int Promedio { get; set; }
        public decimal Estado { get; set; }
    }
}
