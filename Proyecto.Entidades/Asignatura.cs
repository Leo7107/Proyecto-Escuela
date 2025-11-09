namespace Proyecto.Entidades
{
    public class Asignatura
    {
        public int ID_Asignatura { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Creditos { get; set; }
        public int ID_Docente { get; set; }
    }
}
