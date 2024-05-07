using Gestión_Tickest_Pendientes.Entitys;
using Gestión_Tickest_Pendientes.Services;
using signotec.STPadLibNet;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Gestión_Tickest_Pendientes.Services;

namespace Gestión_Tickest_Pendientes.windows
{
    /// <summary>
    /// Lógica de interacción para Ticket.xaml
    /// </summary>
    public partial class WinTicket : Window
    {
        private Context _context = new Context();
        private Empresa _empresa;
        private STPadLib stPad = new STPadLib();
        private DbCommandos _dbCommandos;
        public Aplicacion _aplicacion;
        private TicketDto _ticketDto;
        public WinTicket(string action, Empresa empresa)
        {
            InitializeComponent();
            Actions(action);
            _empresa = empresa;
        }
        public WinTicket(string action, Empresa empresa, Aplicacion aplicacion)
        {
            InitializeComponent();
            _aplicacion = aplicacion;
            _empresa = empresa;
            Actions(action);
            
            
        }
        public WinTicket(string action, Empresa empresa, Aplicacion aplicacion, TicketDto ticketDto)
        {
            InitializeComponent();
            _aplicacion = aplicacion;
            _empresa = empresa;
            _ticketDto = ticketDto; 
            Actions(action);


        }
        private void Actions(string action)
        {
            //Esta funcion realiza una acción difrente el la ventana  WinTicket segun el boton pressionado
            var cmd = new DbCommandos();
            switch (action)
            {
                case "new":
                    
                    MessageBox.Show("new");
                    CerraTableta();
                    inicializarTableta();
                    Title.Content = "Nuevo Ticket";
                    btnTicket.Content = "Guardar";
                    txtFecha.Text = DateTime.Now.ToString();
                    btnTicket.Click += newBotonClick;
                    txtAdvertencia.Visibility = Visibility.Hidden;
                    break;
                case "modify":
                    Title.Content = "Modificar Ticket";
                    btnTicket.Content = "Guardar";
                    txtAlbaran.BorderBrush = Brushes.Red;
                    txtNif.BorderBrush = Brushes.Red;
                    txtFecha.BorderBrush = Brushes.Red;
                    inkFirma.Visibility = Visibility.Hidden;
                    BuscarClienteByCif.Visibility = Visibility.Hidden;
                    btnTicket.Click += modifyBotonClick;
                    FillTicketComplet();
                    MessageBox.Show("modify");
                    break;
                case "view":
                    Title.Content = "Vista Ticket";
                    btnTicket.Content = "Imprimir";
                    btnTicket.Click += viewBotonClick;
                    BuscarClienteByCif.Visibility = Visibility.Hidden;
                    txtAdvertencia.Visibility = Visibility.Hidden;
                    FillTicketComplet();
                    MessageBox.Show("view");
                    break;
            }
        }
        private async void newBotonClick(object sender, RoutedEventArgs e)
        {
            // Al presionar el boton generamos un ticket nuevo y si el cliente no existe en base de datos se guarda
            var cmd = new DbCommandos();


            if (capturarFirma() && !string.IsNullOrEmpty(txtNombre.Text) && !string.IsNullOrEmpty(txtNif.Text) && !string.IsNullOrEmpty(txtDireccion.Text) && !string.IsNullOrEmpty(txtCp.Text)
                && !string.IsNullOrEmpty(txtTelefono.Text) && !string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(txtAlbaran.Text) && !string.IsNullOrEmpty(txtSala.Text) && !string.IsNullOrEmpty(txtMesa.Text)
                 && !string.IsNullOrEmpty(txtNif.Text) && !string.IsNullOrEmpty(txtTotal.Text))
            {
                try
                {
                    //Comprobamos que sear un numero 
                    Convert.ToDouble(txtTotal.Text);
                    try
                    {
                        _dbCommandos = new DbCommandos();
                        //Si no existe el cliente insertar en base de datos
                        if (_dbCommandos.GetClienteCif(txtNif.Text) == null)
                        {
                            _dbCommandos.InsertClient(txtNombre.Text, txtNif.Text, txtDireccion.Text, txtCp.Text, txtTelefono.Text, txtEmail.Text, _empresa.Id_Empresa);
                            _dbCommandos.InsertTicket(txtAlbaran.Text, txtTotal.Text, txtSala.Text, txtMesa.Text, txtNif.Text);

                        }
                        else
                        {
                            _dbCommandos.InsertTicket(txtAlbaran.Text, txtTotal.Text, txtSala.Text, txtMesa.Text, txtNif.Text);
                        }

                        this.Close();
                        _aplicacion.dataTickets.ItemsSource = cmd.GetAllTickets();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("El total no puede contener caracteres");
                    if (stPad.SignatureState == false)
                    {
                        inicializarTableta();
                        inkFirma.Strokes = [];
                    }
                }

            }
            else
            {
                MessageBox.Show("Todos los campo deben de ser rellenados");
            }

        }

        private void modifyBotonClick(object sender, RoutedEventArgs e)
        {
            //Actualizamos todos los datos nuevos modificados en la ventana
            var cmd = new DbCommandos();


            if (!string.IsNullOrEmpty(txtNombre.Text) && !string.IsNullOrEmpty(txtNif.Text) && !string.IsNullOrEmpty(txtDireccion.Text) && !string.IsNullOrEmpty(txtCp.Text)
                && !string.IsNullOrEmpty(txtTelefono.Text) && !string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(txtAlbaran.Text) && !string.IsNullOrEmpty(txtSala.Text) && !string.IsNullOrEmpty(txtMesa.Text)
                 && !string.IsNullOrEmpty(txtNif.Text) && !string.IsNullOrEmpty(txtTotal.Text))
            {
                try
                {
                    //Comprobamos que sera un numero 
                    Convert.ToDouble(txtTotal.Text);
                    try
                    {
                        
                        //Si no existe el cliente insertar en base de datos
                        cmd.UpdateCliente(txtNombre.Text, txtNif.Text, txtDireccion.Text, txtCp.Text, txtTelefono.Text, txtEmail.Text);

                        cmd.UpdateTicket(txtAlbaran.Text, txtTotal.Text, txtSala.Text, txtMesa.Text, txtNif.Text);

                       
                        this.Close();
                        _aplicacion.dataTickets.ItemsSource = cmd.GetAllTickets();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("El total no puede contener caracteres");
                    if (stPad.SignatureState == false)
                    {
                        inicializarTableta();
                        inkFirma.Strokes = [];
                    }
                }
            }
        }
        private void viewBotonClick(object sender, RoutedEventArgs e)
        {
            // Imprimimos un documento con los datos del ticket
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument document = CreateFlowDocument(); // Método para crear el contenido del documento
                printDialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, "Documento a imprimir");
            }
            this.Close();
        }
        public void inicializarTableta()
        {
           
            //Iniciamos el primer dispositivo  que haya enchufado
            try
            {
                stPad.DeviceOpen(0); // Intenta abrir el primer dispositivo conectado.
                

                stPad.SignatureDataReceived += STPad_SignatureDataReceived;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el dispositivo: {ex.Message}");
            }
            try
            {

                // Iniciar la captura de la firma.
                stPad.SignatureStart();
               
                stPad.DisplaySetText(10, 10, signotec.STPadLibNet.TextAlignment.Left, "Firma");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar la captura de la firma: {ex.Message}");
            }
        }

        private void STPad_SignatureDataReceived(object sender, SignatureDataReceivedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                int maxX = 8000; // Máximo valor en X reportado por la tableta
                int maxY = 4000; // Máximo valor en Y reportado por la tableta
                int x = e.xPos;
                int y = e.yPos;

                // Transformar coordenadas a la resolución de tu InkCanvas
                double transformedX = (x * inkFirma.ActualWidth) / maxX;
                double transformedY = (y * inkFirma.ActualHeight) / maxY;

                // Crear un nuevo Stroke para el punto
                StylusPointCollection points = new StylusPointCollection();
                points.Add(new StylusPoint(transformedX, transformedY));

                Stroke stroke = new Stroke(points);
                inkFirma.Strokes.Add(stroke);
            });
        }

        public bool capturarFirma()
        {            
            try
            {
                // Finaliza la captura de la firma.
                // Este paso depende del SDK y puede involucrar el llamado a métodos como SignatureStop() o SignatureConfirm().
                stPad.SignatureConfirm();

                // Obtén un array de bytes de la imagen del pad
                byte[] signatureData = stPad.SignatureGetSignData();
                
                if (signatureData == null || signatureData.Length == 0)
                {
                    return false;
                }
                else
                {                    
                    // guardamos la imagen en un archivo que vamos a reescribir cada vez que se haga una firma en la tab
                    stPad.SignatureSaveAsFileEx("../../../Resource/firma.png", 720, 500, 250, ImageFormat.Png, 5, System.Drawing.Color.Black, SignatureImageFlag.AlignBottom);

                    //path del archivo creado
                    string path = "../../../Resource/firma.png"; // Si el archivo no tiene extensión en el nombre, asegúrate de agregarla aquí

                    // Inicializar BitmapImage con el archivo 
                    BitmapImage bitmapImage = new BitmapImage();                   
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Carga la imagen completamente para mantenerla en memoria
                    bitmapImage.EndInit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                stPad.DeviceClose(0);
            }
        }

        private void BuscarClienteByCif_Click(object sender, RoutedEventArgs e)
        {
            // Este evento se ejecuta cuando se presiona el boton de buscar el la ventana. Este hace una busqueda de un cliente para con esos datos rellenar los campos automaticamente
            var cmd = new DbCommandos();
            var cliente = cmd.GetClienteCif(txtNif.Text);
            if (cliente != null)
            {
                txtNif.Text = cliente.CIF_Cliente;
                txtNombre.Text = cliente.Nombre_Cliente;
                txtTelefono.Text = cliente.Telefono_Cliente;
                txtEmail.Text = cliente.Email_Cliente;
                txtDireccion.Text = cliente.Direccion_Cliente;
                txtCp.Text = cliente.Codigo_Postal;
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //Si cerramos la ventana WinTicket cerramos el dispositivo para evitar conflictos
            CerraTableta();
        }
        private void CerraTableta()
        {
            //cerramos dispositivo
            if (stPad.SignatureState == true)
            {
                stPad.DeviceClose(0);
            }
        }
        private void FillTicketComplet()
        {
            //rellenamos todos los  campos de la ventana con los datos rescatados deun cliente y su ticket 
            try
            {
                txtSala.Text = _ticketDto.Sala;
                txtMesa.Text = _ticketDto.Mesa;
                txtAlbaran.Text = _ticketDto.Id_Albaran;
                txtFecha.Text = _ticketDto.Fecha.ToString();
                txtNombre.Text = _ticketDto.Nombre;
                txtNif.Text = _ticketDto.Cif_Cliente;
                txtTelefono.Text = _ticketDto.Telefono;
                txtEmail.Text = _ticketDto.Email;
                txtDireccion.Text = _ticketDto.Direccion;
                txtCp.Text = _ticketDto.Codigo_Postal;
                txtTotal.Text = _ticketDto.Total.ToString();
                
                if (_ticketDto.Firma != null && _ticketDto.Firma.Length > 0)
                {
                    BitmapImage bitmap = Tools.ConvertToBitmapImage(_ticketDto.Firma);
                    // Crear un nuevo objeto ImageBrush usando el BitmapImage
                    ImageBrush imageBrush = new ImageBrush(bitmap);
                    // Asignar el ImageBrush como el fondo del InkCanvas
                    inkFirma.Background = imageBrush;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private FlowDocument CreateFlowDocument()
        {
            // Crear un nuevo documento de flujo
            FlowDocument document = new FlowDocument();

            // Configurar estilos para el título y el contenido
            Style titleStyle = new Style(typeof(Paragraph));
            titleStyle.Setters.Add(new Setter(Paragraph.FontSizeProperty, 18.0));
            titleStyle.Setters.Add(new Setter(Paragraph.FontWeightProperty, FontWeights.Bold));
            titleStyle.Setters.Add(new Setter(Paragraph.MarginProperty, new Thickness(0, 10, 0, 5)));

            Style contentStyle = new Style(typeof(Paragraph));
            contentStyle.Setters.Add(new Setter(Paragraph.FontSizeProperty, 12.0));
            contentStyle.Setters.Add(new Setter(Paragraph.MarginProperty, new Thickness(0, 0, 0, 5)));

            // Crear un título para el documento
            Paragraph titleParagraph = new Paragraph(new Run("Venta del Pobre Gastro Bar"));
            titleParagraph.Style = titleStyle;
            document.Blocks.Add(titleParagraph);

            // Agregar cada propiedad del ticket al documento
            
            document.Blocks.Add(CreateParagraph("Número de Albarán:", _ticketDto.Id_Albaran.ToString(), contentStyle));
            document.Blocks.Add(CreateParagraph("Fecha:", _ticketDto.Fecha.ToString(), contentStyle));
            document.Blocks.Add(CreateParagraph("Sala:", _ticketDto.Sala, contentStyle));
            document.Blocks.Add(CreateParagraph("Mesa:", _ticketDto.Mesa, contentStyle));
            document.Blocks.Add(CreateParagraph("Total:", _ticketDto.Total.ToString(), contentStyle));
            document.Blocks.Add(CreateParagraph("CIF/NIF:", _ticketDto.NIF_CIF, contentStyle));
            document.Blocks.Add(CreateParagraph("Nombre Cliente:", _ticketDto.Nombre, contentStyle));
            document.Blocks.Add(CreateParagraph("Teléfono:", _ticketDto.Telefono, contentStyle));
            document.Blocks.Add(CreateParagraph("Dirección del Cliente:", _ticketDto.Direccion, contentStyle));
            document.Blocks.Add(CreateParagraph("Email:", _ticketDto.Email, contentStyle));
            document.Blocks.Add(CreateParagraph("Codigo Postal:", _ticketDto.Codigo_Postal.ToString(), contentStyle));
            

            // Agregar la firma como una imagen si está presente
            if (_ticketDto.Firma != null && _ticketDto.Firma.Length > 0)
            {
                BitmapImage bitmap = Tools.ConvertToBitmapImage(_ticketDto.Firma);
                if (bitmap != null)
                {
                    Image image = new Image();
                    image.Source = bitmap;
                    image.Width = 200; // Ajustar el ancho de la imagen según sea necesario
                    image.Margin = new Thickness(0, 10, 0, 0);
                    BlockUIContainer imageContainer = new BlockUIContainer(image);
                    document.Blocks.Add(imageContainer);
                }
            }

            return document;
        }
        private Paragraph CreateParagraph(string label, string value, Style style)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Style = style;
            paragraph.Inlines.Add(new Run(label) { FontWeight = FontWeights.Bold });
            paragraph.Inlines.Add(new Run(" " + value));
            return paragraph;
        }


    }
}
