using System.Collections.Generic;
using System.Linq;
using Proyecto_Escuela.Models;

namespace Proyecto_Escuela.Data
{
    public class AsignaturaRepository
    {
        private readonly List<Asignatura> items = new List<Asignatura>();
        private int nextId = 1;

        public bool Add(Asignatura a)
        {
            if (a == null) return false;
            a.Id = nextId++;
            items.Add(a);
            return true;
        }

        public List<Asignatura> GetAll()
        {
            return items.Select(x => x).ToList();
        }

        public Asignatura GetById(int id)
        {
            return items.FirstOrDefault(x => x.Id == id);
        }

        public bool Update(Asignatura a)
        {
            var existing = GetById(a.Id);
            if (existing == null) return false;
            existing.Codigo = a.Codigo;
            existing.Nombre = a.Nombre;
            existing.Descripcion = a.Descripcion;
            existing.Creditos = a.Creditos;
            existing.DocenteId = a.DocenteId;
            return true;
        }

        public bool Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null) return false;
            return items.Remove(existing);
        }

        public bool AssignDocente(int asignaturaId, int docenteId)
        {
            var a = GetById(asignaturaId);
            if (a == null) return false;
            a.DocenteId = docenteId;
            return true;
        }

        public bool UnassignDocente(int asignaturaId)
        {
            var a = GetById(asignaturaId);
            if (a == null || !a.DocenteId.HasValue) return false;
            a.DocenteId = null;
            return true;
        }

        public void UnassignDocenteFromAll(int docenteId)
        {
            foreach (var a in items.Where(x => x.DocenteId == docenteId))
                a.DocenteId = null;
        }
    }
}