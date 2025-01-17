using System;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EVENTPULSE;

namespace EVENTPULSE
{
    public partial class AgregarFestivalWindow : Window
    {
        public EscenarioModel EscenarioEditado { get; set; }
        public Festival FestivalEditado { get; private set; }
        public ObservableCollection<ArtistaModel> Artistas { get; private set; } = new ObservableCollection<ArtistaModel>();
        public ObservableCollection<EscenarioModel> Escenarios { get; set; }

        // Ruta del archivo de persistencia
        private readonly string filePath = "festival.txt";

        public AgregarFestivalWindow(Festival festival = null)
        {
            InitializeComponent();
            this.DataContext = this;

            // Si te pasa "null" => es un FESTIVAL NUEVO
            if (festival == null)
            {
                festival = new Festival();
            }

            // A PARTIR DE AQUÍ, festival no es null
            // Aseguramos que haya 5 escenarios
            // (Si el festival ya tiene menos de 5, completamos;
            //  si tiene más, no hacemos nada)
            if (festival.Escenarios.Count < 5)
            {
                for (int i = festival.Escenarios.Count; i < 5; i++)
                {
                    festival.Escenarios.Add(new EscenarioModel
                    {
                        Nombre = $"Escenario {i}",
                        TieneInformacion = false
                    });
                }
            }

            // Asignar a la propiedad FestivalEditado
            FestivalEditado = festival;

            // Rellenar datos del festival en los controles
            txtNombreFestival.Text = FestivalEditado.Nombre;
            txtAbonoGeneral.Text = FestivalEditado.Nombre;
            txtUbicacionFestival.Text = FestivalEditado.Ubicación;
            dpFechaFestival.SelectedDate = FestivalEditado.Fecha;
            txtDescripcionFestival.Text = FestivalEditado.Estado;

            // Si quieres enlazar la lista a una propiedad local (Escenarios),
            // hazlo ahora:
            this.Escenarios = FestivalEditado.Escenarios;

            // Resto de lógica
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
                MessageBox.Show("Por favor, completa todos los campos. En caso de no conocerlo, indíquelo o marque 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        private void BtnInformacionEscenario_Click(object sender, RoutedEventArgs e)
        {
            // Identifica el botón que lanzó el evento
            Button btn = sender as Button;
            if (btn?.Tag == null)
            {
                MessageBox.Show("No se ha podido identificar el escenario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Intenta obtener el índice del escenario desde el Tag del botón
            if (!int.TryParse(btn.Tag.ToString(), out int index) || index < 0 || index >= Escenarios.Count)
            {
                MessageBox.Show("El índice del escenario no es válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Obtén el escenario seleccionado
            var escenarioSeleccionado = Escenarios[index];

            // Muestra la información del escenario
            MessageBox.Show(
                $"Nombre: {escenarioSeleccionado.Nombre}\n" +
                $"Artista: {escenarioSeleccionado.Artista}\n" +
                $"Fecha: {escenarioSeleccionado.Fecha}\n" +
                $"Aforo Máximo: {escenarioSeleccionado.AforoMax}\n" +
                $"Tiene Información: {(escenarioSeleccionado.TieneInformacion ? "Sí" : "No")}",
                "Información del Escenario",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        private void BtnEditarEscenario_Click(object sender, RoutedEventArgs e)
        {
            // Identifica el botón que lanzó el evento
            Button btn = sender as Button;
            if (btn?.Tag == null)
            {
                MessageBox.Show("No se ha podido identificar el escenario a editar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Intenta obtener el índice del escenario desde el Tag del botón
            if (!int.TryParse(btn.Tag.ToString(), out int index) || index < 0 || index >= Escenarios.Count)
            {
                MessageBox.Show("El índice del escenario no es válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Obtén el escenario seleccionado
            var escenarioSeleccionado = Escenarios[index];

            // Abre la ventana de edición pasando el escenario seleccionado
            var ventanaEditarEscenario = new AgregarEscenario(escenarioSeleccionado);

            if (ventanaEditarEscenario.ShowDialog() == true)
            {
                // Los cambios se reflejan automáticamente en EscenarioSeleccionado gracias a TwoWay Binding
                MessageBox.Show("Escenario editado correctamente.", "Editar Escenario", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        // ===================================
        // ELIMINAR ESCENARIO
        // ===================================
        private void BtnEliminarEscenario_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn?.Tag == null)
            {
                MessageBox.Show("No se ha podido identificar el escenario a eliminar.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(btn.Tag.ToString(), out int index)
                || index < 0
                || index >= Escenarios.Count)
            {
                MessageBox.Show("El índice del escenario no es válido.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            var escenario = Escenarios[index];

            // Ya NO comprobamos si TieneInformacion es false
            // Ahora simplemente preguntamos si se desea "resetear" ese escenario

            var result = MessageBox.Show(
                $"¿Estás seguro de eliminar la información del escenario: {escenario.Nombre}?",
                "Confirmar Eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                // Marcar el escenario sin información
                escenario.TieneInformacion = false;

                // Restaurar el nombre y limpiar el artista (o quitarlo de la lista, según necesites)
                escenario.Nombre = $"Escenario {index + 1}";
                escenario.Artista = null;

                // Guardar en el archivo si corresponde
                GuardarEscenariosEnArchivo();

                MessageBox.Show("Información del escenario eliminada correctamente.",
                                "Eliminación Exitosa",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }
        private void BtnAgregarInformacionEscenario_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn?.Tag == null)
            {
                MessageBox.Show("No se ha podido identificar el escenario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int index;
            if (!int.TryParse(btn.Tag.ToString(), out index))
            {
                MessageBox.Show("El valor del Tag no es un número válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var escenario = Escenarios[index];

            // Abrir la ventana AgregarEscenario
            var ventanaAgregarEscenario = new AgregarEscenario();
            if (ventanaAgregarEscenario.ShowDialog() == true)
            {
                // El usuario ha guardado la información, ventanaAgregarEscenario.EscenarioEditado contiene los datos
                escenario.TieneInformacion = true;
                escenario.Nombre = ventanaAgregarEscenario.EscenarioEditado.Nombre;
                escenario.Artista = ventanaAgregarEscenario.EscenarioEditado.Artista;
               
                GuardarEscenariosEnArchivo();

                MessageBox.Show("Información agregada correctamente al escenario.",
                                "Agregar Información",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }


        private void GuardarEscenariosEnArchivo()
        {
            using (var writer = new StreamWriter("escenarios.txt", false))
            {
                foreach (var esc in Escenarios)
                {
                    writer.WriteLine($"{esc.Nombre}|{esc.Aforo}|{esc.NumeroSalidas}|{esc.NumeroAsesos}|{esc.NumeroServiciosMedicos}|{esc.Artista}|{esc.Fecha}|{esc.AforoMax}|{esc.TieneInformacion}");
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "Buscar Artista")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Buscar Artista";
                textBox.Foreground = Brushes.Gray;
            }
        }

    }
}


