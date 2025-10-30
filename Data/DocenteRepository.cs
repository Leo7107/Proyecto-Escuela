using System.Collections.Generic;
using System.Linq;
using Proyecto_Escuela.Models;

namespace Proyecto_Escuela.Data
{
    public class DocenteRepository
    {
        private readonly List<Docente> items = new List<Docente>();
        private int nextId = 1;

        public bool Add(Docente d)
        {
            if (d == null) return false;
            d.Id = nextId++;
            items.Add(d);
            return true;
        }

        public List<Docente> GetAll()
        {
            // devolver copia para evitar modificación externa directa
            return items.Select(x => x).ToList();
        }

        public Docente GetById(int id)
        {
            return items.FirstOrDefault(x => x.Id == id);
        }

        public bool Update(Docente d)
        {
            var existing = GetById(d.Id);
            if (existing == null) return false;
            existing.Nombre = d.Nombre;
            existing.Especialidad = d.Especialidad;
            existing.Documento = d.Documento;
            existing.Telefono = d.Telefono;
            existing.Email = d.Email;
            return true;
        }

        public bool Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null) return false;
            return items.Remove(existing);
        }
    }
}