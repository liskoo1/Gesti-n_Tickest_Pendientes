using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestión_Tickest_Pendientes.Entitys
{
    public class Cliente
    {
        [Key]
        public string CIF_Cliente { get; set; }
        [MaxLength]
        public string Nombre_Cliente { get; set; }
        
        public string Direccion_Cliente { get; set; }
        public string Codigo_Postal { get; set; }
        public string Telefono_Cliente { get; set; }
        public string Email_Cliente { get; set; }
        
        public int Id_Empresa {  get; set; }

        [ForeignKey("Id_Empresa")]
        public virtual Empresa Empresa { get; set; }  
    }
}
