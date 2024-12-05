using EVENTPULSE;
using System.Windows;

public partial class InfoArtista : Window
{
    public InfoArtista(ArtistaModel artista)
    {
        DataContext = artista; // Vincular los datos al XAML
    }
}
