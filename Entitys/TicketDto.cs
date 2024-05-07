using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestión_Tickest_Pendientes.Entitys
{

    public class TicketDto
    {
        public string Cif_Cliente { get; set; }
        public string Id_Albaran { get; set; }
        public DateTime Fecha { get; set; }
        public string Sala { get; set; }
        public string Mesa { get; set; }
        public string NIF_CIF { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Codigo_Postal { get; set; }
        public double Total { get; set; }
        public byte[] Firma { get; set; }
    }
}
