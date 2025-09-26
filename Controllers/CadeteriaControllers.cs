using Microsoft.AspNetCore.Mvc;
using tl2_tp4_2025_ramiro_2k2.Models;

namespace tl2_tp4_2025_ramiro_2k2.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class CadeteriaController : ControllerBase
    {
        private readonly Cadeteria _cadeteria;

        public CadeteriaController(Cadeteria cadeteria)
        {
            _cadeteria = cadeteria;
        }

        // [Get] GetPedidos() => Retorna una lista de Pedidos
        [HttpGet("pedidos")]
        public ActionResult<List<Pedido>> GetPedidos()
        {
            return Ok(_cadeteria.Pedidos);
        }

        // [Get] GetCadetes() => Retorna una lista de Cadetes
        [HttpGet("cadetes")]
        public ActionResult<List<Cadete>> GetCadetes()
        {
            return Ok(_cadeteria.ListaCadetes);
        }

        // [Get] GetInforme() => Retorna un objeto Informe
        [HttpGet("informe")]
        public ActionResult GetInforme()
        {
            var detalle = _cadeteria.ListaCadetes
                .Select(c => new
                {
                    Cadete = c.Nombre,
                    CantidadEnvios = _cadeteria.Pedidos.Count(p => p.Cadete?.Id == c.Id && p.Estado == EstadoPedido.Entregado),
                    Monto = _cadeteria.Pedidos.Count(p => p.Cadete?.Id == c.Id && p.Estado == EstadoPedido.Entregado) * Cadeteria.MontoPorEnvio
                })
                .ToList();

            var totalEnvios = detalle.Sum(d => d.CantidadEnvios);
            var totalMonto = detalle.Sum(d => d.Monto);

            return Ok(new { Detalle = detalle, TotalEnvios = totalEnvios, TotalMonto = totalMonto });
        }

        // [Post] AgregarPedido(Pedido pedido)
        [HttpPost("agregar-pedido")]
        public ActionResult AgregarPedido([FromBody] Pedido pedido)
        {
            try
            {
                _cadeteria.IngresarPedido(pedido);
                // CreatedAtAction podría usarse si tenés un endpoint que devuelva un pedido individual
                return Created("", pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // [Put] AsignarPedido(int idPedido, int idCadete)
        [HttpPut("asignar-pedido/{idPedido:int}/{idCadete:int}")]
        public ActionResult AsignarPedido(int idPedido, int idCadete)
        {
            var pedido = _cadeteria.Pedidos.FirstOrDefault(p => p.Numero == idPedido);
            if (pedido == null) return NotFound(new { error = $"Pedido {idPedido} no encontrado" });

            var cad = _cadeteria.ListaCadetes.FirstOrDefault(c => c.Id == idCadete);
            if (cad == null) return NotFound(new { error = $"Cadete {idCadete} no encontrado" });

            // si ya estaba asignado, lo sacamos del anterior
            if (pedido.Cadete != null)
                pedido.Cadete.Pedidos.Remove(pedido);

            // usa el método de tu clase Pedido si existe, o asigna directamente
            // pedido.AsignarCadete(cad);
            pedido.Cadete = cad;
            cad.Pedidos.Add(pedido);

            return Ok(new { mensaje = "Pedido asignado correctamente" });
        }

        // [Put] CambiarEstadoPedido(int idPedido,int NuevoEstado)
        [HttpPut("cambiar-estado/{idPedido:int}/{nuevoEstado:int}")]
        public ActionResult CambiarEstadoPedido(int idPedido, int nuevoEstado)
        {
            if (!Enum.IsDefined(typeof(EstadoPedido), nuevoEstado))
                return BadRequest(new { error = "Estado inválido" });

            var pedido = _cadeteria.Pedidos.FirstOrDefault(p => p.Numero == idPedido);
            if (pedido == null) return NotFound(new { error = $"Pedido {idPedido} no encontrado" });

            pedido.CambiarEstado((EstadoPedido)nuevoEstado);
            return Ok(new { mensaje = "Estado actualizado" });
        }

        // [Put] CambiarCadetePedido(int idPedido,int idNuevoCadete)
        [HttpPut("cambiar-cadete/{idPedido:int}/{idNuevoCadete:int}")]
        public ActionResult CambiarCadetePedido(int idPedido, int idNuevoCadete)
        {
            var pedido = _cadeteria.Pedidos.FirstOrDefault(p => p.Numero == idPedido);
            if (pedido == null) return NotFound(new { error = $"Pedido {idPedido} no encontrado" });

            var nuevo = _cadeteria.ListaCadetes.FirstOrDefault(c => c.Id == idNuevoCadete);
            if (nuevo == null) return NotFound(new { error = $"Cadete {idNuevoCadete} no encontrado" });

            if (pedido.Cadete != null)
                pedido.Cadete.Pedidos.Remove(pedido);

            pedido.Cadete = nuevo;
            nuevo.Pedidos.Add(pedido);

            return Ok(new { mensaje = "Cadete del pedido cambiado correctamente" });
        }
    }
