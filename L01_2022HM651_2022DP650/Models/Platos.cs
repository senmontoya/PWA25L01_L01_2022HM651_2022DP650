using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace L01_2022HM651_2022DP650.Models
{
    public class Platos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPlato { get; set; }
        [StringLength(200)]
        public string nombrePlato { get; set; }
        
        public decimal precio { get; set; }
    }
}
