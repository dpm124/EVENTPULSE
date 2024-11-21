using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EVENTPULSE
{
    public partial class FestivalesWindow : Window
    {
        public ObservableCollection<Dictionary<string, string>> Festivales { get; set; }

        public FestivalesWindow()
        {
            InitializeComponent();

            // Inicializar la lista de festivales desde archivo
            Festivales = new ObservableCollection<Dictionary<string, string>>();
            try
            {
                string rutaArchivo = @"C:\Users\Diego\Desktop\UNI\3º\1er CUATRIMESTRE\3. INTERACCION PERSONA ORDENADOR\LAB\festivalData.txt";
                CargarFestivalesDesdeArchivo(rutaArchivo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar festivales: {ex.Message}");
            }

            // Enlazar los datos al DataGrid
            dgFestivales.ItemsSource = Festivales;
        }

        private void CargarFestivalesDesdeArchivo(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
            {
                MessageBox.Show("El archivo indicado no existe. Verifica la ruta.", "Error de archivo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var lineas = File.ReadAllLines(rutaArchivo);

                if (lineas.Length == 0)
                {
                    MessageBox.Show("El archivo está vacío.", "Archivo vacío", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Obtener los nombres de las columnas desde la primera línea
                var nombresColumnas = lineas[0].Split(',').Select(c => c.Trim()).ToArray();

                // Crear dinámicamente las columnas del DataGrid
                dgFestivales.Columns.Clear();
                foreach (var columna in nombresColumnas)
                {
                    dgFestivales.Columns.Add(new DataGridTextColumn
                    {
                        Header = columna,
                        Binding = new System.Windows.Data.Binding($"[{columna}]"),
                        Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                    });
                }

                // Agregar columna fija para opciones (botones)
                dgFestivales.Columns.Add(new DataGridTemplateColumn
                {
                    Header = "Opciones",
                    CellTemplate = new DataTemplate
                    {
                        VisualTree = CreateOpcionesTemplate()
                    },
                    Width = new DataGridLength(1, DataGridLengthUnitType.Auto)
                });

                // Cargar los datos de las siguientes líneas
                for (int i = 1; i < lineas.Length; i++)
                {
                    var valores = lineas[i].Split(',').Select(v => v.Trim()).ToArray();
                    if (valores.Length != nombresColumnas.Length)
                    {
                        MessageBox.Show($"La línea {i + 1} no coincide con el número de columnas especificadas en el encabezado.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Warning);
                        continue;
                    }

                    var festival = new Dictionary<string, string>();
                    for (int j = 0; j < nombresColumnas.Length; j++)
                    {
                        festival[nombresColumnas[j]] = valores[j];
                    }
                    Festivales.Add(festival);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar festivales desde archivo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private FrameworkElementFactory CreateOpcionesTemplate()
        {
            var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            stackPanelFactory.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Center);

            // Botón Editar
            var editarButton = new FrameworkElementFactory(typeof(Button));
            editarButton.SetValue(Button.ToolTipProperty, "Editar");
            editarButton.SetValue(Button.BackgroundProperty, Brushes.Transparent);
            editarButton.SetValue(Button.BorderBrushProperty, Brushes.Transparent);
            editarButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnEditarClick));
            var editarImage = new FrameworkElementFactory(typeof(Image));
            editarImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri("pack://application:,,,/imagenes/IMGeditar.png")));
            editarImage.SetValue(Image.WidthProperty, 16.0);
            editarImage.SetValue(Image.HeightProperty, 16.0);
            editarButton.AppendChild(editarImage);

            // Botón Borrar
            var borrarButton = new FrameworkElementFactory(typeof(Button));
            borrarButton.SetValue(Button.ToolTipProperty, "Borrar");
            borrarButton.SetValue(Button.BackgroundProperty, Brushes.Transparent);
            borrarButton.SetValue(Button.BorderBrushProperty, Brushes.Transparent);
            borrarButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnBorrarClick));
            var borrarImage = new FrameworkElementFactory(typeof(Image));
            borrarImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri("pack://application:,,,/imagenes/IMGborrar.png")));
            borrarImage.SetValue(Image.WidthProperty, 16.0);
            borrarImage.SetValue(Image.HeightProperty, 16.0);
            borrarButton.AppendChild(borrarImage);

            // Botón Info
            var infoButton = new FrameworkElementFactory(typeof(Button));
            infoButton.SetValue(Button.ToolTipProperty, "Información");
            infoButton.SetValue(Button.BackgroundProperty, Brushes.Transparent);
            infoButton.SetValue(Button.BorderBrushProperty, Brushes.Transparent);
            infoButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnInfoClick));
            var infoImage = new FrameworkElementFactory(typeof(Image));
            infoImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri("pack://application:,,,/imagenes/IMGinfo.png")));
            infoImage.SetValue(Image.WidthProperty, 16.0);
            infoImage.SetValue(Image.HeightProperty, 16.0);
            infoButton.AppendChild(infoImage);

            // Agregar botones al StackPanel
            stackPanelFactory.AppendChild(editarButton);
            stackPanelFactory.AppendChild(borrarButton);
            stackPanelFactory.AppendChild(infoButton);

            return stackPanelFactory;
        }

    private void OnBuscarClick(object sender, RoutedEventArgs e)
        {
            string filtro = txtBuscar.Text.ToLower();
            if (string.IsNullOrWhiteSpace(filtro))
            {
                dgFestivales.ItemsSource = Festivales;
                return;
            }

            var resultados = Festivales
                .Where(f => f.Values.Any(v => v.ToLower().Contains(filtro)))
                .ToList();

            dgFestivales.ItemsSource = new ObservableCollection<Dictionary<string, string>>(resultados);

            if (!resultados.Any())
            {
                MessageBox.Show("No se encontraron festivales con ese criterio de búsqueda.", "Búsqueda sin resultados", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnAgregarFestivalClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcionalidad para añadir festival en desarrollo.");
        }

        private void OnEditarClick(object sender, RoutedEventArgs e)
        {
            var festival = (sender as FrameworkElement)?.DataContext as Dictionary<string, string>;
            if (festival != null)
            {
                MessageBox.Show($"Editar festival: {string.Join(", ", festival.Select(kv => $"{kv.Key}: {kv.Value}"))}");
            }
        }

        private void OnBorrarClick(object sender, RoutedEventArgs e)
        {
            var festival = (sender as FrameworkElement)?.DataContext as Dictionary<string, string>;
            if (festival != null && MessageBox.Show($"¿Estás seguro de que deseas borrar este festival?", "Confirmación", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Festivales.Remove(festival);
            }
        }

        private void OnInfoClick(object sender, RoutedEventArgs e)
        {
            var festival = (sender as FrameworkElement)?.DataContext as Dictionary<string, string>;
            if (festival != null)
            {
                MessageBox.Show($"Información del festival:\n{string.Join("\n", festival.Select(kv => $"{kv.Key}: {kv.Value}"))}",
                                "Información del Festival",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No se pudo recuperar la información del festival.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Método para cambiar el color del estado 


        // Método para salir del programa
        private void OnSalirClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        // Método para ver la Ayuda
        private void OnAyudaClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcionalidad de ayuda en desarrollo.", "Ayuda", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
