 namespace tl2_tp4_2025_ramiro_2k2.Models
{
    //using CadeteriaLib;
    using System;
    using System.Collections.Generic;
    using System.Linq;


    public class Cadeteria
    {
        public string? Nombre { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public List<Cadete> ListaCadetes { get; set; } = new();
        public List<Pedido> Pedidos { get; set; } = new();

        public const float MontoPorEnvio = 500f;

        public Cadeteria() { }

        public Cadeteria(string? nombre, string telefono)
        {
            Nombre = nombre;
            Telefono = telefono;
        }

        public void AgregarCadete(Cadete c)
        {
            if (ListaCadetes.Any(x => x.Id == c.Id))
                throw new Exception($"Ya existe un cadete con Id {c.Id}");
            ListaCadetes.Add(c);
        }

        public bool EliminarCadete(int id) => ListaCadetes.RemoveAll(c => c.Id == id) > 0;

        public void IngresarPedido(Pedido p)
        {
            if (Pedidos.Any(x => x.Numero == p.Numero))
                throw new Exception($"Ya existe el pedido N° {p.Numero}");
            Pedidos.Add(p);
        }

        public bool AsignarPedidoAPorNombre(int numeroPedido, string nombreCadete)
        {
            var pedido = Pedidos.FirstOrDefault(p => p.Numero == numeroPedido);
            if (pedido == null)
            {
                return false; // No existe el pedido
            }

            var cadete = ListaCadetes
                .FirstOrDefault(c => c.Nombre.Equals(nombreCadete, StringComparison.OrdinalIgnoreCase));

            if (cadete == null)
            {
                return false; // No existe el cadete
            }

            // Si el pedido ya estaba asignado a otro cadete, lo removemos primero
            if (pedido.Cadete != null)
            {
                pedido.Cadete.Pedidos.Remove(pedido);
            }

            pedido.AsignarCadete(cadete);
            cadete.Pedidos.Add(pedido);

            return true; // Asignación correcta
        }

        public void CambiarEstadoPedido(int numeroPedido, EstadoPedido nuevoEstado)
        {
            var p = Pedidos.FirstOrDefault(p => p.Numero == numeroPedido)
                    ?? throw new Exception($"No existe el pedido {numeroPedido}");
            p.CambiarEstado(nuevoEstado);
        }

        public float JornalACobrar(int idCadete)
        {
            var cad = ListaCadetes.FirstOrDefault(c => c.Id == idCadete)
                      ?? throw new Exception($"No existe el cadete {idCadete}");

            int entregados = Pedidos.Count(p => p.Cadete?.Id == idCadete && p.Estado == EstadoPedido.Entregado);
            return entregados * MontoPorEnvio;
        }

        public void MostrarInformeJornada()
        {
            Console.WriteLine("\n=== INFORME DE JORNADA ===");
            var info = ListaCadetes
                .Select(c => new
                {
                    Cadete = c.Nombre,
                    CantidadEnvios = Pedidos.Count(p => p.Cadete?.Id == c.Id && p.Estado == EstadoPedido.Entregado),
                    Monto = Pedidos.Count(p => p.Cadete?.Id == c.Id && p.Estado == EstadoPedido.Entregado) * MontoPorEnvio
                })
                .ToList();

            foreach (var i in info)
                Console.WriteLine($"Cadete: {i.Cadete} → Envíos: {i.CantidadEnvios}, Monto: ${i.Monto}");

            int totalEnvios = info.Sum(i => i.CantidadEnvios);
            float totalMonto = info.Sum(i => (float)i.Monto);
            double promedio = info.Count > 0 ? info.Average(i => i.CantidadEnvios) : 0;

            Console.WriteLine("\n--- Totales ---");
            Console.WriteLine($"Total de envíos: {totalEnvios}");
            Console.WriteLine($"Total recaudado: ${totalMonto}");
            Console.WriteLine($"Promedio de envíos por cadete: {promedio:F2}");
        }
    }
}