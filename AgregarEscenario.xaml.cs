using System.Windows;

namespace EVENTPULSE
{
    public partial class AgregarEscenario : Window
    {
        public EscenarioModel EscenarioEditado { get; private set; }

        public AgregarEscenario()
        {
            InitializeComponent();
            EscenarioEditado = new EscenarioModel(); // Crear un nuevo escenario vacío
        }

        public AgregarEscenario(EscenarioModel escenarioExistente)
        {
            InitializeComponent();
            EscenarioEditado = escenarioExistente; // Referencia al escenario existente

            // Cargar los valores en los controles
            txtNombre.Text = escenarioExistente.Nombre;
            txtAforo.Text = escenarioExistente.Aforo.ToString();
            txtNumeroSalidas.Text = escenarioExistente.NumeroSalidas.ToString();
            txtNumeroAsesos.Text = escenarioExistente.NumeroAsesos.ToString();
            txtNumeroServiciosMedicos.Text = escenarioExistente.NumeroServiciosMedicos.ToString();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Validar los campos
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtAforo.Text) ||
                string.IsNullOrWhiteSpace(txtNumeroSalidas.Text) ||
                string.IsNullOrWhiteSpace(txtNumeroAsesos.Text) ||
                string.IsNullOrWhiteSpace(txtNumeroServiciosMedicos.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Asignar los valores al modelo
            EscenarioEditado.Nombre = txtNombre.Text;
            EscenarioEditado.Aforo = int.Parse(txtAforo.Text);
            EscenarioEditado.NumeroSalidas = int.Parse(txtNumeroSalidas.Text);
            EscenarioEditado.NumeroAsesos = int.Parse(txtNumeroAsesos.Text);
            EscenarioEditado.NumeroServiciosMedicos = int.Parse(txtNumeroServiciosMedicos.Text);

            DialogResult = true; // Indicar éxito
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Cancelar operación
            Close();
        }
    }
}
