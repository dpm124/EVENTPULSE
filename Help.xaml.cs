using System;
using System.IO;
using System.Windows;

namespace EVENTPULSE
{
    public partial class Ayuda : Window
    {
        public Ayuda()
        {
            InitializeComponent();
            CargarAutores();
        }

        // Método para cargar los correos de los autores desde un archivo
        private void CargarAutores()
        {
            try
            {
                // Ruta del archivo de correos
                string filePath = @"C:\Users\Diego\source\repos\EVENTPULSE\Correos\correos.txt";

                // Comprobar si el archivo existe
                if (File.Exists(filePath))
                {
                    // Leer el contenido del archivo
                    string[] autores = File.ReadAllLines(filePath);
                    string autoresTexto = string.Join(Environment.NewLine, autores);

                    // Mostrar los autores en el TextBlock
                    AutoresText.Text = autoresTexto;
                }
                else
                {
                    AutoresText.Text = "No se pudo encontrar el archivo de autores.";
                }
            }
            catch (Exception ex)
            {
                // En caso de error, mostrar un mensaje en el TextBlock
                AutoresText.Text = "Error al cargar los datos de los autores: " + ex.Message;
            }
        }
    }
}
