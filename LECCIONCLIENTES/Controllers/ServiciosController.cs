using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LECCIONCLIENTES.Context;  // Asegúrate de que este espacio de nombres apunte a tu contexto de base de datos
using LECCIONCLIENTES.Models;  // Asegúrate de que este espacio de nombres apunte a tu modelo Servicio
using LECCIONCLIENTES.DTO;
using System.Text.Json;
using System.Text;

namespace LECCIONCLIENTES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly ClientesEfContext _context;

        // Definición de la URL base para las solicitudes POST
        private static readonly string BaseUrlServicio = "http://ejemplo.com/api";

        public ServiciosController(ClientesEfContext context)
        {
            _context = context;
        }

        // GET: api/Servicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioDTO>>> GetServicios()
        {
            var list = await _context.Servicios.ToListAsync();
            return convierteaDTOServicio(list);
        }

        private ActionResult<IEnumerable<ServicioDTO>> convierteaDTOServicio(List<Servicio> list)
        {
            List<ServicioDTO> result = new List<ServicioDTO>();
            foreach (var item in list)
            {
                ServicioDTO obj = new ServicioDTO
                {
                    Ids = item.Ids,
                    Descripcion = item.Descripcion
                };
                result.Add(obj);
            }
            return result;
        }

        // GET: api/Servicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioDTO>> GetServicio(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);

            if (servicio == null)
            {
                return NotFound();
            }

            return new ServicioDTO
            {
                Ids = servicio.Ids,
                Descripcion = servicio.Descripcion
            };
        }

        // PUT: api/Servicios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicio(int id, ServicioDTO servicioDTO)
        {
            if (id != servicioDTO.Ids)
            {
                return BadRequest();
            }

            var servicio = transformaDTOaServicio(servicioDTO);
            _context.Entry(servicio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Servicios
        [HttpPost]
        public async Task<ActionResult<Servicio>> PostServicio(ServicioDTO servicioDTO)
        {
            var servicio = transformaDTOaServicio(servicioDTO);
            _context.Servicios.Add(servicio);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ServicioExists(servicio.Ids))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetServicio", new { id = servicio.Ids }, servicio);
        }

        static async Task PostServicioAsync(ServicioDTO objServicio)
        {
            using (HttpClient client = new HttpClient())
            {
                string contenidoJson = JsonSerializer.Serialize(objServicio);
                var contenidoPeticion = new StringContent(contenidoJson, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage respuestaHttp = await client.PostAsync(BaseUrlServicio, contenidoPeticion);

                    if (respuestaHttp.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Servicio creado exitosamente.");
                    }
                    else
                    {
                        Console.WriteLine($"Error: {respuestaHttp.StatusCode}");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Excepción: {e.Message}");
                }
            }
        }

        private Servicio transformaDTOaServicio(ServicioDTO servicioDTO)
        {
            return new Servicio
            {
                Ids = servicioDTO.Ids,
                Descripcion = servicioDTO.Descripcion
            };
        }

        // DELETE: api/Servicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }

            _context.Servicios.Remove(servicio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicioExists(int id)
        {
            return _context.Servicios.Any(e => e.Ids == id);
        }
    }
}
