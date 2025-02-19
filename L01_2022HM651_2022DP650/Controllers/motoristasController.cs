using L01_2022HM651_2022DP650.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace L01_2022HM651_2022DP650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotoristasController : ControllerBase
    {
        private readonly restauranteContext _restauranteContexto;

        public MotoristasController(restauranteContext restauranteContexto)
        {
            _restauranteContexto = restauranteContexto;
        }

        // Obtener todos los motoristas
        [HttpGet]
        [Route("GetAllMotoristas")]
        public IActionResult GetAllMotoristas()
        {
            var listado = _restauranteContexto.Motorista.ToList();
            return Ok(listado);
        }

        // Obtener motoristas filtrados por nombre
        [HttpGet]
        [Route("GetByNombre/{nombre}")]
        public IActionResult GetMotoristasByNombre(string nombre)
        {
            var motoristasFiltrados = _restauranteContexto.Motorista
                .Where(m => m.nombreMotorista.Contains(nombre))
                .ToList();

            if (!motoristasFiltrados.Any())
                return NotFound(new { message = "No se encontraron motoristas con ese nombre" });

            return Ok(motoristasFiltrados);
        }

        // Obtener motorista por ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var motorista = _restauranteContexto.Motorista.Find(id);

            if (motorista == null)
                return NotFound(new { message = "Motorista no encontrado" });

            return Ok(motorista);
        }

        // Agregar un nuevo motorista
        [HttpPost]
        [Route("Add")]
        public IActionResult AddMotorista([FromBody] DTOmotoristas motoDTO)
        {
            try
            {
                if (motoDTO == null)
                    return BadRequest(new { message = "Datos inválidos" });

                var nuevoMotorista = new motoristas
                {
                    nombreMotorista = motoDTO.nombreMotorista
                };

                _restauranteContexto.Motorista.Add(nuevoMotorista);
                _restauranteContexto.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = nuevoMotorista.motoristaId }, nuevoMotorista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno", error = ex.Message });
            }
        }

        // Actualizar un motorista
        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult UpdateMotorista(int id, [FromBody] DTOmotoristas motoDTO)
        {
            var motoristaActual = _restauranteContexto.Motorista.FirstOrDefault(m => m.motoristaId == id);

            if (motoristaActual == null)
                return NotFound(new { message = "Motorista no encontrado" });

            motoristaActual.nombreMotorista = motoDTO.nombreMotorista;

            _restauranteContexto.Entry(motoristaActual).State = EntityState.Modified;
            _restauranteContexto.SaveChanges();

            return Ok(motoristaActual);
        }

        // Eliminar un motorista
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteMotorista(int id)
        {
            var motorista = _restauranteContexto.Motorista.FirstOrDefault(m => m.motoristaId == id);

            if (motorista == null)
                return NotFound(new { message = "Motorista no encontrado" });

            _restauranteContexto.Motorista.Remove(motorista);
            _restauranteContexto.SaveChanges();

            return Ok(new { message = "Motorista eliminado correctamente" });
        }
    }
}
