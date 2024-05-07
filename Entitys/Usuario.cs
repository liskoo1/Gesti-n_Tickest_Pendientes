using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestión_Tickest_Pendientes.Entitys
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Usuario { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Id_Empresa { get; set; }
        [ForeignKey("Id_Empresa")]
        public virtual Empresa Empresa { get; set; }
    }
}
