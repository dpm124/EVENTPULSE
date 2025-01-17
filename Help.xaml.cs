using System;
using System.Collections.Generic;
using System.Windows;

namespace EVENTPULSE
{
    // Clase simple para guardar los datos de un autor
    public class Autor
    {
        public string Email { get; set; }
        public string Nick { get; set; }
        public string Nombre { get; set; }

        public Autor(string email, string nick, string nombre)
        {
            Email = email;
            Nick = nick;
            Nombre = nombre;
        }
    }

    public partial class Ayuda : Window
    {
        public Ayuda()
        {
            InitializeComponent();
            // Cuando la ventana cargue, ejecutamos la carga manual
            this.Loaded += Ayuda_Loaded;
        }

        private void Ayuda_Loaded(object sender, RoutedEventArgs e)
        {
            // Cargar autores de forma manual (no desde archivo)
            CargarAutoresManualmente();
        }

        /// <summary>
        /// Carga varios autores “a mano” y los muestra en el ListBox.
        /// Además, los imprime en consola para demostrar que están cargados.
        /// </summary>
        private void CargarAutoresManualmente()
        {
            // Creamos una lista de autores
            var listaAutores = new List<Autor>
            {
                new Autor("alex.salinero@alu.uclm.es", "alex", "Alex Salinero"),
                new Autor("diego.palomino1@alu.uclm.es", "diego", "Diego Palomino"),
                new Autor("jorge.rodriguez11@alu.uclm.es", "jorge", "Jorge Rodriguez")
            };

            // Mostrar en consola (si el proyecto está configurado como WPF, quizá no veas la consola,
            // pero sirve de ejemplo de cómo imprimirlos)
            foreach (var autor in listaAutores)
            {
                Console.WriteLine($"Correo: {autor.Email}, Nombre: {autor.Nombre}");
            }

            // Enlazar la lista de autores al ListBox
            AutoresList.ItemsSource = listaAutores;
        }
    }
}