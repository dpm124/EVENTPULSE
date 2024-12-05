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

        // Método para cargar los correos desde el archivo
        private void CargarAutores()
        {
            try
            {
                // Ruta del archivo de correos
                string filePath = @"C:º\Users\Diego\source\repos\EVENTPULSE\Correos\correos.txt";

                // Comprobar si el archivo existe
                if (File.Exists(filePath))
                {
                    // Leer el archivo y cargarlo en el TextBlock
                    string[] autores = File.ReadAllLines(filePath);
                    string autoresTexto = string.Join(Environment.NewLine, autores);
                    AutoresText.Text = autoresTexto;
                }
                else
                {
                    AutoresText.Text = "No se pudo cargar la información de los autores.";
                }
            }
            catch (Exception ex)
            {
                // En caso de error, mostrar mensaje de error
                AutoresText.Text = "Error al cargar los datos de los autores: " + ex.Message;
            }
        }
    }
}
