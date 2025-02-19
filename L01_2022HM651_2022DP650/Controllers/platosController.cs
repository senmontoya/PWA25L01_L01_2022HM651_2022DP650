using L01_2022HM651_2022DP650.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2022HM651_2022DP650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class platosController : ControllerBase
    {
        private readonly restauranteContext _restauranteContexto;

        public platosController(restauranteContext restauranteContexto)
        {
            _restauranteContexto = restauranteContexto;
        }
        [HttpGet]
        [Route("GetAllPlatos")]
        public IActionResult GetLibros()
        {
            var listado = this._restauranteContexto.Platos.ToList();
            return Ok(listado);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarAutor([FromBody] PlatosDTO platosDTO)
        {
            try
            {
                if (platosDTO == null)
                    return BadRequest(new { message = "Datos inválidos" });

                var platoNuevo = new Platos
                {
                    nombrePlato = platosDTO.nombrePlato,
                    precio = platosDTO.precio

                };

                _restauranteContexto.Platos.Add(platoNuevo);
                _restauranteContexto.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = platoNuevo.platoId }, platoNuevo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var plato = _restauranteContexto.Platos.Find(id);

                if (plato == null)
                    return NotFound(new { message = "Plato no encontrado" });

                return Ok(plato);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno", error = ex.Message });
            }
        }

        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult ActualizarPlato(int id, [FromBody] Platos platosMod)
        {
            var platoActual = _restauranteContexto.Platos.FirstOrDefault(p => p.platoId == id);

            if (platoActual == null)
                return NotFound(new { message = "Plato no encontrado" });

            platoActual.nombrePlato = platosMod.nombrePlato;
            platoActual.precio = platosMod.precio;

            _restauranteContexto.Entry(platoActual).State = EntityState.Modified;
            _restauranteContexto.SaveChanges();

            return Ok(platoActual);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult EliminarPlato(int id)
        {
            var plato = _restauranteContexto.Platos.FirstOrDefault(p => p.platoId == id);

            if (plato == null)
                return NotFound(new { message = "Plato no encontrado" });

            _restauranteContexto.Platos.Remove(plato);
            _restauranteContexto.SaveChanges();

            return Ok(new { message = "Plato eliminado correctamente" });
        }

    }
}
