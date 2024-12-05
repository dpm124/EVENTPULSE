using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;


namespace EVENTPULSE
{
    public partial class FestivalesWindow : Window
    {
        private ObservableCollection<Festival> festivalesOriginal;

        public ObservableCollection<Festival> Festivales { get; set; }

        private void GuardarFestivales()
        {
            var lineas = festivalesOriginal.Select(f =>
                $"{f.Nombre}|{f.Ubicación}|{f.Fecha:yyyy-MM-dd}|{f.Estado}").ToList();
            File.WriteAllLines(ArchivoFestivales, lineas);
        }

        private void CargarFestivales()
        {
            if (!File.Exists(ArchivoFestivales)) return;

            var lineas = File.ReadAllLines(ArchivoFestivales);
            foreach (var linea in lineas)
            {
                var datos = linea.Split('|');
                festivalesOriginal.Add(new Festival
                {
                    Nombre = datos[0],
                    Ubicación = datos[1],
                    Fecha = DateTime.Parse(datos[2]),
                    Estado = datos[3]
                });
            }
            Festivales = new ObservableCollection<Festival>(festivalesOriginal);
            dgFestivales.ItemsSource = Festivales;
        }
        public FestivalesWindow()
        {
            InitializeComponent();
            festivalesOriginal = new ObservableCollection<Festival>();
            Festivales = new ObservableCollection<Festival>(festivalesOriginal);
            CargarFestivales();
            dgFestivales.ItemsSource = Festivales;
        }


        // Manejador del botón Ayuda
        private void OnAyudaClick(object sender, RoutedEventArgs e)
        {
            // Crear una nueva instancia de la ventana de ayuda
            Ayuda ayuda = new Ayuda();

            // Mostrar la ventana de ayuda
            ayuda.ShowDialog(); // Usar ShowDialog para que sea modal (bloquea la ventana principal hasta que se cierre)
        }

        // Manejador para guardar los festivales en .txt 

        private const string ArchivoFestivales = "festivalesPrueba.txt";

        // Manejador del botón Salir
        private void OnSalirClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Seguro que deseas salir?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
        
        // Manejador del botón Filtrar
        private void OnFiltrarClick(object sender, RoutedEventArgs e)
        {
            var filtroTexto = txtBuscar.Text.ToLower();
            var filtroDesde = dpDesde.SelectedDate;
            var filtroHasta = dpHasta.SelectedDate;

            var resultados = festivalesOriginal.Where(f =>
                (string.IsNullOrEmpty(filtroTexto) || f.Nombre.ToLower().Contains(filtroTexto)) &&
                (!filtroDesde.HasValue || f.Fecha >= filtroDesde) &&
                (!filtroHasta.HasValue || f.Fecha <= filtroHasta)).ToList();

            Festivales.Clear();
            foreach (var festival in resultados)
                Festivales.Add(festival);

            var campoOrdenar = cbOrdenarPor.SelectedItem as ComboBoxItem;
            if (campoOrdenar != null)
            {
                switch (campoOrdenar.Content.ToString())
                {
                    case "Nombre":
                        Festivales = new ObservableCollection<Festival>(Festivales.OrderBy(f => f.Nombre));
                        break;
                    case "Ubicación":
                        Festivales = new ObservableCollection<Festival>(Festivales.OrderBy(f => f.Ubicación));
                        break;
                    case "Fecha":
                        Festivales = new ObservableCollection<Festival>(Festivales.OrderBy(f => f.Fecha));
                        break;
                }
                dgFestivales.ItemsSource = Festivales;
            }
        }

        // Manejador del botón Agregar Festival
        private void OnAgregarFestivalClick(object sender, RoutedEventArgs e)
        {
            var ventanaAgregar = new AgregarFestivalWindow();

            if (ventanaAgregar.ShowDialog() == true)
            {
                var nuevoFestival = new Festival
                {
                    Nombre = ventanaAgregar.FestivalEditado.Nombre,
                    Ubicación = ventanaAgregar.FestivalEditado.Ubicación,
                    Fecha = ventanaAgregar.FestivalEditado.Fecha,
                    Estado = "Activo"
                };

                if (Festivales.Any(f => f.Nombre.Equals(nuevoFestival.Nombre, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Ya existe un festival con ese nombre.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Festivales.Add(nuevoFestival);
                festivalesOriginal.Add(nuevoFestival);
            }
        }

        // Manejador del botón Editar Festival
        private void OnEditarClick(object sender, RoutedEventArgs e)
        {
            if (dgFestivales.SelectedItem is Festival festivalSeleccionado)
            {
                var ventanaEditar = new AgregarFestivalWindow(festivalSeleccionado);

                if (ventanaEditar.ShowDialog() == true)
                {
                    dgFestivales.Items.Refresh();
                    GuardarFestivales();
                    MessageBox.Show("Los cambios han sido guardados correctamente.", "Edición completada", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un festival para editar.", "Editar Festival", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Manejador del botón Borrar Festival
        private void OnBorrarClick(object sender, RoutedEventArgs e)
        {
            if (dgFestivales.SelectedItem is Festival festivalSeleccionado)
            {
                if (MessageBox.Show($"¿Eliminar el festival {festivalSeleccionado.Nombre}?", "Confirmar Eliminación", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Festivales.Remove(festivalSeleccionado);
                    festivalesOriginal.Remove(festivalSeleccionado);
                    GuardarFestivales();
                    MessageBox.Show("Festival eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor selecciona un festival para eliminar.", "Eliminar Festival", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnInformacionFestival_Click(object sender, RoutedEventArgs e)
        {
            if (dgFestivales.SelectedItem is Festival festivalSeleccionado)
            {
                MessageBox.Show($"Nombre: {festivalSeleccionado.Nombre}\n" +
                                $"Ubicación: {festivalSeleccionado.Ubicación}\n" +
                                $"Fecha: {festivalSeleccionado.Fecha.ToShortDateString()}\n" +
                                $"Estado: {festivalSeleccionado.Estado}",
                                "Información del Festival", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un festival para ver la información.", "Información", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }


    public class Festival
    {
        public string Nombre { get; set; }
        public string Ubicación { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
    }
}
