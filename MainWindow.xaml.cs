using Gestión_Tickest_Pendientes.windows;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gestión_Tickest_Pendientes.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using Gestión_Tickest_Pendientes.Services;
using Microsoft.IdentityModel.Tokens;

namespace Gestión_Tickest_Pendientes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Creamos la llamada al contexto
        Context context = new Context ();
        DbCommandos dbCommandos;
        public MainWindow()
        {
            InitializeComponent();
           
        }


        private void IniciarSesion(object sender, RoutedEventArgs e)
        {
            

            // Crea una nueva instancia de la clase Login, que se utilizará para manejar el proceso de inicio de sesión.
            DbCommandos dbCommand = new DbCommandos();
            try
            {
                // Verifica si los campos de texto para el usuario y la contraseña no están vacíos. Usa el operador '||' (OR) que permitirá seguir si al menos uno de los campos tiene contenido.
                if (!string.IsNullOrEmpty(txtUsuario.ToString()) || !string.IsNullOrEmpty(txtPassword.ToString()))
                {
                    // Genera un hash de la contraseña ingresada utilizando un método 'CreateHash' de la clase 'Tools'.
                    string newPass = Tools.CreateHash(txtPassword.Password);

                    // Intenta iniciar sesión con el usuario y la contraseña (ya transformada en hash) a través del método 'login' del objeto 'login'. El resultado es un objeto 'Usuario', que puede ser null si las credenciales no son correctas.
                    Usuario? user = dbCommand.login(txtUsuario.Text, newPass);

                    
                    // Verifica si el objeto 'Usuario' no es null, lo que indicaría un inicio de sesión exitoso.
                    if (user != null)
                    {
                        Empresa? empresa = dbCommand.GetEmpresaId(user.Id_Empresa);
                        if (empresa != null)
                        {
                            // Crea una nueva instancia de la clase Aplicación
                            Aplicacion newWin = new Aplicacion(user, empresa);
                            // Muestra la nueva ventana de la aplicación.
                            newWin.Show();
                            // Cierra la ventana actual.
                            this.Close();
                        }
                        
                    }
                }
                else
                {
                    // Muestra un mensaje al usuario indicando que ambos campos deben ser rellenados.
                    MessageBox.Show("Debes de rellenar ambos campos");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("ErrorS");
            }
            
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Creamos una migracion
            context.Database.Migrate();
;
            // Ejecutamos la migracion con el comando : dotnet ef migrations add nombre_de_la_migracion

            // Tambien puede ser que necesitemos este otro comando: dotnet ef migrations add nombre_de_la_migracion --project path_del_proyecto

            // Para eliminar la ultima migración seria : dotnet ef migrations remove 

            //Nos aseguramos que cree la base de datos conforme a nuestro contexto
            //if ()
            //{
            //    MessageBox.Show("Nueva base de datos creada.");
            //    Empresa empresa = new Empresa("Gastro Bar Venta del Pobre", "55485544D", "Venta del Pobre","950385192","ventadelpobre@gmail.com");
            //    context.empresas.Add(empresa);
            //    context.SaveChanges();

            //} 

        }
    }
}