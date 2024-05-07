using Gestión_Tickest_Pendientes.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Identity.Client;
using NPOI.SS.Formula.Functions;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Markup;
using static Gestión_Tickest_Pendientes.Entitys.TicketDto;
using MessageBox = System.Windows.MessageBox;

namespace Gestión_Tickest_Pendientes.Services
{
    public class DbCommandos 
    {
        private Context _context;
        

        public DbCommandos()
        {
            
        }

        public Usuario? login(string name, string pass)
        {
            //Esta funcion comrueba si existe algun cliente con su nombre y contraseña igual a los datos introducidos
            using ( _context = new Context())
            {
                try
                {
                     var userLog = _context.usuarios
                                    .Where(u => u.Name == name && u.Password == pass)
                                    .FirstOrDefault();
                    if (userLog == null)
                    {
                        MessageBox.Show("El usuario o contraseña no son correcto.");
                        return null;
                    }
                    return userLog ;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return null;
                    
                }
                finally 
                { 
                    _context.dispose(); 
                }
                
            }
                

        }
        public Empresa? GetEmpresaId(int id)
        {
            //Obtenemos los datos de la Empresa por su id
            using (_context = new Context())
            {
                try
                {
                    var empresa = _context.empresas
                                   .Where(u => u.Id_Empresa == id)
                                   .FirstOrDefault();
                    if (empresa == null)
                    {
                        MessageBox.Show("No se ha encontrado la empresa");
                    }
                    return empresa;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return null;

                }
                finally
                {
                    _context.dispose();
                }

            }
        }
        public IEnumerable<TicketDto>? GetAllTickets()
        {
            //Obtenemos todos los tickets 
            try
            {
                using (_context = new Context())
                {

                    var tickets = _context.tickets.Join(_context.clientes,
                                                         ticket => ticket.CIF_Cliente,
                                                         cliente => cliente.CIF_Cliente,
                                                         (ticket, cliente) => new TicketDto
                                                         {
                                                             Cif_Cliente = cliente.CIF_Cliente,
                                                             Id_Albaran = ticket.Id_Albaran,
                                                             Fecha = ticket.Fecha,
                                                             Sala = ticket.Sala,
                                                             Mesa = ticket.Mesa,
                                                             NIF_CIF = cliente.CIF_Cliente,
                                                             Nombre = cliente.Nombre_Cliente,
                                                             Telefono = cliente.Telefono_Cliente,
                                                             Email = cliente.Email_Cliente,
                                                             Direccion = cliente.Direccion_Cliente,
                                                             Codigo_Postal = cliente.Codigo_Postal,
                                                             Total = ticket.Total,
                                                             Firma = ticket.Firma_Cliente
                                                         }).ToList();

                    // Devolver el DataTable
                    return tickets;
                }
            }
            catch (Exception ex )
            {

                MessageBox.Show(ex.Message);
                return null;
            }  
        }
        public async void InsertClient(string name, string nif ,string direction, string cp, string tlf, string email, int id_empresa)
        {
            //Insertamos un cliente nuevo en base de datos
            using (_context = new Context())
            {
                try
                {
                        Cliente cliente = new Cliente();
                        cliente.CIF_Cliente = nif;
                        cliente.Nombre_Cliente = name;
                        cliente.Direccion_Cliente = direction;
                        cliente.Codigo_Postal = cp;
                        cliente.Telefono_Cliente = tlf;
                        cliente.Email_Cliente = email;
                        cliente.Id_Empresa = id_empresa;
                        await _context.clientes.AddAsync(cliente);
                        await _context.SaveChangesAsync();
                   
                    
                }
                catch (DbUpdateException e)
                {

                    MessageBox.Show(e.Message.ToString());
                    
                }
                finally { _context.dispose(); }

            }
                
            
            
        }
        public async void InsertTicket(string id_albaran, string total, string sala, string mesa, string cif)
        {
            //Insertamos un tikect en base de datos
            using (_context = new Context())
            {
                try
                {
                    Ticket ticket = new Ticket();
                    ticket.Id_Albaran = id_albaran;
                    ticket.CIF_Cliente = cif;
                    ticket.Fecha = DateTime.Now;
                    ticket.Total = Convert.ToDouble(total);
                    ticket.Sala = sala;
                    ticket.Mesa = mesa;
                    ticket.Firma_Cliente = File.ReadAllBytes("../../../Resource/firma.png");
                    await _context.tickets.AddAsync(ticket);
                    await _context.SaveChangesAsync();


                }
                catch (DbUpdateException e)
                {

                    MessageBox.Show(e.Message.ToString());

                }
                finally { _context.dispose(); }
            }
                


        }
        public List<Cliente> GetClienteList()
        {
            //Obtenemos todos los clientes que hya en base de datos y devolvemos una lista de ellos
            using (_context = new Context())
            {
             try
                {
                    using (_context = new Context())
                    {
                        var clientes = _context.clientes.ToList();
                        return clientes;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                    return null;
                }
            }     
        }
        public List<Ticket> GetTicketList()
        {
            //Obtenemos todos los tickets que hya en base de datos y devolvemos una lista de ellos
            using (_context = new Context())
            {
                try
                {
                    using (_context = new Context())
                    {
                        var tickets = _context.tickets.ToList();
                        return tickets;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                    return null;
                }
            }
        }
        public Cliente? GetClienteCif(string cif)
        {
            //Obtenemos el cliente a buscar por de su Cif
            using (_context = new Context())
            {
                try
                {
                    var cliente = _context.clientes
                                   .Where(u => u.CIF_Cliente == cif)
                                   .FirstOrDefault();
                    if (cliente == null)
                    {
                        MessageBox.Show("No se ha encontrado el cliente, el cliente introducido será guardado.");
                    }
                    return cliente;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return null;

                }
            }
        }
        public async void UpdateTicket(string id_albaran, string total, string sala, string mesa, string cif)
        {
            //Actualizamos el Tikect en función de los datos que hay en los textbox de cada dato
            using (var context = new Context())
            {
                try
                {
                    //La busqueda la realizamos por la key Id_Albaran
                    Ticket? ticket = context.tickets.FirstOrDefault((t) => t.Id_Albaran == id_albaran);
                    if (ticket == null)
                    {
                        MessageBox.Show("El Numero se albaran no pueder ser modificado. Si quieres puedes eliminar este tiket y crear uno nuevo.");
                    }
                    else
                    {
                        if (double.TryParse(total,out double result))
                        {
                            ticket.CIF_Cliente = cif;
                            ticket.Total = result;
                            ticket.Sala = sala;
                            ticket.Mesa = mesa;
                            await context.SaveChangesAsync();
                        }
                        
                    }
                }
                catch (DbUpdateException e)
                {

                    MessageBox.Show(e.Message.ToString());

                }
                finally { context.dispose(); }
            }
        }
        public async void UpdateCliente(string name, string nif, string direction, string cp, string tlf, string email)
        {
            //Actualizamos el Cliente en función de los datos que hay en los textbox de cada dato
            using (_context = new Context())
            {
                try
                {
                    //La busqueda la realizamos por la key CIF_Cliente
                    var cliente = _context.clientes.FirstOrDefault((c)=>c.CIF_Cliente == nif);
                    if (cliente == null)
                    {
                        MessageBox.Show("El cif introducido no se encuentra en base de datos. Si deseas introducir un cif nuevo genera un cliente nuevo.");
                    }
                    else
                    {                       
                        cliente.Nombre_Cliente = name;
                        cliente.Direccion_Cliente = direction;
                        cliente.Codigo_Postal = cp;
                        cliente.Telefono_Cliente = tlf;
                        cliente.Email_Cliente = email;
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateException e)
                {

                    MessageBox.Show(e.Message.ToString());

                }
                finally { _context.dispose(); }
            }
        }
        public async void DeleteTicket(string id_albaran)
        {
            //Borramos ticket por el numero de albaran
            try
            {
                using (_context = new Context())
                {
                    var ticket = _context.tickets.FirstOrDefault((t) => t.Id_Albaran == id_albaran);
                    _context.tickets.Remove(ticket);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
            
        }
        public IEnumerable<TicketDto> GetAllTicketsCif(string cif)
        {
            using (_context = new Context())
            {

                IEnumerable<TicketDto> tickets = _context.tickets.Join(_context.clientes.Where((c) => c.CIF_Cliente == cif),
                                                     ticket => ticket.CIF_Cliente,
                                                     cliente => cliente.CIF_Cliente,
                                                     (ticket, cliente) => new TicketDto
                                                     {
                                                         Cif_Cliente = cliente.CIF_Cliente,
                                                         Id_Albaran = ticket.Id_Albaran,
                                                         Fecha = ticket.Fecha,
                                                         Sala = ticket.Sala,
                                                         Mesa = ticket.Mesa,
                                                         NIF_CIF = cliente.CIF_Cliente,
                                                         Nombre = cliente.Nombre_Cliente,
                                                         Telefono = cliente.Telefono_Cliente,
                                                         Email = cliente.Email_Cliente,
                                                         Direccion = cliente.Direccion_Cliente,
                                                         Codigo_Postal = cliente.Codigo_Postal,
                                                         Total = ticket.Total,
                                                         Firma = ticket.Firma_Cliente
                                                     })
                                                     
                                                     .ToList();

                // Devolver el DataTable
                return tickets;
            }

        }
        public async void RestaurarClientes(string filePath)
        {
            // Configuramos EPPlus para que use una licencia no comercial
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                // Abrimos el archivo Excel
                
                using (_context = new Context())
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    var package = new ExcelPackage(fileInfo);
                    // Obtener la primera hoja
                    var worksheet = package.Workbook.Worksheets[0];
                    //Por cada fila insertamos los datos en Cliente y lo guardamos en base de datos
                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        Cliente cliente = new Cliente();
                        cliente.CIF_Cliente = worksheet.Cells[row, 1].Text;
                        cliente.Nombre_Cliente = worksheet.Cells[row, 2].Text;
                        cliente.Direccion_Cliente = worksheet.Cells[row, 3].Text;
                        cliente.Codigo_Postal = worksheet.Cells[row, 4].Text;
                        cliente.Telefono_Cliente = worksheet.Cells[row, 5].Text;
                        cliente.Email_Cliente = worksheet.Cells[row, 6].Text;
                        cliente.Id_Empresa = Convert.ToInt32(worksheet.Cells[row, 7].Text);
                        _context.Add(cliente);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public async void RestaurarTickets(string filePath)
        {
            // Configuramos EPPlus para que use una licencia no comercial
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                // Abrimos el archivo Excel

                using (_context = new Context())
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    var package = new ExcelPackage(fileInfo);
                    // Obtener la primera hoja
                    var worksheet = package.Workbook.Worksheets[0];
                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        //Por cada fila insertamos los datos en Tikect y lo guardamos en base de datos
                        Ticket ticket = new Ticket();
                        ticket.Id_Albaran = worksheet.Cells[row, 1].Text;
                        ticket.Total = Convert.ToInt32(worksheet.Cells[row, 2].Text);
                        ticket.Fecha = Convert.ToDateTime(worksheet.Cells[row, 3].Text);
                        ticket.Sala = worksheet.Cells[row, 4].Text;
                        ticket.Mesa = worksheet.Cells[row, 5].Text;
                        //generamos una array separando los indices por el -
                        var datastring = worksheet.Cells[row, 6].Text.Split('-');
                        //generamos un array con la longitud  del array anterior
                        byte[] data = new byte[datastring.Length];
                        //rellenamos el array data con los elementos del array "datastring"
                        var i = 0;
                        foreach ( var c in datastring)
                        {
                            data[i] = Convert.ToByte(c,16);
                            i++;
                        }

                        ticket.Firma_Cliente = data;
                        ticket.CIF_Cliente = worksheet.Cells[row, 7].Text;
                        _context.Add(ticket);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
    }
    
}
