
namespace EVENTPULSE
{
    public class ArtistaModel
    {
        // Propiedades del artista
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Genero { get; set; }
        public string Descripcion { get; set; }
        public string GenerosMusicales { get; set; }
        public string SitioWeb { get; set; }
        public string ActuacionHora { get; set; }
        public string ActuacionFecha { get; set; }
        public string Lugar { get; set; }
        public string Estado { get; set; } = "En espera"; // Valor predeterminado
    }
}
