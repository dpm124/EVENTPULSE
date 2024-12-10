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
            }
            else
            {
                dpActuacionFecha.SelectedDate = null;
            }
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
