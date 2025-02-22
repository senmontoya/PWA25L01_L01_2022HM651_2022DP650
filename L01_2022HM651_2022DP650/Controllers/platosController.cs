﻿using L01_2022HM651_2022DP650.Models;
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
        [HttpGet("filtrarPorPrecio/{precioMax}")]
        public IActionResult FiltrarPedidosPorPrecio(decimal precioMax)
        {
            try
            {
                var pedidos = _restauranteContexto.pedido
                    .Where(p => p.precio < precioMax)
                    .ToList();

                if (pedidos.Count == 0)
                    return NotFound(new { message = "No hay pedidos con precio menor a " + precioMax });

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("PlatoMasPedido/{platoId}")]
        [HttpGet("topNPlatosMasPedidos/{n}")]
        public IActionResult TopPlatos(int n)
        {
            try
            {
                var topPlatos = _restauranteContexto.pedido
                    .GroupBy(p => p.platoId)
                    .Select(g => new
                    {
                        PlatoId = g.Key,
                        TotalPedidos = g.Sum(p => p.cantidad)
                    })
                    .OrderByDescending(p => p.TotalPedidos)
                    .Take(n) 
                    .Join(_restauranteContexto.Platos,
                          pedido => pedido.PlatoId,
                          plato => plato.platoId,
                          (pedido, plato) => new
                          {
                              plato.nombrePlato,
                              pedido.TotalPedidos
                          })
                    .ToList();

                if (topPlatos.Count == 0)
                {
                    return NotFound("No hay pedidos registrados.");
                }

                return Ok(topPlatos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
