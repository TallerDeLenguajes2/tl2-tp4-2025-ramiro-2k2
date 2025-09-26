namespace tl2_tp4_2025_ramiro_2k2.Models
{

    //using CadeteriaLib;
    using System.Collections.Generic;

    public interface IDataProvider
    {
        Cadeteria GetCadeteria();
        List<Cadete> GetCadetes();
    }
}
