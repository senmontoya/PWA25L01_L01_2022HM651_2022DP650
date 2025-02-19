using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace L01_2022HM651_2022DP650.Models
{
    public class motoristas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int motoristaId { get; set; }
        [StringLength(200)]
        public string nombreMotorista { get; set; }
        
    }
}
