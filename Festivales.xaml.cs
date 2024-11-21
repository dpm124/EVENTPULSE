using System.Collections.ObjectModel;
using System.Windows;

namespace EVENTPULSE
{
    public partial class FestivalesWindow : Window
    {
        // Lista observable para gestionar los festivales
        public ObservableCollection<Festival> Festivales { get; set; }

        public FestivalesWindow()
        {
            InitializeComponent();

            // Inicializar la lista de festivales
            Festivales = new ObservableCollection<Festival>
            {
                new Festival { Nombre = "Mad Cool", Ubicacion = "Barcelona", Dia = "21", Mes = "Junio", Estado = "Confirmado" },
                new Festival { Nombre = "Medusa Festival", Ubicacion = "Galicia", Dia = "30", Mes = "Junio", Estado = "Por Confirmar" },
                new Festival { Nombre = "Viña Rock", Ubicacion = "Albacete", Dia = "22", Mes = "Julio", Estado = "Cancelado" }
            };

            // Vincular la lista al DataGrid
            dgFestivales.ItemsSource = Festivales;
        }

        // Evento para el botón de Buscar
        private void OnBuscarClick(object sender, RoutedEventArgs e)
        {
            string filtro = txtBuscar.Text.ToLower();
            var resultados = new ObservableCollection<Festival>();

            foreach (var festival in Festivales)
            {
                if (festival.Nombre.ToLower().Contains(filtro) || festival.Ubicacion.ToLower().Contains(filtro))
                {
                    resultados.Add(festival);
                }
            }

            dgFestivales.ItemsSource = resultados;
        }

        // Evento para el botón de Añadir Festival
        private void OnAgregarFestivalClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcionalidad para añadir festival en desarrollo.");
        }

        // Evento para el botón de Editar
        private void OnEditarClick(object sender, RoutedEventArgs e)
        {
            var festival = (sender as FrameworkElement).DataContext as Festival;
            if (festival != null)
            {
                MessageBox.Show($"Editar festival: {festival.Nombre}");
            }
        }

        // Evento para el botón de Borrar
        private void OnBorrarClick(object sender, RoutedEventArgs e)
        {
            var festival = (sender as FrameworkElement).DataContext as Festival;
            if (festival != null && MessageBox.Show($"¿Estás seguro de que deseas borrar {festival.Nombre}?", "Confirmación", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Festivales.Remove(festival);
            }
        }
    }

    // Clase de dominio para Festival
    public class Festival
    {
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public string Dia { get; set; }
        public string Mes { get; set; }
        public string Estado { get; set; }
    }
}
