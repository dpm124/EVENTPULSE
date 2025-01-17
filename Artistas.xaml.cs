using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace EVENTPULSE
{
    public partial class Artistas : Window
    {
        public ObservableCollection<ArtistaModel> ListaDeArtistas { get; set; }

        public Artistas()
        {
            InitializeComponent();
            ListaDeArtistas = new ObservableCollection<ArtistaModel>();
            dgArtistas.ItemsSource = ListaDeArtistas;
        }

        private void BtnAgregarArtista_Click(object sender, RoutedEventArgs e)
        {
            // Crear instancia de la ventana AgregarArtista
            var ventanaAgregar = new AgregarArtista();

            // Mostrar la ventana de forma modal
            if (ventanaAgregar.ShowDialog() == true)
            {
                // Agregar el nuevo artista a la lista
                ListaDeArtistas.Add(ventanaAgregar.NuevoArtista);
            }
        }


        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (dgArtistas.SelectedItem is ArtistaModel artistaSeleccionado)
            {
                MessageBox.Show($"Información del artista:\n\n" +
                                $"Nombre: {artistaSeleccionado.Nombre}\n" +
                                $"Apellido: {artistaSeleccionado.Apellido}\n" +
                                $"Edad: {artistaSeleccionado.Edad}\n" +
                                $"Caché: {artistaSeleccionado.Cache}\n" +
                                $"Género: {artistaSeleccionado.Genero}",
                                "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Selecciona un artista para ver la información.", "Información", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgArtistas.SelectedItem is ArtistaModel artistaSeleccionado)
            {
                if (MessageBox.Show($"¿Estás seguro de que deseas eliminar a {artistaSeleccionado.Nombre} {artistaSeleccionado.Apellido}?",
                                    "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    ListaDeArtistas.Remove(artistaSeleccionado);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un artista para eliminar.", "Eliminar", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

    public class ArtistaModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Cache { get; set; }
        public string Genero { get; set; }
    }
}
