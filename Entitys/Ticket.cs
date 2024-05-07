using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestión_Tickest_Pendientes.Entitys
{
    public class Ticket
    {
        [Key]
        public string Id_Albaran { get; set; }
        public double Total { get; set; }
        public DateTime Fecha { get; set; }
        
        public string Sala { get; set; }
        public string Mesa { get; set; }
        public byte[] Firma_Cliente { get; set; }

        public string CIF_Cliente { get; set; }
        [ForeignKey("CIF_Cliente")]
        public virtual Cliente Cliente { get; set; }
    }
}
