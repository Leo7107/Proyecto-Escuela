namespace Proyecto.Entidades
{
    public class Calificacion
    {
        public int ID_Calificacion { get; set; }
        public int ID_Estudiante { get; set; }
        public int ID_Asignatura { get; set; }
        // Tres notas individuales (0..10)
        public decimal Nota1 { get; set; }
        public decimal Nota2 { get; set; }
        public decimal Nota3 { get; set; }
        // Promedio calculado (decimal)
        public decimal Promedio { get; set; }
        // Estado: activo/inactivo
        public bool Estado { get; set; }
    }
}
