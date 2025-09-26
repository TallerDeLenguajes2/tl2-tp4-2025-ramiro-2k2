namespace tl2_tp4_2025_ramiro_2k2.Models
{

    //using CadeteriaLib;
    using System.ComponentModel;

    public enum EstadoPedido
    {
        Pendiente,
        Asignado,
        EnCamino,
        Entregado,
        Cancelado
    }

    public class Pedido
    {
        public int Numero { get;  set; }
        public string? Observaciones { get;  set; }
        private Cliente cliente;
        public EstadoPedido Estado { get; set; }
        public Cadete? Cadete { get;  set; }  // Cadete opcional al inicio

        public Pedido(int numero, string? observaciones, Cliente cliente)
        {
            Numero = numero;
            Observaciones = observaciones;
            this.cliente = cliente;
            Estado = EstadoPedido.Pendiente;
        }

        public string VerNombreCliente() => cliente.Nombre;
        public string VerDireccionCliente() => cliente.Direccion;
        public string VerTelefonoCliente() => cliente.Telefono;
        public string VerDatosReferencia() => cliente.DatosReferenciaDireccion ?? "";

        public void AsignarCadete(Cadete cadete)
        {
            Cadete = cadete;
            Estado = EstadoPedido.Asignado;
        }

        public void QuitarCadete()
        {
            Cadete = null;
            if (Estado != EstadoPedido.Cancelado)
                Estado = EstadoPedido.Pendiente;
        }

        public void CambiarEstado(EstadoPedido nuevoEstado)
        {
            Estado = nuevoEstado;
        }
    }

}