using System.Windows;

namespace GestionPruebas
{
    public partial class MainWindow : Window
    {
        private CasoPrueba casoActual;

        public MainWindow()
        {
            InitializeComponent();
            // Inicializamos un caso de prueba y lo vinculamos al DataContext
            casoActual = new CasoPrueba("Login Test", "Verificar que el login funciona correctamente", "Inicio de sesión exitoso");
            DataContext = casoActual;
        }

        // Evento para guardar
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Caso guardado:\n" +
                            $"- Nombre: {casoActual.Nombre}\n" +
                            $"- Descripción: {casoActual.Descripcion}\n" +
                            $"- Resultado Esperado: {casoActual.ResultadoEsperado}\n" +
                            $"- Resultado Obtenido: {casoActual.ResultadoObtenido}\n" +
                            $"- Estado: {casoActual.Estado}");
        }

        // Evento para ver los detalles en otra ventana
        private void VerDetalles_Click(object sender, RoutedEventArgs e)
        {
            DetallesWindow detallesWindow = new DetallesWindow(casoActual);
            detallesWindow.Show();
        }
    }
}
