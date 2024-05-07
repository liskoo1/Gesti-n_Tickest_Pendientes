using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestión_Tickest_Pendientes.Entitys
{
    public class Empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Empresa { get; set; }
        public string Nombre_Empresa { get; set;}
        public string CIF_Empresa { get; set; }
        public string Direccion_Empresa { get; set; }
        public string Telefono_Empresa { get; set; }
        public string Email_Empresa { get; set;}
        public Empresa()
        {

        }
        public Empresa(string NombreEmpresa,string cifEmpresa, string direccionEmpresa, string tlfEmpresa, string emailEmpresa)
        {
            this.Nombre_Empresa = NombreEmpresa;
            this.CIF_Empresa= cifEmpresa;
            this.Direccion_Empresa = direccionEmpresa;
            this.Telefono_Empresa = tlfEmpresa;
            this.Email_Empresa = emailEmpresa;
        }

    }
}
