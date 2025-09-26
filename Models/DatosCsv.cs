namespace tl2_tp4_2025_ramiro_2k2.Models
{
    //using CadeteriaLib;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class DatosCsv : IDataProvider
    {
        private readonly string basePath;

        public DatosCsv(string basePath)
        {
            this.basePath = basePath;
        }

        public Cadeteria GetCadeteria()
        {
            string cadeteriaPath = Path.Combine(basePath, "cadeteria.csv");
            if (!File.Exists(cadeteriaPath))
                throw new FileNotFoundException("No se encontró cadeteria.csv");

            // formato esperado: Nombre;Telefono
            var line = File.ReadAllLines(cadeteriaPath).Skip(1).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(line)) throw new Exception("cadeteria.csv vacío");

            var partes = line.Split(';', StringSplitOptions.TrimEntries);
            return new Cadeteria(partes[0], partes[1]);
        }

        public List<Cadete> GetCadetes()
        {
            string cadetesPath = Path.Combine(basePath, "cadetes.csv");
            if (!File.Exists(cadetesPath))
                throw new FileNotFoundException("No se encontró cadetes.csv");

            return File.ReadAllLines(cadetesPath)
                       .Skip(1)
                       .Where(l => !string.IsNullOrWhiteSpace(l))
                       .Select(l =>
                       {
                           var partes = l.Split(';', StringSplitOptions.TrimEntries);
                           return new Cadete(int.Parse(partes[0]), partes[1], partes[2], partes[3]);
                       }).ToList();
        }
    }

}