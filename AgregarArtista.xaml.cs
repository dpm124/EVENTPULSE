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
            }
            else
            {
                MessageBox.Show("La edad debe ser un número válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            NuevoArtista.Cache = txtCache.Text;
            NuevoArtista.Genero = txtGenero.Text;

            DialogResult = true; // Indicar que se guardó correctamente
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Cancelar la operación
            Close();
        }
    }
}
