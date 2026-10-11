using System.Linq;
using Datos;
using Entidades;

namespace Negocio
{
    public class PersonaNegocio
    {
        private readonly PersonaDatos personaDatos = new PersonaDatos();
        private readonly EstadoPersonaDatos estadoPersonaDatos = new EstadoPersonaDatos();

        public bool Insertar(Persona persona)
        {
            ValidarPersona(persona);
            persona.Estado = true;
            return personaDatos.Insertar(persona);
        }

        public bool Actualizar(Persona persona)
        {
            ValidarId(persona.IdPersona);
            ValidarPersona(persona);
            return personaDatos.Actualizar(persona);
        }

        public bool Eliminar(int idPersona)
        {
            ValidarId(idPersona);
            return personaDatos.Eliminar(idPersona);
        }

        public List<Persona> Listar()
        {
            List<Persona> personas = personaDatos.Listar();
            var consulta =
                from persona in personas
                select persona;

            return consulta.ToList();
        }

        public List<Persona> Buscar(string texto)
        {
            List<Persona> personas = personaDatos.Listar();
            texto = texto.Trim();

            return personas.Where(p =>
                p.Nombres.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                p.Apellidos.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                p.Dni.Contains(texto)).ToList();
        }

        public List<Persona> OrdenarPorApellido()
        {
            return personaDatos.Listar()
                .OrderBy(p => p.Apellidos)
                .ThenBy(p => p.Nombres)
                .ToList();
        }

        public List<PersonaEstado> ListarConEstado()
        {
            List<Persona> personas = personaDatos.ListarTodos();
            List<EstadoPersona> estados = estadoPersonaDatos.Listar();

            var consulta =
                from persona in personas
                join estado in estados on persona.Estado equals estado.Estado
                select new PersonaEstado
                {
                    IdPersona = persona.IdPersona,
                    Nombres = persona.Nombres,
                    Apellidos = persona.Apellidos,
                    Dni = persona.Dni,
                    DescripcionEstado = estado.Descripcion
                };

            return consulta.ToList();
        }

        public List<ResumenEstado> AgruparPorEstado()
        {
            return ListarConEstado()
                .GroupBy(p => p.DescripcionEstado)
                .Select(grupo => new ResumenEstado
                {
                    Estado = grupo.Key,
                    Cantidad = grupo.Count()
                })
                .OrderBy(resumen => resumen.Estado)
                .ToList();
        }

        private static void ValidarPersona(Persona persona)
        {
            if (string.IsNullOrWhiteSpace(persona.Nombres))
                throw new Exception("Los nombres son obligatorios.");
            if (string.IsNullOrWhiteSpace(persona.Apellidos))
                throw new Exception("Los apellidos son obligatorios.");
            if (string.IsNullOrWhiteSpace(persona.Dni))
                throw new Exception("El DNI es obligatorio.");

            persona.Nombres = persona.Nombres.Trim();
            persona.Apellidos = persona.Apellidos.Trim();
            persona.Dni = persona.Dni.Trim();

            if (persona.Nombres.Length > 80 || persona.Apellidos.Length > 80)
                throw new Exception("Los nombres y apellidos admiten hasta 80 caracteres.");
            if (persona.Dni.Length != 8 || !persona.Dni.All(c => c >= '0' && c <= '9'))
                throw new Exception("El DNI debe tener exactamente 8 números (del 0 al 9).");
        }

        private static void ValidarId(int idPersona)
        {
            if (idPersona <= 0)
                throw new Exception("Seleccione una persona del listado para continuar.");
        }
    }
}
