using System;
using System.Windows;

namespace EVENTPULSE
{
    public partial class AgregarArtista : Window
    {
        public ArtistaModel NuevoArtista { get; private set; }

        public AgregarArtista()
        {
            InitializeComponent();
<<<<<<< Updated upstream
            NuevoArtista = new ArtistaModel();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Validar los campos antes de guardar
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtEdad.Text) ||
                string.IsNullOrWhiteSpace(txtCache.Text) ||
                string.IsNullOrWhiteSpace(txtGenero.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Asignar los valores al nuevo artista
            NuevoArtista.Nombre = txtNombre.Text;
            NuevoArtista.Apellido = txtApellido.Text;
            if (int.TryParse(txtEdad.Text, out int edad))
            {
                NuevoArtista.Edad = edad;
=======
            NuevoArtista = new ArtistaModel(); // Inicializar un nuevo objeto ArtistaModel
        }

        // Constructor para editar un artista existente
        public AgregarArtista(ArtistaModel artista)
        {
            InitializeComponent();

            // Cargar los datos del artista en los campos de texto
            txtNombre.Text = artista.Nombre;
            txtApellido.Text = artista.Apellido;
            txtEdad.Text = artista.Edad.ToString();
            txtGenero.Text = artista.Genero;
            txtDescripcion.Text = artista.Descripcion;
            txtGenerosMusicales.Text = artista.GenerosMusicales;
            txtSitioWeb.Text = artista.SitioWeb;
            if (DateTime.TryParse(artista.ActuacionFecha, out DateTime fecha))
            {
                dpActuacionFecha.SelectedDate = fecha;
>>>>>>> Stashed changes
            }
            else
            {
                dpActuacionFecha.SelectedDate = null;
            }
<<<<<<< Updated upstream
            NuevoArtista.Cache = txtCache.Text;
            NuevoArtista.Genero = txtGenero.Text;
=======
            txtActuacionHora.Text = artista.ActuacionHora;
            txtLugar.Text = artista.Lugar;
            cmbEstado.Text = artista.Estado;
        }

        // Método para guardar los datos del nuevo artista
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Asignar los datos del formulario
            NuevoArtista = new ArtistaModel
            {
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Edad = int.TryParse(txtEdad.Text, out int edad) ? edad : 0,
                Genero = txtGenero.Text,
                Descripcion = txtDescripcion.Text,
                GenerosMusicales = txtGenerosMusicales.Text,
                SitioWeb = txtSitioWeb.Text,
                ActuacionFecha = dpActuacionFecha.SelectedDate?.ToString("yyyy-MM-dd") ?? "Sin fecha",
                ActuacionHora = txtActuacionHora.Text,
                Lugar = txtLugar.Text,
                Estado = cmbEstado.Text
            };
>>>>>>> Stashed changes

            DialogResult = true; // Indicar que se guardó correctamente
            Close();
        }

        // Método para cancelar la acción
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Cancelar la operación
            Close();
        }
    }
}
