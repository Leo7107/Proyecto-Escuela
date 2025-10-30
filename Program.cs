using System;
using Proyecto_Escuela.Data;
using Proyecto_Escuela.Models;

namespace Proyecto_Escuela
{
    internal class Program
    {
        static DocenteRepository docenteRepo = new DocenteRepository();
        static AsignaturaRepository asignaturaRepo = new AsignaturaRepository();

        static void Main(string[] args)
        {
            SeedData();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Sistema Académico - Módulos: Docentes y Asignaturas");
                Console.WriteLine("1. Módulo Docentes");
                Console.WriteLine("2. Módulo Asignaturas");
                Console.WriteLine("0. Salir");
                Console.Write("Opción: ");
                var key = Console.ReadLine();

                if (key == "0") break;
                if (key == "1") MenuDocentes();
                if (key == "2") MenuAsignaturas();
            }
        }

        static void SeedData()
        {
            docenteRepo.Add(new Docente { Nombre = "Ana Pérez", Especialidad = "Matemáticas", Documento = "12345678", Telefono = "555-0101", Email = "ana@escuela.local" });
            docenteRepo.Add(new Docente { Nombre = "Luis García", Especialidad = "Lengua", Documento = "87654321", Telefono = "555-0202", Email = "luis@escuela.local" });

            asignaturaRepo.Add(new Asignatura { Codigo = "MAT101", Nombre = "Álgebra I", Descripcion = "Álgebra básica", Creditos = 4 });
            asignaturaRepo.Add(new Asignatura { Codigo = "LEN101", Nombre = "Comunicación", Descripcion = "Expresión oral y escrita", Creditos = 3 });
        }

        static void MenuDocentes()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Módulo Docentes");
                Console.WriteLine("1. Listar docentes");
                Console.WriteLine("2. Crear docente");
                Console.WriteLine("3. Editar docente");
                Console.WriteLine("4. Eliminar docente");
                Console.WriteLine("0. Volver");
                Console.Write("Opción: ");
                var op = Console.ReadLine();

                if (op == "0") break;
                switch (op)
                {
                    case "1": ListarDocentes(); break;
                    case "2": CrearDocente(); break;
                    case "3": EditarDocente(); break;
                    case "4": EliminarDocente(); break;
                    default: break;
                }
                Console.WriteLine("Pulse una tecla para continuar...");
                Console.ReadKey();
            }
        }

        static void ListarDocentes()
        {
            Console.Clear();
            var list = docenteRepo.GetAll();
            Console.WriteLine("Id\tNombre\tEspecialidad\tDocumento\tTel\tEmail");
            foreach (var d in list)
                Console.WriteLine($"{d.Id}\t{d.Nombre}\t{d.Especialidad}\t{d.Documento}\t{d.Telefono}\t{d.Email}");
        }

        static void CrearDocente()
        {
            Console.Clear();
            var d = new Docente();
            Console.Write("Nombre: ");
            d.Nombre = Console.ReadLine();
            Console.Write("Especialidad: ");
            d.Especialidad = Console.ReadLine();
            Console.Write("Documento: ");
            d.Documento = Console.ReadLine();
            Console.Write("Teléfono: ");
            d.Telefono = Console.ReadLine();
            Console.Write("Email: ");
            d.Email = Console.ReadLine();

            var result = docenteRepo.Add(d);
            Console.WriteLine(result ? "Docente creado." : "Error al crear docente.");
        }

        static void EditarDocente()
        {
            Console.Clear();
            Console.Write("Id docente a editar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }
            var d = docenteRepo.GetById(id);
            if (d == null) { Console.WriteLine("Docente no encontrado."); return; }

            Console.Write($"Nombre ({d.Nombre}): ");
            var s = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(s)) d.Nombre = s;
            Console.Write($"Especialidad ({d.Especialidad}): ");
            s = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(s)) d.Especialidad = s;
            Console.Write($"Documento ({d.Documento}): ");
            s = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(s)) d.Documento = s;
            Console.Write($"Teléfono ({d.Telefono}): ");
            s = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(s)) d.Telefono = s;
            Console.Write($"Email ({d.Email}): ");
            s = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(s)) d.Email = s;

            var result = docenteRepo.Update(d);
            Console.WriteLine(result ? "Docente actualizado." : "Error al actualizar.");
        }

        static void EliminarDocente()
        {
            Console.Clear();
            Console.Write("Id docente a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }

            // Al eliminar docente, también desasignamos de asignaturas
            var deleted = docenteRepo.Delete(id);
            if (deleted)
            {
                asignaturaRepo.UnassignDocenteFromAll(id);
                Console.WriteLine("Docente eliminado y desasignado de asignaturas.");
            }
            else Console.WriteLine("Docente no encontrado.");
        }

        static void MenuAsignaturas()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Módulo Asignaturas");
                Console.WriteLine("1. Listar asignaturas");
                Console.WriteLine("2. Crear asignatura");
                Console.WriteLine("3. Editar asignatura");
                Console.WriteLine("4. Eliminar asignatura");
                Console.WriteLine("5. Asignar docente a asignatura");
                Console.WriteLine("6. Desasignar docente de asignatura");
                Console.WriteLine("0. Volver");
                Console.Write("Opción: ");
                var op = Console.ReadLine();

                if (op == "0") break;
                switch (op)
                {
                    case "1": ListarAsignaturas(); break;
                    case "2": CrearAsignatura(); break;
                    case "3": EditarAsignatura(); break;
                    case "4": EliminarAsignatura(); break;
                    case "5": AsignarDocente(); break;
                    case "6": DesasignarDocente(); break;
                    default: break;
                }
                Console.WriteLine("Pulse una tecla para continuar...");
                Console.ReadKey();
            }
        }

        static void ListarAsignaturas()
        {
            Console.Clear();
            var list = asignaturaRepo.GetAll();
            Console.WriteLine("Id\tCódigo\tNombre\tCréditos\tDocente");
            foreach (var a in list)
            {
                var docente = a.DocenteId.HasValue ? docenteRepo.GetById(a.DocenteId.Value)?.Nombre ?? "N/D" : "Sin asignar";
                Console.WriteLine($"{a.Id}\t{a.Codigo}\t{a.Nombre}\t{a.Creditos}\t{docente}");
            }
        }

        static void CrearAsignatura()
        {
            Console.Clear();
            var a = new Asignatura();
            Console.Write("Código: ");
            a.Codigo = Console.ReadLine();
            Console.Write("Nombre: ");
            a.Nombre = Console.ReadLine();
            Console.Write("Descripción: ");
            a.Descripcion = Console.ReadLine();
            Console.Write("Créditos (número): ");
            int.TryParse(Console.ReadLine(), out int c);
            a.Creditos = c;

            var result = asignaturaRepo.Add(a);
            Console.WriteLine(result ? "Asignatura creada." : "Error al crear asignatura.");
        }

        static void EditarAsignatura()
        {
            Console.Clear();
            Console.Write("Id asignatura a editar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }
            var a = asignaturaRepo.GetById(id);
            if (a == null) { Console.WriteLine("Asignatura no encontrada."); return; }

            Console.Write($"Código ({a.Codigo}): ");
            var s = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(s)) a.Codigo = s;
            Console.Write($"Nombre ({a.Nombre}): ");
            s = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(s)) a.Nombre = s;
            Console.Write($"Descripción ({a.Descripcion}): ");
            s = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(s)) a.Descripcion = s;
            Console.Write($"Créditos ({a.Creditos}): ");
            s = Console.ReadLine(); if (int.TryParse(s, out int nc)) a.Creditos = nc;

            var result = asignaturaRepo.Update(a);
            Console.WriteLine(result ? "Asignatura actualizada." : "Error al actualizar.");
        }

        static void EliminarAsignatura()
        {
            Console.Clear();
            Console.Write("Id asignatura a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }
            var deleted = asignaturaRepo.Delete(id);
            Console.WriteLine(deleted ? "Asignatura eliminada." : "Asignatura no encontrada.");
        }

        static void AsignarDocente()
        {
            Console.Clear();
            ListarAsignaturas();
            Console.Write("Id asignatura: ");
            if (!int.TryParse(Console.ReadLine(), out int asigId)) { Console.WriteLine("Id inválido."); return; }
            var asig = asignaturaRepo.GetById(asigId);
            if (asig == null) { Console.WriteLine("Asignatura no encontrada."); return; }

            ListarDocentes();
            Console.Write("Id docente a asignar: ");
            if (!int.TryParse(Console.ReadLine(), out int docId)) { Console.WriteLine("Id inválido."); return; }
            var doc = docenteRepo.GetById(docId);
            if (doc == null) { Console.WriteLine("Docente no encontrado."); return; }

            var ok = asignaturaRepo.AssignDocente(asigId, docId);
            Console.WriteLine(ok ? "Docente asignado." : "Error al asignar docente.");
        }

        static void DesasignarDocente()
        {
            Console.Clear();
            ListarAsignaturas();
            Console.Write("Id asignatura: ");
            if (!int.TryParse(Console.ReadLine(), out int asigId)) { Console.WriteLine("Id inválido."); return; }
            var ok = asignaturaRepo.UnassignDocente(asigId);
            Console.WriteLine(ok ? "Docente desasignado." : "Asignatura no encontrada o sin docente.");
        }
    }
}
