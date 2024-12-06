using System;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using EVENTPULSE;

namespace EVENTPULSE
{
    public partial class AgregarFestivalWindow : Window
    {
        public EscenarioModel EscenarioEditado { get; set;  }
        public Festival FestivalEditado { get; private set; }
        public ObservableCollection<ArtistaModel> Artistas { get; private set; } = new ObservableCollection<ArtistaModel>();
        public ObservableCollection<EscenarioModel> Escenarios { get; set; }

        // Ruta del archivo de persistencia
        private readonly string filePath = "festival.txt";

        // Constructor para añadir un nuevo festival (sin parámetros)
        public AgregarFestivalWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Escenarios = new ObservableCollection<EscenarioModel>
            {
                new EscenarioModel { Nombre = "Escenario PRINCIPAL", TieneInformacion = false },
                new EscenarioModel { Nombre = "Escenario 1", TieneInformacion = false },
                new EscenarioModel { Nombre = "Escenario 2", TieneInformacion = true, Artista = "Artista A", Fecha = "2024-05-15", AforoMax = 500 },
                new EscenarioModel { Nombre = "Escenario 3", TieneInformacion = true, Artista = "Artista B", Fecha = "2024-06-20", AforoMax = 600 },
                new EscenarioModel { Nombre = "Escenario 4", TieneInformacion = false }
            };
            this.DataContext = this; // Enlazar la lista de escenarios con la interfaz
            FestivalEditado = new Festival();
            dgArtistas.ItemsSource = Artistas;
            CargarFestivalDesdeArchivo();
        }

        // Constructor para editar un festival existente
        public AgregarFestivalWindow(Festival festival)
        {
            InitializeComponent();
            FestivalEditado = festival;
            txtNombreFestival.Text = festival.Nombre; // Asignar nombre
            txtAbonoGeneral.Text = festival.Nombre;   // Asignar abono general si aplica
            txtUbicacionFestival.Text = festival.Ubicación;
            dpFechaFestival.SelectedDate = festival.Fecha;
            txtDescripcionFestival.Text = festival.Estado; // Ajustar según corresponda



            dgArtistas.ItemsSource = Artistas;
            CargarFestivalDesdeArchivo();
        }

        // Método para cargar los datos del festival desde el archivo si existe
        private void CargarFestivalDesdeArchivo()
        {
            if (File.Exists(filePath))
            {
                var lineas = File.ReadAllLines(filePath);
                Artistas.Clear();
                foreach (var linea in lineas)
                {
                    var datos = linea.Split('|');
                    if (datos.Length >= 10) // Ajustado para incluir Lugar
                    {
                        Artistas.Add(new ArtistaModel
                        {
                            Nombre = datos[0],
                            Apellido = datos[1],
                            Edad = int.TryParse(datos[2], out int edad) ? edad : 0,
                            Genero = datos[3],
                            Descripcion = datos[4],
                            GenerosMusicales = datos[5],
                            SitioWeb = datos[6],
                            ActuacionFecha = datos[7],
                            ActuacionHora = datos[8],
                            Lugar = datos[9], // Nuevo atributo Lugar
                            Estado = datos.Length > 10 ? datos[10] : "En espera"
                        });
                    }
                }
            }
        }

        // Método para guardar los datos del festival en un archivo
        private void GuardarFestivalEnArchivo()
        {
            using (var writer = new StreamWriter(filePath, false))
            {
                foreach (var artista in Artistas)
                {
                    writer.WriteLine($"{artista.Nombre}|{artista.Apellido}|{artista.Edad}|{(string.IsNullOrWhiteSpace(artista.Genero) ? "N/A" : artista.Genero)}|{(string.IsNullOrWhiteSpace(artista.Descripcion) ? "Sin descripción" : artista.Descripcion)}|{(string.IsNullOrWhiteSpace(artista.GenerosMusicales) ? "Sin géneros" : artista.GenerosMusicales)}|{(string.IsNullOrWhiteSpace(artista.SitioWeb) ? "Sin sitio web" : artista.SitioWeb)}|{(string.IsNullOrWhiteSpace(artista.ActuacionFecha) ? "Sin fecha" : artista.ActuacionFecha)}|{(string.IsNullOrWhiteSpace(artista.ActuacionHora) ? "Sin hora" : artista.ActuacionHora)}|{(string.IsNullOrWhiteSpace(artista.Lugar) ? "Sin lugar" : artista.Lugar)}|{(string.IsNullOrWhiteSpace(artista.Estado) ? "En espera" : artista.Estado)}");
                }
            }
        }

        // Evento para el botón Filtrar Artistas
        private void BtnFiltrarArtistas_Click(object sender, RoutedEventArgs e)
        {
            string filtro = txtBuscarArtista.Text?.ToLower() ?? string.Empty;

            var artistasFiltrados = Artistas.Where(a =>
                a.Nombre.ToLower().Contains(filtro) ||
                a.Apellido.ToLower().Contains(filtro) ||
                a.Genero.ToLower().Contains(filtro)).ToList();

            dgArtistas.ItemsSource = artistasFiltrados;
        }


        // Evento para el botón Reiniciar Filtro Artistas
        private void BtnReiniciarFiltroArtistas_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para reiniciar el filtro
            txtBuscarArtista.Clear();
            dgArtistas.ItemsSource = Artistas;

        }

        // Evento para el botón Agregar Artista
        private void BtnAgregarArtista_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ventanaAgregarArtista = new AgregarArtista();

                if (ventanaAgregarArtista.ShowDialog() == true)
                {
                    var artistaAgregado = ventanaAgregarArtista.NuevoArtista;

                    // Validar que no se estén agregando artistas duplicados
                    if (Artistas.Any(a => a.Nombre.Equals(artistaAgregado.Nombre, StringComparison.OrdinalIgnoreCase) &&
                                          a.Apellido.Equals(artistaAgregado.Apellido, StringComparison.OrdinalIgnoreCase)))
                    {
                        MessageBox.Show("Este artista ya ha sido agregado. Verifica la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Añadir el nuevo artista a la colección
                    Artistas.Add(artistaAgregado);

                    // Guardar los cambios en el archivo
                    GuardarFestivalEnArchivo();

                    // Mensaje de confirmación
                    MessageBox.Show($"Artista agregado correctamente: {artistaAgregado.Nombre} {artistaAgregado.Apellido}",
                        "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Refrescar la tabla para mostrar el nuevo artista
                    dgArtistas.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores en caso de problemas
                MessageBox.Show($"Ocurrió un error al intentar agregar el artista. Detalles:\n{ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Evento para mostrar la información del Artista
        private void BtnInformacionArtista_Click(object sender, RoutedEventArgs e)
        {
            var artistaSeleccionado = dgArtistas.SelectedItem as ArtistaModel;
            if (artistaSeleccionado != null)
            {
                MessageBox.Show(
                    $"Nombre: {artistaSeleccionado.Nombre}\n" +
                    $"Apellido: {artistaSeleccionado.Apellido}\n" +
                    $"Edad: {artistaSeleccionado.Edad}\n" +
                    $"Género: {artistaSeleccionado.Genero}\n" +
                    $"Descripción: {artistaSeleccionado.Descripcion}\n" +
                    $"Géneros Musicales: {artistaSeleccionado.GenerosMusicales}\n" +
                    $"Sitio Web: {artistaSeleccionado.SitioWeb}\n" +
                    $"Fecha de Actuación: {artistaSeleccionado.ActuacionFecha}\n" +
                    $"Hora de Actuación: {artistaSeleccionado.ActuacionHora}\n" +
                    $"Lugar: {artistaSeleccionado.Lugar}\n" + // Mostrar Lugar
                    $"Estado: {artistaSeleccionado.Estado}",
                    "Información Completa del Artista",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un artista.", "Información", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Evento para editar un Artista
        private void BtnEditarArtista_Click(object sender, RoutedEventArgs e)
        {
            var artistaSeleccionado = dgArtistas.SelectedItem as ArtistaModel;

            if (artistaSeleccionado != null)
            {
                // Crea una nueva ventana para editar el artista
                var ventanaEditarArtista = new AgregarArtista(new ArtistaModel
                {
                    Nombre = artistaSeleccionado.Nombre,
                    Apellido = artistaSeleccionado.Apellido,
                    Edad = artistaSeleccionado.Edad,
                    Genero = artistaSeleccionado.Genero,
                    Descripcion = artistaSeleccionado.Descripcion,
                    GenerosMusicales = artistaSeleccionado.GenerosMusicales,
                    SitioWeb = artistaSeleccionado.SitioWeb,
                    ActuacionFecha = artistaSeleccionado.ActuacionFecha,
                    ActuacionHora = artistaSeleccionado.ActuacionHora,
                    Lugar = artistaSeleccionado.Lugar,
                    Estado = artistaSeleccionado.Estado
                });

                // Abre la ventana de edición
                if (ventanaEditarArtista.ShowDialog() == true)
                {
                    // Actualiza los valores del artista seleccionado
                    artistaSeleccionado.Nombre = ventanaEditarArtista.NuevoArtista.Nombre;
                    artistaSeleccionado.Apellido = ventanaEditarArtista.NuevoArtista.Apellido;
                    artistaSeleccionado.Edad = ventanaEditarArtista.NuevoArtista.Edad;
                    artistaSeleccionado.Genero = ventanaEditarArtista.NuevoArtista.Genero;
                    artistaSeleccionado.Descripcion = ventanaEditarArtista.NuevoArtista.Descripcion;
                    artistaSeleccionado.GenerosMusicales = ventanaEditarArtista.NuevoArtista.GenerosMusicales;
                    artistaSeleccionado.SitioWeb = ventanaEditarArtista.NuevoArtista.SitioWeb;
                    artistaSeleccionado.ActuacionFecha = ventanaEditarArtista.NuevoArtista.ActuacionFecha;
                    artistaSeleccionado.ActuacionHora = ventanaEditarArtista.NuevoArtista.ActuacionHora;
                    artistaSeleccionado.Lugar = ventanaEditarArtista.NuevoArtista.Lugar;
                    artistaSeleccionado.Estado = ventanaEditarArtista.NuevoArtista.Estado;

                    // Guarda los cambios en el archivo
                    GuardarFestivalEnArchivo();

                    // Refresca el DataGrid para mostrar los cambios
                    dgArtistas.Items.Refresh();

                    // Mensaje de confirmación
                    MessageBox.Show("Artista actualizado correctamente.", "Editar Artista", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un artista para editar.", "Editar Artista", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Evento para eliminar un Artista
        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var artistaSeleccionado = dgArtistas.SelectedItem as ArtistaModel;
            if (artistaSeleccionado != null)
            {
                if (MessageBox.Show($"¿Estás seguro de eliminar a {artistaSeleccionado.Nombre} {artistaSeleccionado.Apellido}?",
                    "Eliminar Artista", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Artistas.Remove(artistaSeleccionado);
                    GuardarFestivalEnArchivo(); // Actualizar el archivo después de eliminar
                    MessageBox.Show("Artista eliminado correctamente.", "Eliminar Artista", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un artista para eliminar.", "Eliminar Artista", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Evento del botón Guardar
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txtNombreFestival.Text) ||
                string.IsNullOrWhiteSpace(txtAbonoGeneral.Text) ||
                string.IsNullOrWhiteSpace(txtUbicacionFestival.Text) ||
                dpFechaFestival.SelectedDate == null)
            {
                MessageBox.Show("Por favor, completa todos los campos obligatorios.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Actualizar los valores del festival editado
            FestivalEditado.Nombre = txtNombreFestival.Text;
            FestivalEditado.Ubicación = txtUbicacionFestival.Text;
            FestivalEditado.Fecha = dpFechaFestival.SelectedDate.Value;
            FestivalEditado.Estado = txtDescripcionFestival.Text;

            // Guardar los datos del festival en el archivo
            GuardarFestivalEnArchivo();
            MessageBox.Show("Festival guardado correctamente.", "Guardar Festival", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true; // Indicar que se guardaron los cambios
            Close();
        }

        // Manejo del evento cuando el botón "Escenario 1" es presionado
        private void BtnEscenario1_Click(object sender, RoutedEventArgs e)
        {
            var escenario = Escenarios[1];
            if (escenario.TieneInformacion)
            {
                MessageBox.Show($"Escenario 1 - Artista: {escenario.Artista}, Fecha: {escenario.Fecha}, Aforo: {escenario.AforoMax}");
            }
            else
            {
                MessageBox.Show("Escenario 1 aún no tiene información.");
            }
        }

        // Manejo del evento cuando el botón "Escenario Principal" es presionado
        private void BtnEscenarioPrincipal_Click(object sender, RoutedEventArgs e)
        {
            var escenario = Escenarios[0];
            if (escenario.TieneInformacion)
            {
                MessageBox.Show($"Escenario Principal - Artista: {escenario.Artista}, Fecha: {escenario.Fecha}, Aforo: {escenario.AforoMax}");
            }
            else
            {
                MessageBox.Show("Escenario Principal aún no tiene información.");
            }
        }

        // Manejo del evento cuando el botón "Escenario 2" es presionado
        private void BtnEscenario2_Click(object sender, RoutedEventArgs e)
        {
            var escenario = Escenarios[2];
            if (escenario.TieneInformacion)
            {
                MessageBox.Show($"Escenario 2 - Artista: {escenario.Artista}, Fecha: {escenario.Fecha}, Aforo: {escenario.AforoMax}");
            }
            else
            {
                MessageBox.Show("Escenario 2 aún no tiene información.");
            }
        }

        // Manejo del evento cuando el botón "Escenario 3" es presionado
        private void BtnEscenario3_Click(object sender, RoutedEventArgs e)
        {
            var escenario = Escenarios[3];
            if (escenario.TieneInformacion)
            {
                MessageBox.Show($"Escenario 3 - Artista: {escenario.Artista}, Fecha: {escenario.Fecha}, Aforo: {escenario.AforoMax}");
            }
            else
            {
                MessageBox.Show("Escenario 3 aún no tiene información.");
            }
        }

        private void BtnEscenario4_Click(object sender, RoutedEventArgs e)
        {
            var escenario = Escenarios[4];
            if (escenario.TieneInformacion)
            {
                MessageBox.Show($"Escenario 4 - Artista: {escenario.Artista}, Fecha: {escenario.Fecha}, Aforo: {escenario.AforoMax}");
            }
            else
            {
                MessageBox.Show("Escenario 4 aún no tiene información.");
            }
        }

        // Manejo del evento para agregar un nuevo escenario
        private void BtnAgregarEscenario_Click(object sender, RoutedEventArgs e)
        {
            var nuevoEscenario = new EscenarioModel
            {
                Nombre = "Nuevo Escenario",
                TieneInformacion = false
            };

            Escenarios.Add(nuevoEscenario); // Añadir el nuevo escenario a la lista
        }

        private void BtnInformacionEscenario_Click(object sender, RoutedEventArgs e)
        {
            // Obtén el botón que fue presionado
            Button btn = sender as Button;

            // Obtén el índice del escenario desde la colección (asumimos que los botones están vinculados a la lista de escenarios)
            int index = (int)btn.Tag;  // Asegúrate de haber asignado el índice del escenario en el Tag del botón en XAML

            var escenario = Escenarios[index];

            if (escenario.TieneInformacion)
            {
                // Muestra la información del escenario en un mensaje
                MessageBox.Show($"Escenario: {escenario.Nombre}\n" +
                                $"Artista: {escenario.Artista}\n" +
                                $"Fecha: {escenario.Fecha}\n" +
                                $"Aforo Max: {escenario.AforoMax}",
                                "Información del Escenario",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Este escenario aún no tiene información.",
                                "Sin Información",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        private void BtnEditarEscenario_Click(object sender, RoutedEventArgs e)
        {
            // Obtener el botón que fue presionado
            Button btn = sender as Button;

            // Obtener el índice del escenario desde la colección de escenarios (usamos el Tag para almacenar el índice)
            int index = (int)btn.Tag;

            var escenarioSeleccionado = Escenarios[index];

            // Verificar si el escenario tiene información para editar
            if (escenarioSeleccionado != null)
            {
                // Crear una nueva ventana de edición para el escenario seleccionado
                var ventanaEditarEscenario = new AgregarEscenario(escenarioSeleccionado);

                // Mostrar la ventana de edición
                if (ventanaEditarEscenario.ShowDialog() == true)
                {
                    // Actualizar los valores del escenario seleccionado con los nuevos datos
                    escenarioSeleccionado.Nombre = ventanaEditarEscenario.EscenarioEditado.Nombre;
                    escenarioSeleccionado.Aforo = ventanaEditarEscenario.EscenarioEditado.Aforo;
                    escenarioSeleccionado.NumeroSalidas = ventanaEditarEscenario.EscenarioEditado.NumeroSalidas;
                    escenarioSeleccionado.NumeroAsesos = ventanaEditarEscenario.EscenarioEditado.NumeroAsesos;
                    escenarioSeleccionado.NumeroServiciosMedicos = ventanaEditarEscenario.EscenarioEditado.NumeroServiciosMedicos;

                    // Guardar los cambios en el archivo o en la base de datos
                    GuardarFestivalEnArchivo();

                    // Refrescar la interfaz de usuario si es necesario
                    MessageBox.Show("Escenario editado correctamente.", "Editar Escenario", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un escenario para editar.", "Editar Escenario", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnEliminarEscenario_Click(object sender, RoutedEventArgs e)
        {
            // Obtén el botón que fue presionado
            Button btn = sender as Button;

            // Obtén el índice del escenario desde la colección (usamos el Tag para almacenar el índice)
            int index = (int)btn.Tag;

            var escenario = Escenarios[index];

            // Confirmar si el usuario realmente quiere eliminar el escenario
            var result = MessageBox.Show($"¿Estás seguro de eliminar el escenario: {escenario.Nombre}?",
                                         "Confirmar Eliminación",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // Elimina el escenario de la lista
                Escenarios.RemoveAt(index);

                // Actualiza la interfaz o realiza cualquier otro paso necesario después de la eliminación
                MessageBox.Show("Escenario eliminado correctamente.",
                                "Eliminación Exitosa",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        private void BtnGuardarEscenario(object sender, RoutedEventArgs e)
        {
            // Validar los campos
            //if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            //    string.IsNullOrWhiteSpace(txtAforo.Text) ||
            //    string.IsNullOrWhiteSpace(txtNumeroSalidas.Text) ||
            //    string.IsNullOrWhiteSpace(txtNumeroAsesos.Text) ||
            //    string.IsNullOrWhiteSpace(txtNumeroServiciosMedicos.Text))
            //{
            //    MessageBox.Show("Por favor, completa todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            //   return;
            //}



            // Asignar los valores al escenario
            //EscenarioEditado.Nombre = txtNombre.Text;
            //EscenarioEditado.Aforo = int.Parse(txtAforo.Text);
            //EscenarioEditado.NumeroSalidas = int.Parse(txtNumeroSalidas.Text);
            //EscenarioEditado.NumeroAsesos = int.Parse(txtNumeroAsesos.Text);
            //EscenarioEditado.NumeroServiciosMedicos = int.Parse(txtNumeroServiciosMedicos.Text);

            // Establecer el resultado de la ventana como true para indicar éxito
            //DialogResult = true;
            //Close();
            MessageBox.Show("Por favor, completa todos los campos relacionados con el escenario.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;

        }

    }
}


