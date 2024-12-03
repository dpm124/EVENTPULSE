using System;
using System.Windows;

namespace EVENTPULSE
{
    public partial class AgregarFestivalWindow : Window
    {
        public Festival FestivalEditado { get; private set; }

        // Constructor para añadir un nuevo festival (sin parámetros)
        public AgregarFestivalWindow()
        {
            InitializeComponent();
            FestivalEditado = new Festival();
        }

        // Constructor para editar un festival existente
        public AgregarFestivalWindow(Festival festival)
        {
            InitializeComponent();

            FestivalEditado = festival;
            txtNombreFestival.Text = festival.Nombre; // Asignar nombre
            txtAbonoGeneral.Text = festival.Nombre;   // Asignar abono general si aplica
            txtUbicacionFestival.Text = festival.Ubicación;
            dpFechaFestival.SelectedDate = festival.Fecha;
            txtDescripcionFestival.Text = festival.Estado; // Ajustar según corresponda
        }
        private void BtnEditarEscenarios_Click(object sender, RoutedEventArgs e)
        {
            // Crear y mostrar la ventana de escenarios
            var escenarios = new Escenarios();
            escenarios.ShowDialog();
        }

        private void BtnEditarArtistas_Click(object sender, RoutedEventArgs e)
        {
            // Crear y mostrar la ventana de artistas
            var artistas = new Artistas();
            artistas.ShowDialog();
        }

        // Evento del botón Guardar
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txtNombreFestival.Text) ||
                string.IsNullOrWhiteSpace(txtAbonoGeneral.Text) ||
                string.IsNullOrWhiteSpace(txtUbicacionFestival.Text) ||
                dpFechaFestival.SelectedDate == null)
            {
                MessageBox.Show("Por favor, completa todos los campos obligatorios.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Actualizar los valores del festival editado
            FestivalEditado.Nombre = txtNombreFestival.Text;
            FestivalEditado.Ubicación = txtUbicacionFestival.Text;
            FestivalEditado.Fecha = dpFechaFestival.SelectedDate.Value;
            FestivalEditado.Estado = txtDescripcionFestival.Text;

            DialogResult = true; // Indicar que se guardaron los cambios
            Close();
        }
    }
}
