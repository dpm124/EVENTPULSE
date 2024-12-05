using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace EVENTPULSE
{
    public partial class Escenarios : Window
    {
        private ObservableCollection<Escenario> escenariosOriginal;
        public ObservableCollection<Escenario> EscenariosList { get; set; }

        private const string ArchivoEscenarios = "escenariosPrueba.txt";

        public Escenarios()
        {
            InitializeComponent();

            escenariosOriginal = new ObservableCollection<Escenario>();
            EscenariosList = new ObservableCollection<Escenario>();
            CargarEscenarios();
            dgEscenarios.ItemsSource = EscenariosList;
        }

        private void CargarEscenarios()
        {
            if (!File.Exists(ArchivoEscenarios)) return;

            var lineas = File.ReadAllLines(ArchivoEscenarios);
            foreach (var linea in lineas)
            {
                var datos = linea.Split('|');
                escenariosOriginal.Add(new Escenario
                {
                    Nombre = datos[0],
                    Aforo = int.Parse(datos[1]),
                    NumeroSalidas = int.Parse(datos[2]),
                    NumeroAsesos = int.Parse(datos[3]),
                    NumeroServiciosMedicos = int.Parse(datos[4])
                });
            }
            ReiniciarFiltro();
        }

        private void GuardarEscenarios()
        {
            var lineas = escenariosOriginal.Select(e =>
                $"{e.Nombre}|{e.Aforo}|{e.NumeroSalidas}|{e.NumeroAsesos}|{e.NumeroServiciosMedicos}").ToList();
            File.WriteAllLines(ArchivoEscenarios, lineas);
        }

        private void BtnAgregarEscenario_Click(object sender, RoutedEventArgs e)
        {
            var ventanaAgregar = new AgregarEscenario();

            if (ventanaAgregar.ShowDialog() == true)
            {
                var nuevoEscenario = ventanaAgregar.EscenarioEditado;
                escenariosOriginal.Add(nuevoEscenario);
                EscenariosList.Add(nuevoEscenario);
                GuardarEscenarios();
                MessageBox.Show("Escenario agregado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnInformacionEscenario_Click(object sender, RoutedEventArgs e)
        {
            if (dgEscenarios.SelectedItem is Escenario escenarioSeleccionado)
            {
                MessageBox.Show($"Nombre: {escenarioSeleccionado.Nombre}\n" +
                                $"Aforo: {escenarioSeleccionado.Aforo}\n" +
                                $"N° Salidas: {escenarioSeleccionado.NumeroSalidas}\n" +
                                $"N° Asesos: {escenarioSeleccionado.NumeroAsesos}\n" +
                                $"N° Servicios Médicos: {escenarioSeleccionado.NumeroServiciosMedicos}",
                                "Información del Escenario", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Selecciona un escenario para ver su información.", "Información", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnEliminarEscenario_Click(object sender, RoutedEventArgs e)
        {
            if (dgEscenarios.SelectedItem is Escenario escenarioSeleccionado)
            {
                if (MessageBox.Show($"¿Seguro que deseas eliminar el escenario {escenarioSeleccionado.Nombre}?",
                                    "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    escenariosOriginal.Remove(escenarioSeleccionado);
                    EscenariosList.Remove(escenarioSeleccionado);
                    GuardarEscenarios();
                    MessageBox.Show("Escenario eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un escenario para eliminar.", "Eliminar Escenario", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnEditarEscenario_Click(object sender, RoutedEventArgs e)
        {
            if (dgEscenarios.SelectedItem is Escenario escenarioSeleccionado)
            {
                var ventanaEditar = new AgregarEscenario(escenarioSeleccionado);

                if (ventanaEditar.ShowDialog() == true)
                {
                    dgEscenarios.Items.Refresh();
                    GuardarEscenarios();
                    MessageBox.Show("Escenario actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un escenario para editar.", "Editar Escenario", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnFiltrarEscenarios_Click(object sender, RoutedEventArgs e)
        {
            var filtro = txtBuscarEscenario.Text.ToLower();
            var resultados = escenariosOriginal.Where(escenario => escenario.Nombre.ToLower().Contains(filtro)).ToList();

            EscenariosList.Clear();
            foreach (var escenario in resultados)
            {
                EscenariosList.Add(escenario);
            }
        }

        private void BtnReiniciarFiltro_Click(object sender, RoutedEventArgs e)
        {
            ReiniciarFiltro();
        }

        private void ReiniciarFiltro()
        {
            EscenariosList.Clear();
            foreach (var escenario in escenariosOriginal)
            {
                EscenariosList.Add(escenario);
            }
        }
    }

    public class Escenario
    {
        public string Nombre { get; set; }
        public int Aforo { get; set; }
        public int NumeroSalidas { get; set; }
        public int NumeroAsesos { get; set; }
        public int NumeroServiciosMedicos { get; set; }
    }
}
