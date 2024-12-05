using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;
using System.Linq;

namespace EVENTPULSE
{
    public partial class Artistas : Window
    {
        private ObservableCollection<ArtistaModel> artistasOriginal;
        public ObservableCollection<ArtistaModel> ListaDeArtistas { get; set; }

        // Constante para el archivo de almacenamiento
        private const string ArchivoArtistas = "artistasPrueba.txt";

        public Artistas()
        {
            InitializeComponent();
            ListaDeArtistas = new ObservableCollection<ArtistaModel>();
            artistasOriginal = new ObservableCollection<ArtistaModel>();
            CargarArtistas();
            dgArtistas.ItemsSource = ListaDeArtistas;
        }

        // Cargar artistas desde el archivo
        private void CargarArtistas()
        {
            if (!File.Exists(ArchivoArtistas)) return; // Si no existe el archivo, salir.

            var lineas = File.ReadAllLines(ArchivoArtistas);
            foreach (var linea in lineas)
            {
                var datos = linea.Split('|');
                artistasOriginal.Add(new ArtistaModel
                {
                    Nombre = datos[0],
                    Apellido = datos[1],
                    Edad = int.Parse(datos[2]),
                    Cache = datos[3],
                    Genero = datos[4]
                });
            }

            ReiniciarFiltro();
        }

        // Guardar artistas en el archivo
        private void GuardarArtistas()
        {
            var lineas = artistasOriginal.Select(a =>
                $"{a.Nombre}|{a.Apellido}|{a.Edad}|{a.Cache}|{a.Genero}").ToList();
            File.WriteAllLines(ArchivoArtistas, lineas);
        }

        // Agregar artista
        private void BtnAgregarArtista_Click(object sender, RoutedEventArgs e)
        {
            var ventanaAgregar = new AgregarArtista();

            if (ventanaAgregar.ShowDialog() == true)
            {
                ListaDeArtistas.Add(ventanaAgregar.NuevoArtista);
                artistasOriginal.Add(ventanaAgregar.NuevoArtista);
                GuardarArtistas();
            }
        }

        // Información del artista
        private void BtnInformacionArtista_Click(object sender, RoutedEventArgs e)
        {
            if (dgArtistas.SelectedItem is ArtistaModel artistaSeleccionado)
            {
                MessageBox.Show($"Nombre: {artistaSeleccionado.Nombre}\n" +
                                $"Apellido: {artistaSeleccionado.Apellido}\n" +
                                $"Edad: {artistaSeleccionado.Edad}\n" +
                                $"Caché: {artistaSeleccionado.Cache}\n" +
                                $"Género: {artistaSeleccionado.Genero}",
                                "Información del Artista", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Selecciona un artista para ver su información.", "Información", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Eliminar artista
        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgArtistas.SelectedItem is ArtistaModel artistaSeleccionado)
            {
                if (MessageBox.Show($"¿Estás seguro de que deseas eliminar a {artistaSeleccionado.Nombre} {artistaSeleccionado.Apellido}?",
                                    "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    ListaDeArtistas.Remove(artistaSeleccionado);
                    artistasOriginal.Remove(artistaSeleccionado);
                    GuardarArtistas();
                    MessageBox.Show("Artista eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un artista para eliminar.", "Eliminar", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Editar artista
        private void BtnEditarArtista_Click(object sender, RoutedEventArgs e)
        {
            if (dgArtistas.SelectedItem is ArtistaModel artistaSeleccionado)
            {
                var ventanaEditar = new AgregarArtista(artistaSeleccionado);

                if (ventanaEditar.ShowDialog() == true)
                {
                    dgArtistas.Items.Refresh();
                    GuardarArtistas();
                    MessageBox.Show("Artista actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un artista para editar.", "Editar Artista", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Filtrar artistas
        private void BtnFiltrarArtistas_Click(object sender, RoutedEventArgs e)
        {
            var filtro = txtBuscarArtista.Text.ToLower();
            var resultados = artistasOriginal.Where(a => a.Nombre.ToLower().Contains(filtro) ||
                                                         a.Apellido.ToLower().Contains(filtro) ||
                                                         a.Genero.ToLower().Contains(filtro)).ToList();

            ListaDeArtistas.Clear();
            foreach (var artista in resultados)
            {
                ListaDeArtistas.Add(artista);
            }
        }

        // Reiniciar filtro
        private void BtnReiniciarFiltroArtistas_Click(object sender, RoutedEventArgs e)
        {
            ReiniciarFiltro();
        }

        private void ReiniciarFiltro()
        {
            ListaDeArtistas.Clear();
            foreach (var artista in artistasOriginal)
            {
                ListaDeArtistas.Add(artista);
            }
        }
    }

    public class ArtistaModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Cache { get; set; }
        public string Genero { get; set; }
    }
}
