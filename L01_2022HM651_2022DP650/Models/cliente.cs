using System.ComponentModel.DataAnnotations;

namespace L01_2022HM651_2022DP650.Models
{
    public class cliente
    {
        [Key]
        public int clienteId { get; set; }
        public string nombreCliente { get; set; }
        public string direccion { get; set; }
    }
}
