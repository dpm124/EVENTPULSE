using System.Windows;
using System.Windows.Controls;

namespace EVENTPULSE
{
    public partial class InfoArtista : Window
    {
        // Constructor que recibe un modelo de artista
        public InfoArtista(ArtistaModel artista)
        {
            InitializeComponent();

            // Vincular el modelo de datos al contexto de datos del XAML
            DataContext = artista;

            // Configurar el estado inicial del ComboBox según el estado del artista
            SetEstadoInicial(artista.Estado);
        }

        private void AbrirVentanaFestivales()
        {
            FestivalesWindow festivalesWindow = new FestivalesWindow();

            // 1) Al cerrarse la ventana de Festivales, volvemos a mostrar la principal.
            festivalesWindow.Closed += (s, e) =>
            {
                this.Show();
            };

            // 2) Ocultamos la ventana actual (MainWindow) en vez de cerrarla
            this.Hide();

            // 3) Mostramos la ventana de Festivales
            festivalesWindow.Show();
        }

        // Método para establecer el estado inicial en el ComboBox
        private void SetEstadoInicial(string estado)
        {
            if (cbEstado != null)
            {
                foreach (var item in cbEstado.Items)
                {
                    if (item is ComboBoxItem comboBoxItem && comboBoxItem.Content.ToString() == estado)
                    {
                        cbEstado.SelectedItem = comboBoxItem;
                        break;
                    }
                }
            }
        }

        // Método para guardar los cambios realizados al artista
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ArtistaModel artista)
            {
                // Actualizar el estado del artista con la selección actual
                if (cbEstado.SelectedItem is ComboBoxItem selectedItem)
                {
                    artista.Estado = selectedItem.Content.ToString();
                }

                MessageBox.Show("Información del artista actualizada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Cerrar la ventana después de guardar los cambios
            Close();
        }

        // Método para cancelar y cerrar la ventana sin guardar cambios
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
