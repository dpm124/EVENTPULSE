using System.Windows;

namespace EVENTPULSE
{
    public partial class AgregarArtista : Window
    {
        public ArtistaModel NuevoArtista { get; private set; }

        // Constructor para agregar un nuevo artista
        public AgregarArtista()
        {
            InitializeComponent();
            NuevoArtista = new ArtistaModel(); // Inicializar un nuevo objeto ArtistaModel
        }

        // Constructor para editar un artista existente
        public AgregarArtista(ArtistaModel artistaExistente)
        {
            InitializeComponent();
            NuevoArtista = artistaExistente;

            // Cargar los datos del artista en los campos de texto
            txtNombre.Text = artistaExistente.Nombre;
            txtApellido.Text = artistaExistente.Apellido;
            txtEdad.Text = artistaExistente.Edad.ToString();
            txtCache.Text = artistaExistente.Cache;
            txtGenero.Text = artistaExistente.Genero;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Validar que todos los campos estén completos
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtEdad.Text) ||
                string.IsNullOrWhiteSpace(txtCache.Text) ||
                string.IsNullOrWhiteSpace(txtGenero.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Asignar los valores a NuevoArtista
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

            DialogResult = true; // Indicar que los datos se guardaron correctamente
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Indicar que se canceló la operación
            Close();
        }
    }
}
