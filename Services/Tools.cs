using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using signotec.STPadLibNet;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Ink;
using System.Windows.Controls;
using System.IO;
using Gestión_Tickest_Pendientes.Entitys;
using OfficeOpenXml;
using System.Windows.Forms;
using Org.BouncyCastle.Utilities;
using MessageBox = System.Windows.MessageBox;
namespace Gestión_Tickest_Pendientes.Services
{
    internal class Tools
    {
    
        public static string CreateHash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computar el hash de la entrada y obtener el arreglo de bytes.
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convertir el arreglo de bytes a un string en hexadecimal.
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        public static BitmapImage ConvertToBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;
            // Convertir los bytes de la imagen en un objeto BitmapImage
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(imageData);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }
        public async void CreateExcelFileClient(List<Cliente> clientes, string filePath)
        {
            // Configurar EPPlus para usar el paquete sin licencia comercial
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            await Task.Run(() => {
                using (var package = new ExcelPackage())
            {
                // Agregar una hoja de cálculo
                var worksheet = package.Workbook.Worksheets.Add("Clientes");

                // Agregar encabezados de columna
                worksheet.Cells["A1"].Value = "Cif_Cliente";
                worksheet.Cells["B1"].Value = "Nombre_CLiente";
                worksheet.Cells["C1"].Value = "Dirección_Cliente";
                worksheet.Cells["D1"].Value = "Codigo_postal";
                worksheet.Cells["E1"].Value = "Teléfono_Cliente";
                worksheet.Cells["F1"].Value = "Email_Cliente";
                worksheet.Cells["G1"].Value = "Id_Empresa";

                // Llenar las celdas con los datos de los clientes
                int row = 2;
                foreach (var cliente in clientes)
                {
                    worksheet.Cells[$"A{row}"].Value = cliente.CIF_Cliente;
                    worksheet.Cells[$"B{row}"].Value = cliente.Nombre_Cliente;
                    worksheet.Cells[$"C{row}"].Value = cliente.Direccion_Cliente;
                    worksheet.Cells[$"D{row}"].Value = cliente.Codigo_Postal;
                    worksheet.Cells[$"E{row}"].Value = cliente.Telefono_Cliente;
                    worksheet.Cells[$"F{row}"].Value = cliente.Email_Cliente;
                    worksheet.Cells[$"G{row}"].Value = cliente.Id_Empresa;
                    row++;
                }

                // Ajustar el ancho de las columnas según el contenido
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Guardar el archivo Excel en el sistema de archivos
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
            });
            
        }
        public async void CreateExcelFileTicket(List<Ticket> tickets, string filePath)
        {
            // Configurar EPPlus para usar el paquete sin licencia comercial
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            await Task.Run(() =>
            {
                using (var package = new ExcelPackage())
                {
                    // Agregar una hoja de cálculo
                    var worksheet = package.Workbook.Worksheets.Add("Tickets");

                    // Agregar encabezados de columna
                    worksheet.Cells["A1"].Value = "Id_Albaran";
                    worksheet.Cells["B1"].Value = "Total";
                    worksheet.Cells["C1"].Value = "Fecha";
                    worksheet.Cells["D1"].Value = "Sala";
                    worksheet.Cells["E1"].Value = "Mesa";
                    worksheet.Cells["F1"].Value = "Firma";
                    worksheet.Cells["G1"].Value = "CIF_CLiente";

                    // Llenar las celdas con los datos de los clientes
                    int row = 2;
                    foreach (var ticket in tickets)
                    {
                        worksheet.Cells[$"A{row}"].Value = ticket.Id_Albaran;
                        worksheet.Cells[$"B{row}"].Value = ticket.Total;
                        worksheet.Cells[$"C{row}"].Value = ticket.Fecha.ToString();
                        worksheet.Cells[$"D{row}"].Value = ticket.Sala;
                        worksheet.Cells[$"E{row}"].Value = ticket.Mesa;
                        worksheet.Cells[$"F{row}"].Value = BitConverter.ToString(ticket.Firma_Cliente);
                        worksheet.Cells[$"G{row}"].Value = ticket.CIF_Cliente;
                        row++;
                    }

                    // Ajustar el ancho de las columnas según el contenido
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Guardar el archivo Excel en el sistema de archivos
                    FileInfo fileInfo = new FileInfo(filePath);
                    package.SaveAs(fileInfo);
                }
            });
            
        }
        public string? ShowSaveFileDialog()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx|All Files|*.*";
                saveFileDialog.Title = "Guardar como";
                saveFileDialog.FileName = "archivo.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return saveFileDialog.FileName;
                }
            }
            return null; // Devuelve null si el usuario cancela
        }
        
    }
}
