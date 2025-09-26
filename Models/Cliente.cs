namespace tl2_tp4_2025_ramiro_2k2.Models
{

    //using CadeteriaLib;
    public class Cliente
    {
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? DatosReferenciaDireccion { get; set; }

        public Cliente() { }

        public Cliente(string nombre, string direccion, string telefono, string? datosReferenciaDireccion)
        {
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
            DatosReferenciaDireccion = datosReferenciaDireccion;
        }
    }
}
