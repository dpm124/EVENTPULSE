using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EVENTPULSE
{
    public partial class FestivalesWindow : Window
    {
        private ObservableCollection<Festival> festivalesOriginal;

        public ObservableCollection<Festival> Festivales { get; set; }

        public FestivalesWindow()
        {
            InitializeComponent();
            festivalesOriginal = new ObservableCollection<Festival>();
            Festivales = new ObservableCollection<Festival>(festivalesOriginal);
            dgFestivales.ItemsSource = Festivales;
        }

        // Manejador del botón Ayuda ANTIGUO 
        /* private void OnAyudaClick(object sender, RoutedEventArgs e)
        {
            // Crear una nueva instancia de la ventana Ayuda
            var ventanaAyuda = new Ayuda();

            // Mostrar la ventana Ayuda
            ventanaAyuda.ShowDialog(); // Usamos ShowDialog para que sea modal
        } */

        private void OnAyudaClick(object sender, RoutedEventArgs e)
        {
            // 1) Oculto FestivalesWindow antes de mostrar la Ayuda
            this.Hide();

            var ventanaAyuda = new Ayuda();

            // 2) Al hacer ShowDialog, se bloquea esta ventana 
            //    hasta que la de Ayuda se cierre
            ventanaAyuda.ShowDialog();

            // 3) Cuando se cierra Ayuda, volvemos a mostrar FestivalesWindow
            this.Show();
        }



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
            // 1) Ocultamos la ventana de Festivales
            this.Hide();

            // 2) Creamos la ventana de AgregarFestival
            var ventanaAgregar = new AgregarFestivalWindow();

            // Forzamos a que se abra maximizada (pantalla completa dentro de la ventana)
            ventanaAgregar.WindowState = WindowState.Maximized;

            // 3) La abrimos como modal, bloqueando la ejecución hasta que se cierre
            bool? resultado = ventanaAgregar.ShowDialog();

            // 4) Al cerrarse, re-mostramos FestivalesWindow
            this.Show();

            // 5) Si el resultado fue true, significa que el usuario guardó datos
            if (resultado == true)
            {
                var nuevoFestival = new Festival
                {
                    Nombre = ventanaAgregar.FestivalEditado.Nombre,
                    Ubicación = ventanaAgregar.FestivalEditado.Ubicación,
                    Fecha = ventanaAgregar.FestivalEditado.Fecha,
                    Estado = "Activo"
                };

                if (Festivales.Any(f =>
                    f.Nombre.Equals(nuevoFestival.Nombre, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Ya existe un festival con ese nombre.",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
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
                ventanaEditar.ShowDialog();
                // Al cerrar, los cambios quedan en "festivalSeleccionado"
                // (porque has manipulado el mismo objeto).
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un festival para editar.",
                                "Editar Festival",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }


        // Manejador del botón Borrar Festival
        private void OnBorrarClick(object sender, RoutedEventArgs e)
        {
            if (dgFestivales.SelectedItem is Festival festivalSeleccionado)
            {
                if (MessageBox.Show($"¿Eliminar el festival {festivalSeleccionado.Nombre}?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Festivales.Remove(festivalSeleccionado);
                    festivalesOriginal.Remove(festivalSeleccionado);
                }
            }
            else
            {
                MessageBox.Show("Por favor selecciona un festival para eliminar.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

    public class Festival
    {
        public string Nombre { get; set; }
        public string Ubicación { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }

        // AGREGAR: cada Festival tendrá su propia lista de escenarios
        public ObservableCollection<EscenarioModel> Escenarios { get; set; }
            = new ObservableCollection<EscenarioModel>();
    }
}
