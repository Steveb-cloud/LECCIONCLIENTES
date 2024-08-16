using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LECCIONCLIENTES.Context;
using LECCIONCLIENTES.Models;
using LECCIONCLIENTES.DTO;
using System.Text.Json;
using System.Text;

namespace LECCIONCLIENTES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClientesEfContext _context;

        // Definición de la URL base para las solicitudes POST
        private static readonly string BaseUrlTecsu = "http://localhost:5155/api/Cliente";

        public ClientesController(ClientesEfContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientesDTO>>> GetClientes()
        {
            var list = await _context.Clientes.ToListAsync();
            return convierteaDTOCliente(list);
        }

        private ActionResult<IEnumerable<ClientesDTO>> convierteaDTOCliente(List<Cliente> list)
        {
            List<ClientesDTO> result = new List<ClientesDTO>();
            for (int i = 0; i < list.Count; i++)
            {
                ClientesDTO obj = new ClientesDTO();
                var item = list[i];
                obj.Idc = item.Idc;
                obj.Nombre = item.Nombre;
                obj.Apellido = item.Apellido;
                result.Add(obj);
            }
            return result;
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, ClientesDTO clienteDTO)
        {
            Cliente result = transformaDTOaClientes(clienteDTO);
            if (id != result.Idc)
            {
                return BadRequest();
            }

            _context.Entry(result).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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

        // POST: api/Clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(ClientesDTO clienteDTO)
        {
            Cliente cliente = transformaDTOaClientes(clienteDTO);
            _context.Clientes.Add(cliente);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClienteExists(cliente.Idc))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCliente", new { id = cliente.Idc }, cliente);
        }

        static async Task PostClienteTecsuAsync(ClientesDTO objCliente)
        {
            using (HttpClient client = new HttpClient())
            {
                string contenidoJson = JsonSerializer.Serialize(objCliente);
                var contenidoPeticion = new StringContent(contenidoJson, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage respuestaHttp = await client.PostAsync(BaseUrlTecsu, contenidoPeticion);

                    if (respuestaHttp.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Cliente creado exitosamente.");
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

        private Cliente transformaDTOaClientes(ClientesDTO clientesDTO)
        {
            Cliente obj = new Cliente
            {
                Idc = clientesDTO.Idc,
                Nombre = clientesDTO.Nombre,
                Apellido = clientesDTO.Apellido
            };
            return obj;
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Idc == id);
        }
    }
}

