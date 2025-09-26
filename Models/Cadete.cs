    namespace tl2_tp4_2025_ramiro_2k2.Models
{
    //using CadeteriaLib;


    public class Cadete
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public Cadete() { }

        public Cadete(int id, string nombre, string direccion, string telefono)
        {
            Id = id;
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
        }
        public int CantidadPedidosEntregados()
        {
            return Pedidos.Count(p => p.Estado == EstadoPedido.Entregado);
        }
    }
}