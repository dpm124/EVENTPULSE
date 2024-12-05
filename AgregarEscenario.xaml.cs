using System.Windows;

namespace EVENTPULSE
{
    public partial class AgregarEscenario : Window
    {
        public Escenario EscenarioEditado { get; private set; }

        // Constructor para agregar un nuevo escenario
        public AgregarEscenario()
        {
            InitializeComponent();
            EscenarioEditado = new Escenario();
            DataContext = EscenarioEditado;
        }

        // Constructor para editar un escenario existente
        public AgregarEscenario(Escenario escenarioExistente)
        {
            InitializeComponent();
            EscenarioEditado = escenarioExistente;
            DataContext = EscenarioEditado;
        }

        // Guardar cambios
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EscenarioEditado.Nombre))
            {
                MessageBox.Show("El nombre no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        // Cancelar operación
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
