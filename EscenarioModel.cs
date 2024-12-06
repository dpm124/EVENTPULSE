public class EscenarioModel
{
    public string Nombre { get; set; }
    public bool TieneInformacion { get; set; }
    public string Artista { get; set; }
    public string Fecha { get; set; }
    public int AforoMax { get; set; }
    public int Aforo { get; set; } // Añadir esta propiedad
    public int NumeroSalidas { get; set; } // Añadir esta propiedad
    public int NumeroAsesos { get; set; } // Añadir esta propiedad
    public int NumeroServiciosMedicos { get; set; } // Añadir esta propiedad
}
