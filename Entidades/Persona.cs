namespace Entidades
{
    public class Persona
    {
        public int IdPersona { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}
