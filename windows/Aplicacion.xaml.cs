using Gestión_Tickest_Pendientes.Entitys;
using Gestión_Tickest_Pendientes.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Gestión_Tickest_Pendientes.Entitys;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using NPOI.SS.Formula.Functions;

namespace Gestión_Tickest_Pendientes.windows
{
    /// <summary>
    /// Lógica de interacción para Aplicacion.xaml
    /// </summary>
    public partial class Aplicacion : Window
    {
        private Usuario _usario;
        private Empresa _empresa;
        private WinTicket ticket;
        private DbCommandos cmd = new DbCommandos();
        private TicketDto ticketDto = new TicketDto();

        public Aplicacion(Usuario usario, Empresa empresa)
        {
            InitializeComponent();
            _usario = usario;
            _empresa = empresa;
            dataTickets.ItemsSource = cmd.GetAllTickets();
        }
        private void btnNewTicket_Click(object sender, RoutedEventArgs e)
        {
            ticket = new WinTicket("new", _empresa, this);
            ticket.Show();
        }

        private void btnModificarTicket_Click(object sender, RoutedEventArgs e)
        {
            if (_Selection())
            {
                ticket = new WinTicket("modify", _empresa,this, ticketDto);
                ticket.Show();
            }
            else
            {
                System.Windows.MessageBox.Show("Debes de seleccionar un ticket.");
            }
            
            
            
        }

        private void btnVerTicket_Click(object sender, RoutedEventArgs e)
        {
            if (_Selection())
            {
                ticket = new WinTicket("view", _empresa, this, ticketDto);
                ticket.Show();
            }
            else
            {
                MessageBox.Show("Debes de seleccionar un ticket.");
            }
        }

        private bool _Selection()
        {
            //creamos un objeto TicketDto con la fila seleccionada en el datagrid
             ticketDto = (TicketDto)dataTickets.SelectedItem;
            return (ticketDto != null);

        }

        private void btnEliminarTicket_Click(object sender, RoutedEventArgs e)
        {
            if (_Selection())
            {
                var security = MessageBox.Show($"Vas a eliminar el albarán nº {ticketDto.Id_Albaran} estas seguro?","Eliminar",MessageBoxButton.YesNo,MessageBoxImage.Question);
                if(security == MessageBoxResult.Yes)
                {
                    cmd.DeleteTicket(ticketDto.Id_Albaran);
                    dataTickets.ItemsSource = cmd.GetAllTickets();
                }
                
            }
            else
            {
                MessageBox.Show("Debes de seleccionar un ticket.");
            }
            
        }

        private void txtBuscador_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtBuscador.Text.Length == 0)
            {
                dataTickets.ItemsSource = cmd.GetAllTickets();
            }
            else
            {
                dataTickets.ItemsSource = cmd.GetAllTicketsCif(txtBuscador.Text);
            }
        }

        private void btnImprimirTabla_Click(object sender, RoutedEventArgs e)
        {
            

            var pd = new System.Windows.Controls.PrintDialog();
            
            if (pd.ShowDialog() == true)
            {
                IDocumentPaginatorSource idocument = CreateFlowDocument(dataTickets) as IDocumentPaginatorSource;
                pd.PrintDocument(idocument.DocumentPaginator, "Printing DataGrid");
            }
        }
        private FlowDocument CreateFlowDocument(DataGrid dataGrid)
        {
            var doc = new FlowDocument();
            doc.ColumnWidth = 500;
            var table = new Table();
            doc.Blocks.Add(table);

            // Agregar columnas al documento
            foreach (var column in dataGrid.Columns)
            {
                var col = new TableColumn();
                col.Width = new GridLength(column.ActualWidth);
                table.Columns.Add(col);
            }

            // Crear y añadir encabezado de tabla
            var header = new TableRow();
            foreach (var column in dataGrid.Columns)
            {
                header.Cells.Add(new TableCell(new Paragraph(new Run(column.Header.ToString()))));
            }
            var rowGroup = new TableRowGroup();
            rowGroup.Rows.Add(header);
            table.RowGroups.Add(rowGroup);

            // Añadir datos
            foreach (var item in dataGrid.Items)
            {
                var row = new TableRow();
                foreach (DataGridColumn column in dataGrid.Columns)
                {
                    if (column.GetCellContent(item) is TextBlock cellContent)
                    {
                        row.Cells.Add(new TableCell(new Paragraph(new Run(cellContent.Text))));
                    }
                    else
                    {
                        row.Cells.Add(new TableCell());
                    }
                }
                rowGroup.Rows.Add(row);
            }

            return doc;
        }

        private void btnCopiaSeguridad_Click(object sender, RoutedEventArgs e)
        {
            var clientes = cmd.GetClienteList();
            var tickets = cmd.GetTicketList();
            Tools tools = new Tools();
            string path = tools.ShowSaveFileDialog();

            tools.CreateExcelFileClient(clientes, path.Replace("archivo","clients"));
            tools.CreateExcelFileTicket(tickets, path.Replace("archivo", "tickets"));

        }

        private void btnRestaurarDB_Click(object sender, RoutedEventArgs e)
        {
            Tools tools = new Tools();

            string path = tools.ShowSaveFileDialog();
            cmd.RestaurarTickets(path);
        }
        
    }
}
