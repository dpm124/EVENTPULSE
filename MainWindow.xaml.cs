using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace EVENTPULSE
{
    public partial class MainWindow : Window
    {
        // Lista de usuarios predefinidos
        private List<Usuario> usuarios = new List<Usuario>
        {
            new Usuario { Correo = "alex.salinero@alu.uclm.es", Contraseña = "alex", Nombre = "Alex Salinero" },
            new Usuario { Correo = "diego.palomino1@alu.uclm.es", Contraseña = "diego", Nombre = "Diego Palomino" },
            new Usuario { Correo = "jorge.rodriguez11@alu.uclm.es", Contraseña = "jorge", Nombre = "Jorge Rodriguez" },
            new Usuario { Correo = "root", Contraseña = "root", Nombre = "root" }
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        // Método para validar las credenciales
        private bool ValidarUsuario(string correo, string contraseña, out string nombreUsuario)
        {
            nombreUsuario = string.Empty;

            // Comprobar las credenciales en la lista de usuarios
            foreach (var usuario in usuarios)
            {
                if (usuario.Correo == correo && usuario.Contraseña == contraseña)
                {
                    nombreUsuario = usuario.Nombre;
                    return true; // Usuario válido
                }
            }

            return false; // Usuario no encontrado o credenciales incorrectas
        }

        // Método para manejar el evento de acceso
        private void OnAccederClick(object sender, RoutedEventArgs e)
        {
            // Obtener valores de los campos de texto
            string correoIngresado = UsernameTextBox.Text.Trim();
            string contraseñaIngresada = PasswordBox.Password.Trim();

            if (ValidarUsuario(correoIngresado, contraseñaIngresada, out string nombreUsuario))
            {
                // Mostrar pantalla de confirmación
                MostrarConfirmacionAcceso(nombreUsuario);
            }
            else
            {
                // Mostrar mensaje de error si las credenciales no coinciden
                MessageBox.Show("Correo o contraseña incorrectos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Mostrar la pantalla de confirmación de acceso
        private void MostrarConfirmacionAcceso(string nombreUsuario)
        {
            // Limpiar el StackPanel derecho
            RightPanel.Children.Clear();

            // Añadir imagen de usuario al panel derecho
            Image userIcon = new Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new System.Uri("/imagenes/icon-7797704.png", System.UriKind.Relative)),
                Width = 136,
                Height = 123,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 20)
            };
            RightPanel.Children.Add(userIcon);

            // Añadir etiqueta con el nombre del usuario
            TextBlock userName = new TextBlock
            {
                Text = nombreUsuario,
                FontSize = 18,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 5)
            };
            RightPanel.Children.Add(userName);

            // Añadir etiqueta de confirmación de acceso
            TextBlock confirmAccess = new TextBlock
            {
                Text = "Va a acceder con este usuario. ¿Desea continuar?",
                FontSize = 14,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                FontWeight = FontWeights.Bold, // Texto en negrita
                Foreground = System.Windows.Media.Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(10, 10, 10, 20)
            };
            RightPanel.Children.Add(confirmAccess);

            // Añadir botones de "Volver" y "Acceder"
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0)
            };

            Button volverButton = new Button
            {
                Content = "Volver",
                Width = 140,
                Height = 45,
                Background = System.Windows.Media.Brushes.IndianRed, // Fondo rojo
                Foreground = System.Windows.Media.Brushes.White, // Texto blanco
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                FontWeight = FontWeights.Bold, // Texto en negrita
                BorderBrush = System.Windows.Media.Brushes.DarkRed,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(10, 0, 5, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            volverButton.Click += (s, args) => ResetLoginView();
            buttonPanel.Children.Add(volverButton);

            Button accederButton = new Button
            {
                Content = "Acceder",
                Width = 140,
                Height = 45,
                Background = System.Windows.Media.Brushes.LightSkyBlue,
                Foreground = System.Windows.Media.Brushes.White,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                FontWeight = FontWeights.Bold,
                BorderBrush = System.Windows.Media.Brushes.DeepSkyBlue,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(5, 0, 10, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            accederButton.Click += (s, args) => AbrirVentanaFestivales();
            buttonPanel.Children.Add(accederButton);

            RightPanel.Children.Add(buttonPanel);
        }


        // Abrir la ventana de festivales y cerrar la ventana actual
        private void AbrirVentanaFestivales()
        {
            FestivalesWindow festivalesWindow = new FestivalesWindow();
            festivalesWindow.Show();
            this.Close(); // Cerrar la ventana de login
        }

        private void BtnAñadirFestival_Click(object sender, RoutedEventArgs e)
        {
            AgregarFestivalWindow ventana = new AgregarFestivalWindow();
            ventana.Show(); // Abre la ventana secundaria
        }

        // Método para resetear la vista de login
        // Método para resetear la vista de login
        // Método para resetear la vista de login
        private void ResetLoginView()
        {
            // Verificar si los campos de usuario y contraseña están vacíos
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Por favor, ingrese su usuario y contraseña antes de volver.",
                    "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Detener el proceso si los campos están vacíos
            }

            // Limpiar el campo de usuario y contraseña
            UsernameTextBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;

            // Asegúrate de que RightPanel tenga el diseño inicial
            RightPanel.Children.Clear();

            // Restaurar el diseño inicial del login
            Image userIcon = new Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new System.Uri("/imagenes/icon-7797704.png", System.UriKind.Relative)),
                Width = 136,
                Height = 123,
                Margin = new Thickness(0, 20, 0, 20)
            };
            RightPanel.Children.Add(userIcon);

            TextBlock userLabel = new TextBlock
            {
                Text = "Usuario o correo",
                FontSize = 16,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                FontWeight = FontWeights.SemiBold,
                Foreground = System.Windows.Media.Brushes.Black,
                Margin = new Thickness(10, 10, 0, 5)
            };
            RightPanel.Children.Add(userLabel);

            UsernameTextBox = new TextBox
            {
                Height = 30,
                Margin = new Thickness(10, 0, 10, 20),
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                FontSize = 14,
                Padding = new Thickness(5)
            };
            RightPanel.Children.Add(UsernameTextBox);

            TextBlock passwordLabel = new TextBlock
            {
                Text = "Contraseña",
                FontSize = 16,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                FontWeight = FontWeights.SemiBold,
                Foreground = System.Windows.Media.Brushes.Black,
                Margin = new Thickness(10, 10, 0, 5)
            };
            RightPanel.Children.Add(passwordLabel);

            PasswordBox = new PasswordBox
            {
                Height = 30,
                Margin = new Thickness(10, 0, 10, 20),
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                FontSize = 14,
                Padding = new Thickness(5)
            };
            RightPanel.Children.Add(PasswordBox);

            Button accederButton = new Button
            {
                Content = "Acceder",
                Width = 120,
                Height = 35,
                Background = System.Windows.Media.Brushes.LightBlue,
                Foreground = System.Windows.Media.Brushes.White,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(5),
                Margin = new Thickness(0, 20, 0, 0)
            };
            accederButton.Click += OnAccederClick;
            RightPanel.Children.Add(accederButton);

            // Reinicializar la lista de usuarios (si es necesario, sin usar correos.txt)
            usuarios = new List<Usuario>
    {
        new Usuario { Correo = "alex.salinero@alu.uclm.es", Contraseña = "alex", Nombre = "Alex Salinero" },
        new Usuario { Correo = "diego.palomino1@alu.uclm.es", Contraseña = "diego", Nombre = "Diego Palomino" },
        new Usuario { Correo = "jorge.rodriguez11@alu.uclm.es", Contraseña = "jorge", Nombre = "Jorge Rodriguez" },
        new Usuario { Correo = "root", Contraseña = "root", Nombre = "root" }
    };
        }
    }

        // Clase Usuario
        public class Usuario
    {
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public string Nombre { get; set; }
    }
}
