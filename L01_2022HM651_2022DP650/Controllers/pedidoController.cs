using Microsoft.AspNetCore.Http;
using L01_2022HM651_2022DP650.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace L01_2022HM651_2022DP650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pedidoController : ControllerBase
    {
        private readonly restauranteContext _restauranteContexto;

        public pedidoController(restauranteContext restauranteContexto)
        {
            _restauranteContexto = restauranteContexto;
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarPedido([FromBody] pedido pedido)
        {
            try
            {
                _restauranteContexto.pedido.Add(pedido);
                _restauranteContexto.SaveChanges();
                return Ok("Pedido guardado");
            }
            catch (Exception bug)
            {
                return BadRequest(bug.Message);
            }
        }

        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult ActualizarPedido(int id, [FromBody] pedido pedido)
        {
            try
            {
                pedido? pedidoActual = (from p in _restauranteContexto.pedido where p.pedidoId == id select p).FirstOrDefault();
                if (pedidoActual == null)
                {
                    return NotFound("Pedido no encontrado");
                }
                pedidoActual.motoristaId = pedido.motoristaId;
                pedidoActual.clienteId = pedido.clienteId;
                pedidoActual.platoId = pedido.platoId;
                pedidoActual.cantidad = pedido.cantidad;
                pedidoActual.precio = pedido.precio;
                _restauranteContexto.Entry(pedidoActual).State = EntityState.Modified;
                _restauranteContexto.SaveChanges();
                return Ok("Pedido actualizado");
            }
            catch (Exception bug)
            {
                return BadRequest(bug.Message);
            }
        }

    }
}
